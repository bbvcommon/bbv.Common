//-------------------------------------------------------------------------------
// <copyright file="CsvWriterTest.cs" company="bbv Software Services AG">
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
    using System.Globalization;

    using NUnit.Framework;

    [TestFixture]
    public class CsvWriterTest
    {
        private CsvWriter testee;

        [SetUp]
        public void SetUp()
        {
            this.testee = new CsvWriter();
        }

        [Test]
        public void FormatSimpleValues()
        {
            string[] values = new[] { "1", "2", "hello", "world" };
            string expected = "1,2,hello,world\r\n";

            Assert.AreEqual(expected, this.testee.Write(values, ","));
        }

        [Test]
        public void TrailingWhiteSpaces()
        {
            string[] values = new[] { " 1 ", "\t2 ", "hello\t", "\tworld\t" };
            string expected = "\" 1 \",\"\t2 \",\"hello\t\",\"\tworld\t\"\r\n";

            Assert.AreEqual(expected, this.testee.Write(values, ","));
        }

        [Test]
        public void EmbeddedCommas()
        {
            string[] values = new[] { "hello, world", "The answer is, 42" };
            string expected = "\"hello, world\",\"The answer is, 42\"\r\n";

            Assert.AreEqual(expected, this.testee.Write(values, ","));
        }

        [Test]
        public void EmbeddedDoubleQuotes()
        {
            string[] values = new[] { "hello \"world\"", "The answer is \"42\"" };
            string expected = "\"hello \"world\"\",\"The answer is \"42\"\"\r\n";

            Assert.AreEqual(expected, this.testee.Write(values, ","));
        }

        [Test]
        public void EmbeddedLineBreak()
        {
            string[] values = new[] { "hello\r\nworld", "The answer is\r\n42" };
            string expected = "\"hello\r\nworld\",\"The answer is\r\n42\"\r\n";

            Assert.AreEqual(expected, this.testee.Write(values, ","));
        }

        [Test]
        public void SystemListSeparatorIsUsedAsDefaultSeparator()
        {
            string[] values = new[] { "hello", "world" };
            string expected = "hello" + CultureInfo.CurrentCulture.TextInfo.ListSeparator + "world\r\n";

            Assert.AreEqual(expected, this.testee.Write(values));
        }
    }
}