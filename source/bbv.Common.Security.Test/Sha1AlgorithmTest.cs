//-------------------------------------------------------------------------------
// <copyright file="Sha1AlgorithmTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Security
{
    using System;
    using System.IO;
    using System.Text;
    using NUnit.Framework;

    /// <summary>
    /// Tests for the <see cref="SHA1Algorithm"/> class
    /// </summary>
    [TestFixture]
    public class Sha1AlgorithmTest
    {
        /// <summary>
        /// The object under test
        /// </summary>
        private SHA1Algorithm testee;

        /// <summary>
        /// Sets up the <see cref="testee"/>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new SHA1Algorithm();
        }

        /// <summary>
        /// Calculates the sha1 hash from empty string and verifies it is correct
        /// </summary>
        [Test]
        public void GetSha1HashFromString()
        {
            Assert.AreEqual("da39a3ee5e6b4b0d3255bfef95601890afd80709", this.testee.ComputeHash(string.Empty));
        }

        /// <summary>
        /// Calculates the sha1 hash from a memory stream containing an empty string and verifies it is correct
        /// </summary>
        [Test]
        public void GetSha1HashFromStream()
        {
            Stream stream = new MemoryStream(Encoding.Default.GetBytes(string.Empty));
            Assert.AreEqual("da39a3ee5e6b4b0d3255bfef95601890afd80709", this.testee.ComputeHashFromStream(stream));
        }

        /// <summary>
        /// Calculating sha1 from a null string throws an argument null exception
        /// </summary>
        [Test]
        public void GetSha1HashFromNullString()
        {
            Assert.Throws<ArgumentNullException>(
                () => this.testee.ComputeHash(null));
        }

        /// <summary>
        /// Calculating sha1 from a null stream throws a null reference exception
        /// </summary>
        [Test]
        public void GetSha1HashFromNullStream()
        {
            Assert.Throws<NullReferenceException>(
                () => this.testee.ComputeHashFromStream(null));
        }

        /// <summary>
        /// Calculating sha1 from string.empty with custom format {0:X2} results in an upper case hash value
        /// </summary>
        [Test]
        public void GetSha1HashFromStringFormat()
        {
            this.testee.Format = "{0:X2}";
            Assert.AreEqual("DA39A3EE5E6B4B0D3255BFEF95601890AFD80709", this.testee.ComputeHash(string.Empty));
        }

        /// <summary>
        /// Calculating sha1 from a stream containing an empty string with custom format {0:X2} results in an upper case hash value
        /// </summary>
        [Test]
        public void GetSha1HashFromStreamFormat()
        {
            this.testee.Format = "{0:X2}";
            Stream stream = new MemoryStream(Encoding.Default.GetBytes(string.Empty));
            Assert.AreEqual("DA39A3EE5E6B4B0D3255BFEF95601890AFD80709", this.testee.ComputeHashFromStream(stream));
        }

        /// <summary>
        /// Asserts that the format is set correctly
        /// </summary>
        [Test]
        public void SetFormat()
        {
            string format = "{0:Format}";

            this.testee.Format = format;
            Assert.AreEqual(format, this.testee.Format);
        }
    }
}