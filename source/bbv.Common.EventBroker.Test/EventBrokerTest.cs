//-------------------------------------------------------------------------------
// <copyright file="EventBrokerTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.EventBroker
{
    using System;
    using System.Threading;
    using NUnit.Framework;

    /// <summary>
    /// Test for <see cref="EventBroker"/>.
    /// </summary>
    /// <remarks>
    /// Note that no tests that require a UI thread can be done. --> therefore the <see cref="UnitTestFactory"/> is used.
    /// </remarks>
    [TestFixture]
    public class EventBrokerTest
    {
        /// <summary>
        /// The object under test.
        /// </summary>
        private EventBroker testee;

        /// <summary>
        /// A publisher
        /// </summary>
        private Publisher p;

        /// <summary>
        /// A subscriber.
        /// </summary>
        private Subscriber s;

        /// <summary>
        /// Sets up the test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Subscriber.Count = 0;

            this.testee = new EventBroker();
            this.p = new Publisher();
            this.s = new Subscriber();

            this.testee.Register(this.p);
            this.testee.Register(this.s);
        }

        /// <summary>
        /// Test clean up.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (this.p != null)
            {
                this.testee.Unregister(this.p);
            }

            if (this.s != null)
            {
                this.testee.Unregister(this.s);
            }

            this.testee.Dispose();
        }

        #region SimpleEvent

        /// <summary>
        /// An event is routed from a publisher to a subscriber.
        /// </summary>
        [Test]
        public void SimpleEvent()
        {
            this.p.CallSimpleEvent();

            Assert.IsTrue(this.s.SimpleEventCalled);
        }
               
        /// <summary>
        /// All subscribers are notified.
        /// </summary>
        [Test]
        public void SimpleEvent2Subscribers()
        {
            Subscriber s2 = new Subscriber();
            this.testee.Register(s2);

            this.p.CallSimpleEvent();

            Assert.IsTrue(this.s.SimpleEventCalled);
            Assert.IsTrue(s2.SimpleEventCalled);
        }

        #endregion

        #region CustomEventArgs

        /// <summary>
        /// Custom event arguments can be used for events.
        /// </summary>
        [Test]
        public void CustomEventArgs()
        {
            const string Value = "Test";

            this.p.CallCustomEventArgs(Value);

            Assert.IsNotNull(this.s.ReceivedCustomEventArguments);
            Assert.AreEqual(Value, this.s.ReceivedCustomEventArguments.String);
        }

        #endregion

        #region BackgroundThread

        /// <summary>
        /// Subscription can be done on a background thread.
        /// </summary>
        [Test]
        public void BackgroundThread()
        {
            this.p.CallBackgroundThread();

            Assert.IsTrue(this.s.Signal.WaitOne(1000, false), "signal not set.");

            Assert.AreNotEqual(-1, this.s.ThreadId);
            Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, this.s.ThreadId);
        }

        #endregion

        #region MultiplePublicationTokens

        /// <summary>
        /// Multiple event topics can be defined for a single event.
        /// </summary>
        [Test]
        public void MultiplePublicationTokens()
        {
            this.p.CallMultiplePublicationTokens();

            Assert.IsTrue(this.s.MultiplePublicationToken1Called);
            Assert.IsTrue(this.s.MultiplePublicationToken2Called);
            Assert.IsTrue(this.s.MultiplePublicationToken3Called);
        }

        #endregion

        #region MultipleSubscriptionTokens

        /// <summary>
        /// A handler method can subscribe to multiple event topics.
        /// </summary>
        [Test]
        public void MultipleSubscriptionTokens()
        {
            this.p.CallMultipleSubscriptionTokens();

            Assert.AreEqual(3, this.s.MultipleSubscriptionTokensCount);
        }

        #endregion

        #region CancelEventArgs

        /// <summary>
        /// Event arguments can pass results back to caller.
        /// </summary>
        [Test]
        public void CancelEventArgs()
        {
            bool cancel = this.p.CallCancelEvent();

            Assert.IsTrue(cancel, "Result from event call should be true. Return value could not be given back from handler.");
        }

        #endregion
    }
}
