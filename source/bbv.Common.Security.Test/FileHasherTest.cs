//-------------------------------------------------------------------------------
// <copyright file="FileHasherTest.cs" company="bbv Software Services AG">
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
    using System.Globalization;
    using System.IO;
    using NUnit.Framework;
    using TestUtilities;

    /// <summary>
    /// Tests for the <see cref="FileHasher"/> class
    /// </summary>
    [TestFixture]
    public class FileHasherTest
    {
        /// <summary>
        /// The hash value of an empty file
        /// </summary>
        private const string EmptyFileHash = "da39a3ee5e6b4b0d3255bfef95601890afd80709";

        /// <summary>
        /// The object under test
        /// </summary>
        private FileHasher testee;

        /// <summary>
        /// The path for the hash file
        /// </summary>
        private string hashFilePath;

        /// <summary>
        /// The path for the file the be hashed
        /// </summary>
        private string filePath;

        /// <summary>
        /// Sets up the testee
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new FileHasher();
            this.filePath = Path.GetTempFileName();
            this.hashFilePath = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", this.filePath, "sha1");
        }

        /// <summary>
        /// When a hash for an existing file is requested an *.sha1 file containing the hash is created in the same directory
        /// </summary>
        [Test]
        public void WriteSha1Hash()
        {
            using (new TemporaryFileHolder(this.filePath, string.Empty))
            {
                this.testee.WriteSha1HashFile(this.filePath);

                Assert.IsTrue(File.Exists(this.hashFilePath), "The hash file was not written on disk");
            }

            File.Delete(this.hashFilePath);
        }

        /// <summary>
        /// When the sha1 for a not existing file is written a <see cref="FileNotFoundException"/> is thrown
        /// </summary>
        [Test]
        public void WriteSha1HashForNotExistingFile()
        {
            File.Delete(this.filePath);

            Assert.Throws<FileNotFoundException>(
                () => this.testee.WriteSha1HashFile(this.filePath));
        }

        /// <summary>
        /// When the sha1 hash file for a file is written and the hash file already exists, it is overwritten unless the file is locked
        /// </summary>
        [Test]
        public void WriteSha1HashWhenHashFileExists()
        {
            using (TemporaryFileHolder file = new TemporaryFileHolder(this.filePath, string.Empty), hashFile = new TemporaryFileHolder(this.hashFilePath, string.Empty))
            {
                this.testee.WriteSha1HashFile(this.filePath);
                Assert.AreEqual(EmptyFileHash, this.testee.ReadSha1HashFile(this.filePath), "The hash file was not overwritten with the correct hash value");
            }
        }

        /// <summary>
        /// When the sha1 hash file is read for an existing file the content of the file.sha1 is returned
        /// </summary>
        [Test]
        public void ReadSha1Hash()
        {
            using (TemporaryFileHolder file = new TemporaryFileHolder(this.filePath, string.Empty), hashFile = new TemporaryFileHolder(this.hashFilePath, EmptyFileHash))
            {
                string sha1 = this.testee.ReadSha1HashFile(this.filePath);
                Assert.AreEqual(EmptyFileHash, sha1, "The hash value read from the hash file is wrong");
            }
        }

        /// <summary>
        /// When the sha1 hash file is read for a non-existing file a <see cref="FileNotFoundException"/> is thrown
        /// </summary>
        [Test]
        public void ReadSha1HashForNotExistingFile()
        {
            File.Delete(this.filePath);

            Assert.Throws<FileNotFoundException>(
                () => this.testee.ReadSha1HashFile(this.filePath));
        }

        /// <summary>
        /// When the sha1 hash file is read for a non-existing hash file a <see cref="FileNotFoundException"/> is thrown
        /// </summary>
        [Test]
        public void ReadSha1HashForNotExistingHashFile()
        {
            using (new TemporaryFileHolder(this.filePath, string.Empty))
            {
                Assert.Throws<FileNotFoundException>(
                    () => this.testee.ReadSha1HashFile(this.filePath));
            }
        }

        /// <summary>
        /// The sha1 hash value for an existing file is calculated and verified for correctness
        /// </summary>
        [Test]
        public void CalculateSha1Hash()
        {
            using (new TemporaryFileHolder(this.filePath, string.Empty))
            {
                string sha1 = this.testee.CalculateSha1HashValue(this.filePath);

                Assert.AreEqual(EmptyFileHash, sha1, "The calculated hash value is wrong");
            }
        }

        /// <summary>
        /// When the sha1 hash value is calculated for a non-existing file a <see cref="FileNotFoundException"/> is thrown
        /// </summary>
        [Test]
        public void CalculateSha1HashForNotExistingFile()
        {
            File.Delete(this.filePath);

            Assert.Throws<FileNotFoundException>(
                () => this.testee.CalculateSha1HashValue(this.filePath));
        }

        /// <summary>
        /// Writes the hash file for an empty file, reads the previously written hash file and compares it with the calculated hash value
        /// </summary>
        [Test]
        public void CompareHashWithHashFile()
        {
            using (new TemporaryFileHolder(this.filePath, string.Empty))
            {
                this.testee.WriteSha1HashFile(this.filePath);
                string readHash = this.testee.ReadSha1HashFile(this.filePath);
                string calculatedHash = this.testee.CalculateSha1HashValue(this.filePath);

                Assert.AreEqual(readHash, calculatedHash, "The read and the calculated hash values are different");
            }

            File.Delete(this.hashFilePath);
        }
    }
}