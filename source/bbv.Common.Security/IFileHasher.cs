//-------------------------------------------------------------------------------
// <copyright file="IFileHasher.cs" company="bbv Software Services AG">
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
    /// <summary>
    /// Provides functionality to read and write hash codes for files
    /// </summary>
    public interface IFileHasher
    {
        /// <summary>
        /// Writes a file containing the sha1 hash value of the file given. The file is named <paramref name="filePath"/>.sha1
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void WriteSha1HashFile(string filePath);

        /// <summary>
        /// Reads the sha1 hash value from the file named <paramref name="filePath"/>.sha1
        /// </summary>
        /// <param name="filePath">The file path of the origin file, not the hash file.</param>
        /// <returns>The sha1 hash value from the file given</returns>
        string ReadSha1HashFile(string filePath);

        /// <summary>
        /// Calculates the sha1 hash value for the file path given
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The sha1 hash value for the file path given</returns>
        string CalculateSha1HashValue(string filePath);
    }
}