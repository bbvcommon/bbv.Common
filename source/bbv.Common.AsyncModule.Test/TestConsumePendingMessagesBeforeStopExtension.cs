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
// File:        TestConsumePendingMessagesBeforeStopExtension.cs
// Author:      Adrian Krummenacher / AK
//
// History:
// --------
// Date          Name   Version   Description
// 11-JAN-2006   AK		1.0       Creation     
//
/***************************************************************************/

using System;
using NUnit.Framework;
using NMock2;
using NMock2.Matchers;

namespace bbv.Common.AsyncModule
{
    [TestFixture]
    public class TestConsumePendingMessagesBeforeStopExtension
    {
        /// <summary>
        /// Mock factory.
        /// </summary>
        private Mockery m_mockery;

        /// <summary>
        /// Mock.
        /// </summary>
        private MockModuleController m_moduleController;

        /// <summary>
        /// Mock.
        /// </summary>
        private IModuleCoordinator m_moduleCoordinator;

        [SetUp]
        public void SetUp()
        {
            m_mockery = new Mockery();
            m_moduleController = new MockModuleController();
            m_moduleCoordinator = m_mockery.NewMock<IModuleCoordinator>();
        }

        [TearDown]
        public void TearDonw()
        {
            m_moduleController.ConsumeMessagesTimer.Stop();
        }

        [Test]
        public void EmptyMessageQueueOnStop()
        {
            ConsumePendingMessagesBeforeStopExtension extension =
                new ConsumePendingMessagesBeforeStopExtension(1000, 400);

            extension.ModuleController = m_moduleController;

            extension.Attach();

            // Stopping.
            DateTime beforeCallDateTime = DateTime.Now;
            m_moduleController.RaiseBeforeModuleStopEvent();
            DateTime afterCallDateTime = DateTime.Now;

            TimeSpan callTimeSpan = afterCallDateTime - beforeCallDateTime;
            Assert.IsTrue((callTimeSpan.TotalMilliseconds < 100));

            m_moduleController.RaiseAfterModuleStopEvent();

            extension.Detach();
        }

        [Test]
        public void StopAfterMaxWaitTime()
        {
            ConsumePendingMessagesBeforeStopExtension extension =
                new ConsumePendingMessagesBeforeStopExtension(1000, 10);

            extension.ModuleController = m_moduleController;

            extension.Attach();

            // Not stopping.
            bool cancel;
            m_moduleController.RaiseBeforeEnqueueMessageEvent("Test", 77, out cancel);
            Assert.IsFalse(cancel);

            m_moduleController.EnqueueMessage(77);
            Assert.AreEqual(1, m_moduleController.EnquedMessages.Count);

            // Stopping.
            DateTime beforeCallDateTime = DateTime.Now;
            m_moduleController.RaiseBeforeModuleStopEvent();
            DateTime afterCallDateTime = DateTime.Now;

            TimeSpan callTimeSpan = afterCallDateTime - beforeCallDateTime;
            Assert.IsTrue((callTimeSpan.TotalMilliseconds > 800) && (callTimeSpan.TotalMilliseconds < 1200));

            m_moduleController.RaiseBeforeEnqueueMessageEvent("Test", 88, out cancel);
            Assert.IsTrue(cancel);
            m_moduleController.RaiseAfterModuleStopEvent();

            extension.Detach();
        }

        [Test]
        public void StopAfterFirstInterval()
        {
            ConsumePendingMessagesBeforeStopExtension extension =
                new ConsumePendingMessagesBeforeStopExtension(1000, 400);

            extension.ModuleController = m_moduleController;

            extension.Attach();

            // Not stopping.
            bool cancel;
            m_moduleController.RaiseBeforeEnqueueMessageEvent("Test", 77, out cancel);
            Assert.IsFalse(cancel);

            m_moduleController.EnqueueMessage(77);
            Assert.AreEqual(1, m_moduleController.EnquedMessages.Count);

            m_moduleController.ConsumeMessagesTimer.AutoReset = false;
            m_moduleController.ConsumeMessagesTimer.Interval = 300;
            m_moduleController.ConsumeMessagesTimer.Start();

            // Stopping.
            DateTime beforeCallDateTime = DateTime.Now;
            m_moduleController.RaiseBeforeModuleStopEvent();
            DateTime afterCallDateTime = DateTime.Now;

            TimeSpan callTimeSpan = afterCallDateTime - beforeCallDateTime;
            Assert.IsTrue((callTimeSpan.TotalMilliseconds > 300) && (callTimeSpan.TotalMilliseconds < 500));
            Assert.AreEqual(0, m_moduleController.EnquedMessages.Count);
            
            m_moduleController.RaiseBeforeEnqueueMessageEvent("Test", 88, out cancel);
            Assert.IsTrue(cancel);
            m_moduleController.RaiseAfterModuleStopEvent();

            extension.Detach();
        }
    }
}
