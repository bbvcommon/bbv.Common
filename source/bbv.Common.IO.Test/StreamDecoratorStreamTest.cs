//-------------------------------------------------------------------------------
// <copyright file="StreamDecoratorStreamTest.cs" company="bbv Software Services AG">
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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Ports;

    using NUnit.Framework;

    /// <summary>
    /// Tests the implementation of <see cref="StreamDecoratorStream"/>.
    /// </summary>
    [TestFixture]
    public class StreamDecoratorStreamTest
    {
        /// <summary>
        /// The object that is tested.
        /// </summary>
        private StreamDecoratorTestStream streamDecorator;

        /// <summary>
        /// The stream that is decorated for the read and write tests
        /// </summary>
        private MemoryStream memoryStream;

        /// <summary>
        /// Used to create the serialStream
        /// </summary>
        private SerialPort serialPort;

        /// <summary>
        /// The stream that is decorated for the timeout tests. 
        /// </summary>
        private Stream serialStream;

        /// <summary>
        /// The stream that is decorated to test the property accesses. 
        /// </summary>
        private TestStream testStream;

        private bool canRun;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            foreach (var portName in SerialPort.GetPortNames())
            {
                // Should be  if (portName.Equals("COM1", StringComparison.OrdinalIgnoreCase))
                if (portName == "Com1")
                {
                    this.canRun = true;
                    return;
                }
            }

            this.canRun = false;
        }

        /// <summary>
        /// Sets up each test.
        /// Creates the object under test and the test streams.
        /// </summary>
        public void SetUp()
        {
            if (!this.canRun)
            {
                throw new IgnoreException("Com1 not defined");
            }

            this.serialPort = new SerialPort();
            this.serialPort.PortName = "Com1";
            this.serialPort.Open();
            this.serialStream = this.serialPort.BaseStream;
            this.memoryStream = new MemoryStream();
            this.streamDecorator = new StreamDecoratorTestStream(this.memoryStream);
            this.testStream = new TestStream();
        }

        /// <summary>
        /// Tears down each test.
        /// Closed the serial port.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (this.serialPort != null)
            {
                this.serialPort.Close();
            }
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.Write"/> forwards the written data to the decorated stream.
        /// </summary>
        [Test]
        public void WriteForwardsDataToDecoratedStream()
        {
            this.SetUp();

            byte[] data = ByteArrayHelper.CreateByteArray(6);
            this.streamDecorator.Write(data, 1, 4);
            ByteArrayHelper.CompareByteArrays(data, this.memoryStream.ToArray(), 1, 4);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.WriteByte"/> forwards the written data to the decorated stream.
        /// </summary>
        [Test]
        public void WriteByteForwardsDataToDecoratedStream()
        {
            this.SetUp();

            byte[] data = ByteArrayHelper.CreateByteArray(6);
            for (int i = 1; i < 5; i++)
            {
                this.streamDecorator.WriteByte(data[i]);
            }

            ByteArrayHelper.CompareByteArrays(data, this.memoryStream.ToArray(), 1, 4);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.BeginWrite"/> forwards the written data to the decorated stream.
        /// </summary>
        [Test]
        public void AsynchronousWriteForwardsDataToDecoratedStream()
        {
            this.SetUp();

            byte[] data = ByteArrayHelper.CreateByteArray(6);
            IAsyncResult asyncResult = this.streamDecorator.BeginWrite(data, 1, 4, null, null);
            this.streamDecorator.EndWrite(asyncResult);
            ByteArrayHelper.CompareByteArrays(data, this.memoryStream.ToArray(), 1, 4);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.Read"/> gets the data from the decorated stream.
        /// </summary>
        [Test]
        public void ReadGetsDataFromDecoratedStream()
        {
            this.SetUp();

            byte[] data = ByteArrayHelper.CreateByteArray(6);
            this.memoryStream.Write(data, 0, 6);
            Assert.AreEqual(this.streamDecorator.Position, 6);
            Assert.AreEqual(this.streamDecorator.Length, 6);
            this.streamDecorator.Position = 0;
            byte[] readData = new byte[8];
            this.streamDecorator.Read(readData, 1, 6);
            ByteArrayHelper.CompareByteArrays(readData, data, 1, 6);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.ReadByte"/> gets the data from the decorated stream.
        /// </summary>
        [Test]
        public void ReadByteGetsDataFromDecoratedStream()
        {
            this.SetUp();

            byte[] data = ByteArrayHelper.CreateByteArray(6);
            this.memoryStream.Write(data, 0, 6);
            this.streamDecorator.Position = 0;
            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(data[i], this.streamDecorator.ReadByte());
            }
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.BeginRead"/> gets the data from the decorated stream.
        /// </summary>
        [Test]
        public void AsynchronousReadGetsDataFromDecoratedStream()
        {
            this.SetUp();

            byte[] data = ByteArrayHelper.CreateByteArray(6);
            this.memoryStream.Write(data, 0, 6);
            this.memoryStream.Position = 0;
            byte[] readData = new byte[8];
            IAsyncResult asyncResult = this.streamDecorator.BeginRead(readData, 1, 6, null, null);
            this.streamDecorator.EndRead(asyncResult);
            ByteArrayHelper.CompareByteArrays(readData, data, 1, 6);
        }

        /// <summary>
        /// Tests if getting and setting the <see cref=" StreamDecoratorStream.ReadTimeout"/> gets / sets the
        /// value from the decorated device.
        /// </summary>
        [Test]
        public void ReadTimeoutGetInformationFromDecoratedDevice()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.serialStream);
            this.serialStream.ReadTimeout = 100;
            Assert.AreEqual(this.streamDecorator.ReadTimeout, 100);
            this.serialStream.ReadTimeout = 200;
            Assert.AreEqual(this.streamDecorator.ReadTimeout, 200);
            this.streamDecorator.ReadTimeout = 300;
            Assert.AreEqual(this.serialStream.ReadTimeout, 300);
            this.streamDecorator.ReadTimeout = 400;
            Assert.AreEqual(this.serialStream.ReadTimeout, 400);
        }

        /// <summary>
        /// Tests if getting and setting the <see cref=" StreamDecoratorStream.WriteTimeout"/> gets / sets the
        /// value from the decorated device.
        /// </summary>        
        [Test]
        public void WriteTimeoutGetInformationFromDecoratedDevice()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.serialStream);
            this.serialStream.WriteTimeout = 100;
            Assert.AreEqual(this.streamDecorator.WriteTimeout, 100);
            this.serialStream.WriteTimeout = 200;
            Assert.AreEqual(this.streamDecorator.WriteTimeout, 200);
            this.streamDecorator.WriteTimeout = 300;
            Assert.AreEqual(this.serialStream.WriteTimeout, 300);
            this.streamDecorator.WriteTimeout = 400;
            Assert.AreEqual(this.serialStream.WriteTimeout, 400);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.CanSeek"/> gets the information from the decorated device.
        /// </summary>        
        [Test]
        public void CanSeekGetInformationFromDecoratedDevice()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.testStream);
            this.testStream.SetCanSeek(true);
            Assert.IsTrue(this.streamDecorator.CanSeek);
            this.testStream.SetCanSeek(false);
            Assert.IsFalse(this.streamDecorator.CanSeek);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.CanRead"/> gets the information from the decorated device.
        /// </summary>        
        [Test]
        public void CanReadGetInformationFromDecoratedDevice()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.testStream);
            this.testStream.SetCanRead(true);
            Assert.IsTrue(this.streamDecorator.CanRead);
            this.testStream.SetCanRead(false);
            Assert.IsFalse(this.streamDecorator.CanRead);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.CanWrite"/> gets the information from the decorated device.
        /// </summary>        
        [Test]
        public void CanWriteGetInformationFromDecoratedDevice()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.testStream);
            this.testStream.SetCanWrite(true);
            Assert.IsTrue(this.streamDecorator.CanWrite);
            this.testStream.SetCanWrite(false);
            Assert.IsFalse(this.streamDecorator.CanWrite);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.ToString"/> gets the information from the decorated device.
        /// </summary>        
        [Test]
        public void ToStringGetInformationFromDecoratedDevice()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.testStream);
            Assert.AreEqual(this.streamDecorator.ToString(), this.testStream.ToString());
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.ToString"/> gets the information from the decorated device.
        /// </summary>        
        [Test]
        public void SetLengthSetsLengthOfDecoratedDevice()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.testStream);
            this.streamDecorator.SetLength(100);
            Assert.AreEqual(this.testStream.Length, 100);
            this.streamDecorator.SetLength(200);
            Assert.AreEqual(this.testStream.Length, 200);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.Seek"/> request is forwarded to the decorated stream.
        /// </summary>        
        [Test]
        public void SeekRequestForwardedToDecoratedStream()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.testStream);
            this.streamDecorator.Seek(999, SeekOrigin.End);
            Assert.IsTrue(this.testStream.SeekWasCalled);
            Assert.AreEqual(this.testStream.SeekOffset, 999);
            Assert.AreEqual(this.testStream.SeekOrigin, SeekOrigin.End);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.Flush"/> request is forwarded to the decorated stream.
        /// </summary>        
        [Test]
        public void FlushRequestForwardedToDecoratedStream()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.testStream);
            this.streamDecorator.Flush();
            Assert.IsTrue(this.testStream.FlushWasCalled);
        }

        /// <summary>
        /// Tests if <see cref=" StreamDecoratorStream.Close"/> request is forwarded to the decorated stream.
        /// </summary>        
        [Test]
        public void CloseRequestForwardedToDecoratedStream()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(this.testStream);
            this.streamDecorator.Close();
            Assert.IsTrue(this.testStream.CloseWasCalled);
        }

        /// <summary>
        /// Tests if a <see cref="InvalidOperationException"/> is called when no stream is assigned and an
        /// operation is executed.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NoStreamExceptionThrownWhenNoStreamIsAssigned()
        {
            this.SetUp();

            this.streamDecorator = new StreamDecoratorTestStream(null);
            this.streamDecorator.Position = 10;
        }

        /// <summary>
        /// A test implementation of <see cref="Stream"/>.
        /// </summary>
        private class TestStream : Stream
        {
            /// <summary>
            /// The value that is returned for <see cref="CanRead"/>.
            /// </summary>
            private bool canRead;

            /// <summary>
            /// The value that is returned for <see cref="CanSeek"/>.
            /// </summary>
            private bool canSeek;

            /// <summary>
            /// The value that is returned for <see cref="CanWrite"/>.
            /// </summary>
            private bool canWrite;

            /// <summary>
            /// The value that is returned for <see cref="Length"/>.
            /// </summary>
            private long length;

            /// <summary>
            /// The value that is returned / set for <see cref="Position"/>.
            /// </summary>
            private long position;

            /// <summary>
            /// Gets a value indicating whether the current stream supports reading.
            /// </summary>
            /// <value></value>
            /// <returns>true if the stream supports reading; otherwise, false.</returns>
            public override bool CanRead
            {
                get
                {
                    return this.canRead;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports seeking.
            /// </summary>
            /// <value></value>
            /// <returns>true if the stream supports seeking; otherwise, false.</returns>
            public override bool CanSeek
            {
                get
                {
                    return this.canSeek;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the current stream supports writing.
            /// </summary>
            /// <value></value>
            /// <returns>true if the stream supports writing; otherwise, false.</returns>
            public override bool CanWrite
            {
                get
                {
                    return this.canWrite;
                }
            }

            /// <summary>
            /// Gets the length in bytes of the stream.
            /// </summary>
            /// <value></value>
            /// <returns>A long value representing the length of the stream in bytes.</returns>
            /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking.
            /// </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.
            /// </exception>
            public override long Length
            {
                get
                {
                    return this.length;
                }
            }

            /// <summary>
            /// Gets or sets the position within the current stream.
            /// </summary>
            /// <value></value>
            /// <returns>The current position within the stream.</returns>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.
            /// </exception>
            public override long Position
            {
                get
                {
                    return this.position;
                }

                set
                {
                    this.position = value;
                }
            }

            public bool SeekWasCalled { get; private set; }

            public long SeekOffset { get; private set; }

            public SeekOrigin SeekOrigin { get; private set; }

            public bool FlushWasCalled { get; private set; }

            public bool CloseWasCalled { get; private set; }

            public void SetCanRead(bool value)
            {
                this.canRead = value;
            }

            public void SetCanSeek(bool value)
            {
                this.canSeek = value;
            }

            public void SetCanWrite(bool value)
            {
                this.canWrite = value;
            }

            /// <summary>
            /// Writes a sequence of bytes to the current stream and advances the
            /// current position within this stream by the number of bytes written.
            /// </summary>
            /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.
            /// </param>
            /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current
            /// stream.</param>
            /// <param name="count">The number of bytes to be written to the current stream.</param>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.
            /// </exception>
            /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
            /// <exception cref="T:System.ArgumentException">The sum of offset and count is greater than the buffer length.
            /// </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
            /// </summary>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            public override void Flush()
            {
                this.FlushWasCalled = true;
            }

            /// <summary>
            /// Sets the position within the current stream.
            /// </summary>
            /// <param name="offset">A byte offset relative to the origin parameter.</param>
            /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"></see> indicating the reference 
            /// point used to obtain the new position.</param>
            /// <returns>
            /// The new position within the current stream.
            /// </returns>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream 
            /// is constructed from a pipe or console output. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. 
            /// </exception>
            public override long Seek(long offset, SeekOrigin origin)
            {
                this.SeekWasCalled = true;
                this.SeekOffset = offset;
                this.SeekOrigin = origin;
                return 1010;
            }

            /// <summary>
            /// Closes the current stream and releases any resources (such as sockets and file handles) associated with 
            /// the current stream.
            /// </summary>
            public override void Close()
            {
                this.CloseWasCalled = true;
            }

            /// <summary>
            /// Sets the length of the current stream.
            /// </summary>
            /// <param name="value">The desired length of the current stream in bytes.</param>
            /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, 
            /// such as if the stream is constructed from a pipe or console output. </exception>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. 
            /// </exception>
            public override void SetLength(long value)
            {
                this.length = value;
            }

            /// <summary>
            /// Reads a sequence of bytes from the current stream and advances the 
            /// position within the stream by the number of bytes read.
            /// </summary>
            /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte 
            /// array with the values between offset and (offset + count - 1) replaced by the bytes read from the current 
            /// source.</param>
            /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the 
            /// current stream.</param>
            /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
            /// <returns>
            /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that
            /// many bytes are not currently available, or zero (0) if the end of the stream has been reached.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length.
            /// </exception>
            /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.
            /// </exception>
            /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
            /// <exception cref="T:System.ArgumentNullException">buffer is null. </exception>
            /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative. </exception>
            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override string ToString()
            {
                return "TestOutput";
            }
        }

        /// <summary>
        /// A concrete implementation of the abstract class <see cref="StreamDecoratorStream"/>
        /// </summary>
        private class StreamDecoratorTestStream : StreamDecoratorStream
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StreamDecoratorStreamTest.StreamDecoratorTestStream"/> class. 
            /// </summary>
            /// <param name="decoratedStream">
            /// The decorated stream.
            /// </param>
            public StreamDecoratorTestStream(Stream decoratedStream)
                : base(decoratedStream)
            {
            }
        }
    }
}