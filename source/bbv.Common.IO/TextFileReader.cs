//-------------------------------------------------------------------------------
// <copyright file="TextFileReader.cs" company="bbv Software Services AG">
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

    /// <summary>
    /// Contains methods to read a file into a string or into a stream.
    /// </summary>
    public class TextFileReader : ITextReader
    {
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFileReader"/> class.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/>is null</exception>
        public TextFileReader(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "Can't initialize TextFileReader without a path!");
            }

            this.path = path;
        }

        /// <summary>
        /// Gets the content of the file as string.
        /// </summary>
        /// <returns>The file as a string.</returns>
        public string GetString()
        {
            using (StreamReader reader = new StreamReader(this.path))
            {
                return reader.ReadToEnd();
            } 
        }

        /// <summary>
        /// Gets the stream of the file.
        /// </summary>
        /// <returns>The file as a stream.</returns>
        public Stream GetStream()
        {
            return new FileStream(this.path, FileMode.Open, FileAccess.Read);
        }
    }
}