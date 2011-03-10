//-------------------------------------------------------------------------------
// <copyright file="ModuleControllerInitializationTest.cs" company="bbv Software Services AG">
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
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class ModuleControllerInitializationTest
    {
        [Test]
        public void InitWithNullModuleThrowsArgumentNullException()
        {
            var testee = new ModuleController();
            Assert.Throws<ArgumentNullException>(
                () => testee.Initialize(null));
        }

        [Test]
        public void InitWithNegativeThreadNumberThrowsArgumentException()
        {
            var testee = new ModuleController();
            Assert.Throws<ArgumentException>(
                () => testee.Initialize(new TestModule(), -1));
        }

        [Test]
        public void InitWithZeroThreadNumberThrowsArgumentException()
        {
            var testee = new ModuleController();
            Assert.Throws<ArgumentException>(
                () => testee.Initialize(new TestModule(), 0));
        }

        [Test]
        public void InitWithModuleWithWrongMessageCOnsumerMethodSignature()
        {
            var testee = new ModuleController();
            Assert.Throws<ArgumentException>(
                () => testee.Initialize(new ModuleWithWrongMessageConsumerMethodSignature()));
        }

        [Test]
        public void InitWithModuleWithModuleWithNoMessageConsumerMethod()
        {
            var testee = new ModuleController();
            Assert.Throws<ArgumentException>(
                () => testee.Initialize(new ModuleWithNoMessageConsumerMethod()));
        }

        [Test]
        public void StartWithForgroundThreads()
        {
            ModuleController testee = new ModuleController();
            testee.Initialize(new TestModule());
            testee.Start();
        }

        [Test]
        public void StartWithBackgroundThreads()
        {
            ModuleController testee = new ModuleController();
            testee.Initialize(new TestModule(), true);
            testee.Start();
        }

        public class ModuleWithWrongMessageConsumerMethodSignature
        {
            [MessageConsumer]
            public void Consume(int i, string s)
            {
            }
        }

        public class ModuleWithNoMessageConsumerMethod
        {
            public void Consume(int i)
            {
            }
        }
    }
}