//-------------------------------------------------------------------------------
// <copyright file="MD5Algorithm.cs" company="bbv Software Services AG">
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
    /// Contains methods to compute a MD5 hash value from the specified input data.
    /// </summary>
    public class MD5Algorithm : IHashAlgorithm
    {
        /// <summary>
        /// The hex format which is used for string formatting.
        /// </summary>
        public const string HexStringFormat = "{0:x2}";

        private string format;

        /// <summary>
        /// Initializes a new instance of the <see cref="MD5Algorithm"/> class.
        /// </summary>
        public MD5Algorithm()
        {
            this.format = HexStringFormat;
        }

        /// <summary>
        /// Gets or sets the format of the hash value.
        /// </summary>
        /// <value>The format, {0:x2}.</value>
        public string Format
        {
            get
            {
                return this.format;
            }

            set
            {
                this.format = value;
            }
        }

        /// <summary>
        /// Computes the hash value for the specified string.
        /// </summary>
        /// <param name="text">The text for the input data.</param>
        /// <returns>Hash value as a string.</returns>
        public string ComputeHash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.Default.GetBytes(text));

            return this.BytesToString(bytes);
        }

        /// <summary>
        /// Computes the hash for the specified stream.
        /// </summary>
        /// <param name="stream">The stream for the input data.</param>
        /// <returns>Hash value as a string.</returns>
        public string ComputeHashFromStream(Stream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(stream);

            return this.BytesToString(bytes);
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
                sb.AppendFormat(this.format, b);
            }

            return sb.ToString();
        }
    }
}