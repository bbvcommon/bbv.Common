//-------------------------------------------------------------------------------
// <copyright file="StreamLoggerStreamTest.cs" company="bbv Software Services AG">
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
    using System.IO;

    using FluentAssertions;

    using log4net;

    using Moq;

    using Xunit;

    public class StreamLoggerStreamTest
    {
        private readonly Mock<ILog> logMock;

        private readonly MemoryStream memoryStream;

        private readonly StreamLoggerStream testee;

        public StreamLoggerStreamTest()
        {
            this.logMock = new Mock<ILog>();
            this.memoryStream = new MemoryStream();

            this.testee = new StreamLoggerStream(this.memoryStream, this.logMock.Object, 16, 8);
        }

        [Fact]
        public void WriteLogsAndForwardsDataToDecoratedStream()
        {
            byte[] data = CreateByteArray(6);

            this.logMock.Setup(log => log.IsDebugEnabled).Returns(true);
            
            this.testee.Write(data, 1, 4);
            
            this.logMock.Verify(log => log.Debug(It.IsAny<string>()));
            CompareByteArrays(data, this.memoryStream.ToArray(), 1, 4);
        }

        [Fact]
        public void WriteByteLogsAndForwardsDataToDecoratedStream()
        {
            byte[] data = CreateByteArray(1);

            this.testee.WriteByte(data[0]);

            this.logMock.Verify(log => log.DebugFormat(It.IsAny<string>(), It.IsAny<byte>()));
            CompareByteArrays(data, this.memoryStream.ToArray(), 0, 1);
        }

        [Fact]
        public void ReadLogsData()
        {
            this.logMock.Setup(log => log.IsDebugEnabled).Returns(true);

            byte[] data = CreateByteArray(6);
            this.memoryStream.Write(data, 0, 6);

            this.testee.Position.Should().Be(6);
            this.testee.Length.Should().Be(6);

            this.testee.Position = 0;

            var readData = new byte[8];

            this.testee.Read(readData, 1, 6);

            this.logMock.Verify(log => log.Debug(It.IsAny<string>()));
            CompareByteArrays(readData, data, 1, 6);
        }

        [Fact]
        public void ReadByteLogsData()
        {
            byte[] data = CreateByteArray(1);
            this.memoryStream.Write(data, 0, 1);
            this.testee.Position = 0;

            this.testee.ReadByte().Should().Be(data[0]);

            this.logMock.Verify(log => log.DebugFormat(It.IsAny<string>(), It.IsAny<object>()));
        }

        [Fact]
        public void StreamArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new StreamLoggerStream(null, this.logMock.Object, 16, 8));
        }

        [Fact]
        public void LoggerArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new StreamLoggerStream(this.memoryStream, null, 16, 8));
        }

        private static byte[] CreateByteArray(int length)
        {
            var random = new Random();

            var result = new byte[length];
            random.NextBytes(result);

            return result;
        }

        private static void CompareByteArrays(byte[] original, byte[] copy, int orginalIndex, int orginalLength)
        {
            CompareByteArrays(original, orginalIndex, copy, 0, orginalLength);
            
            Assert.True(copy == null || copy.Length == orginalLength);
        }

        /// <summary>
        /// Compares two byte arrays.
        /// </summary>
        /// <param name="original">The expected byte array.</param>
        /// <param name="originalIndex">The first byte that is used for comparison in the expected byte array.</param>
        /// <param name="copy">The byte array that is checked for equality.</param>
        /// <param name="copyIndex">The first byte that is used for comparison in the array that is checked for 
        /// equality.</param>
        /// <param name="count">The umber of bytes that are checked.</param>
        private static void CompareByteArrays(byte[] original, int originalIndex, byte[] copy, int copyIndex, int count)
        {
            // Assert both null or not null
            if (original == null)
            {
                copy.Should().BeNull("because original is null.");
                return;
            }

            copy.Should().NotBeNull();

            (copy.Length - copyIndex).Should().BeGreaterOrEqualTo(count);

            // Check bytes
            for (int i = 0; i < count; i++)
            {
                copy[i + copyIndex].Should().Be(original[i + originalIndex]);
            }
        }
    }
}
