//-------------------------------------------------------------------------------
// <copyright file="HashService.cs" company="bbv Software Services AG">
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

    using bbv.Common.IO;

    /// <summary>
    /// Contains methods for hash generating from strings or from the specified input data.
    /// </summary>
    public class HashService : IHashService
    {
        private readonly IHashAlgorithm algorithm;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HashService"/> class with an Hashalgorithm.
        /// </summary>
        /// <param name="algorithm">The hashalgorithm.</param>
        /// <exception cref="ArgumentNullException"><paramref name="algorithm"/>is null</exception>
        public HashService(IHashAlgorithm algorithm)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException("algorithm", "Can't initialize hashservice without a hashalgorithm!");
            }

            this.algorithm = algorithm;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Computes the hash value from a text.
        /// </summary>
        /// <param name="text">The text, null is equal as an empty string.</param>
        /// <returns>Hash as a hex-string.</returns>
        public string GetHashFromString(string text)
        {
            string source = text;
            if (source == null)
            {
                source = string.Empty;
            }

            return this.algorithm.ComputeHash(source);
        }

        /// <summary>
        /// Computes the hash value from the content of a file.
        /// </summary>
        /// <param name="reader">The reader for the input data.</param>
        /// <returns>Hash as a hex-string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/>is null</exception>
        public string GetHashFromReader(ITextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            using (Stream stream = reader.GetStream())
            {
                return this.algorithm.ComputeHashFromStream(stream);
            }
        }

        /// <summary>
        /// Writes the hash file (hash.extension) from a text.
        /// </summary>
        /// <param name="text">The text, null is equal as an empty string.</param>
        /// <param name="writer">The textwriter for the output.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>is null</exception>
        public void WriteHashFromString(string text, ITextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            string hash = this.GetHashFromString(text);
            writer.Write(hash);
        }

        /// <summary>
        /// Writes the hash value file (filename.extension) from the content of a File.
        /// </summary>
        /// <param name="reader">The reader for the input data.</param>
        /// <param name="writer">The textwriter for the output.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/>is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>is null</exception>
        public void WriteHash(ITextReader reader, ITextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            string hash = this.GetHashFromReader(reader);
            writer.Write(hash);
        }

        /// <summary>
        /// Computes a hash value of the text and compares it with the other hash value.
        /// </summary>
        /// <param name="text">The text to compare the hash against.</param>
        /// <param name="hash">The hash to be compared against the text.</param>
        /// <returns>
        /// true if the computed hash is equal to the other, otherwise false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/>is null</exception>
        public bool CompareWithHash(string text, string hash)
        {
            if (hash == null)
            {
                throw new ArgumentNullException("hash");
            }

            return this.GetHashFromString(text).Equals(hash, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Compares the generated hash value from the text with the hash value from the file.
        /// </summary>
        /// <param name="text">The text for generating the hash.</param>
        /// <param name="reader">The textreader for the hash value.</param>
        /// <returns>
        /// true if the hash from the file is equal to the computed hash.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/>is null</exception>
        public bool CompareWithHash(string text, ITextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return this.CompareWithHash(text, reader.GetString());
        }

        /// <summary>
        /// Compares the computed hash value with the hash value from the input data.
        /// </summary>
        /// <param name="reader">The textreader for computing the hash.</param>
        /// <param name="hashReader">The textreader for the hash value.</param>
        /// <returns>
        /// true if the hash from the textreader is equal to the computed hash.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/>is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="hashReader"/>is null</exception>
        public bool CompareWithHash(ITextReader reader, ITextReader hashReader)
        {
            if (hashReader == null)
            {
                throw new ArgumentNullException("hashReader");
            }

            string hash = this.GetHashFromReader(reader);
            return hash.Equals(hashReader.GetString(), StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Compares the computed hash value with the hash value from the input data.
        /// </summary>
        /// <param name="reader">The textreader for computing the hash.</param>
        /// <param name="hash">The hash value.</param>
        /// <returns>
        /// true if the computed hash from the textreader is equal to the other hash.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/>is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/>is null</exception>
        public bool CompareWithHash(ITextReader reader, string hash)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (hash == null)
            {
                throw new ArgumentNullException("hash");
            }

            return this.GetHashFromReader(reader).Equals(hash, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion
    }
}