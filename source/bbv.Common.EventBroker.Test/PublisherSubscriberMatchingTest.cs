//-------------------------------------------------------------------------------
// <copyright file="PublisherSubscriberMatchingTest.cs" company="bbv Software Services AG">
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
    /// Tests verification that publisher and subscriber are matching on registration.
    /// </summary>
    [TestFixture]
    public class PublisherSubscriberMatchingTest
    {
        /// <summary>The object under test</summary>
        private EventBroker testee;

        /// <summary>
        /// Sets up the event broker.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();
        }

        /// <summary>
        /// Matching publisher and subscribers are working.
        /// </summary>
        [Test]
        public void Match()
        {
            NormalPublisher p = new NormalPublisher();
            NormalSubscriber s = new NormalSubscriber();

            this.testee.Register(p);
            this.testee.Register(s);

            p.Fire();
        }

        /// <summary>
        /// Matching publisher and subscribers are working. Reversed registration order.
        /// </summary>
        [Test]
        public void MatchReversedRegistration()
        {
            NormalPublisher p = new NormalPublisher();
            NormalSubscriber s = new NormalSubscriber();

            this.testee.Register(s);
            this.testee.Register(p);

            p.Fire();
        }

        /// <summary>
        /// Publisher with <see cref="EventArgs"/> and subscriber with custom event args do not match.
        /// </summary>
        [Test]
        public void PassNormalToCustomEventArgs()
        {
            NormalPublisher p = new NormalPublisher();
            CustomSubscriber s = new CustomSubscriber();

            this.testee.Register(p);

            Assert.Throws<EventTopicException>(
                () => this.testee.Register(s));
        }

        /// <summary>
        /// Publisher with <see cref="EventArgs"/> and subscriber with custom event args do not match. Reversed registration order.
        /// </summary>
        [Test]
        public void PassNormalToCustomEventArgsReversedRegistration()
        {
            NormalPublisher p = new NormalPublisher();
            CustomSubscriber s = new CustomSubscriber();

            this.testee.Register(s);

            Assert.Throws<EventTopicException>(
                () => this.testee.Register(p));
        }

        /// <summary>
        /// Publisher with custom <see cref="EventArgs"/> and subscriber with normal event args do match.
        /// </summary>
        [Test]
        public void PassCustomToNormalEventArgs()
        {
            CustomPublisher p = new CustomPublisher();
            NormalSubscriber s = new NormalSubscriber();

            this.testee.Register(p);
            this.testee.Register(s);

            p.Fire();
        }

        /// <summary>
        /// Publisher with custom <see cref="EventArgs"/> and subscriber with normal event args do match. Reversed registration order.
        /// </summary>
        [Test]
        public void PassCustomToNormalEventArgsReversedRegistration()
        {
            CustomPublisher p = new CustomPublisher();
            NormalSubscriber s = new NormalSubscriber();

            this.testee.Register(s);
            this.testee.Register(p);

            p.Fire();
        }

        /// <summary>
        /// Generic <see cref="EventArgs"/> can be passed to normal event args.
        /// </summary>
        [Test]
        public void PassGenericToNormalEventArgs()
        {
            GenericPublisher p = new GenericPublisher();
            NormalSubscriber s = new NormalSubscriber();

            this.testee.Register(p);
            this.testee.Register(s);

            p.Fire();
        }
        
        /// <summary>
        /// Publisher with <see cref="EventHandler"/>
        /// </summary>
        public class NormalPublisher
        {
            /// <summary>
            /// An event with custom event args.
            /// </summary>
            [EventPublication("Event")]
            public event EventHandler Event;

            /// <summary>
            /// Fires the event.
            /// </summary>
            public void Fire()
            {
                this.Event(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Publisher with <see cref="EventHandler{EventArgs}"/>.
        /// </summary>
        public class GenericPublisher
        {
            /// <summary>
            /// An event with custom event args.
            /// </summary>
            [EventPublication("Event")]
            public event EventHandler<EventArgs> Event;

            /// <summary>
            /// Fires the event.
            /// </summary>
            public void Fire()
            {
                this.Event(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Publisher with <see cref="EventHandler{CustomEventArgs}"/>.
        /// </summary>
        public class CustomPublisher
        {
            /// <summary>
            /// An event with custom event args.
            /// </summary>
            [EventPublication("Event")]
            public event EventHandler<DescribeToTest.CustomEventArgs> Event;

            /// <summary>
            /// Fires the event.
            /// </summary>
            public void Fire()
            {
                this.Event(this, new DescribeToTest.CustomEventArgs());
            }
        }

        /// <summary>
        /// Subscriber with <see cref="EventHandler"/> signature.
        /// </summary>
        public class NormalSubscriber
        {
            /// <summary>
            /// handles the event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription("Event", typeof(Handlers.Publisher))]
            public void Handler(object sender, EventArgs e)
            {
            }
        }

        /// <summary>
        /// Subscriber with <see cref="EventHandler{CustomEventArgs}"/> signature.
        /// </summary>
        public class CustomSubscriber
        {
            /// <summary>
            /// handles the event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription("Event", typeof(Handlers.Publisher))]
            public void Handler(object sender, DescribeToTest.CustomEventArgs e)
            {
            }
        }
    }
}