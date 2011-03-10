//-------------------------------------------------------------------------------
// <copyright file="ReflectionMethodHandleProblem.cs" company="bbv Software Services AG">
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
    using Events;
    using NUnit.Framework;

    /// <summary>
    /// Tests that the reflection calls do not introduce additional ghost methods due to a defect in .net reflection.
    /// </summary>
    [TestFixture]
    public class ReflectionMethodHandleProblem
    {
        /// <summary>
        /// The object under test.
        /// </summary>
        private EventBroker testee;

        /// <summary>
        /// A test publisher with event publication.
        /// </summary>
        public interface ITestPublisher
        {
            /// <summary>
            /// Test event.
            /// </summary>
            [EventPublication(@"event://ITestPublisher/MyEvent")]
            event EventHandler<EventArgs<int>> MyEvent;

            /// <summary>
            /// Fires the event.
            /// </summary>
            void DoStuff();
        }

        /// <summary>
        /// A test subscriber with an event subscription.
        /// </summary>
        public interface ITestSubscriber
        {
            /// <summary>
            /// Gets the value received on the event.
            /// </summary>
            /// <value>value received on the event.</value>
            int MyValue { get; }

            /// <summary>
            /// Handles my event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The instance containing the event data.</param>
            [EventSubscription(@"event://ITestPublisher/MyEvent", typeof(Handlers.Publisher))]
            void HandleMyEvent(object sender, EventArgs<int> e);
        }

        /// <summary>
        /// No ghost methods are introduced through registration and handling events.
        /// </summary>
        [Test]
        public void NoGhostMethodsAreIntroduced()
        {
            this.testee = new EventBroker();

            ITestPublisher testPublisher = new MyPublisher();
            ITestSubscriber testSubscriber = new MySubscriber();

            int methodCount = testSubscriber.GetType().GetMethods().GetLength(0);

            this.testee.Register(testPublisher);

            Assert.AreEqual(methodCount, testSubscriber.GetType().GetMethods().GetLength(0), "Registration of publisher introduced ghost methods.");

            this.testee.Register(testSubscriber);

            Assert.AreEqual(methodCount, testSubscriber.GetType().GetMethods().GetLength(0), "Registration of subscriber introduced ghost methods.");

            testPublisher.DoStuff();

            Assert.AreEqual(methodCount, testSubscriber.GetType().GetMethods().GetLength(0), "Calling handler method introduced ghost methods.");
            Assert.AreEqual(6, testSubscriber.MyValue);
        }

        /// <summary>
        /// A test subscriber.
        /// </summary>
        public class MySubscriber : ITestSubscriber
        {
            /// <summary>
            /// Gets the value received on the event.
            /// </summary>
            /// <value>value received on the event.</value>
            public int MyValue
            {
                get;
                private set;
            }

            /// <summary>
            /// Handles my event.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The instance containing the event data.</param>
            public void HandleMyEvent(object sender, EventArgs<int> e)
            {
                this.MyValue = e.Value;
            }
        }

        /// <summary>
        /// A test publisher.
        /// </summary>
        public class MyPublisher : ITestPublisher
        {
            /// <summary>
            /// Test event.
            /// </summary>
            public event EventHandler<EventArgs<int>> MyEvent;

            /// <summary>
            /// Fires the event.
            /// </summary>
            public void DoStuff()
            {
                if (null != this.MyEvent)
                {
                    this.MyEvent(this, new EventArgs<int>(6));
                }
            }
        }
    }
}