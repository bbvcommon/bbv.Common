// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashServiceIntegrationTest.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG   Licensed under the Apache License, Version 2.0 (the "License");   you may not use this file except in compliance with the License.   You may obtain a copy of the License at       http://www.apache.org/licenses/LICENSE-2.0   Unless required by applicable law or agreed to in writing, software   distributed under the License is distributed on an "AS IS" BASIS,   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.   See the License for the specific language governing permissions and   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace bbv.Common.Security
{
    using System;
    using System.IO;

    using bbv.Common.IO;

    using NUnit.Framework;

    [TestFixture]
    public class HashServiceIntegrationTest
    {
        private const string Text = "text";

        private IHashService testee;

        private string path, md5Path;

        [SetUp]
        public void SetUp()
        {
            this.path = Path.GetTempFileName();
            this.md5Path = Path.GetTempFileName();

            this.testee = new HashService(new MD5Algorithm());
        }

        [Test]
        public void ComputeHash()
        {
            string hash = this.testee.GetHashFromString(Text);

            Assert.AreEqual(new MD5Algorithm().ComputeHash(Text), hash);

            Console.WriteLine(hash);
        }

        [Test]
        public void WriteHashFile()
        {
            new TextFileWriter(this.path).Write(Text);

            TextFileReader reader = new TextFileReader(this.path);
            TextFileWriter writer = new TextFileWriter(this.md5Path);

            this.testee.WriteHash(reader, writer);

            Assert.AreEqual(new MD5Algorithm().ComputeHash(Text), new TextFileReader(this.md5Path).GetString());
        }

        [Test]
        public void CompareHashFile()
        {
            new TextFileWriter(this.path).Write(Text);

            new TextFileWriter(this.md5Path).Write(this.testee.GetHashFromString(Text));

            TextFileReader reader = new TextFileReader(this.path);
            TextFileReader hashreader = new TextFileReader(this.md5Path);

            Assert.IsTrue(this.testee.CompareWithHash(reader, hashreader));
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(this.path))
            {
                File.Delete(this.path);
            }

            if (File.Exists(this.md5Path))
            {
                File.Delete(this.md5Path);
            }
        }
    }
}