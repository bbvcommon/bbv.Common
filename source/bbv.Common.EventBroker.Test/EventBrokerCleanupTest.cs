//-------------------------------------------------------------------------------
// <copyright file="EventBrokerCleanupTest.cs" company="bbv Software Services AG">
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
    using NUnit.Framework;

    /// <summary>
    /// Tests that the event broker cleans up correctly.
    /// </summary>
    [TestFixture]
    public class EventBrokerCleanupTest
    {
        /// <summary>
        /// The object under test.
        /// </summary>
        private EventBroker testee;

        /// <summary>
        /// For test: a simple event.
        /// </summary>
        public event EventHandler SimpleEvent;

        /// <summary>
        /// Set up of the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();
            Subscriber.Count = 0;
        }

        /// <summary>
        /// Subscribers that are no longer referenced can be collected by the garbage collector.
        /// The event broker does not keep them alive.
        /// </summary>
        [Test]
        public void FreedSubscriber()
        {
            Publisher p = new Publisher();
            Subscriber s1 = new Subscriber();
            Subscriber s2 = new Subscriber();

            this.testee.Register(p);
            this.testee.Register(s1);
            this.testee.Register(s2);

// ReSharper disable RedundantAssignment
            s1 = null; // kill reference to s1
// ReSharper restore RedundantAssignment
            GC.Collect();  // breaks up the weak reference to the subscriber

            p.CallCount();

            Assert.AreEqual(1, Subscriber.Count);

            Assert.Greater(s2.ThreadId, -10); // just some stupid code to prevent s2 from being collected
        }

        /// <summary>
        /// When the event broker is disposed then the events published by properties are unregistered.
        /// </summary>
        [Test]
        public void DisposeUnregistersPublicationByProperty()
        {
            Publisher p = new Publisher();
            Subscriber s = new Subscriber();

            this.testee.Register(p);
            this.testee.Register(s);

            this.testee.Dispose();

            Assert.Throws<NullReferenceException>(
                () => p.CallSimpleEvent(),
                "Should result in a null reference exception because the event should not be registered by the event topic anymore.");
        }

        /// <summary>
        /// Publications made in code can be removed from the event broker.
        /// </summary>
        [Test]
        public void PublicationsInCodeCanBeRemoved()
        {
            Subscriber s = new Subscriber();

            this.testee.AddPublication(EventTopics.SimpleEvent, this, ref this.SimpleEvent, HandlerRestriction.None);
            this.testee.Register(s);

            this.testee.RemovePublication(EventTopics.SimpleEvent, this, ref this.SimpleEvent);

            Assert.Throws<NullReferenceException>(
                () => this.SimpleEvent(this, EventArgs.Empty),
                "Should result in a null reference exception because the event should not be registered by the event topic anymore.");
        }

        /// <summary>
        /// Subscriptions made in code can be removed from the event broker.
        /// </summary>
        [Test]
        public void SubscriptionsInCodeCanBeRemoved()
        {
            Publisher p = new Publisher();
            Subscriber s = new Subscriber();

            this.testee.Register(p);
            this.testee.AddSubscription(EventTopics.SimpleEvent, s, s.SimpleEvent, new Handlers.Publisher());

            this.testee.RemoveSubscription(EventTopics.SimpleEvent, s, s.SimpleEvent);

            p.CallSimpleEvent();

            Assert.IsFalse(s.SimpleEventCalled);
        }
    }
}