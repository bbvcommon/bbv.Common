//-------------------------------------------------------------------------------
// <copyright file="AddSubscriptionTest.cs" company="bbv Software Services AG">
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
    /// Tests that subscriptions can be registered directly on the event broker.
    /// </summary>
    [TestFixture]
    public class AddSubscriptionTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private EventBroker testee;

        /// <summary>
        /// For test: whether the simple event was called.
        /// </summary>
        private bool simpleEventWasCalled;

        /// <summary>
        /// For test: the value passed in the custom event arguments.
        /// </summary>
        private string valueFromCustomEventArgs;

        /// <summary>
        /// A simple event for testing.
        /// </summary>
        [EventPublication(EventTopics.SimpleEvent)]
        public event EventHandler SimpleEvent;

        /// <summary>
        /// Event with custom event for testing.
        /// </summary>
        [EventPublication(EventTopics.CustomEventArgs)]
        public event EventHandler<CustomEventArguments> CustomEvent;

        /// <summary>
        /// Initializes a test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();

            this.testee.Register(this);

            this.simpleEventWasCalled = false;
            this.valueFromCustomEventArgs = null;
        }

        /// <summary>
        /// An event can be registered on the event broker and will be relayed to subscribers.
        /// </summary>
        [Test]
        public void AddPublication()
        {
            this.testee.AddSubscription(EventTopics.SimpleEvent, this, this.HandleSimpleEvent, new Handlers.Publisher());

            this.SimpleEvent(this, EventArgs.Empty);

            Assert.IsTrue(this.simpleEventWasCalled);
        }

        /// <summary>
        /// An event with custom event arguments can be registered on the event broker and will be relayed to subscribers.
        /// </summary>
        [Test]
        public void AddPublicationWithCustomEventHandler()
        {
            const string Value = "test";
            this.testee.AddSubscription<CustomEventArguments>(EventTopics.CustomEventArgs, this, this.HandleCustomEvent, new Handlers.Publisher());
            
            this.CustomEvent(this, new CustomEventArguments(Value));

            Assert.AreEqual(Value, this.valueFromCustomEventArgs);
        }

        /// <summary>
        /// Handles the simple event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void HandleSimpleEvent(object sender, EventArgs e)
        {
            this.simpleEventWasCalled = true;
        }

        /// <summary>
        /// Handles the custom event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void HandleCustomEvent(object sender, CustomEventArguments e)
        {
            this.valueFromCustomEventArgs = e.String;
        }
    }
}