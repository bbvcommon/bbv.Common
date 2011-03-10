//-------------------------------------------------------------------------------
// <copyright file="HandlerMethodRegistrationTest.cs" company="bbv Software Services AG">
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
    /// Subscriptions can be added directly on event broker.
    /// </summary>
    [TestFixture]
    public class HandlerMethodRegistrationTest
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
        /// A method on another object can be registered as event handler and unregistered again.
        /// </summary>
        [Test]
        public void RegisterHandlerMethod()
        {
            Publisher p = new Publisher();
            Subscriber s = new Subscriber();

            this.testee.RegisterHandlerMethod(EventTopics.SimpleEvent, s, s.Handle, new Handlers.Publisher());
            this.testee.Register(p);

            p.CallSimpleEvent();

            Assert.IsTrue(s.Called, "event was not handled.");

            s.Reset();
            this.testee.RemoveSubscription(EventTopics.SimpleEvent, s, s.Handle);

            p.CallSimpleEvent();

            Assert.IsFalse(s.Called, "event should not have been handled.");
        }

        /// <summary>
        /// A method on another object can be registered as event handler and unregistered again.
        /// </summary>
        [Test]
        public void RegisterCustomEventHandlerMethod()
        {
            Publisher p = new Publisher();
            Subscriber s = new Subscriber();

            this.testee.RegisterHandlerMethod<CustomEventArguments>(EventTopics.CustomEventArgs, s, s.HandleCustomEvent, new Handlers.Publisher());
            this.testee.Register(p);

            p.CallCustomEventArgs("test");

            Assert.IsTrue(s.Called, "event was not handled.");

            s.Reset();
            this.testee.RemoveSubscription<CustomEventArguments>(EventTopics.CustomEventArgs, s, s.HandleCustomEvent);

            p.CallCustomEventArgs("test");

            Assert.IsFalse(s.Called, "event should not have been handled.");
        }
        
        /// <summary>
        /// Test subscriber.
        /// </summary>
        public class Subscriber
        {
            /// <summary>
            /// Gets a value indicating whether this <see cref="Subscriber"/> is called.
            /// </summary>
            /// <value><c>true</c> if called; otherwise, <c>false</c>.</value>
            public bool Called { get; private set; }

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public void Reset()
            {
                this.Called = false;
            }

            /// <summary>
            /// Test handler.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            public void Handle(object sender, EventArgs e)
            {
                this.Called = true;
            }

            /// <summary>
            /// Handles the custom event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The event arguments.</param>
            public void HandleCustomEvent(object sender, CustomEventArguments e)
            {
                this.Called = true;
            }
        }
    }
}