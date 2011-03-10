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
// File:        TestTimedTriggerExtension.cs
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
using System.Threading;

namespace bbv.Common.AsyncModule
{
    [TestFixture]
    public class TestTimedTriggerExtension
    {
        /// <summary>
        /// Mock.
        /// </summary>
        private MockModuleController m_moduleController;

        [SetUp]
        public void SetUp()
        {
            m_moduleController = new MockModuleController();
        }

        /// <summary>
        /// Tests if the timed trigger is occurres in the preconfigured 
        /// intervals.
        /// </summary>
        [Test]
        public void PreConfiguredTimedTrigger()
        {
            TimedTriggerExtension extension = new TimedTriggerExtension(500, 1000, true);
            extension.ModuleController = m_moduleController;
            extension.Attach();

            m_moduleController.RaiseAfterModuleStartEvent();
            Assert.AreEqual(0, m_moduleController.EnquedMessages.Count);
            Thread.Sleep(750);
            Assert.AreEqual(1, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[0]);
            Thread.Sleep(1000);
            Assert.AreEqual(2, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[1]);
            Thread.Sleep(1000);
            Assert.AreEqual(3, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[2]);
            m_moduleController.RaiseBeforeModuleStopEvent();
            Thread.Sleep(1000);
            Assert.AreEqual(3, m_moduleController.EnquedMessages.Count);
            
            extension.Detach();
        }

        /// <summary>
        /// Test if the timed trigger can be reconfigured while the 
        /// module is running.
        /// </summary>
        [Test]
        public void ChangeTimedTriggerWithRunningModule()
        {
            TimedTriggerExtension extension = new TimedTriggerExtension();
            extension.ModuleController = m_moduleController;
            extension.Attach();

            m_moduleController.RaiseAfterModuleStartEvent();
            Assert.AreEqual(0, m_moduleController.EnquedMessages.Count);
            Thread.Sleep(100);
            Assert.AreEqual(0, m_moduleController.EnquedMessages.Count);
            extension.ChangeTimer(500, 1000);
            Thread.Sleep(750);
            Assert.AreEqual(1, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[0]);
            Thread.Sleep(1000);
            Assert.AreEqual(2, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[1]);
            extension.ChangeTimer(500, 500);
            Thread.Sleep(750);
            Assert.AreEqual(3, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[2]);
            extension.ChangeTimer(Timeout.Infinite, Timeout.Infinite);
            Thread.Sleep(750);
            Assert.AreEqual(3, m_moduleController.EnquedMessages.Count);

            extension.Detach();
        }

        /// <summary>
        /// Test the behaviour that with a restart the timer is reset too.
        /// </summary>
        [Test]
        public void ResetTriggerOnRestart()
        {
            TimedTriggerExtension extension = new TimedTriggerExtension(200, 100, true);
            extension.ModuleController = m_moduleController;
            extension.Attach();

            m_moduleController.RaiseAfterModuleStartEvent();
            Assert.AreEqual(0, m_moduleController.EnquedMessages.Count);
            Thread.Sleep(100);
            m_moduleController.RaiseBeforeModuleStopEvent();
            m_moduleController.RaiseAfterModuleStartEvent();
            Thread.Sleep(150);
            Assert.AreEqual(0, m_moduleController.EnquedMessages.Count);
            Thread.Sleep(100);
            Assert.AreEqual(1, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[0]);
            Thread.Sleep(100);
            Assert.AreEqual(2, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[1]);
            m_moduleController.RaiseBeforeModuleStopEvent();
            m_moduleController.RaiseAfterModuleStartEvent();
            Thread.Sleep(150);
            Assert.AreEqual(2, m_moduleController.EnquedMessages.Count);
            Thread.Sleep(100);
            Assert.AreEqual(3, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[2]);

            extension.Detach();
        }

        /// <summary>
        /// Test the behaviour that with a restart the timer is rescheduled.
        /// </summary>
        [Test]
        public void RescheduleTriggerOnRestart()
        {
            TimedTriggerExtension extension = new TimedTriggerExtension(200, 100, false);
            extension.ModuleController = m_moduleController;
            extension.Attach();

            m_moduleController.RaiseAfterModuleStartEvent();
            Assert.AreEqual(0, m_moduleController.EnquedMessages.Count);
            Thread.Sleep(100);
            m_moduleController.RaiseBeforeModuleStopEvent();
            m_moduleController.RaiseAfterModuleStartEvent();
            Thread.Sleep(150);
            Assert.AreEqual(1, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[0]);
            Thread.Sleep(100);
            Assert.AreEqual(2, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[1]);
            m_moduleController.RaiseBeforeModuleStopEvent();
            m_moduleController.RaiseAfterModuleStartEvent();
            Thread.Sleep(100);
            Assert.AreEqual(3, m_moduleController.EnquedMessages.Count);
            Assert.IsInstanceOfType(typeof(TimedTriggerMessage), m_moduleController.EnquedMessages[2]);

            extension.Detach();
        }
    }
}
