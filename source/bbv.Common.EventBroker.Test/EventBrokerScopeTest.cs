//-------------------------------------------------------------------------------
// <copyright file="EventBrokerScopeTest.cs" company="bbv Software Services AG">
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
    using System.Diagnostics.CodeAnalysis;

    using Matchers;

    using NUnit.Framework;

    /// <summary>
    /// Tests the scope feature of the event broker.
    /// </summary>
    [TestFixture]
    public class EventBrokerScopeTest
    {
        /// <summary>
        /// The testee.
        /// </summary>
        private EventBroker testee;

        /// <summary>A named publisher</summary>
        private NamedPublisher publisher;

        /// <summary>a subscriber that is a parent to the publisher.</summary>
        private NamedSubscriber subscriberParent;

        /// <summary>a subscriber that is a twin to the publisher.</summary>
        private NamedSubscriber subscriberTwin;

        /// <summary>a subscriber that is a sibling to the publisher.</summary>
        private NamedSubscriber subscriberSibling;

        /// <summary>a subscriber that is a child to the publisher.</summary>
        private NamedSubscriber subscriberChild;

        /// <summary>
        /// Set up publisher and subscribers.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();

            this.publisher = new NamedPublisher("Test.One");
            this.subscriberParent = new NamedSubscriber("Test");
            this.subscriberTwin = new NamedSubscriber("Test.One");
            this.subscriberSibling = new NamedSubscriber("Test.Two");
            this.subscriberChild = new NamedSubscriber("Test.One.Child");

            this.testee.Register(this.publisher);
            this.testee.Register(this.subscriberParent);
            this.testee.Register(this.subscriberTwin);
            this.testee.Register(this.subscriberSibling);
            this.testee.Register(this.subscriberChild);
        }

        /// <summary>
        /// Test global publications.
        /// </summary>
        [Test]
        public void GlobalPublicationScope()
        {
            this.publisher.CallGlobalEvent();

            Assert.IsTrue(
                this.subscriberParent.CalledGlobally,
                "global publication from child should be handled by global subscription.");
            Assert.IsTrue(
                this.subscriberParent.CalledFromChild,
                "global publication from child should be handled by subscriptions to Children.");
            Assert.IsFalse(
                this.subscriberParent.CalledFromParent,
                "global publications from child should not be handled by subscriptions to parents.");

            Assert.IsTrue(
                this.subscriberTwin.CalledGlobally,
                "global publication from twin should be handled by global subscription.");
            Assert.IsTrue(
                this.subscriberTwin.CalledFromChild,
                "global publication from twin should be handled by subscriptions to Children.");
            Assert.IsTrue(
                this.subscriberTwin.CalledFromParent,
                "global publications from twin should be handled by subscriptions to parents.");

            Assert.IsTrue(
                this.subscriberSibling.CalledGlobally,
                "global publication from sibling should be handled by global subscription.");
            Assert.IsFalse(
                this.subscriberSibling.CalledFromChild,
                "global publication from sibling should not be handled by subscriptions to Children.");
            Assert.IsFalse(
                this.subscriberSibling.CalledFromParent,
                "global publications from sibling should not be handled by subscriptions to parents.");

            Assert.IsTrue(
                this.subscriberChild.CalledGlobally,
                "global publication from parent should be handled by global subscription.");
            Assert.IsFalse(
                this.subscriberChild.CalledFromChild,
                "global publication from parent should not be handled by subscriptions to Children.");
            Assert.IsTrue(
                this.subscriberChild.CalledFromParent,
                "global publications from parent should be handled by subscriptions to parents.");
        }

        /// <summary>
        /// Tests publications to parents.
        /// </summary>
        [Test]
        public void ParentPublicationScope()
        {
            this.publisher.CallParentEvent();

            Assert.IsTrue(
                this.subscriberParent.CalledGlobally,
                "parent publication from child should be handled by global subscription.");
            Assert.IsTrue(
                this.subscriberParent.CalledFromChild,
                "parent publication from child should be handled by subscriptions to Children.");
            Assert.IsFalse(
                this.subscriberParent.CalledFromParent,
                "parent publications from child should not be handled by subscriptions to parents.");

            Assert.IsTrue(
                this.subscriberTwin.CalledGlobally,
                "parent publication from twin should be handled by global subscription.");
            Assert.IsTrue(
                this.subscriberTwin.CalledFromChild,
                "parent publication from twin should be handled by subscriptions to Children.");
            Assert.IsTrue(
                this.subscriberTwin.CalledFromParent,
                "parent publications from twin should be handled by subscriptions to parents.");

            Assert.IsFalse(
                this.subscriberSibling.CalledGlobally,
                "parent publication from sibling should not be handled by global subscription.");
            Assert.IsFalse(
                this.subscriberSibling.CalledFromChild,
                "parent publication from sibling should not be handled by subscriptions to Children.");
            Assert.IsFalse(
                this.subscriberSibling.CalledFromParent,
                "parent publications from sibling should not be handled by subscriptions to parents.");

            Assert.IsFalse(
                this.subscriberChild.CalledGlobally,
                "parent publication from parent should not be handled by global subscription.");
            Assert.IsFalse(
                this.subscriberChild.CalledFromChild,
                "parent publication from parent should not be handled by subscriptions to Children.");
            Assert.IsFalse(
                this.subscriberChild.CalledFromParent,
                "parent publications from parent should not be handled by subscriptions to parents.");
        }

        /// <summary>
        /// Tests publications to children.
        /// </summary>
        [Test]
        public void ChildPublicationScope()
        {
            this.publisher.CallChildrenEvent();

            Assert.IsFalse(
                this.subscriberParent.CalledGlobally,
                "child publication from child should not be handled by global subscription.");
            Assert.IsFalse(
                this.subscriberParent.CalledFromChild,
                "child publication from child should not be handled by subscriptions to Children.");
            Assert.IsFalse(
                this.subscriberParent.CalledFromParent,
                "child publications from child should not be handled by subscriptions to parents.");

            Assert.IsTrue(
                this.subscriberTwin.CalledGlobally,
                "child publication from twin should be handled by global subscription.");
            Assert.IsTrue(
                this.subscriberTwin.CalledFromChild,
                "child publication from twin should be handled by subscriptions to Children.");
            Assert.IsTrue(
                this.subscriberTwin.CalledFromParent,
                "child publications from twin should be handled by subscriptions to parents.");

            Assert.IsFalse(
                this.subscriberSibling.CalledGlobally,
                "child publication from sibling should not be handled by global subscription.");
            Assert.IsFalse(
                this.subscriberSibling.CalledFromChild,
                "child publication from sibling should not be handled by subscriptions to Children.");
            Assert.IsFalse(
                this.subscriberSibling.CalledFromParent,
                "child publications from sibling should not be handled by subscriptions to parents.");

            Assert.IsTrue(
                this.subscriberChild.CalledGlobally,
                "child publication from parent should be handled by global subscription.");
            Assert.IsFalse(
                this.subscriberChild.CalledFromChild,
                "child publication from parent should not be handled by subscriptions to Children.");
            Assert.IsTrue(
                this.subscriberChild.CalledFromParent,
                "child publications from parent should be handled by subscriptions to parents.");
        }

        /// <summary>
        /// Helper class for defining event topic constant,
        /// </summary>
        private class Topics
        {
            /// <summary>
            /// The URI f the event topic.
            /// </summary>
            public const string EventTopic = "topic://EventTopic";
        }

        /// <summary>
        /// A named publisher.
        /// </summary>
        private class NamedPublisher : INamedItem
        {
            /// <summary>
            /// The name of the publisher.
            /// </summary>
            private readonly string name;

            /// <summary>
            /// Initializes a new instance of the <see cref="EventBrokerScopeTest.NamedPublisher"/> class.
            /// </summary>
            /// <param name="name">The name of the publisher.</param>
            public NamedPublisher(string name)
            {
                this.name = name;
            }

            /// <summary>
            /// A globally published event.
            /// </summary>
            [EventPublication(Topics.EventTopic)]
            public event EventHandler GlobalEvent;

            /// <summary>
            /// An event that is fired to parents and siblings only.
            /// </summary>
            [EventPublication(Topics.EventTopic, typeof(PublishToParents))]
            public event EventHandler ParentEvent;

            /// <summary>
            /// An event that is fired to children and siblings only.
            /// </summary>
            [EventPublication(Topics.EventTopic, typeof(PublishToChildren))]
            public event EventHandler ChildrenEvent;

            /// <summary>
            /// Gets the name of the event broker item that is used for scope matchers.
            /// </summary>
            /// <value>The name of the event broker item.</value>
            public string EventBrokerItemName
            {
                get
                {
                    return this.name;
                }
            }

            /// <summary>
            /// Calls the global event.
            /// </summary>
            public void CallGlobalEvent()
            {
                this.GlobalEvent(this, EventArgs.Empty);
            }

            /// <summary>
            /// Calls the parent event.
            /// </summary>
            public void CallParentEvent()
            {
                this.ParentEvent(this, EventArgs.Empty);
            }

            /// <summary>
            /// Calls the children event.
            /// </summary>
            public void CallChildrenEvent()
            {
                this.ChildrenEvent(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// A named subscriber.
        /// </summary>
        private class NamedSubscriber : INamedItem
        {
            /// <summary>
            /// Name of the subscriber.
            /// </summary>
            private readonly string name;

            /// <summary>
            /// Initializes a new instance of the <see cref="EventBrokerScopeTest.NamedSubscriber"/> class.
            /// </summary>
            /// <param name="name">The name of the subscriber.</param>
            public NamedSubscriber(string name)
            {
                this.name = name;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the global subscription was called.
            /// </summary>
            public bool CalledGlobally { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the subscription to parents was called.
            /// </summary>
            public bool CalledFromParent { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the subscription to children was called.
            /// </summary>
            public bool CalledFromChild { get; set; }

            /// <summary>
            /// Gets the name of the event broker item that is used for scope matchers.
            /// </summary>
            /// <value>The name of the event broker item.</value>
            public string EventBrokerItemName
            {
                get
                {
                    return this.name;
                }
            }

            /// <summary>
            /// global handler
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription(Topics.EventTopic, typeof(Handlers.Publisher))]
            public void GlobalHandler(object sender, EventArgs e)
            {
                this.CalledGlobally = true;
            }

            /// <summary>
            /// handler that listens only to parents and siblings.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription(Topics.EventTopic, typeof(Handlers.Publisher), typeof(SubscribeToParents))]
            public void ParentHandler(object sender, EventArgs e)
            {
                this.CalledFromParent = true;
            }

            /// <summary>
            /// handler that listens only to children.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            [EventSubscription(Topics.EventTopic, typeof(Handlers.Publisher), typeof(SubscribeToChildren))]
            public void ChildrenHandler(object sender, EventArgs e)
            {
                this.CalledFromChild = true;
            }
        }
    }
}