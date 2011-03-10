//-------------------------------------------------------------------------------
// <copyright file="EventTesterTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.TestUtilities.Test
{
    using System;
    using NMock2;
    using NUnit.Framework;

    /// <summary>
    /// Tests the implementation of <see cref="EventTester{T}"/>.
    /// </summary>
    [TestFixture]
    public class EventTesterTest
    {
        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mockery mockery;

        /// <summary>
        /// The interface that is used to fire events.
        /// </summary>
        private ITestInterface testInterface;

        /// <summary>
        /// The interface that is used to fire events.
        /// </summary>
        private ITestInterfaceGeneric testInterfaceGeneric;

        /// <summary>
        /// Helper interface to fire an event that is monitored by the EventHelper.
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// The event that is fired.
            /// </summary>
            event EventHandler MyEvent;
        }

        /// <summary>
        /// Helper interface to fire an event that is monitored by the EventHelper.
        /// </summary>
        public interface ITestInterfaceGeneric
        {
            /// <summary>
            /// The event that is fired.
            /// </summary>
            event EventHandler<EventArgs> MyEvent;
        }

        /// <summary>
        /// Sets up all tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();
            this.testInterface = this.mockery.NewMock<ITestInterface>();
            this.testInterfaceGeneric = this.mockery.NewMock<ITestInterfaceGeneric>();
        }

        /// <summary>
        /// Was fires is true just after the event occurred.
        /// </summary>
        [Test]
        public void WasFired()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");
            using (EventTester eventHelper = new EventTester(this.testInterface, "MyEvent"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                Assert.IsFalse(eventHelper.WasFired, "Was fires was true before the event was fired.");
                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.IsTrue(eventHelper.WasFired, "Was fires was false after the event was fired.");

                Expect.Once.On(this.testInterface).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// AssertWasFired throws an <see cref="AssertionException"/> if the event was not fired. Once it was fired
        /// no exception occurs anymore.
        /// </summary>
        [Test]
        public void AssertWasFired()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");
            Expect.Once.On(this.testInterface).EventRemove("MyEvent");

            using (EventTester eventHelper = new EventTester(this.testInterface, "MyEvent"))
            {
                // Check if an AssertionException is thrown if the event has not occurred.
                Assert.Throws<EventTesterException>(eventHelper.AssertWasFired);

                // After the event was fired no exception occurs anymore
                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                eventHelper.AssertWasFired();

                // Another invocation can occur and still no execption is thrown
                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                eventHelper.AssertWasFired();
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// InvocationCount returns the number of invocations.
        /// </summary>
        [Test]
        public void InvocationCount()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");
            using (EventTester eventHelper = new EventTester(this.testInterface, "MyEvent"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                // 0 invocations have orrured
                Assert.AreEqual(0, eventHelper.InvocationCount);

                // Check that the invocation count increases with each invocation
                for (int i = 1; i < 10; i++)
                {
                    Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                    Assert.AreEqual(i, eventHelper.InvocationCount);
                }

                Expect.Once.On(this.testInterface).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// AssertInvocationCount throws an exception if the current InvocationCount is not equal to the specified number.
        /// </summary>
        [Test]
        public void AssertInvocationCount()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");
            Expect.Once.On(this.testInterface).EventRemove("MyEvent");

            using (EventTester eventHelper = new EventTester(this.testInterface, "MyEvent"))
            {
                eventHelper.AssertInvocationCount(0);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(1));

                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(0));
                eventHelper.AssertInvocationCount(1);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(2));

                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(1));
                eventHelper.AssertInvocationCount(2);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(3));
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// If the number of expected invocation is the same as the effective number of invocations no exception is thrown.
        /// </summary>
        [Test]
        public void AssertInvocationCountAutomatic()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");
            Expect.Once.On(this.testInterface).EventRemove("MyEvent");

            using (new EventTester(this.testInterface, "MyEvent", 2))
            {
                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// If the number of expected invocation is not the same as the effective number of invocations an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        [Test]
        public void AssertInvocationCountAutomaticException()
        {
            Assert.Throws<EventTesterException>(
                () =>
                    {
                        Expect.Once.On(this.testInterface).EventAdd("MyEvent");
                        using (new EventTester(this.testInterface, "MyEvent", 2))
                        {
                            this.mockery.VerifyAllExpectationsHaveBeenMet();

                            Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);

                            Expect.Once.On(this.testInterface).EventRemove("MyEvent");
                        }
                    });

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Was fires is true just after the event occurred.
        /// </summary>
        [Test]
        public void WasFiredGeneric()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            using (EventTester<EventArgs> eventHelper = new EventTester<EventArgs>(this.testInterfaceGeneric, "MyEvent"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                Assert.IsFalse(eventHelper.WasFired, "Was fires was true before the event was fired.");
                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.IsTrue(eventHelper.WasFired, "Was fires was false after the event was fired.");

                Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }
        
        /// <summary>
        /// Was fires is true just after the event occurred.
        /// </summary>
        [Test]
        public void AssertWasFiredGeneric()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");

            using (EventTester<EventArgs> eventHelper = new EventTester<EventArgs>(this.testInterfaceGeneric, "MyEvent"))
            {
                // Check if an EventTesterException is thrown if the event has not occurred.
                Assert.Throws<EventTesterException>(eventHelper.AssertWasFired);

                // After the event was fired no exception occurs anymore
                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);
                eventHelper.AssertWasFired();

                // Another invocation can occur and still no execption is thrown
                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);
                eventHelper.AssertWasFired();
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// InvocationCount returns the number of invocations.
        /// </summary>
        [Test]
        public void InvocationCountGeneric()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            using (EventTester<EventArgs> eventHelper = new EventTester<EventArgs>(this.testInterfaceGeneric, "MyEvent"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                // 0 invocations have orrured
                Assert.AreEqual(0, eventHelper.InvocationCount);

                // Check that the invocation count increases with each invocation
                for (int i = 1; i < 10; i++)
                {
                    Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);
                    Assert.AreEqual(i, eventHelper.InvocationCount);
                }

                Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// AssertInvocationCount throws an exception if the current InvocationCount is not equal to the specified number.
        /// </summary>
        [Test]
        public void AssertInvocationCountGeneric()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");

            using (EventTester<EventArgs> eventHelper = new EventTester<EventArgs>(this.testInterfaceGeneric, "MyEvent"))
            {
                eventHelper.AssertInvocationCount(0);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(1));

                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(0));
                eventHelper.AssertInvocationCount(1);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(2));

                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(1));
                eventHelper.AssertInvocationCount(2);
                Assert.Throws<EventTesterException>(() => eventHelper.AssertInvocationCount(3));
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// If the number of expected invocation is the same as the effective number of invocations no exception is thrown.
        /// </summary>
        [Test]
        public void AssertInvocationCountAutomaticGeneric()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            using (new EventTester<EventArgs>(this.testInterfaceGeneric, "MyEvent", 2))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);
                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);

                Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// If the number of expected invocation is not the same as the effective number of invocations an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        [Test]
        public void AssertInvocationCountAutomaticExceptionGeneric()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            var eventTester = new EventTester<EventArgs>(this.testInterfaceGeneric, "MyEvent", 1);

            this.mockery.VerifyAllExpectationsHaveBeenMet();

            Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);
            Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);

            Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");

            Assert.Throws<EventTesterException>(
                () => eventTester.Dispose());

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Was fires is true just after the event occurred when the event matcher matches.
        /// </summary>
        [Test]
        public void WasFiredWhenEventMatcherMatched()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");

            using (EventTester eventHelper = new EventTester(this.testInterface, "MyEvent", (s, e) => true, "everything"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                Assert.IsFalse(eventHelper.WasFired, "Was fires was true before the event was fired.");
                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.IsTrue(eventHelper.WasFired, "Was fires was false after the event was fired.");

                Expect.Once.On(this.testInterface).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Was fires is true just after the event occurred when the event matcher matches.
        /// </summary>
        [Test]
        public void WasFiredGenericWhenEventMatcherMatched()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");

            using (EventTester<EventArgs> eventHelper = new EventTester<EventArgs>(this.testInterfaceGeneric, "MyEvent", (s, e) => true, "everything"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                Assert.IsFalse(eventHelper.WasFired, "Was fires was true before the event was fired.");
                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.IsTrue(eventHelper.WasFired, "Was fires was false after the event was fired.");

                Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Was fires is false just after the event occurred when the event matcher did not match.
        /// </summary>
        [Test]
        public void WasNotFiredWhenEventMatcherDidNotMatch()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");

            using (EventTester<EventArgs> eventHelper = new EventTester<EventArgs>(this.testInterfaceGeneric, "MyEvent", (s, e) => false, "nothing"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                Assert.IsFalse(eventHelper.WasFired, "Was fires was true before the event was fired.");
                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.IsFalse(eventHelper.WasFired, "Was fires was true after the event was fired.");

                Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Was fires is false just after the event occurred when the event matcher did not match.
        /// </summary>
        [Test]
        public void WasNotFiredGenericWhenEventMatcherDidNotMatch()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");

            using (EventTester eventHelper = new EventTester(this.testInterface, "MyEvent", (s, e) => false, "nothing"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                Assert.IsFalse(eventHelper.WasFired, "Was fires was true before the event was fired.");
                Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty);
                Assert.IsFalse(eventHelper.WasFired, "Was fires was true after the event was fired.");

                Expect.Once.On(this.testInterface).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// If a event handler is specified it is called when the event occurs.
        /// </summary>
        [Test]
        public void DelegateExecuted()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");
            object eventSender = new object();
            EventArgs eventArgs = EventArgs.Empty;
            bool senderEqual = false;
            bool eventArgumentEqual = false;

            using (new EventTester(
                this.testInterface,
                "MyEvent",
                (sender, e) =>
                {
                    senderEqual = eventSender == sender;
                    eventArgumentEqual = eventArgs == e;
                }))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();
                Fire.On(this.testInterface).Event("MyEvent").With(eventSender, eventArgs);
                Expect.Once.On(this.testInterface).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();

            Assert.IsTrue(senderEqual, "The event sender was not the expected one.");
            Assert.IsTrue(eventArgumentEqual, "The event argument was not the expected one.");
        }

        /// <summary>
        /// If a event handler is specified it is called when the event occurs when the event matcher matches.
        /// </summary>
        [Test]
        public void DelegateExecutedWhenMatcherMatches()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");
            object eventSender = new object();
            EventArgs eventArgs = EventArgs.Empty;
            bool senderEqual = false;
            bool eventArgumentEqual = false;

            using (new EventTester(
                this.testInterface,
                "MyEvent",
                (sender, e) =>
                {
                    senderEqual = eventSender == sender;
                    eventArgumentEqual = eventArgs == e;
                },
                (sender, e) => true,
                "everything"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();
                Fire.On(this.testInterface).Event("MyEvent").With(eventSender, eventArgs);
                Expect.Once.On(this.testInterface).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();

            Assert.IsTrue(senderEqual, "The event sender was not the expected one.");
            Assert.IsTrue(eventArgumentEqual, "The event argument was not the expected one.");
        }

        /// <summary>
        /// If a event handler is specified it is not called when the event occurs when the event matcher did not match.
        /// </summary>
        [Test]
        public void DelegateNotExecutedWhenMatcherDidNotMatch()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");
            object eventSender = new object();
            EventArgs eventArgs = EventArgs.Empty;
            bool senderEqual = false;
            bool eventArgumentEqual = false;

            using (new EventTester(
                this.testInterface,
                "MyEvent",
                (sender, e) =>
                {
                    senderEqual = eventSender == sender;
                    eventArgumentEqual = eventArgs == e;
                },
                (sender, e) => false,
                "nothing"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();
                Fire.On(this.testInterface).Event("MyEvent").With(eventSender, eventArgs);
                Expect.Once.On(this.testInterface).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();

            Assert.IsFalse(senderEqual, "The sender can never be equal when the delegate is not executed.");
            Assert.IsFalse(eventArgumentEqual, "The event arguments can never be equal when the delegate is not executed.");
        }

        /// <summary>
        /// If a event handler is specified it is called when the event occurs when the matcher matches.
        /// </summary>
        [Test]
        public void DelegateExecutedGenericWhenMatcherMatches()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            object eventSender = new object();
            EventArgs eventArgs = EventArgs.Empty;
            bool senderEqual = false;
            bool eventArgumentEqual = false;

            using (new EventTester<EventArgs>(
                this.testInterfaceGeneric,
                "MyEvent",
                (sender, e) =>
                {
                    senderEqual = eventSender == sender;
                    eventArgumentEqual = eventArgs == e;
                },
                (sender, e) => true,
                "everything"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();
                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(eventSender, eventArgs);
                Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();

            Assert.IsTrue(senderEqual, "The event sender was not the expected one.");
            Assert.IsTrue(eventArgumentEqual, "The event argument was not the expected one.");
        }

        /// <summary>
        /// If a event handler is specified it is not called when the event occurs when the event matcher did not match.
        /// </summary>
        [Test]
        public void DelegateGenericNotExecutedWhenMatcherDidNotMatch()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            object eventSender = new object();
            EventArgs eventArgs = EventArgs.Empty;
            bool senderEqual = false;
            bool eventArgumentEqual = false;

            using (new EventTester<EventArgs>(
                this.testInterfaceGeneric,
                "MyEvent",
                (sender, e) =>
                {
                    senderEqual = eventSender == sender;
                    eventArgumentEqual = eventArgs == e;
                },
                (sender, e) => false,
                "nothing"))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();
                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(eventSender, eventArgs);
                Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();

            Assert.IsFalse(senderEqual, "The sender can never be equal when the delegate is not executed.");
            Assert.IsFalse(eventArgumentEqual, "The event arguments can never be equal when the delegate is not executed.");
        }

        /// <summary>
        /// If a event handler is specified it is called when the event occurs.
        /// </summary>
        [Test]
        public void DelegateExecutedGeneric()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            object eventSender = new object();
            EventArgs eventArgs = EventArgs.Empty;
            bool senderEqual = false;
            bool eventArgumentEqual = false;

            using (new EventTester<EventArgs>(
                this.testInterfaceGeneric,
                "MyEvent",
                (sender, e) =>
                {
                    senderEqual = eventSender == sender;
                    eventArgumentEqual = eventArgs == e;
                }))
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();
                Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(eventSender, eventArgs);
                Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();

            Assert.IsTrue(senderEqual, "The event sender was not the expected one.");
            Assert.IsTrue(eventArgumentEqual, "The event argument was not the expected one.");
        }

        /// <summary>
        /// Exceptions thrown in event handler have to be passed through.
        /// </summary>
        [Test]
        public void ExceptionThrowingEventHandler()
        {
            Expect.Once.On(this.testInterface).EventAdd("MyEvent");
            Expect.Once.On(this.testInterface).EventRemove("MyEvent");

            using (new EventTester(this.testInterface, "MyEvent", (sender, e) => { throw new ApplicationException("test"); }))
            {
                Assert.Throws<ApplicationException>(
                    () => Fire.On(this.testInterface).Event("MyEvent").With(null, EventArgs.Empty));
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Exceptions thrown in event handler have to be passed through.
        /// </summary>
        [Test]
        public void ExceptionThrowingEventHandlerGeneric()
        {
            Expect.Once.On(this.testInterfaceGeneric).EventAdd("MyEvent");
            Expect.Once.On(this.testInterfaceGeneric).EventRemove("MyEvent");

            using (new EventTester<EventArgs>(this.testInterfaceGeneric, "MyEvent", (sender, e) => { throw new ApplicationException("test"); }))
            {
                Assert.Throws<ApplicationException>(
                    () => Fire.On(this.testInterfaceGeneric).Event("MyEvent").With(null, EventArgs.Empty));
            }

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}