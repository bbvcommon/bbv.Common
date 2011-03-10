//-------------------------------------------------------------------------------
// <copyright file="ByteArrayHelper.cs" company="bbv Software Services AG">
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

    using NUnit.Framework;

    /// <summary>
    /// Provides functionality to handle byte arrays
    /// </summary>
    internal static class ByteArrayHelper
    {
        /// <summary>
        /// The random generator that is internally used.
        /// </summary>
        private static readonly Random random = new Random(15865);

        /// <summary>
        /// Compares two byte arrays.
        /// </summary>
        /// <param name="orginal">The expected byte array.</param>
        /// <param name="copy">The byte array that is checked for equality.</param>
        /// <param name="orginalIndex">The first byte that is used for comparison in the expected byte array.</param>
        /// <param name="orginalLength">The umber of bytes that are checked.</param>
        public static void CompareByteArrays(byte[] orginal, byte[] copy, int orginalIndex, int orginalLength)
        {
            CompareByteArrays(orginal, orginalIndex, copy, 0, orginalLength);
            Assert.IsTrue(copy == null || copy.Length == orginalLength);
        }

        /// <summary>
        /// Compares two byte arrays.
        /// </summary>
        /// <param name="orginal">The expected byte array.</param>
        /// <param name="orginalIndex">The first byte that is used for comparison in the expected byte array.</param>
        /// <param name="copy">The byte array that is checked for equality.</param>
        /// <param name="copyIndex">The first byte that is used for comparison in the array that is checked for 
        /// equality.</param>
        /// <param name="count">The umber of bytes that are checked.</param>
        public static void CompareByteArrays(byte[] orginal, int orginalIndex, byte[] copy, int copyIndex, int count)
        {
            // Assert both null or not null
            if (orginal == null)
            {
                Assert.IsNull(copy);
                return;
            }

            Assert.IsNotNull(copy);

            // Check length
            Assert.GreaterOrEqual(copy.Length - copyIndex, count);

            // Check bytes
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(orginal[i + orginalIndex], copy[i + copyIndex]);
            }
        }

        /// <summary>
        /// Creates a random byte array using the same seed every time the tests run.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns>A pseudo random byte array.</returns>
        public static byte[] CreateByteArray(int length)
        {
            byte[] result = new byte[length];
            random.NextBytes(result);
            return result;
        }
    }
}