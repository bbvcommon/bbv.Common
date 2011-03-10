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
// File:        TestRetryExtension.cs
// Author:      Adrian Krummenacher / AK
//
// History:
// --------
// Date          Name   Version   Description
// 22-AUG-2006   AK		1.0       Creation     
//
/***************************************************************************/

using System;
using NUnit.Framework;
using NMock2;
using NMock2.Matchers;

namespace bbv.Common.AsyncModule
{
    [TestFixture]
    public class TestRetryExtension
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

        /// <summary>
        /// Tests if the retry is initalized the number of times configured.
        /// </summary>
        [Test]
        public void Retry()
        {
            ArgumentException exception = new ArgumentException("TestException");
            RetryExtension extension = new RetryExtension("Scheduler", 1000, 2, typeof(Exception));
            extension.ModuleController = m_moduleController;
            extension.ModuleCoordinator = m_moduleCoordinator;
            extension.RetryExceptionTypes.Add(typeof(ArgumentException));
            extension.Attach();

            Expect.Exactly(2).On(m_moduleCoordinator).Method("PostMessage").With(
                new EqualMatcher("Scheduler"),
                new ScheduledMessageMatcher("TestModule", "TestMessage"));

            bool exceptionHandled;
            m_moduleController.RaiseConsumeMessageExceptionOccurred("TestModule",
                "TestMessage", exception, out exceptionHandled);
            Assert.IsTrue(exceptionHandled);

            m_moduleController.RaiseConsumeMessageExceptionOccurred("TestModule",
                "TestMessage", exception, out exceptionHandled);
            Assert.IsTrue(exceptionHandled);

            m_moduleController.RaiseConsumeMessageExceptionOccurred("TestModule",
                "TestMessage", exception, out exceptionHandled);
            Assert.IsFalse(exceptionHandled);

            extension.Detach();

            m_mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Checks if the retry count is reset if a message was processed 
        /// successfully.
        /// </summary>
        [Test]
        public void ResetRetry()
        {
            ArgumentException exception = new ArgumentException("TestException");
            RetryExtension extension = new RetryExtension("Scheduler", 1000, 2, typeof(Exception));
            extension.ModuleController = m_moduleController;
            extension.ModuleCoordinator = m_moduleCoordinator;
            extension.RetryExceptionTypes.Add(typeof(ArgumentException));
            extension.Attach();

            Expect.Exactly(4).On(m_moduleCoordinator).Method("PostMessage").With(
                new EqualMatcher("Scheduler"),
                new ScheduledMessageMatcher("TestModule", "TestMessage"));

            bool exceptionHandled;
            m_moduleController.RaiseConsumeMessageExceptionOccurred("TestModule",
                "TestMessage", exception, out exceptionHandled);
            Assert.IsTrue(exceptionHandled);

            m_moduleController.RaiseConsumeMessageExceptionOccurred("TestModule",
                "TestMessage", exception, out exceptionHandled);
            Assert.IsTrue(exceptionHandled);

            m_moduleController.RaiseAfterConsumeMessageEvent("TestModule", "TestMessage");

            m_moduleController.RaiseConsumeMessageExceptionOccurred("TestModule",
                "TestMessage", exception, out exceptionHandled);
            Assert.IsTrue(exceptionHandled);

            m_moduleController.RaiseConsumeMessageExceptionOccurred("TestModule",
                "TestMessage", exception, out exceptionHandled);
            Assert.IsTrue(exceptionHandled);

            m_moduleController.RaiseConsumeMessageExceptionOccurred("TestModule",
                "TestMessage", exception, out exceptionHandled);
            Assert.IsFalse(exceptionHandled);

            extension.Detach();

            m_mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Checks if only registered exception types are handled.
        /// </summary>
        [Test]
        public void NotSupportedException()
        {
            ArgumentException exception = new ArgumentException("TestException");
            RetryExtension extension = new RetryExtension("Scheduler", 1000, 2, typeof(NotImplementedException));
            extension.ModuleController = m_moduleController;
            extension.ModuleCoordinator = m_moduleCoordinator;
            extension.Attach();

            Expect.Never.On(m_moduleCoordinator).Method("PostMessage");

            bool exceptionHandled;
            m_moduleController.RaiseConsumeMessageExceptionOccurred("TestModule",
                "TestMessage", exception, out exceptionHandled);
            Assert.IsFalse(exceptionHandled);

            extension.Detach();

            m_mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
