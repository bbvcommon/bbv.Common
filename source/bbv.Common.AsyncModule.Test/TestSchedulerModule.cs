/***************************************************************************/
// Copyright 2007 bbv Software Services AG
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
/***************************************************************************/
// Project:     bbv.Common.AsyncModule
// ------------------------------------------------
// File:        TestSchedulerModule.cs
// Author:      Adrian Krummenacher / AK
//
// History:
// --------
// Date          Name   Version   Description
// 23-AUG-2006   AK		1.0       Creation     
//
/***************************************************************************/

using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

namespace bbv.Common.AsyncModule
{
    /// <summary>
    /// Mock.
    /// </summary>
    public class MockScheduledMessageReceiver
    {
        public List<string> ConsumedMessages = new List<string>();

        [MessageConsumer]
        public void ConsumeMessage(string message)
        {
            ConsumedMessages.Add(message);
        }
    }
    
    [TestFixture]
    public class TestSchedulerModule
    {
        /// <summary>
        /// Mock of the camera manager
        /// </summary>
        protected MockScheduledMessageReceiver m_scheduledMessageReceiver;
        
        /// <summary>
        /// The module coordinator.
        /// </summary>
        protected ModuleCoordinator m_moduleCoordinator;

        /// <summary>
        /// Prepares the mocks
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            m_scheduledMessageReceiver = new MockScheduledMessageReceiver();
            m_moduleCoordinator = new ModuleCoordinator();
        }

        /// <summary>
        /// Sends a scheduled message with a relative time in ms.
        /// </summary>
        [Test]
        public void SendScheduledMessageWithRelativeTime()
        {
            m_moduleCoordinator.AddModule("ScheduledMessageReceiver", m_scheduledMessageReceiver);
            m_moduleCoordinator.AddModule("Scheduler", new SchedulerModule());
            m_moduleCoordinator.AddExtension<TimedTriggerExtension>("Scheduler", new TimedTriggerExtension());
            m_moduleCoordinator.StartAll();

            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage1", 100));
            Assert.AreEqual(0, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Thread.Sleep(200);
            Assert.AreEqual(1, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Assert.AreEqual("TestMessage1", m_scheduledMessageReceiver.ConsumedMessages[0]);

            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage2", 200));
            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage3", 100));
            Thread.Sleep(150);
            Assert.AreEqual(2, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Assert.AreEqual("TestMessage3", m_scheduledMessageReceiver.ConsumedMessages[1]);
            Thread.Sleep(100);
            Assert.AreEqual(3, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Assert.AreEqual("TestMessage2", m_scheduledMessageReceiver.ConsumedMessages[2]);

            m_moduleCoordinator.StopAll();
        }

        /// <summary>
        /// Sends a scheduled message with a absulte time like '2006-02-04 11:34:23:000'
        /// </summary>
        [Test]
        public void SendScheduledMessageWithAbsoluteTime()
        {
            m_moduleCoordinator.AddModule("ScheduledMessageReceiver", m_scheduledMessageReceiver);
            m_moduleCoordinator.AddModule("Scheduler", new SchedulerModule());
            m_moduleCoordinator.AddExtension<TimedTriggerExtension>("Scheduler", new TimedTriggerExtension());
            m_moduleCoordinator.StartAll();

            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage1", DateTime.Now.AddMilliseconds(100)));
            Assert.AreEqual(0, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Thread.Sleep(200);
            Assert.AreEqual(1, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Assert.AreEqual("TestMessage1", m_scheduledMessageReceiver.ConsumedMessages[0]);

            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage2", DateTime.Now.AddMilliseconds(200)));
            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage3", DateTime.Now.AddMilliseconds(100)));
            Thread.Sleep(150);
            Assert.AreEqual(2, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Assert.AreEqual("TestMessage3", m_scheduledMessageReceiver.ConsumedMessages[1]);
            Thread.Sleep(100);
            Assert.AreEqual(3, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Assert.AreEqual("TestMessage2", m_scheduledMessageReceiver.ConsumedMessages[2]);

            m_moduleCoordinator.StopAll();
        }

        /// <summary>
        /// Sends a message, which is too late and has to be scheduled immediately.
        /// </summary>
        [Test]
        public void SendScheduledMessageWhichIsToLate()
        {
            m_moduleCoordinator.AddModule("ScheduledMessageReceiver", m_scheduledMessageReceiver);
            m_moduleCoordinator.AddModule("Scheduler", new SchedulerModule());
            m_moduleCoordinator.AddExtension<TimedTriggerExtension>("Scheduler", new TimedTriggerExtension());
            m_moduleCoordinator.StartAll();

            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage1", DateTime.Now.AddMilliseconds(-100)));
            Thread.Sleep(20);
            Assert.AreEqual(1, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Assert.AreEqual("TestMessage1", m_scheduledMessageReceiver.ConsumedMessages[0]);

            m_moduleCoordinator.StopAll();
        }

        /// <summary>
        /// Use the timed trigger in a way, so that the scheduled messages are still
        /// scheduled on the same time (approximately).
        /// </summary>
        [Test]
        public void RescheduleOnRestart()
        {
            m_moduleCoordinator.AddModule("ScheduledMessageReceiver", m_scheduledMessageReceiver);
            m_moduleCoordinator.AddModule("Scheduler", new SchedulerModule());
            m_moduleCoordinator.AddExtension<TimedTriggerExtension>("Scheduler", new TimedTriggerExtension(false));
            m_moduleCoordinator.StartAll();

            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage1", DateTime.Now.AddMilliseconds(300)));
            Thread.Sleep(50);
            Assert.AreEqual(0, m_scheduledMessageReceiver.ConsumedMessages.Count);
            m_moduleCoordinator.StopAll();
            Thread.Sleep(50);
            m_moduleCoordinator.StartAll();
            Thread.Sleep(50);
            Assert.AreEqual(0, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Thread.Sleep(160);
            Assert.AreEqual(1, m_scheduledMessageReceiver.ConsumedMessages.Count);
            Assert.AreEqual("TestMessage1", m_scheduledMessageReceiver.ConsumedMessages[0]);

            m_moduleCoordinator.StopAll();
        }

        /// <summary>
        /// Bugfix test. More than one ScheduledMessage with an identical DueTime
        /// should not throw an exception.
        /// </summary>
        [Test]
        public void IdenticalDueTime()
        {
            m_moduleCoordinator.AddModule("ScheduledMessageReceiver", m_scheduledMessageReceiver);
            m_moduleCoordinator.AddModule("Scheduler", new SchedulerModule());
            m_moduleCoordinator.AddExtension<TimedTriggerExtension>("Scheduler", new TimedTriggerExtension(false));
            m_moduleCoordinator.StartAll();

            DateTime deliveryDate = DateTime.Now.AddMilliseconds(300);

            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage1", deliveryDate));
            m_moduleCoordinator.PostMessage("Scheduler",
                new ScheduledMessage("ScheduledMessageReceiver", "TestMessage2", deliveryDate));

            m_moduleCoordinator.StopAll();
        }
    }
}
