//-------------------------------------------------------------------------------
// <copyright file="AdvancedRegistrationTest.cs" company="bbv Software Services AG">
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
//   Contains software or other content adapted from 
//   Smart Client – Composite UI Application Block, 
//   2005 Microsoft Corporation. All rights reserved.
//-------------------------------------------------------------------------------

namespace bbv.Common.EventBroker
{
    using NUnit.Framework;

    /// <summary>
    /// Tests for advanced registration feature.
    /// </summary>
    [TestFixture]
    public class AdvancedRegistrationTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private EventBroker testee;

        /// <summary>
        /// Initializes a test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();    
        }

        /// <summary>
        /// When an object is registered then the event broker calls the register method
        /// and passes the register interface as argument and when the object is unregistered then the event broker calls unregister.
        /// </summary>
        [Test]
        public void RegisterAndUnregister()
        {
            ObjectNeedingAdvancedRegistration o = new ObjectNeedingAdvancedRegistration();

            this.testee.Register(o);

            Assert.IsTrue(o.Registered, "Register was not called.");

            this.testee.Unregister(o);

            Assert.IsFalse(o.Registered, "Unregister was not called.");
        }

        /// <summary>
        /// Class for testing initialization.
        /// </summary>
        public class ObjectNeedingAdvancedRegistration : IEventBrokerRegisterable
        {
            /// <summary>
            /// Gets or sets a value indicating whether Initialize was called.
            /// </summary>
            /// <value><c>true</c> if [initialize was called]; otherwise, <c>false</c>.</value>
            public bool Registered { get; set; }

            /// <summary>
            /// Initializes this instance.
            /// </summary>
            /// <param name="eventRegisterer">The event registerer to register publications and subscriptions.</param>
            public void Register(IEventRegisterer eventRegisterer)
            {
                this.Registered = true;
            }

            /// <summary>
            /// The publisher or subscribe has to clean-up all registrations made in call to <see cref="Register"/>.
            /// </summary>
            /// <param name="eventRegisterer">The event registerer.</param>
            public void Unregister(IEventRegisterer eventRegisterer)
            {
                this.Registered = false;
            }
        }
    }
}