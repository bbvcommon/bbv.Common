// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashServiceTest.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG   Licensed under the Apache License, Version 2.0 (the "License");   you may not use this file except in compliance with the License.   You may obtain a copy of the License at       http://www.apache.org/licenses/LICENSE-2.0   Unless required by applicable law or agreed to in writing, software   distributed under the License is distributed on an "AS IS" BASIS,   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.   See the License for the specific language governing permissions and   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace bbv.Common.Security
{
    using System;
    using System.IO;

    using bbv.Common.IO;

    using NMock2;

    using NUnit.Framework;

    [TestFixture]
    public class HashServiceTest
    {
        private const string Text = "test";
        private const string Hash = "hash";

        private Mockery mockery;
        private IHashService testee;
        private IHashAlgorithm algorithm;
        private ITextReader reader;
        private ITextWriter writer;
        private ITextReader hashReader;

        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();

            this.algorithm = this.mockery.NewMock<IHashAlgorithm>();

            this.reader = this.mockery.NewMock<ITextReader>();

            this.writer = this.mockery.NewMock<ITextWriter>();

            this.hashReader = this.mockery.NewMock<ITextReader>();

            this.testee = new HashService(this.algorithm);
        }

        [Test]
        public void CreateHashServiceTest()
        {
            Assert.IsNotNull(new HashService(this.algorithm));
        }

        [Test]
        public void GetHashFromString()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(Text).Will(Return.Value(Hash));

            Assert.AreEqual(Hash, this.testee.GetHashFromString(Text));

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void GetHashFromNullString()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(string.Empty).Will(Return.Value(Hash));

            Assert.AreEqual(Hash, this.testee.GetHashFromString(null));

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void GetHashFromReader()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));

            Assert.AreEqual(Hash, this.testee.GetHashFromReader(this.reader));

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void GetHashFromNullReader()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));

            Assert.Throws<ArgumentNullException>(
                () => this.testee.GetHashFromReader(null));
        }

        [Test]
        public void WriteHashFromString()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(Text).Will(Return.Value(Hash));
            Expect.Once.On(this.writer).Method("Write").With(Hash);

            this.testee.WriteHashFromString(Text, this.writer);

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void WriteHashFromNullString()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(string.Empty).Will(Return.Value(Hash));
            Expect.Once.On(this.writer).Method("Write").With(Hash);

            this.testee.WriteHashFromString(null, this.writer);

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void WriteHashFromStringWithNullWriter()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(Text).Will(Return.Value(Hash));
            Expect.Once.On(this.writer).Method("Write").With(Hash);

            Assert.Throws<ArgumentNullException>(
                () => this.testee.WriteHashFromString(Text, null));
        }

        [Test]
        public void WriteHashFromReader()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));
            Expect.Once.On(this.writer).Method("Write").With(Hash);

            this.testee.WriteHash(this.reader, this.writer);

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void WriteHashFromNullReader()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));
            Expect.Once.On(this.writer).Method("Write").With(Hash);

            Assert.Throws<ArgumentNullException>(
                () => this.testee.WriteHash(null, this.writer));
        }

        [Test]
        public void WriteHashFromReaderWithNullWriter()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));
            Expect.Once.On(this.writer).Method("Write").With(Hash);

            Assert.Throws<ArgumentNullException>(
                () => this.testee.WriteHash(this.reader, null));
        }

        [Test]
        public void CompareComputedHashFromStringWithHash()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(Text).Will(Return.Value(Hash));

            Assert.IsTrue(this.testee.CompareWithHash(Text, Hash));

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CompareComputedHashFromNullStringWithHash()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(string.Empty).Will(Return.Value(Hash));

            Assert.IsTrue(this.testee.CompareWithHash((string)null, Hash));

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CompareComputedHashFromStringWithNullHash()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(Text).Will(Return.Value(Hash));

            Assert.Throws<ArgumentNullException>(
                () => this.testee.CompareWithHash(Text, (string)null));
        }

        [Test]
        public void CompareComputedHashFromStringWithHashFromReader()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(Text).Will(Return.Value(Hash));
            Expect.Once.On(this.hashReader).Method("GetString").Will(Return.Value(Hash));

            Assert.IsTrue(this.testee.CompareWithHash(Text, this.hashReader));

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CompareComputedHashFromNullStringWithHashFromReader()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(Text).Will(Return.Value(Hash));
            Expect.Once.On(this.hashReader).Method("GetString").Will(Return.Value(Hash));

            Assert.IsTrue(this.testee.CompareWithHash(Text, this.hashReader));

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CompareComputedHashFromStringWithHashFromNullReader()
        {
            Expect.Once.On(this.algorithm).Method("ComputeHash").With(Text).Will(Return.Value(Hash));
            Expect.Once.On(this.hashReader).Method("GetString").Will(Return.Value(Hash));

            Assert.Throws<ArgumentNullException>(
                () => this.testee.CompareWithHash(Text, (ITextReader)null));
        }

        [Test]
        public void CompareComputedHashFromReaderWithHash()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));

            Assert.IsTrue(this.testee.CompareWithHash(this.reader, Hash));

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CompareComputedHashFromNullReaderWithHash()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));

            Assert.Throws<ArgumentNullException>(
                () => this.testee.CompareWithHash((ITextReader)null, Hash));
        }

        [Test]
        public void CompareComputedHashFromReaderWithNullHash()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));

            Assert.Throws<ArgumentNullException>(
                () => this.testee.CompareWithHash(this.reader, (string)null));
        }

        [Test]
        public void CompareComputedHashFromReaderWithHashFromReader()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));
            Expect.Once.On(this.hashReader).Method("GetString").Will(Return.Value(Hash));

            Assert.IsTrue(this.testee.CompareWithHash(this.reader, this.hashReader));

            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CompareComputedHashFromNullReaderWithHashFromReader()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));
            Expect.Once.On(this.hashReader).Method("GetString").Will(Return.Value(Hash));

            Assert.Throws<ArgumentNullException>(
                () => this.testee.CompareWithHash((ITextReader)null, this.hashReader));
        }

        [Test]
        public void CompareComputedHashFromReaderWithHashFromNullReader()
        {
            Stream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(Text));

            Expect.Once.On(this.reader).Method("GetStream").Will(Return.Value(stream));
            Expect.Once.On(this.algorithm).Method("ComputeHashFromStream").Will(Return.Value(Hash));
            Expect.Once.On(this.hashReader).Method("GetString").Will(Return.Value(Hash));

            Assert.Throws<ArgumentNullException>(
                () => this.testee.CompareWithHash(this.reader, (ITextReader)null));
        }

        [Test]
        public void GetHashFromStringWithoutHashAlgorithm()
        {
            Assert.Throws<ArgumentNullException>(
                () => new HashService(null));
        }
    }
}
