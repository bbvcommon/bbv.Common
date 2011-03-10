//-------------------------------------------------------------------------------
// <copyright file="CsvParserTest.cs" company="bbv Software Services AG">
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
    using NUnit.Framework;

    [TestFixture]
    public class CsvParserTest
    {
        private CsvParser parser;

        [SetUp]
        public void SetUp()
        {
            this.parser = new CsvParser();
        }

        [TearDown]
        public void TearDown()
        {
            this.parser = null;
        }

        [Test]
        public void ParseWithComma()
        {
            string line = "\"00501\",\"ABC\",Test \"XYZ,,,,,\"9, Strasse\",\"\",,\"8200\",\"Ort\",\"CH\"";
            string[] values = this.parser.Parse(line, ',');

            Assert.AreEqual("00501", values[0]);
            Assert.AreEqual("ABC", values[1]);
            Assert.AreEqual("Test \"XYZ", values[2]);
            Assert.AreEqual(string.Empty, values[3]);
            Assert.AreEqual(string.Empty, values[4]);
            Assert.AreEqual(string.Empty, values[5]);
            Assert.AreEqual(string.Empty, values[6]);
            Assert.AreEqual("9, Strasse", values[7]);
            Assert.AreEqual(string.Empty, values[8]);
            Assert.AreEqual(string.Empty, values[9]);
            Assert.AreEqual("8200", values[10]);
            Assert.AreEqual("Ort", values[11]);
            Assert.AreEqual("CH", values[12]);
        }

        [Test]
        public void ParseWithSemicolon()
        {
            string line = "\"00501\";\"ABC\";\"Test XYZ\";\"\";\"\";\"\";\"\";\"9, Strasse\";\"\";\"\";\"8200\";\"Ort\";\"CH\"";
            string[] values = this.parser.Parse(line, ';');

            Assert.AreEqual("00501", values[0]);
            Assert.AreEqual("ABC", values[1]);
            Assert.AreEqual("Test XYZ", values[2]);
            Assert.AreEqual(string.Empty, values[3]);
            Assert.AreEqual(string.Empty, values[4]);
            Assert.AreEqual(string.Empty, values[5]);
            Assert.AreEqual(string.Empty, values[6]);
            Assert.AreEqual("9, Strasse", values[7]);
            Assert.AreEqual(string.Empty, values[8]);
            Assert.AreEqual(string.Empty, values[9]);
            Assert.AreEqual("8200", values[10]);
            Assert.AreEqual("Ort", values[11]);
            Assert.AreEqual("CH", values[12]);
        }

        [Test]
        public void ParseDoubleQuote()
        {
            string line = "\"00501\"\"ABC\",\"Test XYZ\",\"\"\"\"\"\"\"\",\"9, Strasse\",\"\"\"\",\"8200\"\"Ort\"\"CH\"";
            string[] values = this.parser.Parse(line);

            Assert.AreEqual("00501\"ABC", values[0]);
            Assert.AreEqual("Test XYZ", values[1]);

            // the number of empty strings is irrelevant because the resulting string is trimmed.
            Assert.AreEqual("\"\"\"", values[2]);
            Assert.AreEqual("9, Strasse", values[3]);
            Assert.AreEqual("\"", values[4]);
            Assert.AreEqual("8200\"Ort\"CH", values[5]);
        }

        [Test]
        public void ParseSpaceAndEmptyString()
        {
            string line = "\"00501\", \"ABC\" ,\"Test XYZ\",";
            string[] values = this.parser.Parse(line);

            Assert.AreEqual("00501", values[0]);
            Assert.AreEqual(" \"ABC\" ", values[1]);
            Assert.AreEqual("Test XYZ", values[2]);
            Assert.AreEqual(string.Empty, values[3]);
        }

        [Test]
        public void ParseUnvalidValuesNoQuote()
        {
            Assert.Throws<CsvParseException>(
                () => this.parser.Parse("\"00501, ABC"));
        }

        [Test]
        public void ParseUnvalidValuesSpaceAfterQuote()
        {
            Assert.Throws<CsvParseException>(
                () => this.parser.Parse("\"00501\" , \"ABC\""));
        }

        [Test]
        public void ParseWithoutQuotes()
        {
            string line = "personal_id;nachname;vorname;";
            string[] values = this.parser.Parse(line, ';');
            Assert.AreEqual("personal_id", values[0]);
            Assert.AreEqual("nachname", values[1]);
            Assert.AreEqual("vorname", values[2]);
        }
    }
}
