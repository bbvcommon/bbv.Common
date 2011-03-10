// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MD5AlgorithmTest.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG   Licensed under the Apache License, Version 2.0 (the "License");   you may not use this file except in compliance with the License.   You may obtain a copy of the License at       http://www.apache.org/licenses/LICENSE-2.0   Unless required by applicable law or agreed to in writing, software   distributed under the License is distributed on an "AS IS" BASIS,   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.   See the License for the specific language governing permissions and   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace bbv.Common.Security
{
    using System;
    using System.IO;

    using NUnit.Framework;

    [TestFixture]
    public class MD5AlgorithmTest
    {
        private IHashAlgorithm testee;

        [SetUp]
        public void SetUp()
        {
            this.testee = new MD5Algorithm();
        }

        [Test]
        public void GetMD5HashFromString()
        {
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", this.testee.ComputeHash(string.Empty), this.testee.ComputeHash(string.Empty));
        }

        [Test]
        public void GetMD5HashFromStream()
        {
            string text = string.Empty;
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(text));
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", this.testee.ComputeHashFromStream(stream));
        }

        [Test]
        public void GetMD5HashFromNullString()
        {
            Assert.Throws<ArgumentNullException>(
                () => this.testee.ComputeHash(null));
        }

        [Test]
        public void GetMD5HashFromStringFormat()
        {
            this.testee.Format = "{0:X2}";
            Assert.AreEqual("D41D8CD98F00B204E9800998ECF8427E", this.testee.ComputeHash(string.Empty), this.testee.ComputeHash(string.Empty));
        }

        [Test]
        public void GetMD5HashFromStringNullFormat()
        {
            this.testee.Format = null;
            
            Assert.Throws<ArgumentNullException>(
                () => this.testee.ComputeHash(string.Empty));
        }

        [Test]
        public void SetFormat()
        {
            string format = "{0:Format}";

            this.testee.Format = format;

            Assert.AreEqual(format, this.testee.Format);
        }
    }
}