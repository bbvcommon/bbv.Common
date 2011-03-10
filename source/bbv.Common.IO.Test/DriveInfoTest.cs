//-------------------------------------------------------------------------------
// <copyright file="DriveInfoTest.cs" company="bbv Software Services AG">
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
    using NUnit.Framework;

    /// <summary>
    /// Tests the <see cref="DriveUtilities"/> class.
    /// </summary>
    [TestFixture]
    public class DriveInfoTest
    {
        /// <summary>
        /// Transforms the bytes in readable form.
        /// </summary>
        [Test]
        public void TransformBytesInReadableForm()
        {
            // to make sure that the method is doing the formatting right 
            // the formatting should be tested for all double numbers which would take way too long
            // to be acceptable. still, if required the whole double-range can be tested.
            Assert.AreEqual("1 Byte", DriveUtilities.FormatByteSize(1), "Formatted string was not correct.");
            Assert.AreEqual("10 Bytes", DriveUtilities.FormatByteSize(10), "Formatted string was not correct.");
            Assert.AreEqual("4.9 KB", DriveUtilities.FormatByteSize(5000), "Formatted string was not correct.");
            Assert.AreEqual("49.5 MB", DriveUtilities.FormatByteSize(51943000), "Formatted string was not correct.");
            Assert.AreEqual("2.7 GB", DriveUtilities.FormatByteSize(2873641824), "Formatted string was not correct.");
            Assert.AreEqual("13.6 TB", DriveUtilities.FormatByteSize(14995116277760), "Formatted string was not correct.");
            Assert.AreEqual("-10000 Bytes", DriveUtilities.FormatByteSize(-10000), "Formatted string was not correct.");
        }
    }
}