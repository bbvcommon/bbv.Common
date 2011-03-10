//-------------------------------------------------------------------------------
// <copyright file="FileHasher.cs" company="bbv Software Services AG">
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
    using IO;

    /// <summary>
    /// Implements <see cref="IFileHasher"/>to provide functionality to read and write hash codes for files
    /// </summary>
    public class FileHasher : IFileHasher
    {
        /// <summary>
        /// The file extension for sha1 hash files
        /// </summary>
        private const string Sha1Extension = "sha1";

        /// <summary>
        /// The hash algorithm to calculate hash values
        /// </summary>
        private readonly IHashService hashService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHasher"/> class.
        /// </summary>
        public FileHasher()
        {
            this.hashService = new HashService(new SHA1Algorithm());
        }

        /// <summary>
        /// Writes a file containing the sha1 hash code of the file given. The file is named <paramref name="filePath"/>.sha1
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void WriteSha1HashFile(string filePath)
        {
            ITextReader reader = new TextFileReader(filePath);
            ITextWriter writer = new TextFileWriter(GetSha1FilePath(filePath));
            this.hashService.WriteHash(reader, writer);
        }

        /// <summary>
        /// Reads the sha1 hash value from the file named <paramref name="filePath"/>.sha1
        /// </summary>
        /// <param name="filePath">The file path of the origin file, not the hash file.</param>
        /// <returns>The sha1 hash value from the file given</returns>
        public string ReadSha1HashFile(string filePath)
        {
            ITextReader reader = new TextFileReader(GetSha1FilePath(filePath));
            return reader.GetString();
        }

        /// <summary>
        /// Calculates the sha1 hash value for the file path given
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The sha1 hash value for the file path given</returns>
        public string CalculateSha1HashValue(string filePath)
        {
            ITextReader reader = new TextFileReader(filePath);
            return this.hashService.GetHashFromReader(reader);
        }

        /// <summary>
        /// Gets the file path of the sha1 hash file for a given origin file path
        /// </summary>
        /// <param name="filePath">The origin file path.</param>
        /// <returns>The file path of the sha1 hash file</returns>
        private static string GetSha1FilePath(string filePath)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", filePath, Sha1Extension);
        }
    }
}