//-------------------------------------------------------------------------------
// <copyright file="DefaultTopicConventionTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.MappingEventBroker.Conventions
{
    using bbv.Common.EventBroker.Internals;

    using Moq;

    using Xunit;

    public class DefaultTopicConventionTest
    {
        private readonly DefaultTopicConvention testee;

        public DefaultTopicConventionTest()
        {
            this.testee = new DefaultTopicConvention();
        }

        [Fact]
        public void IsCandidate_WhenTopicEqualsDefaultInput_MustReturnTrue()
        {
            var eventTopic = new Mock<IEventTopic>();
            eventTopic.SetupGet(t => t.Uri).Returns(DefaultTopicConvention.EventTopicUriInput);

            Assert.True(this.testee.IsCandidate(eventTopic.Object));
        }

        [Fact]
        public void IsCandidate_WhenTopicEqualsDefaultOutput_MustReturnFalse()
        {
            var eventTopic = new Mock<IEventTopicInfo>();
            eventTopic.SetupGet(t => t.Uri).Returns(DefaultTopicConvention.EventTopicUriOutput);

            Assert.False(this.testee.IsCandidate(eventTopic.Object));
        }

        [Fact]
        public void MapTopic_MustRewriteTopic()
        {
            var uri = @"topic://bbv.Common.AutoMapperEventBrokerExtension.Conventions/DefaultTopicConventionTest";
            var expected = @"mapped://bbv.Common.AutoMapperEventBrokerExtension.Conventions/DefaultTopicConventionTest";

            var result = this.testee.MapTopic(uri);

            Assert.Equal(expected, result);
        }
    }
}