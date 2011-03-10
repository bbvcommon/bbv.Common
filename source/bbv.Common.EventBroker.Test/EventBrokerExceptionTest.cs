//-------------------------------------------------------------------------------
// <copyright file="EventBrokerExceptionTest.cs" company="bbv Software Services AG">
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
    /// Tests for exception behavior.
    /// </summary>
    [TestFixture]
    public class EventBrokerExceptionTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private EventBroker testee;

        /// <summary>
        /// Set up of tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();
        }

        #region Multiple Publications

        /// <summary>
        /// A publisher cannot define the same publication twice.
        /// </summary>
        [Test]
        public void MultiplePublication()
        {
            InvalidPublisherRepeatedEventPublication p = new InvalidPublisherRepeatedEventPublication();

            Assert.Throws<RepeatedPublicationException>(
                () => this.testee.Register(p));
        }

        #endregion

        #region Wrong Signature

        /// <summary>
        /// Publisher has to use correct signature for events.
        /// </summary>
        [Test]
        public void WrongEventSignature()
        {
            InvalidPublisherWrongEventSignature p = new InvalidPublisherWrongEventSignature();

            Assert.Throws<InvalidPublicationSignatureException>(
                () => this.testee.Register(p));
        }

        /// <summary>
        /// Subscriber has to use correct signature.
        /// </summary>
        [Test]
        public void WrongSubscriptionSignature()
        {
            InvalidSubscriberWrongSignature s = new InvalidSubscriberWrongSignature();

            Assert.Throws<InvalidSubscriptionSignatureException>(
                () => this.testee.Register(s));
        }

        #endregion

        #region Exception Handling

        /// <summary>
        /// When a subscriber throws an error during handling of event then this exception is packed into
        /// an <see cref="EventTopicException"/>.
        /// </summary>
        [Test]
        public void ExceptionHandling()
        {
            try
            {
                Publisher p = new Publisher();
                SubscriberThrowingException s = new SubscriberThrowingException();

                this.testee.Register(p);
                this.testee.Register(s);

                p.CallSimpleEvent();

                Assert.Fail("must not be reached.");
            }
            catch (EventTopicException e)
            {
                Assert.IsTrue(e.InnerException is SubscriberThrowingException.TestException);
            }
        }

        /// <summary>
        /// When several subscribers throw exceptions during handling of event then all these exceptions
        /// are packed together into a <see cref="EventTopicException"/>.
        /// </summary>
        [Test]
        public void ExceptionHandlingMultipleException()
        {
            Publisher p = new Publisher();
            SubscriberThrowingException s1 = new SubscriberThrowingException();
            SubscriberThrowingException s2 = new SubscriberThrowingException();

            this.testee.Register(p);
            this.testee.Register(s1);
            this.testee.Register(s2);

            Assert.Throws<EventTopicException>(
                () => p.CallSimpleEvent());
        }

        #endregion

        #region Static Event / Handler

        /// <summary>
        /// Static event must not be published.
        /// </summary>
        [Test]
        public void StaticEvent()
        {
            InvalidPublisherStaticEvent p = new InvalidPublisherStaticEvent();

            Assert.Throws<StaticPublisherEventException>(
                () => this.testee.Register(p));
        }

        /// <summary>
        /// Static method must not be subscribers.
        /// </summary>
        [Test]
        public void StaticHandler()
        {
            InvalidSubscriberStaticHandler s = new InvalidSubscriberStaticHandler();

            Assert.Throws<StaticSubscriberHandlerException>(
                () => this.testee.Register(s));
        }

        #endregion

        #region Not User Interface Thread

        /// <summary>
        /// Subscribing with an user interface handler is not allowed outside the user interface thread.
        /// </summary>
        [Test]
        public void NotInterfaceThread()
        {
            Assert.Throws<NotUserInterfaceThreadException>(
                () => this.testee.Register(new UserInterfaceSubscriber()));
        }

        /// <summary>
        /// For unit tests it is possible to replace the user interface handlers with publisher handlers.
        /// This allows unit testing of such subscriptions.
        /// </summary>
        [Test]
        public void CheckOnUserInterfaceThreadCanBeSwitchedOff()
        {
            this.testee = new EventBroker(new UnitTestFactory());
            this.testee.Register(new UserInterfaceSubscriber());
        }

        #endregion

        #region Publisher and Subscribers

        /// <summary>
        /// A publisher with wrong event signature.
        /// </summary>
        public class InvalidPublisherWrongEventSignature
        {
            /// <summary>
            /// Delegate for event.
            /// </summary>
            /// <param name="name">A name for testing purposes.</param>
            /// <returns>just for testing.</returns>
            public delegate int MyEventHandler(string name);

            /// <summary>
            /// An event with the wrong signature.
            /// </summary>
            [EventPublication(EventTopics.SimpleEvent)]
            public event MyEventHandler SimpleEvent;

            /// <summary>
            /// Invokes the simple event.
            /// </summary>
            /// <returns>The result.</returns>
            protected int OnSimpleEvent()
            {
                MyEventHandler handler = this.SimpleEvent;
                if (handler != null)
                {
                    handler(string.Empty);
                }

                return -1;
            }
        }

        /// <summary>
        /// Publisher that declares twice the same publication.
        /// </summary>
        public class InvalidPublisherRepeatedEventPublication
        {
            /// <summary>
            /// Event with twice the same publication.
            /// </summary>
            [EventPublication(EventTopics.SimpleEvent)]
            [EventPublication(EventTopics.SimpleEvent)]
            public event EventHandler SimpleEvent1;

            /// <summary>
            /// Calls <see cref="SimpleEvent1"/>.
            /// </summary>
            public void CallSimpleEvent1()
            {
                this.SimpleEvent1(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Publisher with a private event.
        /// </summary>
        public class InvalidPublisherPrivateEvent
        {
            /// <summary>
            /// A private event with a publication.
            /// </summary>
            [EventPublication(EventTopics.SimpleEvent)]
            private event EventHandler SimpleEvent;

            /// <summary>
            /// Invokes the simple event.
            /// </summary>
            /// <param name="e">The event arguments</param>
            protected void OnSimpleEvent(EventArgs e)
            {
                EventHandler handler = this.SimpleEvent;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        /// <summary>
        /// Publisher with a static event.
        /// </summary>
        public class InvalidPublisherStaticEvent
        {
            /// <summary>
            /// A static event with a publication.
            /// </summary>
            [EventPublication(EventTopics.SimpleEvent)]
            public static event EventHandler SimpleEvent;

            /// <summary>
            /// Invokes the simple event.
            /// </summary>
            /// <param name="e">The event arguments</param>
            protected static void OnSimpleEvent(EventArgs e)
            {
                EventHandler handler = SimpleEvent;
                if (handler != null)
                {
                    handler(null, e);
                }
            }
        }

        /// <summary>
        /// Subscriber with wrong handler method signature.
        /// </summary>
        public class InvalidSubscriberWrongSignature
        {
            /// <summary>
            /// Handler method with wrong signature.
            /// </summary>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription(EventTopics.SimpleEvent, typeof(Handlers.Publisher))]
            public void SimpleEvent(EventArgs e)
            {
            }
        }

        /// <summary>
        /// Subscriber with custom event arguments that do not match with published event arguments.
        /// </summary>
        public class InvalidSubscriberWrongSignatureWrongEventArgs
        {
            /// <summary>
            /// A subscription method with custom event arguments.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The event arguments.</param>
            [EventSubscription(EventTopics.SimpleEvent, typeof(Handlers.Publisher))]
            public void SimpleEvent(object sender, CustomEventArguments e)
            {
            }
        }

        /// <summary>
        /// Subscriber throwing exception.
        /// </summary>
        public class SubscriberThrowingException
        {
            /// <summary>
            /// Handler method that throws an exception.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription(EventTopics.SimpleEvent, typeof(Handlers.Publisher))]
            public void SimpleEvent(object sender, EventArgs e)
            {
                throw new TestException();
            }

            /// <summary>
            /// A sample exception.
            /// </summary>
            public class TestException : Exception
            {
            }
        }

        /// <summary>
        /// Subscriber that throws exception on background thread.
        /// </summary>
        public class SubscriberThrowingExceptionBackgroundWorker
        {
            /// <summary>
            /// Handler method (background) that throws an exception.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription(EventTopics.SimpleEvent, typeof(Handlers.Background))]
            public void SimpleEvent(object sender, EventArgs e)
            {
                throw new TestException();
            }

            /// <summary>
            /// A sample exception.
            /// </summary>
            public class TestException : Exception
            {
            }
        }

        /// <summary>
        /// Subscriber with static handler method.
        /// </summary>
        public class InvalidSubscriberStaticHandler
        {
            /// <summary>
            /// Static handler method.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription(EventTopics.SimpleEvent, typeof(Handlers.Publisher))]
            public static void SimpleEvent(object sender, EventArgs e)
            {
            }
        }

        /// <summary>
        /// Subscriber with handler method on user interface thread.
        /// </summary>
        public class UserInterfaceSubscriber
        {
            /// <summary>
            /// Handler method on user interface.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription(EventTopics.SimpleEvent, typeof(Handlers.UserInterface))]
            public void SimpleEvent(object sender, EventArgs e)
            {
            }
        }

        #endregion
    }
}