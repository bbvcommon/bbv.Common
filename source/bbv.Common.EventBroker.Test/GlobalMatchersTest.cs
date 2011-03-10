//-------------------------------------------------------------------------------
// <copyright file="GlobalMatchersTest.cs" company="bbv Software Services AG">
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
    using System.IO;

    using bbv.Common.EventBroker.Internals;
    using bbv.Common.EventBroker.Matchers;

    using FluentAssertions;

    using NUnit.Framework;

    public class GlobalMatchersTest
    {
        private EventBroker testee;

        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();
        }

        [Test]
        public void GlobalPublicationMatcher()
        {
            var publicationMatcher = new TestPublicationMatcher();

            var publisher = new Publisher();
            var subscriber = new Subscriber();

            this.testee.Register(publisher);
            this.testee.Register(subscriber);

            this.testee.AddGlobalMatcher(publicationMatcher);

            publisher.CallSimpleEvent();

            subscriber.SimpleEventCalled.Should().BeFalse();
        }

        private class TestPublicationMatcher : IPublicationMatcher
        {
            public bool Match(IPublication publication, ISubscription subscription, EventArgs e)
            {
                return false;
            }

            public void DescribeTo(TextWriter writer)
            {
                writer.WriteLine("test global publication matcher");
            }
        }
    }
}