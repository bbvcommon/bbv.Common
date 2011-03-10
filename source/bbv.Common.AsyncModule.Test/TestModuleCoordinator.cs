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
// File:        TestModuleCoordinator.cs
// Author:      Adrian Krummenacher / AK
//
// History:
// --------
// Date          Name   Version   Description
// 09-AUG-2006   AK		1.0       Creation     
//
/***************************************************************************/

using System;
using NUnit.Framework;

namespace bbv.Common.AsyncModule
{
    [TestFixture]
    public class TestModuleCoordinator
    {
        [Test]
        public void TestModuleWhichImplementsInterface()
        {
            MockModuleImplementingInterface module = new MockModuleImplementingInterface();
            ModuleCoordinator coordinator = new ModuleCoordinator();
            coordinator.AddModule("TestModule", module);
            coordinator.StartAll();
            coordinator.PostMessage("TestModule", "TestMessage");
            coordinator.PostMessage("TestModule", 77);
            System.Threading.Thread.Sleep(100);
            coordinator.StopAll();

            Assert.AreSame(coordinator, module.ModuleCoordinator);
            Assert.AreEqual(2, module.ReceivedMessages.Count);
            Assert.IsInstanceOfType(typeof(string), module.ReceivedMessages[0]);
            Assert.AreEqual("TestMessage", (string)module.ReceivedMessages[0]);

            Assert.IsInstanceOfType(typeof(int), module.ReceivedMessages[1]);
            Assert.AreEqual(77, (int)module.ReceivedMessages[1]);
        }

        [Test]
        public void TestModuleUsingAttributes()
        {
            MockModuleUsingAttributes module = new MockModuleUsingAttributes();
            ModuleCoordinator coordinator = new ModuleCoordinator();
            coordinator.AddModule("TestModule", module);
            coordinator.StartAll();
            coordinator.PostMessage("TestModule", "TestMessage");
            coordinator.PostMessage("TestModule", 77);
            System.Threading.Thread.Sleep(100);
            coordinator.StopAll();

            Assert.AreSame(coordinator, module.ModuleCoordinator);
            Assert.AreEqual(2, module.ReceivedMessages.Count);
            Assert.IsInstanceOfType(typeof(string), module.ReceivedMessages[0]);
            Assert.AreEqual("TestMessage", (string)module.ReceivedMessages[0]);

            Assert.IsInstanceOfType(typeof(int), module.ReceivedMessages[1]);
            Assert.AreEqual(77, (int)module.ReceivedMessages[1]);
        }
    }
}
