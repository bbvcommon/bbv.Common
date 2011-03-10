//-------------------------------------------------------------------------------
// <copyright file="RegistrationInInterfaceTest.cs" company="bbv Software Services AG">
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
    using Exceptions;
    using NUnit.Framework;

    /// <summary>
    /// Tests event publication and subscription attributes in interfaces.
    /// </summary>
    [TestFixture]
    public class RegistrationInInterfaceTest
    {
        /// <summary>The event broker</summary>
        private EventBroker eventbroker;

        /// <summary>
        /// Interface with event publication.
        /// </summary>
        public interface IPublisher
        {
            /// <summary>
            /// Event with publication.
            /// </summary>
            [EventPublication("topic")]
            event EventHandler Event;

            /// <summary>
            /// Fires the <see cref="Event"/>.
            /// </summary>
            void FireEvent();
        }

        /// <summary>
        /// Interface with event subscription.
        /// </summary>
        public interface ISubscriber
        {
            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="ISubscriber"/> was called.
            /// </summary>
            /// <value><c>true</c> if called; otherwise, <c>false</c>.</value>
            bool Called { get; set; }

            /// <summary>
            /// Handler of the Event from the publisher with subscription.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription("topic", typeof(Handlers.Publisher))]
            void Handler(object sender, EventArgs e);
        }

        /// <summary>
        /// Sets up the event broker.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.eventbroker = new EventBroker();
        }

        /// <summary>
        /// If a publication attribute in an interface is found it is registered.
        /// </summary>
        [Test]
        public void PublicationsOnInterfaceAreRecognized()
        {
            IPublisher p = new PublisherImplementingInterface();
            SubscriberWithEventSubscription s = new SubscriberWithEventSubscription();

            this.eventbroker.Register(p);
            this.eventbroker.Register(s);

            p.FireEvent();

            Assert.IsTrue(s.Called);
        }

        /// <summary>
        /// If the publisher implements the interface explicitly the event is still registered correctly.
        /// </summary>
        [Test]
        public void PublicationsOnExplicitlyImplementedInterfaceAreRecognized()
        {
            IPublisher p = new PublisherWithExplicitInterfaceImplementation();
            SubscriberWithEventSubscription s = new SubscriberWithEventSubscription();

            this.eventbroker.Register(p);
            this.eventbroker.Register(s);

            p.FireEvent();

            Assert.IsTrue(s.Called);
        }

        /// <summary>
        /// An exception is thrown if the event publication is repeated in the implementation of the interface.
        /// </summary>
        [Test]
        public void RepeatedEventPublication()
        {
            IPublisher p = new PublisherWithRepeatedEventPublication();

            Assert.Throws<RepeatedPublicationException>(
                () => this.eventbroker.Register(p));
        }

        /// <summary>
        /// Subscription attributes in interfaces are processed correctly.
        /// </summary>
        [Test]
        public void SubscriptionsAreRecognizedOnInterfaces()
        {
            IPublisher p = new PublisherImplementingInterface();
            SubscriberImplementingInterface s = new SubscriberImplementingInterface();

            this.eventbroker.Register(p);
            this.eventbroker.Register(s);

            p.FireEvent();

            Assert.IsTrue(s.Called);
        }

        /// <summary>
        /// An exception is thrown if the subscriber repeats the subscription attribute in its implementation.
        /// </summary>
        [Test]
        public void RepeatedSubscription()
        {
            IPublisher p = new PublisherImplementingInterface();
            ISubscriber s = new SubscriberWithRepeatedEventSubscription();

            this.eventbroker.Register(p);

            Assert.Throws<RepeatedSubscriptionException>(
                () => this.eventbroker.Register(s));
        }

        /// <summary>
        /// Implements the interface as it should be made.
        /// </summary>
        public class PublisherImplementingInterface : IPublisher
        {
            /// <summary>
            /// Event with publication in the interface.
            /// </summary>
            public event EventHandler Event;

            /// <summary>
            /// Fires the <see cref="Event"/>.
            /// </summary>
            public void FireEvent()
            {
                if (this.Event != null)
                {
                    this.Event(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Explicit implementation of the publisher interface.
        /// </summary>
        public class PublisherWithExplicitInterfaceImplementation : IPublisher
        {
            /// <summary>
            /// internal event for the explicit implementation.
            /// </summary>
            private event EventHandler e;

            /// <summary>
            /// Event with publication in interface.
            /// </summary>
            event EventHandler IPublisher.Event
            {
                add { this.e += value; }
                remove { this.e -= value; }
            }

            /// <summary>
            /// Fires the <see cref="Event"/>.
            /// </summary>
            void IPublisher.FireEvent()
            {
                this.e(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// This publisher repeats the publication.
        /// </summary>
        public class PublisherWithRepeatedEventPublication : IPublisher
        {
            /// <summary>
            /// Event with publication.
            /// </summary>
            [EventPublication("topic")]
            public event EventHandler Event;

            /// <summary>
            /// Fires the <see cref="Event"/>.
            /// </summary>
            public void FireEvent()
            {
                if (this.Event != null)
                {
                    this.Event(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// A normal subscriber.
        /// </summary>
        public class SubscriberWithEventSubscription
        {
            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="SubscriberWithEventSubscription"/> was called.
            /// </summary>
            /// <value><c>true</c> if called; otherwise, <c>false</c>.</value>
            public bool Called { get; set; }

            /// <summary>
            /// Handles the Event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription("topic", typeof(Handlers.Publisher))]
            public void Handler(object sender, EventArgs e)
            {
                this.Called = true;
            }
        }

        /// <summary>
        /// This subscriber implement the interface as it should be done.
        /// </summary>
        public class SubscriberImplementingInterface : ISubscriber
        {
            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="ISubscriber"/> was called.
            /// </summary>
            /// <value><c>true</c> if called; otherwise, <c>false</c>.</value>
            public bool Called { get; set; }

            /// <summary>
            /// Handler of the Event from the publisher with subscription.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            public void Handler(object sender, EventArgs e)
            {
                this.Called = true;
            }
        }

        /// <summary>
        /// This subscriber repeats the subscription.
        /// </summary>
        public class SubscriberWithRepeatedEventSubscription : ISubscriber
        {
            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="ISubscriber"/> was called.
            /// </summary>
            /// <value><c>true</c> if called; otherwise, <c>false</c>.</value>
            public bool Called { get; set; }

            /// <summary>
            /// Handler of the Event from the publisher with subscription.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription("topic", typeof(Handlers.Publisher))]
            public void Handler(object sender, EventArgs e)
            {
                this.Called = true;
            }
        }
    }
}