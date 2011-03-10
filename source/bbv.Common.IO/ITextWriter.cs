//-------------------------------------------------------------------------------
// <copyright file="ITextWriter.cs" company="bbv Software Services AG">
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
    using System.IO;

    /// <summary>
    /// Contains methods to write text to the specified output.
    /// </summary>
    public interface ITextWriter
    {
        /// <summary>
        /// Writes the text to the specified output.
        /// </summary>
        /// <param name="content">The filecontent.</param>
        void Write(string content);

        /// <summary>
        /// Writes the stream to the specified output.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bufferSize">The size of the buffer.</param>
        void Write(Stream stream, int bufferSize);
    }
}