//-------------------------------------------------------------------------------
// <copyright file="DateTimeHelperTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Data
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class DateTimeHelperTest
    {
        private readonly DateTime early = DateTime.Parse("2008-01-01 00:00:00.000");
        private readonly DateTime late = DateTime.Parse("2008-01-01 23:59:59.999");

        [Test]
        public void CompareDaySameDateTime()
        {
            Assert.AreEqual(0, DateTimeHelper.CompareDay(this.late, this.early), "should be in same day.");
        }

        [Test]
        public void CompareDaySameInts()
        {
            Assert.AreEqual(0, DateTimeHelper.CompareDay(1, 1, 2008, this.late), "should be in same day.");
        }

        [Test]
        public void CompareDaySmaller()
        {
            Assert.AreEqual(-1, DateTimeHelper.CompareDay(31, 12, 2007, this.early), "should be smaller");
        }

        [Test]
        public void CompareDayGreater()
        {
            Assert.AreEqual(1, DateTimeHelper.CompareDay(2, 1, 2008, this.late), "should be smaller");
        }
    }
}
