//-------------------------------------------------------------------------------
// <copyright file="FileMove.cs" company="bbv Software Services AG">
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
    /// Moves files recursivly
    /// </summary>
    /// <example>
    /// Normally use static method on <see cref="FileActionCommand.MoveRecursiv(string, string)"/>:
    /// <code>
    /// FileActionCommand.FileMove(sourcePath, searchPattern, excludeDirs, destinationPath);
    /// </code>
    /// In special cases you may use:
    /// <code>
    /// FileMove fm = new FileMove(sourcePath, searchPattern, excludeDirs, destinationPath);
    /// fm.ExecuteRecursiv(sourcePath);
    /// </code>
    /// </example>
    public class FileMove : FileActionCommand
    {
        private readonly bool overwrite;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMove"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        /// <param name="destinationPath">Destination Path</param>
        /// <param name="overwrite">True if existing files should be overriden. Default is false.</param>
        public FileMove(string sourcePath, string destinationPath, bool overwrite)
            : base(sourcePath)
        {
            this.DestinationPath = destinationPath;
            this.overwrite = overwrite;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMove"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="destinationPath">
        /// Destination Path
        /// </param>
        public FileMove(string sourcePath, string destinationPath)
            : base(sourcePath)
        {
            this.DestinationPath = destinationPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMove"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="searchPattern">
        /// Only files that match this pattern are moved.
        /// </param>
        /// <param name="destinationPath">
        /// Destination Path
        /// </param>
        public FileMove(string sourcePath, string searchPattern, string destinationPath)
            : base(sourcePath, searchPattern)
        {
            this.DestinationPath = destinationPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMove"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="excludeDirs">
        /// Directories to exclude from move.
        /// </param>
        /// <param name="destinationPath">
        /// Destination Path
        /// </param>
        public FileMove(string sourcePath, ArrayList excludeDirs, string destinationPath)
            : base(sourcePath, excludeDirs)
        {
            this.DestinationPath = destinationPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMove"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">
        /// Source Path at which to start to move.
        /// </param>
        /// <param name="excludeDirs">
        /// Directories to exclude from move.
        /// </param>
        /// <param name="destinationPath">
        /// Destination Path
        /// </param>
        /// <param name="overwrite">
        /// True if existing files should be overriden.
        /// </param>
        public FileMove(string sourcePath, ArrayList excludeDirs, string destinationPath, bool overwrite)
            : base(sourcePath, excludeDirs)
        {
            this.DestinationPath = destinationPath;
            this.overwrite = overwrite;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMove"/> class. 
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
        /// <param name="destinationPath">
        /// Destination Path
        /// </param>
        public FileMove(string sourcePath, string searchPattern, ArrayList excludeDirs, string destinationPath)
            : base(sourcePath, searchPattern, excludeDirs)
        {
            this.DestinationPath = destinationPath;
        }

        /// <summary>
        /// Gets or sets the destination path
        /// </summary>
        protected string DestinationPath { get; set; }

        /// <summary>
        /// Moves the given file to destination path.
        /// </summary>
        /// <param name="fileName">File to move.</param>
        public override void FileAction(string fileName)
        {
            string dummy;
            this.FileAction(fileName, out dummy);
        }

        /// <summary>
        /// Moves the given file to destination path.
        /// </summary>
        /// <param name="fileName">File to move.</param>
        /// <param name="destinationFilePath">Path of the moved file (new).</param>
        protected virtual void FileAction(string fileName, out string destinationFilePath)
        {
            destinationFilePath = Path.Combine(this.DestinationPath, GetDiffPath(this.SourcePath, fileName));
            Directory.CreateDirectory(Path.GetDirectoryName(destinationFilePath));
            if (File.Exists(destinationFilePath) && this.overwrite)
            {
                // Delete to make following move working
                File.Delete(destinationFilePath);
            }

            try
            {
                File.Move(fileName, destinationFilePath);
            }
            catch (IOException)
            {
                if (!this.IgnoreLockedFiles)
                {
                    throw;
                }
            }
        }
    }
}