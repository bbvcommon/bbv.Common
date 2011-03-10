//-------------------------------------------------------------------------------
// <copyright file="EventTestListIntegrationTest.cs" company="bbv Software Services AG">
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
    /// Integration tests of the <see cref="EventTestList"/> and the <see cref="IEventTester"/>.
    /// </summary>
    [TestFixture]
    public class EventTestListIntegrationTest
    {
        /// <summary>
        /// The mock factory.
        /// </summary>
        private Mockery mockery;

        /// <summary>
        /// The test interface used in the tests to fire events.
        /// </summary>
        private ITestInterface testInterface;

        /// <summary>
        /// The interface that is used in the tests to fire events.
        /// </summary>
        public interface ITestInterface
        {
            /// <summary>
            /// A none generic event
            /// </summary>
            event EventHandler NoneGenericEvent;

            /// <summary>
            /// A generic event.
            /// </summary>
            event EventHandler<EventArgs> GenericEvent;
        }

        /// <summary>
        /// Sets up all tests. Creates the test interface and the mock factory.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();
            this.testInterface = this.mockery.NewMock<ITestInterface>();
            Stub.On(this.testInterface).EventAdd("NoneGenericEvent");
            Stub.On(this.testInterface).EventRemove("NoneGenericEvent");
            Stub.On(this.testInterface).EventAdd("GenericEvent");
            Stub.On(this.testInterface).EventRemove("GenericEvent");
        }

        /// <summary>
        /// Tears down all tests. Verifies that all expectations were met.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// If the <see cref="EventTestList"/> is used unordered they are allowed to occur unordered but they must 
        /// occur for the expected count.
        /// </summary>
        [Test]
        public void UnorderedOccurrence()
        {
            using (new EventTestList()
                       {
                           new EventTester(this.testInterface, "NoneGenericEvent", 1), 
                           new EventTester<EventArgs>(this.testInterface, "GenericEvent", 1)
                       })
            {
                Fire.On(this.testInterface).Event("GenericEvent").With(this.testInterface, EventArgs.Empty);
                Fire.On(this.testInterface).Event("NoneGenericEvent").With(this.testInterface, EventArgs.Empty);
            }
        }

        /// <summary>
        /// If the <see cref="EventTestList"/> is used unordered they are allowed to occur unordered, but if one
        /// event was not fired for the expected count a <see cref="AssertionException"/> is thrown.
        /// </summary>
        [Test]
        public void UnorderedOccurrenceOneMissing()
        {
            Assert.Throws<EventTesterException>(
                () =>
                    {
                        using (new EventTestList()
                                   {
                                       new EventTester(this.testInterface, "NoneGenericEvent", 1),
                                       new EventTester<EventArgs>(this.testInterface, "GenericEvent", 1)
                                   })
                        {
                            Fire.On(this.testInterface).Event("GenericEvent").With(this.testInterface, EventArgs.Empty);
                        }
                    });
        }

        /// <summary>
        /// If the <see cref="EventTestList"/> is used ordered and they occur in the expected order no exception is
        /// thrown.
        /// </summary>
        [Test]
        public void OrderedOccurrence()
        {
            using (new EventTestList(true)
                       {
                           new EventTester(this.testInterface, "NoneGenericEvent", 1), 
                           new EventTester<EventArgs>(this.testInterface, "GenericEvent", 1)
                       })
            {
                Fire.On(this.testInterface).Event("NoneGenericEvent").With(this.testInterface, EventArgs.Empty);
                Fire.On(this.testInterface).Event("GenericEvent").With(this.testInterface, EventArgs.Empty);
            }
        }

        /// <summary>
        /// If the <see cref="EventTestList"/> is used ordered and they do not occur in the expected order a
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        [Test]
        public void OrderedOccurrenceInvalidOrder()
        {
            Assert.Throws<EventTesterException>(
                () =>
                    {
                        using (new EventTestList(true)
                                   {
                                       new EventTester(this.testInterface, "NoneGenericEvent", 2),
                                       new EventTester<EventArgs>(this.testInterface, "GenericEvent", 1)
                                   })
                        {
                            Fire.On(this.testInterface).Event("NoneGenericEvent").With(this.testInterface, EventArgs.Empty);
                            Fire.On(this.testInterface).Event("GenericEvent").With(this.testInterface, EventArgs.Empty);
                            Fire.On(this.testInterface).Event("NoneGenericEvent").With(this.testInterface, EventArgs.Empty);
                        }
                    });
        }

        /// <summary>
        /// If the <see cref="EventTestList"/> is used ordered and they occur in the expected order but the last
        /// expected event is not fired an <see cref="AssertionException"/> is thrown.
        /// </summary>
        [Test]
        public void OrderedOccurrenceLastMissing()
        {
            Assert.Throws<EventTesterException>(
                () =>
                {
                    using (new EventTestList(true)
                                   {
                                       new EventTester(this.testInterface, "NoneGenericEvent", 2),
                                       new EventTester<EventArgs>(this.testInterface, "GenericEvent", 1)
                                   })
                    {
                        Fire.On(this.testInterface).Event("NoneGenericEvent").With(this.testInterface, EventArgs.Empty);
                        Fire.On(this.testInterface).Event("NoneGenericEvent").With(this.testInterface, EventArgs.Empty);
                    }
                });
        }
    }
}
