//-------------------------------------------------------------------------------
// <copyright file="FileGetter.cs" company="bbv Software Services AG">
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
    /// Gets all files inside a directory tree.
    /// </summary>
    /// <example>
    /// Normally use static method <see cref="FileActionCommand.GetFilesRecursiv(string)"/>:
    /// <code>
    /// ArrayList files = FileActionCommand.GetFilesRecursiv(sourcePath, searchPattern, excludeDirs);
    /// </code>
    /// In special cases you may use:
    /// <code>
    /// FileGetter fg = new FileGetter(sourcePath, searchPattern, excludeDirs);
    /// fg.ExecuteRecursiv(sourcePath);
    /// ArrayList files = fg.Files;
    /// </code>
    /// </example>
    public class FileGetter : FileActionCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileGetter"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        public FileGetter(string sourcePath)
            : base(sourcePath)
        {
            this.Files = new ArrayList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileGetter"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="searchPattern">
        /// Only files that match this pattern are taken for the command.
        /// </param>
        public FileGetter(string sourcePath, string searchPattern)
            : base(sourcePath, searchPattern)
        {
            this.Files = new ArrayList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileGetter"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="excludeDirs">
        /// Directories to exclude from move.
        /// </param>
        public FileGetter(string sourcePath, ArrayList excludeDirs)
            : base(sourcePath, excludeDirs)
        {
            this.Files = new ArrayList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileGetter"/> class. 
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
        public FileGetter(string sourcePath, string searchPattern, ArrayList excludeDirs)
            : base(sourcePath, searchPattern, excludeDirs)
        {
            this.Files = new ArrayList();
        }

        /// <summary>
        /// Gets all fiels in the tree.
        /// </summary>
        public ArrayList Files { get; private set; }

        /// <summary>
        /// Called each time a file is found.
        /// </summary>
        /// <param name="fileName">Found file</param>
        public override void FileAction(string fileName)
        {
            this.Files.Add(Path.GetFullPath(fileName));
        }
    }
}