//-------------------------------------------------------------------------------
// <copyright file="NumberOfDirsAndFiles.cs" company="bbv Software Services AG">
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

    /// <summary>
    /// Calculates how many files and directories are in the tree.
    /// </summary>
    /// <example>
    /// Normally use static method on <see cref="FileActionCommand.NumberOfDirsAndFiles(string)"/>:
    /// <code>
    /// int nrOfFilesAndDirectories = FileActionCommand.NumberOfDirsAndFiles(sourcePath, searchPattern, excludeDirs);
    /// </code>
    /// In special cases you may use:
    /// <code>
    /// NumberOfDirsAndFiles nof = new NumberOfDirsAndFiles(sourcePath, searchPattern, excludeDirs);
    /// nof.ExecuteRecursiv(sourcePath);
    /// int nrOfFilesAndDirectories = nof.Count;
    /// </code>
    /// </example>
    public class NumberOfDirsAndFiles : FileActionCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberOfDirsAndFiles"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        public NumberOfDirsAndFiles(string sourcePath)
            : base(sourcePath)
        {
            // since initialdirectory doesn't count
            this.Count = -1; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberOfDirsAndFiles"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="searchPattern">
        /// Only files that match this pattern are taken for the command.
        /// </param>
        public NumberOfDirsAndFiles(string sourcePath, string searchPattern)
            : base(sourcePath, searchPattern)
        {
            // since initialdirectory doesn't count
            this.Count = -1; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberOfDirsAndFiles"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="excludeDirs">
        /// Directories to exclude from move.
        /// </param>
        public NumberOfDirsAndFiles(string sourcePath, ArrayList excludeDirs)
            : base(sourcePath, excludeDirs)
        {
            // since initialdirectory doesn't count
            this.Count = -1; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberOfDirsAndFiles"/> class. 
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
        public NumberOfDirsAndFiles(string sourcePath, string searchPattern, ArrayList excludeDirs)
            : base(sourcePath, searchPattern, excludeDirs)
        {
            // since initialdirectory doesn't count
            this.Count = -1; 
        }

        /// <summary>
        /// Gets number of files and directories in tree.
        /// </summary>
        public long Count { get; private set; }

        /// <summary>
        /// Called for each directory in the source path -> Count + 1
        /// </summary>
        /// <param name="path">The full path.</param>
        public override void ExecuteRecursiv(string path)
        {
            this.Count ++;
            base.ExecuteRecursiv(path);
        }

        /// <summary>
        /// Called each time a file is found -> Count + 1
        /// </summary>
        /// <param name="fileName">Found file</param>
        public override void FileAction(string fileName)
        {
            this.Count ++;
        }
    }
}