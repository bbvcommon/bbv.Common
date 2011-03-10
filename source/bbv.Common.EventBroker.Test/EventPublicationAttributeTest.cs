//-------------------------------------------------------------------------------
// <copyright file="EventPublicationAttributeTest.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;
    using NUnit.Framework;

    /// <summary>
    /// Tests on the <see cref="EventPublicationAttribute"/> class.
    /// </summary>
    [TestFixture]
    public class EventPublicationAttributeTest
    {
        /// <summary>
        /// A topic for testing.
        /// </summary>
        private const string Topic = "topic";

        /// <summary>
        /// A publication with a topic can be created.
        /// </summary>
        [Test]
        public void CreationWithTopic()
        {
            EventPublicationAttribute testee = new EventPublicationAttribute(Topic);

            Assert.AreEqual(Topic, testee.Topic);
            Assert.AreEqual(0, new List<Type>(testee.MatcherTypes).Count);
            Assert.AreEqual(HandlerRestriction.None, testee.HandlerRestriction);
        }

        /// <summary>
        /// A publication with a topic and handler restriction can be created.
        /// </summary>
        [Test]
        public void CreationWithTopicAndHandlerRestriction()
        {
            EventPublicationAttribute testee = new EventPublicationAttribute(Topic, HandlerRestriction.Synchronous);

            Assert.AreEqual(Topic, testee.Topic);
            Assert.AreEqual(0, new List<Type>(testee.MatcherTypes).Count);
            Assert.AreEqual(HandlerRestriction.Synchronous, testee.HandlerRestriction);
        }

        /// <summary>
        /// A publication with a topic can be created.
        /// </summary>
        [Test]
        public void CreationWithTopicAndMatcherTypes()
        {
            EventPublicationAttribute testee = new EventPublicationAttribute(Topic, typeof(int), typeof(string));

            Assert.AreEqual(Topic, testee.Topic);
            Assert.AreEqual(2, new List<Type>(testee.MatcherTypes).Count);
            Assert.AreEqual(HandlerRestriction.None, testee.HandlerRestriction);
        }

        // TODO: check that all constructors result in correct values when accessed through properties (specially the matcher types)
    }
}