//-------------------------------------------------------------------------------
// <copyright file="TextFileWriter.cs" company="bbv Software Services AG">
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
    using System.Text;

    /// <summary>
    /// Contains methods to write a text to the specified file.
    /// </summary>
    public class TextFileWriter : ITextWriter
    {
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFileWriter"/> class.
        /// </summary>
        /// <param name="path">The path to write the text into.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/>is null</exception>
        public TextFileWriter(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "Can't initialize TextFileWriter without a path!");
            }

            this.path = path;
            this.Encoding = Encoding.Default;
        }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Writes the the to a specified file.
        /// </summary>
        /// <param name="content">The filecontent.</param>
        public void Write(string content)
        {
            using (StreamWriter writer = new StreamWriter(this.path))
            {
                writer.Write(content, this.Encoding);
            }
        }

        /// <summary>
        /// Writes the stream to the specified output.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bufferSize">The size of the buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/>If not greater than 0.</exception>
        public void Write(Stream stream, int bufferSize)
        {
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize", bufferSize, "Must be greater than 0!");
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                using (StreamWriter writer = new StreamWriter(this.path))
                {
                    char[] buffer = new char[bufferSize];
                    while (reader.Peek() >= 0)
                    {
                        writer.Write(buffer, 0, reader.Read(buffer, 0, buffer.Length));
                    }

                    writer.Flush();
                }
            }
        }
    }
}