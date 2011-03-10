//-------------------------------------------------------------------------------
// <copyright file="FileCopy.cs" company="bbv Software Services AG">
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
    /// Copies files recursivly
    /// </summary>
    /// <example>
    /// Normally use static method on <see cref="FileActionCommand.CopyRecursiv(string, string, bool)"/>:
    /// <code>
    /// FileActionCommand.FileCopy(sourcePath, searchPattern, excludeDirs, destinationPath, overwrite);
    /// </code>
    /// In special cases you may use:
    /// <code>
    /// FileCopy fc = new FileCopy(sourcePath, searchPattern, excludeDirs, destinationPath, overwrite);
    /// fc.ExecuteRecursiv(sourcePath);
    /// </code>
    /// </example>
    public class FileCopy : FileActionCommand
    {
        private readonly string destinationPath;
        private readonly bool overwrite;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopy"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        /// <param name="destinationPath">Destination Path</param>
        /// <param name="overwrite">True if existing files should be overriden.</param>
        public FileCopy(string sourcePath, string destinationPath, bool overwrite)
            : base(sourcePath)
        {
            this.destinationPath = destinationPath;
            this.overwrite = overwrite;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopy"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        /// <param name="searchPattern">Only files that match this pattern are taken for the command.</param>
        /// <param name="destinationPath">Destination Path</param>
        /// <param name="overwrite">True if existing files should be overriden.</param>
        public FileCopy(string sourcePath, string searchPattern, string destinationPath, bool overwrite)
            : base(sourcePath, searchPattern)
        {
            this.destinationPath = destinationPath;
            this.overwrite = overwrite;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopy"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        /// <param name="excludeDirs">Directories to exclude from move.</param>
        /// <param name="destinationPath">Destination Path</param>
        /// <param name="overwrite">True if existing files should be overriden.</param>
        public FileCopy(string sourcePath, ArrayList excludeDirs, string destinationPath, bool overwrite)
            : base(sourcePath, excludeDirs)
        {
            this.destinationPath = destinationPath;
            this.overwrite = overwrite;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopy"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        /// <param name="searchPattern">Only files that match this pattern are taken for the command.</param>
        /// <param name="excludeDirs">Directories to exclude from move.</param>
        /// <param name="destinationPath">Destination Path</param>
        /// <param name="overwrite">True if existing files should be overriden.</param>
        public FileCopy(
            string sourcePath, string searchPattern, ArrayList excludeDirs, string destinationPath, bool overwrite)
            : base(sourcePath, searchPattern, excludeDirs)
        {
            this.destinationPath = destinationPath;
            this.overwrite = overwrite;
        }

        /// <summary>
        /// Copies the given file to destination path.
        /// </summary>
        /// <param name="fileName">File to copy.</param>
        public override void FileAction(string fileName)
        {
            string destFilename = Path.Combine(this.destinationPath, GetDiffPath(this.SourcePath, fileName));
            Directory.CreateDirectory(Path.GetDirectoryName(destFilename));
            try
            {
                File.Copy(fileName, destFilename, this.overwrite);
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