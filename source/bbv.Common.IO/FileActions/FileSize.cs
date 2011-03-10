//-------------------------------------------------------------------------------
// <copyright file="FileSize.cs" company="bbv Software Services AG">
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
    using System.Collections;
    using System.IO;

    /// <summary>
    /// Calculates size of a directory tree.
    /// </summary>
    /// <example>
    /// Normally use static method on <see cref="FileActionCommand.GetSizeRecursiv(string)"/>:
    /// <code>
    /// int size = FileActionCommand.FileSize(sourcePath, searchPattern, excludeDirs);
    /// </code>
    /// In special cases you may use:
    /// <code>
    /// FileSize fs = new FileSize(sourcePath, searchPattern, excludeDirs);
    /// fs.ExecuteRecursiv(sourcePath);
    /// int size = fs.Size;
    /// </code>
    /// </example>
    public class FileSize : FileActionCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSize"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        public FileSize(string sourcePath)
            : base(sourcePath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSize"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="searchPattern">
        /// Only files that match this pattern are taken for the command.
        /// </param>
        public FileSize(string sourcePath, string searchPattern)
            : base(sourcePath, searchPattern)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSize"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="excludeDirs">
        /// Directories to exclude from move.
        /// </param>
        public FileSize(string sourcePath, ArrayList excludeDirs)
            : base(sourcePath, excludeDirs)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSize"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="searchPattern">
        /// Only files that match this pattern are taken for the command.
        /// </param>
        /// <param name="excludeDirs">
        /// Directories to exclude from move.
        /// </param>
        public FileSize(string sourcePath, string searchPattern, ArrayList excludeDirs)
            : base(sourcePath, searchPattern, excludeDirs)
        {
        }

        /// <summary>
        /// Gets size of the directory tree.
        /// </summary>
        public long Size { get; private set; }

        /// <summary>
        /// Adds size the given file.
        /// </summary>
        /// <param name="fileName">Found file</param>
        public override void FileAction(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            this.Size += fi.Length;
        }
    }
}