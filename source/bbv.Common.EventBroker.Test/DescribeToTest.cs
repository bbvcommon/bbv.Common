//-------------------------------------------------------------------------------
// <copyright file="DescribeToTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.EventBroker
{
    using System;
    using System.IO;
    using Matchers;
    using NUnit.Framework;

    /// <summary>
    /// Tests for self description feature of the event broker.
    /// </summary>
    [TestFixture]
    public class DescribeToTest
    {
        /// <summary>
        /// the testee.
        /// </summary>
        private EventBroker testee;

        /// <summary>
        /// Initializes the <see cref="testee"/>.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EventBroker();
        }

        /// <summary>
        /// Tests describe to.
        /// </summary>
        [Test]
        public void DescribeTo()
        {
            P1 p1 = new P1();
            P2 p2 = new P2();
            S1 s1 = new S1();
            S2 s2 = new S2();

            this.testee.Register(p1);
            this.testee.Register(p2);
            this.testee.Register(s1);
            this.testee.Register(s2);

            StringWriter writer = new StringWriter();
            this.testee.DescribeTo(writer);

            writer.Close();
            writer.ToString();
        }

        /// <summary>
        /// A publisher without name.
        /// </summary>
        public class P1
        {
            /// <summary>
            /// A simple event.
            /// </summary>
            [EventPublication("E1")]
            public event EventHandler E1;

            /// <summary>
            /// An event with custom event args.
            /// </summary>
            [EventPublication("E2")]
            public event EventHandler<CustomEventArgs> E2;

            /// <summary>
            /// Fires event E1.
            /// </summary>
            public void FireE1()
            {
                this.E1(this, EventArgs.Empty);
            }

            /// <summary>
            /// Fires event E2 .
            /// </summary>
            public void FireE2()
            {
                this.E2(this, null);
            }
        }

        /// <summary>
        /// A publisher with a name
        /// </summary>
        public class P2 : INamedItem
        {
            /// <summary>
            /// An event that is published only to children.
            /// </summary>
            [EventPublication("E1", typeof(PublishToChildren))]
            public event EventHandler E1;

            /// <summary>
            /// Gets the name of the event broker item that is used for scope matchers.
            /// </summary>
            /// <value>The name of the event broker item.</value>
            public string EventBrokerItemName
            {
                get { return "P2"; }
            }

            /// <summary>
            /// Fires the event E1.
            /// </summary>
            public void FireE1()
            {
                this.E1(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// A simple subscriber.
        /// </summary>
        public class S1
        {
            /// <summary>
            /// A simple subscription.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The event args.</param>
            [EventSubscription("E1", typeof(Handlers.Publisher))]
            public void H1(object sender, EventArgs e)
            {
            }
        }

        /// <summary>
        /// A named subscriber.
        /// </summary>
        public class S2 : INamedItem
        {
            /// <summary>
            /// Gets the name of the event broker item that is used for scope matchers.
            /// </summary>
            /// <value>The name of the event broker item.</value>
            public string EventBrokerItemName
            {
                get { return "S2"; }
            }

            /// <summary>
            /// A handler with custom event args.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="bbv.Common.EventBroker.DescribeToTest.CustomEventArgs"/> instance containing the event data.</param>
            [EventSubscription("E2", typeof(Handlers.Publisher))]
            public void H2(object sender, CustomEventArgs e)
            {
            }
        }

        /// <summary>
        /// Custom event args used in the tests.
        /// </summary>
        public class CustomEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets a sample value.
            /// </summary>
            public int I { get; set; }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override string ToString()
            {
                return this.I.ToString();
            }
        }
    }
}