//-------------------------------------------------------------------------------
// <copyright file="EventbrokerCountTest.cs" company="bbv Software Services AG">
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
    using NUnit.Framework;

    /// <summary>
    /// Tests scenarios with none or multiple publishers and subscribers.
    /// </summary>
    [TestFixture]
    public class EventBrokerCountTest
    {
        /// <summary>
        /// No subscriber present.
        /// </summary>
        [Test]
        public void PublisherWithoutSubscriber()
        {
            EventBroker eb = new EventBroker();

            Publisher p = new Publisher();
            Subscriber.Count = 0;

            eb.Register(p);

            p.CallCount();

            eb.Unregister(p);

            Assert.AreEqual(0, Subscriber.Count);
        }

        /// <summary>
        /// Multiple publishers and subscribers for the same event topic.
        /// </summary>
        [Test]
        public void MultiplePublisherMultipleSubscriber()
        {
            EventBroker eb = new EventBroker();
            Subscriber.Count = 0;

            Publisher p1 = new Publisher();
            Publisher p2 = new Publisher();

            Subscriber s1 = new Subscriber();
            Subscriber s2 = new Subscriber();
            Subscriber s3 = new Subscriber();

            eb.Register(p1);
            eb.Register(p2);
            eb.Register(s1);
            eb.Register(s2);
            eb.Register(s3);

            p1.CallCount();
            p2.CallCount();

            eb.Unregister(p1);
            eb.Unregister(p2);
            eb.Unregister(s1);
            eb.Unregister(s2);
            eb.Unregister(s3);

            Assert.AreEqual(6, Subscriber.Count);
        }
    }
}