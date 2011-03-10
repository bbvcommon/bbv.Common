//-------------------------------------------------------------------------------
// <copyright file="ModuleControllerMultiThreadModuleTest.cs" company="bbv Software Services AG">
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

    using NUnit.Framework;

    [TestFixture]
    public class ModuleControllerMultiThreadModuleTest
    {
        private ModuleController testee;
        private TestModule module;

        [SetUp]
        public void SetUp()
        {
            this.module = new TestModule();
        }

        [Test]
        public void ConsumeMessagesOnMultipleWorkerThreads()
        {
            const int NumerOfMessages = 1000;

            this.module.ConsumeDelay = TimeSpan.FromMilliseconds(0);
            this.testee = new ModuleController();
            this.testee.Initialize(this.module, 10);

            for (int i = 0; i < NumerOfMessages; i++)
            {
                this.testee.EnqueueMessage(i);
            }

            AutoResetEvent signal = new AutoResetEvent(false);
            int count = 0;
            object padlock = new object();
            this.testee.AfterConsumeMessage += delegate
                {
                    lock (padlock)
                    {
                        count++;
                        if (count == NumerOfMessages)
                        {
                            signal.Set();
                        }
                    }
                };

            this.testee.Start();

            Assert.IsTrue(signal.WaitOne(10000, false), "not all messages consumed. Consumed " + this.module.Messages.Count);
            this.testee.Stop();
        }

        [Test]
        public void StopControllerWhenMultipleConsumersAreRunning()
        {
            const int NumerOfMessages = 100;

            this.module.ConsumeDelay = TimeSpan.FromMilliseconds(10);
            this.testee = new ModuleController();
            this.testee.Initialize(this.module, 10);

            for (int i = 0; i < NumerOfMessages; i++)
            {
                this.testee.EnqueueMessage(i);
            }

            this.testee.Start();

            Thread.Sleep(50);

            this.testee.Stop();

            Assert.Less(this.module.Messages.Count, NumerOfMessages, "there should not be enough time for all messages to be consumed.");
            Assert.Greater(this.testee.MessageCount, 0, "there should be messages left to consume.");
        }
    }
}