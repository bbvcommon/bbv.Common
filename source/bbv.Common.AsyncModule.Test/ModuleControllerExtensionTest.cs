//-------------------------------------------------------------------------------
// <copyright file="ModuleControllerExtensionTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.AsyncModule
{
    using bbv.Common.AsyncModule.Extensions;

    using NMock2;

    using NUnit.Framework;

    [TestFixture]
    public class ModuleControllerExtensionTest
    {
        private ModuleController testee;

        private Mockery mockery;
        private TestModule module;

        public interface IExtension : IModuleExtension
        {
        }

        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();

            this.module = new TestModule();

            this.testee = new ModuleController();
            this.testee.Initialize(this.module);
        }

        [TearDown]
        public void TearDown()
        {
            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void ExtensionsCanBeAttachedAndQueried()
        {
            IExtension extension = this.mockery.NewMock<IExtension>();

            Expect.Once.On(extension).SetProperty("ModuleController").To(this.testee);
            Expect.Once.On(extension).Method("Attach");

            this.testee.AddExtension(extension);

            Assert.AreEqual(extension, this.testee.GetExtension<IExtension>());
        }
    }
}