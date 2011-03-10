//-------------------------------------------------------------------------------
// <copyright file="EventRegistrationTest.cs" company="bbv Software Services AG">
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
//   Contains software or other content adapted from 
//   Smart Client – Composite UI Application Block, 
//   2005 Microsoft Corporation. All rights reserved.
//-------------------------------------------------------------------------------

namespace bbv.Common.EventBroker
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Tests that events and handler methods can be registered directly on event broker.
    /// </summary>
    [TestFixture]
    public class EventRegistrationTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private EventBroker testee;

        /// <summary>
        /// Initializes a test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();
        }

        /// <summary>
        /// An event of another object can be registered on the event broker.
        /// </summary>
        [Test]
        public void RegisterEvent()
        {
            Publisher p = new Publisher();
            Subscriber s = new Subscriber();

            this.testee.RegisterEvent(EventTopics.SimpleEvent, p, "Event", HandlerRestriction.None);
            this.testee.Register(s);

            p.CallEvent();

            Assert.IsTrue(s.SimpleEventCalled, "event was not handled.");
        }

        /// <summary>
        /// When trying to register an event that does not exist then an exception is thrown.
        /// </summary>
        [Test]
        public void RegisterUnknownEvent()
        {
            Publisher p = new Publisher();

            Assert.Throws<Exceptions.PublisherEventNotFoundException>(
                () => this.testee.RegisterEvent(EventTopics.SimpleEvent, p, "UnknownEvent", HandlerRestriction.None));
        }

        /// <summary>
        /// If null is passed as publisher then an exception is thrown.
        /// </summary>
        [Test]
        public void NullPublisher()
        {
            Assert.Throws<ArgumentNullException>(
                () => this.testee.RegisterEvent(EventTopics.SimpleEvent, null, "Event", HandlerRestriction.None));
        }

        // TODO: unregister

        /// <summary>
        /// Test publisher
        /// </summary>
        public class Publisher
        {
            /// <summary>
            /// Test event.
            /// </summary>
            public event EventHandler Event;

            /// <summary>
            /// Calls the test event.
            /// </summary>
            public void CallEvent()
            {
                this.Event(this, EventArgs.Empty);
            }
        }
    }
}
