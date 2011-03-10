//-------------------------------------------------------------------------------
// <copyright file="EventTestListTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.TestUtilities.Test
{
    using System;
    using NMock2;
    using NUnit.Framework;

    /// <summary>
    /// Tests the implementation of <see cref="EventTestList"/>.
    /// </summary>
    [TestFixture]
    public class EventTestListTest
    {
        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mockery mockery;

        /// <summary>
        /// Sets up all tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();
        }

        /// <summary>
        /// Tears down all tests.
        /// Verifies that all expectations were met.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.mockery.VerifyAllExpectationsHaveBeenMet();            
        }

        /// <summary>
        /// If the list is used unordered the events can occur in the opposite order.
        /// </summary>
        [Test]
        public void UnorderedOccurrence()
        {
            IEventTester tester1 = this.mockery.NewMock<IEventTester>();
            Expect.Once.On(tester1).EventAdd("Invocation");
            IEventTester tester2 = this.mockery.NewMock<IEventTester>();
            Expect.Once.On(tester2).EventAdd("Invocation");

            using (new EventTestList()
                       {
                           tester1,
                           tester2
                       })
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();

                Fire.On(tester2).Event("Invocation").With(tester2, EventArgs.Empty);
                Fire.On(tester1).Event("Invocation").With(tester1, EventArgs.Empty);

                Expect.Once.On(tester1).Method("Dispose").WithNoArguments();
                Expect.Once.On(tester2).Method("Dispose").WithNoArguments();
            }
        }

        /// <summary>
        /// If the list is used ordered no exception occurs when they are fired in the expected order.
        /// When an event occurs all previous events are checked if they were fired the expected times.
        /// </summary>
        [Test]
        public void OrderedEvents()
        {
            IEventTester tester1 = this.mockery.NewMock<IEventTester>();
            Expect.Once.On(tester1).EventAdd("Invocation");
            IEventTester tester2 = this.mockery.NewMock<IEventTester>();
            Expect.Once.On(tester2).EventAdd("Invocation");

            using (new EventTestList(true)
                       {
                           tester1,
                           tester2
                       })
            {
                this.mockery.VerifyAllExpectationsHaveBeenMet();
                Fire.On(tester1).Event("Invocation").With(tester1, EventArgs.Empty);

                Expect.Once.On(tester1).Method("AssertComplete").WithNoArguments();
                Fire.On(tester2).Event("Invocation").With(tester2, EventArgs.Empty);

                Expect.Once.On(tester1).Method("Dispose").WithNoArguments();
                Expect.Once.On(tester2).Method("Dispose").WithNoArguments();
            }
        }

        /// <summary>
        /// If the list is used ordered no exception occurs when they are fired in the expected order.
        /// When an event occurs all previous events are checked if they were fired the expected times.
        /// </summary>
        [Test]
        public void CompleteAssertionExceptionIsReThrown()
        {
            IEventTester tester1 = this.mockery.NewNamedMock<IEventTester>("tester1");
            IEventTester tester2 = this.mockery.NewNamedMock<IEventTester>("tester2");

            Expect.Once.On(tester1).EventAdd("Invocation");
            Expect.Once.On(tester2).EventAdd("Invocation");

            Expect.Once.On(tester1).Method("AssertComplete").WithNoArguments()
                .Will(Throw.Exception(new EventTesterException("Not Complete")));
            Expect.Once.On(tester1).Method("Dispose").WithNoArguments()
                .Will(Throw.Exception(new EventTesterException("Failed")));
            Expect.Once.On(tester2).Method("Dispose").WithNoArguments();

            Assert.Throws<EventTesterException>(
                () =>
                    {
                        using (new EventTestList(true)
                                   {
                                       tester1,
                                       tester2
                                   })
                        {
                            Fire.On(tester1).Event("Invocation").With(tester1, EventArgs.Empty);
                            Fire.On(tester2).Event("Invocation").With(tester2, EventArgs.Empty);
                        }
                    });

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// If the list is used ordered no exception occurs when they are fired in the expected order.
        /// When an event occurs all previous events are checked if they were fired the expected times.
        /// </summary>
        [Test]
        public void DisposeAssertionExceptionIsReThrown()
        {
            IEventTester tester1 = this.mockery.NewMock<IEventTester>();
            IEventTester tester2 = this.mockery.NewMock<IEventTester>();

            Expect.Once.On(tester1).EventAdd("Invocation");
            Expect.Once.On(tester2).EventAdd("Invocation");

            try
            {
                using (new EventTestList(true)
                           {
                               tester1,
                               tester2
                           })
                {
                    this.mockery.VerifyAllExpectationsHaveBeenMet();

                    Fire.On(tester1).Event("Invocation").With(tester1, EventArgs.Empty);

                    Expect.Once.On(tester1).Method("AssertComplete").WithNoArguments();
                    Fire.On(tester2).Event("Invocation").With(tester2, EventArgs.Empty);

                    Expect.Once.On(tester1).Method("Dispose").WithNoArguments();
                    Expect.Once.On(tester2).Method("Dispose").WithNoArguments()
                        .Will(Throw.Exception(new EventTesterException("Dispose Failed")));
                }
            }
            catch (EventTesterException e)
            {
                Assert.AreEqual("Dispose Failed", e.InnerException.Message);
                return;
            }

            Assert.Fail("Dispose of tester2 did not work as expected.");
        }
    }
}
