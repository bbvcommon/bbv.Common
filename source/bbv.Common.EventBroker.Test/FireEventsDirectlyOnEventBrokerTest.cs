//-------------------------------------------------------------------------------
// <copyright file="FireEventsDirectlyOnEventBrokerTest.cs" company="bbv Software Services AG">
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

    using bbv.Common.EventBroker.Extensions;
    using bbv.Common.EventBroker.Internals;

    using FluentAssertions;

    using Xunit;

    using Assert = Xunit.Assert;

    /// <summary>
    /// Tests that event can be fired directly on the event broker.
    /// </summary>
    public class FireEventsDirectlyOnEventBrokerTest : EventBrokerExtensionBase
    {
        private readonly EventBroker testee;

        private IPublication interceptedPublication;

        /// <summary>
        /// Initializes a new instance of the <see cref="FireEventsDirectlyOnEventBrokerTest"/> class.
        /// </summary>
        public FireEventsDirectlyOnEventBrokerTest()
        {
            this.testee = new EventBroker();
            this.testee.AddExtension(this);
        }

        /// <summary>
        /// An event can be fired directly on the event broker without a registration of a publisher.
        /// </summary>
        [Fact]
        public void FireEvent()
        {
            Subscriber s = this.RegisterSubscriber();

            this.testee.Fire(EventTopics.SimpleEvent, this, HandlerRestriction.None, this, EventArgs.Empty);

            Assert.True(s.SimpleEventCalled, "event was not received.");
        }

        [Fact]
        public void FireEvent_PublicationMustContainEventArgumentsType()
        {
            this.testee.Fire(EventTopics.SimpleEvent, this, HandlerRestriction.None, this, EventArgs.Empty);

            this.interceptedPublication.Should().NotBeNull();
            Assert.Equal(typeof(EventArgs), this.interceptedPublication.EventArgsType);
        }

        /// <summary>
        /// An event can be fired directly on the event broker without a registration of a publisher.
        /// </summary>
        [Fact]
        public void FireCustomEvent()
        {
            Subscriber s = this.RegisterSubscriber();

            CustomEventArguments e = new CustomEventArguments("test");
            this.testee.Fire(EventTopics.CustomEventArgs, this, HandlerRestriction.None, this, e);

            s.ReceivedCustomEventArguments.Should().BeSameAs(e);
        }

        [Fact]
        public void FireCustomEvent_PublicationMustContainCustemEventArgumentsType()
        {
            CustomEventArguments e = new CustomEventArguments("test");
            this.testee.Fire(EventTopics.CustomEventArgs, this, HandlerRestriction.None, this, e);

            this.interceptedPublication.Should().NotBeNull();
            Assert.Equal(typeof(CustomEventArguments), this.interceptedPublication.EventArgsType);
        }

        public override void FiringEvent(IEventTopicInfo eventTopic, IPublication publication, object sender, EventArgs e)
        {
            base.FiringEvent(eventTopic, publication, sender, e);

            this.interceptedPublication = publication;
        }

        private Subscriber RegisterSubscriber()
        {
            Subscriber s = new Subscriber();
            this.testee.Register(s);
            return s;
        }
    }
}