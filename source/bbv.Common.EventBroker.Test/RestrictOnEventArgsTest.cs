//-------------------------------------------------------------------------------
// <copyright file="RestrictOnEventArgsTest.cs" company="bbv Software Services AG">
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
    using System.ComponentModel;
    using NUnit.Framework;

    /// <summary>
    /// Tests that subscriptions can be restricted by the event argument.
    /// </summary>
    [TestFixture]
    public class RestrictOnEventArgsTest
    {
        /// <summary>
        /// A subscriber can restrict the handling of an event depending on the event argument.
        /// </summary>
        [Test]
        public void Restrict()
        {
            EventBroker eventBroker = new EventBroker();

            Publisher p = new Publisher();
            Subscriber s = new Subscriber();

            eventBroker.Register(p);
            eventBroker.Register(s);

            CancelEventArgs e = new CancelEventArgs(false);
            p.Fire(e);

            Assert.AreEqual(1, s.NumberOfHandledEvents, "should be handled.");

            p.Fire(e);
            Assert.AreEqual(1, s.NumberOfHandledEvents, "should not be handled.");
        }

        [Test]
        public void SeveralHandlers()
        {
            EventBroker eventBroker = new EventBroker();

            Publisher p = new Publisher();
            Subscriber s1 = new Subscriber();
            Subscriber s2 = new Subscriber();

            eventBroker.Register(p);
            eventBroker.Register(s1);
            eventBroker.Register(s2);

            CancelEventArgs e = new CancelEventArgs(false);
            p.Fire(e);

            Assert.AreEqual(1, s1.NumberOfHandledEvents, "s1 was not called");
            Assert.AreEqual(0, s2.NumberOfHandledEvents, "s2 was called");
        }

        /// <summary>
        /// A test publisher.
        /// </summary>
        public class Publisher
        {
            /// <summary>
            /// Test event.
            /// </summary>
            [EventPublication("topic")]
            public event EventHandler<CancelEventArgs> Event;

            /// <summary>
            /// Fires the <see cref="Event"/>.
            /// </summary>
            /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
            public void Fire(CancelEventArgs e)
            {
                this.Event(this, e);
            }
        }

        /// <summary>
        /// A test subscriber.
        /// </summary>
        public class Subscriber
        {
            /// <summary>
            /// Gets or sets the number of handled events.
            /// </summary>
            public int NumberOfHandledEvents { get; set; }

            /// <summary>
            /// Event handler.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
            [EventSubscription("topic", typeof(Handlers.Publisher), typeof(Matchers.NotAlreadyCanceled))]
            public void Handler(object sender, CancelEventArgs e)
            {
                this.NumberOfHandledEvents++;

                e.Cancel = true;
            }
        }
    }
}