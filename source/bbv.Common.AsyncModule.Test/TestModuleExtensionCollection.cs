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
// File:        TestModuleExtensionCollection.cs
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
using NMock2;

namespace bbv.Common.AsyncModule
{
    public interface IMockExtensionTypeA : IModuleExtension { }
    public interface IMockExtensionTypeB : IModuleExtension { }
    public interface IMockExtensionTypeC { }
    public class MockExtensionTypeC : IMockExtensionTypeC { }
    public interface IMockExtensionTypeD : IMockExtensionTypeB { }

    [TestFixture]
    public class TestModuleExtensionCollection
    {
        /// <summary>
        /// NMock master object.
        /// </summary>
        protected Mockery m_mockery;

        /// <summary>
        /// Mock of a module extension.
        /// </summary>
        protected IMockExtensionTypeA m_extensionA;

        /// <summary>
        /// Mock of a module extension.
        /// </summary>
        protected IMockExtensionTypeB m_extensionB;

        /// <summary>
        /// Dummy extension.
        /// </summary>
        protected MockExtensionTypeC m_extensionC;

        /// <summary>
        /// Mock of a module extension.
        /// </summary>
        protected IMockExtensionTypeB m_extensionD;

        /// <summary>
        /// Prepares the mocks.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            m_mockery = new Mockery();
            m_extensionA = m_mockery.NewMock<IMockExtensionTypeA>();
            m_extensionB = m_mockery.NewMock<IMockExtensionTypeB>();
            m_extensionC = new MockExtensionTypeC();
            m_extensionD = m_mockery.NewMock<IMockExtensionTypeD>();
        }


        /// <summary>
        /// The add and delete of the hash table are overriden to add
        /// the following behavior:
        /// - Call Attach if an Extension is added, which implements IModuleExtension.
        /// - Call Deattach if an Extension is removed, which implements IModuleExtension.
        /// - If the key already exists, remove the current version and add the new one.
        /// </summary>
        [Test]
        public void AddAndRemove()
        {
            Expect.Once.On(m_extensionA).Method("Attach");
            Expect.Once.On(m_extensionB).Method("Attach");
            Expect.Once.On(m_extensionA).Method("Detach");
            Expect.Once.On(m_extensionB).Method("Detach");

            ModuleExtensionCollection extensions = new ModuleExtensionCollection();
            extensions.Add(typeof(IMockExtensionTypeA), m_extensionA);
            extensions.Add(typeof(IMockExtensionTypeB), m_extensionB);
            extensions.Add(typeof(IMockExtensionTypeC), m_extensionC);

            object extension = extensions[typeof(IMockExtensionTypeA)];
            Assert.AreSame(m_extensionA, extension);
            extension = extensions[typeof(IMockExtensionTypeB)];
            Assert.AreSame(m_extensionB, extension);
            extension = extensions[typeof(IMockExtensionTypeC)];
            Assert.AreSame(m_extensionC, extension);
            
            extensions.Remove(typeof(IMockExtensionTypeA));
            extensions.Remove(typeof(IMockExtensionTypeB));
            extensions.Remove(typeof(IMockExtensionTypeC));

            m_mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// The collection provides an add and get method with 
        /// generic types, which can be used if you do not like 
        /// type casts all over the code.
        /// </summary>
        [Test]
        public void AddAndGetWithGenerics()
        {
            Expect.Once.On(m_extensionA).Method("Attach");
            Expect.Once.On(m_extensionB).Method("Attach");

            ModuleExtensionCollection extensions = new ModuleExtensionCollection();
            extensions.Add<IMockExtensionTypeA>(m_extensionA);
            extensions.Add<IMockExtensionTypeB>(m_extensionB);
            extensions.Add<IMockExtensionTypeC>(m_extensionC);

            IMockExtensionTypeA extensionA = extensions.Get<IMockExtensionTypeA>();
            Assert.AreSame(m_extensionA, extensionA);

            IMockExtensionTypeB extensionB = extensions.Get<IMockExtensionTypeB>();
            Assert.AreSame(m_extensionB, extensionB);

            IMockExtensionTypeC extensionC = extensions.Get<IMockExtensionTypeC>();
            Assert.AreSame(m_extensionC, extensionC);

            IMockExtensionTypeD extensionD = extensions.Get<IMockExtensionTypeD>();
            Assert.IsNull(extensionD);

            m_mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Override an extisting extension.
        /// </summary>
        [Test]
        public void OverrideAnInstalledExtension()
        {
            Expect.Once.On(m_extensionA).Method("Attach");
            Expect.Once.On(m_extensionB).Method("Attach");
            Expect.Once.On(m_extensionB).Method("Detach");
            Expect.Once.On(m_extensionD).Method("Attach");

            ModuleExtensionCollection extensions = new ModuleExtensionCollection();
            extensions.Add<IMockExtensionTypeA>(m_extensionA);
            extensions.Add<IMockExtensionTypeB>(m_extensionB);
            extensions.Add<IMockExtensionTypeC>(m_extensionC);

            IMockExtensionTypeA extensionA = extensions.Get<IMockExtensionTypeA>();
            Assert.AreSame(m_extensionA, extensionA);

            IMockExtensionTypeB extensionB = extensions.Get<IMockExtensionTypeB>();
            Assert.AreSame(m_extensionB, extensionB);

            IMockExtensionTypeC extensionC = extensions.Get<IMockExtensionTypeC>();
            Assert.AreSame(m_extensionC, extensionC);

            extensions.Add<IMockExtensionTypeB>(m_extensionD);

            IMockExtensionTypeB extensionD = extensions.Get<IMockExtensionTypeB>();
            Assert.AreSame(m_extensionD, extensionD);

            m_mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
