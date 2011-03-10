//-------------------------------------------------------------------------------
// <copyright file="HandlerRestrictionTest.cs" company="bbv Software Services AG">
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
    /// Tests the restriction for handlers feature.
    /// The publisher can restrict the kind of handling of its fired event by the subscriber: whether it has
    /// to be handled synchronously or asynchronously.
    /// </summary>
    [TestFixture]
    public class HandlerRestrictionTest
    {
        /// <summary>The testee</summary>
        private EventBroker testee;

        /// <summary>
        /// Sets up <see cref="testee"/>.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();
        }

        /// <summary>
        /// Synchronous and asynchronous event handling is allowed if the publisher does not restrict it.
        /// </summary>
        [Test]
        public void NoRestriction()
        {
            PublisherWithoutRestriction p = new PublisherWithoutRestriction();
            SubscriberWithSynchronousHandler sync = new SubscriberWithSynchronousHandler();
            SubscriberWithAsynchronousHandler async = new SubscriberWithAsynchronousHandler();

            this.testee.Register(p);
            this.testee.Register(sync);
            this.testee.Register(async);
        }

        /// <summary>
        /// If there is a restriction to allow only synchronous handling then synchronous handling works.
        /// </summary>
        [Test]
        public void SynchronousRestriction_SynchronousHandler()
        {
            PublisherWithSynchronousRestriction p = new PublisherWithSynchronousRestriction();
            SubscriberWithSynchronousHandler sync = new SubscriberWithSynchronousHandler();

            this.testee.Register(p);
            this.testee.Register(sync);
        }

        /// <summary>
        /// If there is a restriction to allow only synchronous handling then asynchronous handling does not works.
        /// </summary>
        [Test]
        public void SynchronousRestriction_AsynchronousHandler()
        {
            PublisherWithSynchronousRestriction p = new PublisherWithSynchronousRestriction();
            SubscriberWithAsynchronousHandler async = new SubscriberWithAsynchronousHandler();

            this.testee.Register(p);

            Assert.Throws<EventTopicException>(
                () => this.testee.Register(async));
        }

        /// <summary>
        /// If there is a restriction to allow only asynchronous handling then asynchronous handling works.
        /// </summary>
        [Test]
        public void AsynchronousRestriction_AsynchronousHandler()
        {
            PublisherWithAsynchronousRestriction p = new PublisherWithAsynchronousRestriction();
            SubscriberWithAsynchronousHandler async = new SubscriberWithAsynchronousHandler();

            this.testee.Register(p);
            this.testee.Register(async);
        }

        /// <summary>
        /// If there is a restriction to allow only asynchronous handling then synchronous handling does not works.
        /// </summary>
        [Test]
        public void AsynchronousRestriction_SynchronousHandler()
        {
            PublisherWithAsynchronousRestriction p = new PublisherWithAsynchronousRestriction();
            SubscriberWithSynchronousHandler sync = new SubscriberWithSynchronousHandler();

            this.testee.Register(p);

            Assert.Throws<EventTopicException>(
                () => this.testee.Register(sync));
        }

        /// <summary>
        /// Checks the <see cref="HandlerKind"/> of the handlers contained by default in the <see cref="EventBroker"/>.
        /// </summary>
        [Test]
        public void HandlersReturnCorrectHandlerKind()
        {
            Assert.AreEqual(HandlerKind.Synchronous, (new Handlers.Publisher()).Kind);
            Assert.AreEqual(HandlerKind.Asynchronous, (new Handlers.Background()).Kind);
            Assert.AreEqual(HandlerKind.Synchronous, (new Handlers.UserInterface()).Kind);
            Assert.AreEqual(HandlerKind.Asynchronous, (new Handlers.UserInterfaceAsync()).Kind);
        }

        /// <summary>
        /// Publisher without any restrictions.
        /// </summary>
        public class PublisherWithoutRestriction
        {
            /// <summary>Sample event</summary>
            [EventPublication("test")]
            public event EventHandler AnEvent;

            /// <summary>
            /// Fires the sample event.
            /// </summary>
            public void FireEvent()
            {
                this.AnEvent(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Publisher with synchronous restriction.
        /// </summary>
        public class PublisherWithSynchronousRestriction
        {
            /// <summary>Sample event</summary>
            [EventPublication("test", HandlerRestriction.Synchronous)]
            public event EventHandler AnEvent;

            /// <summary>
            /// Fires the sample event.
            /// </summary>
            public void FireEvent()
            {
                this.AnEvent(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Publisher with asynchronous restriction.
        /// </summary>
        public class PublisherWithAsynchronousRestriction
        {
            /// <summary>Sample event</summary>
            [EventPublication("test", HandlerRestriction.Asynchronous)]
            public event EventHandler AnEvent;

            /// <summary>
            /// Fires the sample event.
            /// </summary>
            public void FireEvent()
            {
                this.AnEvent(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Subscriber with a synchronous handler.
        /// </summary>
        public class SubscriberWithSynchronousHandler
        {
            /// <summary>
            /// Sample handler.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription("test", typeof(Handlers.Publisher))]
            public void Handler(object sender, EventArgs e)
            {
            }
        }

        /// <summary>
        /// Subscriber with an asynchronous handler.
        /// </summary>
        public class SubscriberWithAsynchronousHandler
        {
            /// <summary>
            /// Sample handler.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription("test", typeof(Handlers.Background))]
            public void Handler(object sender, EventArgs e)
            {
            }
        }
    }
}