﻿//-------------------------------------------------------------------------------
// <copyright file="MassTransitEventBrokerBusTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.DistributedEventBroker.MassTransitAdapter
{
    using System;

    using bbv.Common.DistributedEventBroker.Messages;

    using MassTransit;

    using Moq;

    using Xunit;

    public class MassTransitEventBrokerBusTest
    {
        private readonly Mock<IServiceBus> serviceBus;

        private readonly MassTransitEventBrokerBus testee;

        public MassTransitEventBrokerBusTest()
        {
            this.serviceBus = new Mock<IServiceBus>();

            this.testee = new MassTransitEventBrokerBus(this.serviceBus.Object);
        }

        [Fact]
        public void Publish_MustPublishMessageOnBus()
        {
            var message = new Mock<IEventFired>();

            this.testee.Publish(message.Object);

            this.serviceBus.Verify(bus => bus.Publish(message.Object, It.IsAny<Action<IPublishContext<IEventFired>>>()));
        }
    }
}
