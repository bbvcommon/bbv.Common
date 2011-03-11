//-------------------------------------------------------------------------------
// <copyright file="StringTruncationFormatterTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Formatters
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class StringTruncationFormatterTest
    {
        [Test]
        public void WithoutLength()
        {
            Assert.AreEqual("12345678  ", string.Format(new StringTruncationFormatter(), "{0,-10}", "12345678"));
        }

        [Test]
        public void WithLengthStringShortEnough()
        {
            Assert.AreEqual("12345678  ", string.Format(new StringTruncationFormatter(), "{0,-10:L10}", "12345678"));
        }

        [Test]
        public void WithLengthStringTooLong()
        {
            Assert.AreEqual("1234567890", string.Format(new StringTruncationFormatter(), "{0,-10:L10}", "1234567890123"));
        }

        [Test]
        public void MultipleArguments()
        {
            Assert.AreEqual("1234567890   235.25", string.Format(new StringTruncationFormatter(), "{0,-10:L10}{1,9:###.##}", "1234567890123", 235.25));
        }

        [Test]
        public void NotIFormattableArgument()
        {
            Assert.AreEqual("Tes", string.Format(new StringTruncationFormatter(), "{0:L3}", new TestClassNotFormattable()));
        }

        [Test]
        public void NotIFormattableArgumentNormalFormat()
        {
            Assert.AreEqual("Test", string.Format(new StringTruncationFormatter(), "{0}", new TestClassNotFormattable()));
        }

        [Test]
        public void Formattable()
        {
            Assert.AreEqual("form", string.Format(new StringTruncationFormatter(), "{0:L4}", new TestClassFormattable()));
        }

        [Test]
        public void NullValue()
        {
            Assert.Throws<ArgumentNullException>(
                () => string.Format(new StringTruncationFormatter(), "{0:L10}", null));
        }

        public class TestClassNotFormattable
        {
            public override string ToString()
            {
                return "Test";
            }
        }

        public class TestClassFormattable : IFormattable
        {
            public string ToString(string format, IFormatProvider formatProvider)
            {
                return "formatted";
            }
        }
    }
}
