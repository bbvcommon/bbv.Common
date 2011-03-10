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

    using log4net;

    using NMock2;

    using NUnit.Framework;

    /// <summary>
    /// Tests the implementation on <see cref="StreamLoggerStream"/>
    /// </summary>
    [TestFixture]
    public class StreamLoggerStreamTest
    {
        /// <summary>
        /// The <see cref="Mockery"/> that is used to create mock objects.
        /// </summary>
        private Mockery mockery;

        /// <summary>
        /// Mocked instance of <see cref="ILog"/> that is passed to the <see cref="StreamLoggerStream"/> to use for
        /// logging.
        /// </summary>
        private ILog logger;

        /// <summary>
        /// The stream that is passed to the <see cref="StreamLoggerStream"/> as the stream that is decorated.
        /// </summary>
        private MemoryStream memoryStream;

        /// <summary>
        /// The object under test.
        /// </summary>
        private StreamLoggerStream streamLoggerStream;

        /// <summary>
        /// Sets up all tests. Creates the object under test and the mocked objects.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();
            this.logger = this.mockery.NewMock<ILog>();
            this.memoryStream = new MemoryStream();
            this.streamLoggerStream = new StreamLoggerStream(this.memoryStream, this.logger, 16, 8);
        }

        /// <summary>
        /// Verifies that all expectations are met
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Tests if write forwards the data to the decorated stream and logs them to the logger.
        /// </summary>
        [Test]
        public void WriteLogsAndForwardsDataToDecoratedStream()
        {
            byte[] data = ByteArrayHelper.CreateByteArray(6);
            Expect.Once.On(this.logger).GetProperty("IsDebugEnabled").Will(Return.Value(true));
            Expect.Once.On(this.logger).Method("Debug").WithAnyArguments();
            this.streamLoggerStream.Write(data, 1, 4);
            ByteArrayHelper.CompareByteArrays(data, this.memoryStream.ToArray(), 1, 4);
        }

        /// <summary>
        /// Tests if WriteByte forwards the data to the decorated stream and logs them to the logger.
        /// </summary>
        [Test]
        public void WriteByteLogsAndForwardsDataToDecoratedStream()
        {
            byte[] data = ByteArrayHelper.CreateByteArray(1);
            Expect.Once.On(this.logger).Method("DebugFormat").WithAnyArguments();
            this.streamLoggerStream.WriteByte(data[0]);
            ByteArrayHelper.CompareByteArrays(data, this.memoryStream.ToArray(), 0, 1);
        }

        /// <summary>
        /// Tests if Read logs the read data and that the data is correctly received from the decorated stream.
        /// </summary>
        [Test]
        public void ReadLogsData()
        {
            // Create Data to read and write it to the memory stream
            byte[] data = ByteArrayHelper.CreateByteArray(6);
            this.memoryStream.Write(data, 0, 6);
            Assert.AreEqual(this.streamLoggerStream.Position, 6);
            Assert.AreEqual(this.streamLoggerStream.Length, 6);
            this.streamLoggerStream.Position = 0;

            // Set up expectancies
            Expect.Once.On(this.logger).GetProperty("IsDebugEnabled").Will(Return.Value(true));
            Expect.Once.On(this.logger).Method("Debug").WithAnyArguments();

            // Read the data and compare it to the original
            byte[] readData = new byte[8];
            this.streamLoggerStream.Read(readData, 1, 6);
            ByteArrayHelper.CompareByteArrays(readData, data, 1, 6);
        }

        /// <summary>
        /// Tests if ReadData logs the read data and that the data is correctly received from the decorated stream.
        /// </summary>
        [Test]
        public void ReadByteLogsData()
        {
            // Create Data to read and write it to the memory stream
            byte[] data = ByteArrayHelper.CreateByteArray(1);
            this.memoryStream.Write(data, 0, 1);
            this.streamLoggerStream.Position = 0;

            // Set up expectancies
            Expect.Once.On(this.logger).Method("DebugFormat").WithAnyArguments();

            // Read the data and compare it to the original
            Assert.AreEqual(data[0], this.streamLoggerStream.ReadByte());
        }

        /// <summary>
        /// Tests if the <see cref="StreamLoggerStream"/> constructor throws an <see cref="ArgumentNullException"/>
        /// when null is passed as stream.
        /// </summary>
        [Test]
        public void StreamArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new StreamLoggerStream(null, this.logger, 16, 8));
        }

        /// <summary>
        /// Tests if the <see cref="StreamLoggerStream"/> constructor throws an <see cref="ArgumentNullException"/>
        /// when null is passed as logger.
        /// </summary>
        [Test]
        public void LoggerArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new StreamLoggerStream(this.memoryStream, null, 16, 8));
        }
    }
}
