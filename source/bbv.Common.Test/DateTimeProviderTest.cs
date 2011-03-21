//-------------------------------------------------------------------------------
// <copyright file="DateTimeProviderTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Tests for the <see cref="DateTimeProvider"/> class
    /// </summary>
    [TestFixture]
    public class DateTimeProviderTest
    {
        /// <summary>
        /// The object under test
        /// </summary>
        private DateTimeProvider testee;

        /// <summary>
        /// Sets up the tests
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new DateTimeProvider();
        }

        /// <summary>
        /// When getting the now property, the date time provider returns a valid value
        /// </summary>
        [Test]
        public void GetNow()
        {
            DateTime now = this.testee.Now;
            Assert.IsNotNull(now, "Now must return a date time value not null");
            Assert.LessOrEqual(now, DateTime.Now, "Now must not return a value in the future");
        }

        /// <summary>
        /// When getting the today property, the date time provider returns a valid value with time of day equal to 00:00:00
        /// </summary>
        [Test]
        public void GetToday()
        {
            DateTime today = this.testee.Today;
            Assert.IsNotNull(today, "Today must return a date time value not null");
            Assert.AreEqual(0, today.Hour, "The hour component must be zero");
            Assert.AreEqual(0, today.Minute, "The minute component must be zero");
            Assert.AreEqual(0, today.Second, "The second component must be zero");
            Assert.AreEqual(0, today.Millisecond, "The millisecond component must be zero");
        }
    }
}