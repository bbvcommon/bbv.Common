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
// File:        TestWatchdogModule.cs
// Author:      Adrian Krummenacher / AK
//
// History:
// --------
// Date          Name   Version   Description
// 23-AUG-2006   AK		1.0       Creation     
//
/***************************************************************************/

using System;
using bbv.Common.AsyncModule.Events;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;

namespace bbv.Common.AsyncModule
{
    /// <summary>
    /// Mock.
    /// </summary>
    public class MockSuicidalModule
    {
        [MessageConsumer]
        public void ConsumeMessage(bool killThread)
        {
            if (killThread)
            {
                System.Threading.Thread.CurrentThread.Abort();
            }
        }
    }

    public class MockUnhandledExceptionReceiver
    {
        public List<UnhandledModuleExceptionEventArgs> Exceptions = new List<UnhandledModuleExceptionEventArgs>();

        public void OnUnhandledExceptionOccurred(object sender, UnhandledModuleExceptionEventArgs e)
        {
            Exceptions.Add(e);
        }
    }
    
    [TestFixture]
    public class TestWatchdogModule
    {
        /// <summary>
        /// Mock of the camera manager
        /// </summary>
        protected MockSuicidalModule m_suicidalModule;

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
            m_suicidalModule = new MockSuicidalModule();
            m_moduleCoordinator = new ModuleCoordinator();
        }

        /// <summary>
        /// Recovers a module two times. The third time an exception 
        /// is sent to the module controller.
        /// </summary>
        [Test]
        [Ignore("Test has timing problems.")]
        public void RecoverTwoTimesAndThenGiveUp()
        {
            MockUnhandledExceptionReceiver exceptionReceiver = new MockUnhandledExceptionReceiver();
            m_moduleCoordinator.UnhandledExceptionOccurred +=
                new EventHandler<UnhandledModuleExceptionEventArgs>(exceptionReceiver.OnUnhandledExceptionOccurred);
            m_moduleCoordinator.AddModule("SuicidalModule", m_suicidalModule);
            m_moduleCoordinator.AddModule("Watchdog", new WatchdogModule(2));
            m_moduleCoordinator.AddExtension<TimedTriggerExtension>("Watchdog", new TimedTriggerExtension(100,100, true));
            m_moduleCoordinator.StartAll();

            Assert.IsTrue(m_moduleCoordinator.ModuleControllers["SuicidalModule"].IsAlive);
            m_moduleCoordinator.PostMessage("SuicidalModule", true);
            Thread.Sleep(50);
            Assert.IsFalse(m_moduleCoordinator.ModuleControllers["SuicidalModule"].IsAlive);
            Thread.Sleep(100);
            Assert.IsTrue(m_moduleCoordinator.ModuleControllers["SuicidalModule"].IsAlive);
            m_moduleCoordinator.PostMessage("SuicidalModule", true);
            Thread.Sleep(100);
            m_moduleCoordinator.PostMessage("SuicidalModule", true);
            Thread.Sleep(100);
            Assert.IsFalse(m_moduleCoordinator.ModuleControllers["SuicidalModule"].IsAlive);

            Assert.AreEqual(4, exceptionReceiver.Exceptions.Count);
            Assert.IsInstanceOfType(typeof(ThreadAbortException), exceptionReceiver.Exceptions[0].UnhandledException);
            Assert.IsInstanceOfType(typeof(ThreadAbortException), exceptionReceiver.Exceptions[1].UnhandledException);
            Assert.IsInstanceOfType(typeof(ThreadAbortException), exceptionReceiver.Exceptions[2].UnhandledException); 
            Assert.AreEqual("SuicidalModule", exceptionReceiver.Exceptions[3].ModuleName);
            Assert.AreEqual("Module SuicidalModule died and could not be restarted.",
                exceptionReceiver.Exceptions[3].UnhandledException.Message);

            m_moduleCoordinator.StopAll();

        }
    }
}
