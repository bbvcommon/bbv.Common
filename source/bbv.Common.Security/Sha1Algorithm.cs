//-------------------------------------------------------------------------------
// <copyright file="Sha1Algorithm.cs" company="bbv Software Services AG">
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
    using System.Collections;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Contains methods to compute sha 1 algorithm from input data
    /// </summary>
    public class SHA1Algorithm : IHashAlgorithm
    {
        /// <summary>
        /// The format string for hex representation 
        /// </summary>
        public const string HexStringFormat = "{0:x2}";

        /// <summary>
        /// Initializes a new instance of the <see cref="SHA1Algorithm"/> class.
        /// </summary>
        public SHA1Algorithm()
        {
            this.Format = HexStringFormat;
        }

        /// <summary>
        /// Gets or sets the format of the hash value.
        /// </summary>
        /// <value>The format, {0:x2}.</value>
        public string Format { get; set; }

        /// <summary>
        /// Computes the hash value for the specified string.
        /// </summary>
        /// <param name="text">The text for the input data.</param>
        /// <returns>Hash value as a string.</returns>
        public string ComputeHash(string text)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(Encoding.Default.GetBytes(text));

            return this.BytesToString(hash);
        }

        /// <summary>
        /// Computes the hash for the specified Stream.
        /// </summary>
        /// <param name="stream">The stream for the input data.</param>
        /// <returns>Hash value as a string.</returns>
        public string ComputeHashFromStream(Stream stream)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(stream);

            return this.BytesToString(hash);
        }

        /// <summary>
        /// Convert an array of bytes to a string in the specified format.
        /// </summary>
        /// <param name="bytes">The array of bytes.</param>
        /// <returns>String value of the byte array.</returns>
        private string BytesToString(IEnumerable bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.AppendFormat(this.Format, b);
            }

            return sb.ToString();
        }
    }
}