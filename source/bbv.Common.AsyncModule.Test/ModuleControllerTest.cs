//-------------------------------------------------------------------------------
// <copyright file="ModuleControllerTest.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace bbv.Common.AsyncModule
{
    using System;
    using System.Threading;

    using bbv.Common.AsyncModule.Events;

    using NUnit.Framework;

    [TestFixture]
    public class ModuleControllerTest
    {
        private ModuleController testee;
        private TestModule module;

        [SetUp]
        public void SetUp()
        {
            this.module = new TestModule();
            this.testee = new ModuleController();
            this.testee.Initialize(this.module);

            this.module.Controller = this.testee;

            Assert.IsFalse(this.testee.IsAlive, "controller should not be active yet.");
        }

        [Test]
        public void ControllerCanBeStartedAndStopped()
        {
            bool beforeModuleStartFired = false;
            bool afterModuleStartFired = false;

            bool beforeModuleStopFired = false;
            bool afterModuleStopFired = false;

            this.testee.BeforeModuleStart += delegate { beforeModuleStartFired = true; };
            this.testee.AfterModuleStart += delegate { afterModuleStartFired = true; };

            this.testee.BeforeModuleStop += delegate { beforeModuleStopFired = true; };
            this.testee.AfterModuleStop += delegate { afterModuleStopFired = true; };

            this.testee.Start();
            Assert.IsTrue(this.testee.IsAlive, "controller is not alive");

            this.testee.Stop();
            Assert.IsFalse(this.testee.IsAlive, "controller is still alive");

            Assert.IsTrue(beforeModuleStartFired, "BeforeModuleStart was not fired.");
            Assert.IsTrue(afterModuleStartFired, "AfterModuleStart was not fired.");
            Assert.IsTrue(beforeModuleStopFired, "BeforeModuleStop was not fired.");
            Assert.IsTrue(afterModuleStopFired, "AfterModuleStop was not fired.");
        }

        [Test]
        public void StartOnAStartedControllerDoesNotCrash()
        {
            this.testee.Start();

            this.testee.BeforeModuleStart += delegate
                                            {
                                                Assert.Fail("no notification allowed.");
                                            };
            this.testee.Start();

            Assert.IsTrue(this.testee.IsAlive);
        }

        [Test]
        public void StopOnAStoppedControllerDoesNotCrash()
        {
            this.testee.AfterModuleStop += delegate
                                            {
                                                Assert.Fail("no notification allowed.");
                                            };

            this.testee.Stop();
        }

        [Test]
        public void StopAsyncOnAStoppedControllerDoesNotCrash()
        {
            this.testee.AfterModuleStop += delegate
                                            {
                                                Assert.Fail("no notification allowed.");
                                            };

            this.testee.StopAsync();
        }

        [Test]
        public void MessagesAreConsumed()
        {
            const int NumberOfMessages = 6;

            this.EnqueueAndConsume(NumberOfMessages);
        }

        [Test]
        public void MessagesAreConsumedMultithreaded()
        {
            this.module = new TestModule();
            this.testee = new ModuleController();
            this.testee.Initialize(this.module, 10);

            this.module.Controller = this.testee;

            const int NumberOfMessages = 1000;

            this.EnqueueAndConsume(NumberOfMessages);
        }

        [Test]
        public void Stop()
        {
            this.module.ConsumeDelay = TimeSpan.FromMilliseconds(10);
            for (int i = 0; i < 10;  i++)
            {
                this.testee.EnqueueMessage(i);
            }

            // stop module when first message is consumed
            this.testee.BeforeConsumeMessage += delegate
                                               {
                                                   // asynchronously stop controller
                                                   ThreadPool.QueueUserWorkItem(
                                                       delegate
                                                           {
                                                               this.testee.Stop();
                                                           });
                                               };

            AutoResetEvent moduleStopped = new AutoResetEvent(false);
            this.testee.AfterModuleStop += delegate
                                          {
                                              moduleStopped.Set();
                                          };

            this.testee.Start();

            Assert.IsTrue(moduleStopped.WaitOne(1000, false), "module not stopped");

            Assert.AreEqual(9, this.testee.MessageCount, "there should be still 9 messages in the queue.");
            Assert.IsFalse(this.testee.IsAlive, "still alive");
        }

        [Test]
        [Ignore("fragile test")]
        public void StopAsynchronously()
        {
            AutoResetEvent moduleStopped = new AutoResetEvent(false);
            this.testee.AfterModuleStop += delegate
                                          {
                                              moduleStopped.Set();
                                          };

            this.module.ConsumeDelay = TimeSpan.FromMilliseconds(10);
            this.testee.EnqueueMessage(1);
            this.testee.EnqueueMessage(2);
            this.testee.EnqueueMessage(3);

            this.testee.Start();
            Thread.Sleep(10);
            this.testee.StopAsync();

            Assert.IsTrue(moduleStopped.WaitOne(1000, false), "module was not stopped.");
            Assert.Greater(this.testee.MessageCount, 0);
        }

        [Test]
        public void WorkerThreadCanNotStopModuleSynchronously()
        {
            AutoResetEvent exceptionThrown = new AutoResetEvent(false);
            this.testee.UnhandledModuleExceptionOccured += delegate
                                                          {
                                                              exceptionThrown.Set();
                                                          };

            this.testee.EnqueueMessage(new StopMessage());
            this.testee.Start();

            Assert.IsTrue(exceptionThrown.WaitOne(1000, false), "no exception was thrown");

            this.testee.Stop();
        }

        [Test]
        public void WorkerThreadCanCallStopAsync()
        {
            AutoResetEvent moduleStopped = new AutoResetEvent(false);
            this.testee.AfterModuleStop += delegate
                                          {
                                              moduleStopped.Set();
                                          };

            this.testee.EnqueueMessage(new StopAsyncMessage());
            this.testee.Start();

            Assert.IsTrue(moduleStopped.WaitOne(1000, false), "module was not stopped.");
        }

        [Test]
        public void ExceptionsInMessageConsumationDoNotCrashModule()
        {
            Exception exception = null;
            AutoResetEvent exceptionThrown = new AutoResetEvent(false);
            this.testee.UnhandledModuleExceptionOccured += delegate(object sender, UnhandledModuleExceptionEventArgs e)
                                                          {
                                                              exception = e.UnhandledException;
                                                              exceptionThrown.Set();
                                                          };

            this.testee.Start();
            this.testee.EnqueueMessage(new ThrowExceptionMessage());

            Assert.IsTrue(exceptionThrown.WaitOne(1000, false), "exception was not thrown");
            this.testee.Stop();

            Assert.AreEqual("test", exception.InnerException.Message);
        }

        [Test]
        public void ExceptionsInMessageConsumationCanBeHandled()
        {
            this.testee.ConsumeMessageExceptionOccurred += delegate(object sender, ConsumeMessageExceptionEventArgs e)
                                                          {
                                                              e.ExceptionHandled = e.Exception.Message == "test";
                                                          };

            this.testee.Start();

            this.testee.EnqueueMessage(new ThrowExceptionMessage());

            this.testee.Stop();
        }

        [Test]
        public void MessagesInQueueCanBeCleared()
        {
            this.module.ConsumeDelay = TimeSpan.FromMilliseconds(1);

            this.testee.EnqueueMessage(1);
            this.testee.EnqueueMessage(2);
            this.testee.EnqueueMessage(3);
            this.testee.EnqueueMessage(4);
            this.testee.EnqueueMessage(5);

            object[] messages = this.testee.ClearMessages();

            Assert.AreEqual(0, this.testee.MessageCount, "MessageCount");
            Assert.AreEqual(5, messages.Length);
        }

        [Test]
        public void MessagesCanBeSkippedAndAfterConsumeMessageIsAlwaysFired()
        {
            this.testee.BeforeConsumeMessage += delegate(object sender, BeforeConsumeMessageEventArgs e)
                                               {
                                                   // skip all string messages
                                                   e.Cancel = e.Message is string;
                                               };

            AutoResetEvent signal = new AutoResetEvent(false);
            int msgCount = 0;
            this.testee.AfterConsumeMessage += delegate
                                              {
                                                  msgCount++;
                                                  if (msgCount == 5)
                                                  {
                                                      signal.Set();
                                                  }
                                              };

            this.testee.Start();

            this.testee.EnqueueMessage(1);
            this.testee.EnqueueMessage("hello");
            this.testee.EnqueueMessage("world");
            this.testee.EnqueueMessage(2);
            this.testee.EnqueueMessage(3);

            Assert.IsTrue(signal.WaitOne(100000, false), "did not receive signal.");

            this.testee.Stop();

            Assert.AreEqual(3, this.module.Messages.Count, "wrong number of messages consumed.");
        }

        [Test]
        public void PriorityEnqueueMessage()
        {
            this.testee.EnqueueMessage(1);
            this.testee.EnqueueMessage(2);
            this.testee.EnqueuePriorityMessage(0);

            object[] consumedMessages = new object[3];
            int i = 0;
            AutoResetEvent allMessagesConsumed = new AutoResetEvent(false);

            this.testee.AfterConsumeMessage += (sender, e) =>
                                              {
                                                  consumedMessages[i++] = e.Message;

                                                  if (i == 3)
                                                  {
                                                      allMessagesConsumed.Set();
                                                  }
                                              };

            this.testee.Start();

            Assert.IsTrue(allMessagesConsumed.WaitOne(1000));
            Assert.AreEqual(0, consumedMessages[0]);
            Assert.AreEqual(1, consumedMessages[1]);
            Assert.AreEqual(2, consumedMessages[2]);
        }

        private void EnqueueAndConsume(int numberOfMessages)
        {
            int beforeEnqueueMessage = 0;
            int afterEnqueueMessage = 0;
            int beforeConsumeMessage = 0;
            int afterConsumeMessage = 0;

            AutoResetEvent allMessagesConsumed = new AutoResetEvent(false);

            this.testee.BeforeEnqueueMessage += delegate
                                                    {
                                                        lock (this)
                                                        {
                                                            beforeEnqueueMessage++;
                                                        }
                                                    };
            this.testee.AfterEnqueueMessage += delegate
                                                   {
                                                       lock (this)
                                                       {
                                                           afterEnqueueMessage++;
                                                       }
                                                   };

            this.testee.BeforeConsumeMessage += delegate
                                                    {
                                                        lock (this)
                                                        {
                                                            beforeConsumeMessage++;
                                                        }
                                                    };
            this.testee.AfterConsumeMessage += delegate
            {
                lock (this)
                {
                    afterConsumeMessage++;
                }

                if (afterConsumeMessage == numberOfMessages)
                {
                    allMessagesConsumed.Set();
                }
            };

            // enqueue messages before start
            for (int i = 0; i < numberOfMessages / 2; i++) 
            {
                this.testee.EnqueueMessage(i);
            }

            Assert.AreEqual(numberOfMessages / 2, this.testee.Messages.Length);

            this.testee.Start();

            // enqueue messages after start
            for (int i = 0; i < numberOfMessages - (numberOfMessages / 2); i++) 
            {
                this.testee.EnqueueMessage(i);
            }

            Assert.IsTrue(allMessagesConsumed.WaitOne(10000, false), "not all messages were consumed. Consumed " + afterConsumeMessage + " messages.");

            Assert.AreEqual(numberOfMessages, beforeEnqueueMessage, "beforeConsumeMessage");
            Assert.AreEqual(numberOfMessages, afterEnqueueMessage, "afterEnqueueMessage");
            Assert.AreEqual(numberOfMessages, beforeConsumeMessage, "beforeConsumeMessage");
            Assert.AreEqual(numberOfMessages, afterConsumeMessage, "afterConsumeMessage");
        }
    }
}