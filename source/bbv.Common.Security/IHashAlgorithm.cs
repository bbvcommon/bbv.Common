//-------------------------------------------------------------------------------
// <copyright file="IHashAlgorithm.cs" company="bbv Software Services AG">
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
    using System.IO;

    /// <summary>
    /// Contains methods to compute a hash value from the specified input data.
    /// </summary>
    public interface IHashAlgorithm
    {
        /// <summary>
        /// Gets or sets the format of the hash value.
        /// </summary>
        /// <value>The format, {0:x2}.</value>
        string Format { get; set; }

        /// <summary>
        /// Computes the hash value for the specified string.
        /// </summary>
        /// <param name="text">The text for the input data.</param>
        /// <returns>Hash value as a string.</returns>
        string ComputeHash(string text);

        /// <summary>
        /// Computes the hash for the specified Stream.
        /// </summary>
        /// <param name="stream">The stream for the input data.</param>
        /// <returns>Hash value as a string.</returns>
        string ComputeHashFromStream(Stream stream);
    }
}