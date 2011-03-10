//-------------------------------------------------------------------------------
// <copyright file="TextFileReaderWriterTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.IO
{
    using System;
    using System.IO;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class TextFileReaderWriterTest
    {
        private string path, path2;
        private  const string content = "content";

        [SetUp]
        public void SetUp()
        {
            path = Path.GetTempFileName();
            path2 = Path.GetTempFileName();
        }

        [Test]
        public void WriteReadFile()
        {
            TextFileWriter writer = new TextFileWriter(path);
            writer.Write(content);

            TextFileReader reader = new TextFileReader(path);
            
            Assert.AreEqual(content, reader.GetString());
        }

        [Test]
        public void WriteReadFileStream()
        {
            new TextFileWriter(path).Write(content);

            using (Stream stream = new TextFileReader(path).GetStream())
            {
                new TextFileWriter(path2).Write(stream, 3);
            }
            Assert.AreEqual(content, new TextFileReader(path2).GetString());
        }

        [Test]
        public void GetSetEncoding()
        {
            TextFileWriter writer = new TextFileWriter(path);
            writer.Encoding = Encoding.ASCII;

            Assert.AreEqual(Encoding.ASCII, writer.Encoding);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TextFileReaderWithNullPath()
        {
            new TextFileReader(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TextFileWriterWithNullPath()
        {
            new TextFileWriter(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TextFileWriterWithNullStream()
        {
            TextFileWriter writer = new TextFileWriter(path);
            writer.Write(null, 2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TextFileWriterWithNullBuffersize()
        {
            TextFileWriter writer = new TextFileWriter(path);
            writer.Write( new MemoryStream( Encoding.Default.GetBytes("text") ) , 0 );
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(path))
                File.Delete(path);
            if (File.Exists(path2))
                File.Delete(path2);
        }
    }
}