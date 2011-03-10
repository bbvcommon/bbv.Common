//-------------------------------------------------------------------------------
// <copyright file="SkipIfDuplicateInQueueModuleExtensionTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.AsyncModule.Extensions
{
    using bbv.Common.AsyncModule.Events;

    using NMock2;

    using NUnit.Framework;

    [TestFixture]
    public class SkipIfDuplicateInQueueModuleExtensionTest
    {
        private Mockery mockery = new Mockery();
        private IModuleController controller;

        private SkipIfDuplicateInQueueModuleExtension testee;

        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();
            this.controller = this.mockery.NewMock<IModuleController>();

            this.testee = new SkipIfDuplicateInQueueModuleExtension();

            this.testee.ModuleController = this.controller;

            Expect.Once.On(this.controller).EventAdd("BeforeConsumeMessage");
            this.testee.Attach();
        }

        [Test]
        public void MessagesAreConsumedIfNoMessageInQueue()
        {
            Stub.On(this.controller).GetProperty("Messages").Will(Return.Value(new object[] { }));

            BeforeConsumeMessageEventArgs e = new BeforeConsumeMessageEventArgs(this, "test");

            Fire.On(this.controller).Event("BeforeConsumeMessage").With(this, e);

            Assert.IsFalse(e.Cancel);
        }

        [Test]
        public void MessagesAreConsumedIfNoDuplicateInQueue()
        {
            Stub.On(this.controller).GetProperty("Messages").Will(Return.Value(new object[] { "hello", "world" }));

            BeforeConsumeMessageEventArgs e = new BeforeConsumeMessageEventArgs(this, "test");

            Fire.On(this.controller).Event("BeforeConsumeMessage").With(this, e);

            Assert.IsFalse(e.Cancel);
        }

        [Test]
        public void MessagesAreNotConsumedIfDuplicateInQueue()
        {
            Stub.On(this.controller).GetProperty("Messages").Will(Return.Value(new object[] { "hello", "test", "world" }));

            BeforeConsumeMessageEventArgs e = new BeforeConsumeMessageEventArgs(this, "test");

            Fire.On(this.controller).Event("BeforeConsumeMessage").With(this, e);

            Assert.IsTrue(e.Cancel);
        }

        [Test]
        public void Cleanup()
        {
            Expect.Once.On(this.controller).EventRemove("BeforeConsumeMessage");
            this.testee.Detach();
        }
    }
}