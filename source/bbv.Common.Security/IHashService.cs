//-------------------------------------------------------------------------------
// <copyright file="IHashService.cs" company="bbv Software Services AG">
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
    using bbv.Common.IO;

    /// <summary>
    /// Contains methods for hash generating from strings or from the specified input data.
    /// </summary>
    public interface IHashService
    {
        /// <summary>
        /// Computes the hash value from a text.
        /// </summary>
        /// <param name="text">The text, null is equal to an empty string.</param>
        /// <returns>Hash as a hex-string.</returns>
        string GetHashFromString(string text);

        /// <summary>
        /// Computes the hash value from the input data.
        /// </summary>
        /// <param name="reader">The reader for the input data.</param>
        /// <returns>Hash as a hex-string.</returns>
        string GetHashFromReader(ITextReader reader);

        /// <summary>
        /// Writes the computed hash value to the sepcified output.
        /// </summary>
        /// <param name="text">The text, null is equal to an empty string.</param>
        /// <param name="writer">The textwriter for the output.</param>
        void WriteHashFromString(string text, ITextWriter writer);

        /// <summary>
        /// Writes the computed hash value to the specified output.
        /// </summary>
        /// <param name="reader">The reader for the input data.</param>
        /// <param name="writer">The textwriter for the output.</param>
        void WriteHash(ITextReader reader, ITextWriter writer);

        /// <summary>
        /// Compares the computed hash value with the hash value.
        /// </summary>
        /// <param name="text">The text for computing the hash value.</param>
        /// <param name="hash">The hash value.</param>
        /// <returns>true if the computed hash is equal to the other.</returns>
        bool CompareWithHash(string text, string hash);

        /// <summary>
        /// Compares the computed hash value with the hash value from the input data.
        /// </summary>
        /// <param name="text">The text for computing the hash.</param>
        /// <param name="reader">The textreader for the hash value.</param>
        /// <returns>true if the hash from the textreader is equal to the computed hash.</returns>
        bool CompareWithHash(string text, ITextReader reader);

        /// <summary>
        /// Compares the computed hash value with the hash value from the input data.
        /// </summary>
        /// <param name="reader">The textreader for computing the hash.</param>
        /// <param name="hashReader">The textreader for the hash value.</param>
        /// <returns>true if the hash from the textreader is equal to the computed hash.</returns>
        bool CompareWithHash(ITextReader reader, ITextReader hashReader);

        /// <summary>
        /// Compares the computed hash value with the hash value from the input data.
        /// </summary>
        /// <param name="reader">The textreader for computing the hash.</param>
        /// <param name="hash">The hash value.</param>
        /// <returns>true if the computed hash from the textreader is equal to the other hash.</returns>
        bool CompareWithHash(ITextReader reader, string hash);
    }
}
