//-------------------------------------------------------------------------------
// <copyright file="EventTopicCollectionTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.MappingEventBroker
{
    using bbv.Common.EventBroker.Internals;

    using Moq;

    using Xunit;

    public class EventTopicCollectionTest
    {
        private EventTopicCollection testee;

        public EventTopicCollectionTest()
        {
            this.testee = new EventTopicCollection();
        }

        [Fact]
        public void ItemsMustBeKeyedAfterUri()
        {
            const string Topic = "Topic";

            var eventTopic = new Mock<IEventTopicInfo>();
            eventTopic.SetupGet(t => t.Uri).Returns(Topic);

            this.testee.Add(eventTopic.Object);

            var result = this.testee[Topic];

            Assert.Same(eventTopic.Object, result);
        }
    }
}