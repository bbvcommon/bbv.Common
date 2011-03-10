//-------------------------------------------------------------------------------
// <copyright file="FileActionCommand.cs" company="bbv Software Services AG">
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

    // REFACTOR:
    //
    // - modify code to match coding guidlines

    /// <summary>
    /// Base class for recursive file actions. The action is executed for each file in the source path.
    /// </summary>
    public abstract class FileActionCommand
    {
        private readonly string searchPattern = "*";
        private readonly ArrayList excludeDirs = new ArrayList();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileActionCommand"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start the action.</param>
        protected FileActionCommand(string sourcePath)
        {
            this.SourcePath = sourcePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileActionCommand"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start the action.</param>
        /// <param name="excludeDirs">Directories to exclude from action.</param>
        protected FileActionCommand(string sourcePath, ArrayList excludeDirs)
        {
            this.SourcePath = sourcePath;
            this.excludeDirs = excludeDirs;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileActionCommand"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start the action.</param>
        /// <param name="searchPattern">Only files that match this pattern are taken for the command.</param>
        protected FileActionCommand(string sourcePath, string searchPattern)
        {
            this.SourcePath = sourcePath;
            this.searchPattern = searchPattern;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileActionCommand"/> class.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start the action.</param>
        /// <param name="searchPattern">Only files that match this pattern are taken for the command.</param>
        /// <param name="excludeDirs">Directories to exclude from action.</param>
        protected FileActionCommand(string sourcePath, string searchPattern, ArrayList excludeDirs)
        {
            this.SourcePath = sourcePath;
            this.searchPattern = searchPattern;
            this.excludeDirs = excludeDirs;
        }

        /// <summary>
        /// This event is fired after FileAction has completed a file.
        /// </summary>
        public event FileActionExecutedEventHandler FileActionExecuted;

        /// <summary>
        /// Gets or sets a value indicating whether to ignore locked files or not. If locked files are not ignored, an exception is thrown.
        /// </summary>
        public bool IgnoreLockedFiles { get; set; }

        /// <summary>
        /// Gets or sets the Source Path at which to start the action.
        /// </summary>
        protected string SourcePath { get; set; }

        /// <summary>
        /// Moves files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        /// <param name="destinationPath">Destination Path</param>
        public static void MoveRecursiv(string sourcePath, string destinationPath)
        {
            FileMove fm = new FileMove(sourcePath, destinationPath);
            fm.ExecuteRecursiv(sourcePath);
        }

        /// <summary>
        /// Moves files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        /// <param name="searchPattern">Only files that match this pattern are taken for the command.</param>
        /// <param name="destinationPath">Destination Path</param>
        public static void MoveRecursiv(string sourcePath, string searchPattern, string destinationPath)
        {
            FileMove fm = new FileMove(sourcePath, searchPattern, destinationPath);
            fm.ExecuteRecursiv(sourcePath);
        }

        /// <summary>
        /// Moves files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        /// <param name="excludeDirs">Directories to exclude from move.</param>
        /// <param name="destinationPath">Destination Path</param>
        public static void MoveRecursiv(string sourcePath, ArrayList excludeDirs, string destinationPath)
        {
            FileMove fm = new FileMove(sourcePath, excludeDirs, destinationPath);
            fm.ExecuteRecursiv(sourcePath);
        }

        /// <summary>
        /// Moves files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to move.</param>
        /// <param name="searchPattern">Only files that match this pattern are taken for the command.</param>
        /// <param name="excludeDirs">Directories to exclude from move.</param>
        /// <param name="destinationPath">Destination Path</param>
        public static void MoveRecursiv(string sourcePath, string searchPattern, ArrayList excludeDirs, string destinationPath)
        {
            FileMove fm = new FileMove(sourcePath, searchPattern, excludeDirs, destinationPath);
            fm.ExecuteRecursiv(sourcePath);
        }

        /// <summary>
        /// Copy files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to copy.</param>
        /// <param name="destinationPath">Destination Path</param>
        /// <param name="overwrite">True if existing files should be overriden.</param>
        public static void CopyRecursiv(string sourcePath, string destinationPath, bool overwrite)
        {
            FileCopy fc = new FileCopy(sourcePath, destinationPath, overwrite);
            fc.ExecuteRecursiv(sourcePath);
        }

        /// <summary>
        /// Copy files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to copy.</param>
        /// <param name="searchPattern">Only files that match this pattern are copied.</param>
        /// <param name="destinationPath">Destination Path</param>
        /// <param name="overwrite">True if existing files should be overriden.</param>
        public static void CopyRecursiv(string sourcePath, string searchPattern, string destinationPath, bool overwrite)
        {
            FileCopy fc = new FileCopy(sourcePath, searchPattern, destinationPath, overwrite);
            fc.ExecuteRecursiv(sourcePath);
        }

        /// <summary>
        /// Copy files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to copy.</param>
        /// <param name="excludeDirs">Directories to exclude from copy.</param>
        /// <param name="destinationPath">Destination Path</param>
        /// <param name="overwrite">True if existing files should be overriden.</param>
        public static void CopyRecursiv(string sourcePath, ArrayList excludeDirs, string destinationPath, bool overwrite)
        {
            FileCopy fc = new FileCopy(sourcePath, excludeDirs, destinationPath, overwrite);
            fc.ExecuteRecursiv(sourcePath);
        }

        /// <summary>
        /// Copy files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to copy.</param>
        /// <param name="searchPattern">Only files that match this pattern are copied.</param>
        /// <param name="excludeDirs">Directories to exclude from copy.</param>
        /// <param name="destinationPath">Destination Path</param>
        /// <param name="overwrite">True if existing files should be overriden.</param>
        public static void CopyRecursiv(string sourcePath, string searchPattern, ArrayList excludeDirs, string destinationPath, bool overwrite)
        {
            FileCopy fc = new FileCopy(sourcePath, searchPattern, excludeDirs, destinationPath, overwrite);
            fc.ExecuteRecursiv(sourcePath);
        }

        /// <summary>
        /// Count files and directories recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to count.</param>
        /// <returns>Returns number of files an directories in directory tree.</returns>
        public static long NumberOfDirsAndFiles(string sourcePath)
        {
            NumberOfDirsAndFiles nof = new NumberOfDirsAndFiles(sourcePath);
            nof.ExecuteRecursiv(sourcePath);
            return nof.Count;
        }

        /// <summary>
        /// Count files and directories recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to count.</param>
        /// <param name="searchPattern">Only files that match this pattern are counted.</param>
        /// <returns>Returns number of files an directories in directory tree.</returns>
        public static long NumberOfDirsAndFiles(string sourcePath, string searchPattern)
        {
            NumberOfDirsAndFiles nof = new NumberOfDirsAndFiles(sourcePath, searchPattern);
            nof.ExecuteRecursiv(sourcePath);
            return nof.Count;
        }

        /// <summary>
        /// Count files and directories recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to count.</param>
        /// <param name="excludeDirs">Directories to exclude from count.</param>
        /// <returns>Returns number of files an directories in directory tree.</returns>
        public static long NumberOfDirsAndFiles(string sourcePath, ArrayList excludeDirs)
        {
            NumberOfDirsAndFiles nof = new NumberOfDirsAndFiles(sourcePath, excludeDirs);
            nof.ExecuteRecursiv(sourcePath);
            return nof.Count;
        }

        /// <summary>
        /// Count files and directories recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to count.</param>
        /// <param name="searchPattern">Only files that match this pattern are counted.</param>
        /// <param name="excludeDirs">Directories to exclude from count.</param>
        /// <returns>Returns number of files an directories in directory tree.</returns>
        public static long NumberOfDirsAndFiles(string sourcePath, string searchPattern, ArrayList excludeDirs)
        {
            NumberOfDirsAndFiles nof = new NumberOfDirsAndFiles(sourcePath, searchPattern, excludeDirs);
            nof.ExecuteRecursiv(sourcePath);
            return nof.Count;
        }

        /// <summary>
        /// Gets files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to get.</param>
        /// <returns>Returns an arraylist of filenames.</returns>
        public static ArrayList GetFilesRecursiv(string sourcePath)
        {
            FileGetter fg = new FileGetter(sourcePath);
            fg.ExecuteRecursiv(sourcePath);
            return fg.Files;
        }

        /// <summary>
        /// Gets files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to get.</param>
        /// <param name="searchPattern">Only files that match this pattern are getted.</param>
        /// <returns>Returns an arraylist of filenames.</returns>
        public static ArrayList GetFilesRecursiv(string sourcePath, string searchPattern)
        {
            FileGetter fg = new FileGetter(sourcePath, searchPattern);
            fg.ExecuteRecursiv(sourcePath);
            return fg.Files;
        }

        /// <summary>
        /// Gets files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to get.</param>
        /// <param name="excludeDirs">Directories to exclude from get.</param>
        /// <returns>Returns an arraylist of filenames.</returns>
        public static ArrayList GetFilesRecursiv(string sourcePath, ArrayList excludeDirs)
        {
            FileGetter fg = new FileGetter(sourcePath, excludeDirs);
            fg.ExecuteRecursiv(sourcePath);
            return fg.Files;
        }

        /// <summary>
        /// Gets files recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to get.</param>
        /// <param name="searchPattern">Only files that match this pattern are getted.</param>
        /// <param name="excludeDirs">Directories to exclude from get.</param>
        /// <returns>Returns an arraylist of filenames.</returns>
        public static ArrayList GetFilesRecursiv(string sourcePath, string searchPattern, ArrayList excludeDirs)
        {
            FileGetter fg = new FileGetter(sourcePath, searchPattern, excludeDirs);
            fg.ExecuteRecursiv(sourcePath);
            return fg.Files;
        }

        /// <summary>
        /// Gets file size recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to calculate filesize.</param>
        /// <returns>Returns size of directory tree.</returns>
        public static long GetSizeRecursiv(string sourcePath)
        {
            FileSize fs = new FileSize(sourcePath);
            fs.ExecuteRecursiv(sourcePath);
            return fs.Size;
        }

        /// <summary>
        /// Gets file size recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to calculate filesize.</param>
        /// <param name="searchPattern">Only files that match this pattern are counted.</param>
        /// <returns>Returns size of directory tree.</returns>
        public static long GetSizeRecursiv(string sourcePath, string searchPattern)
        {
            FileSize fs = new FileSize(sourcePath, searchPattern);
            fs.ExecuteRecursiv(sourcePath);
            return fs.Size;
        }

        /// <summary>
        /// Gets file size recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to calculate filesize.</param>
        /// <param name="excludeDirs">Directories to exclude from filesize.</param>
        /// <returns>Returns size of directory tree.</returns>
        public static long GetSizeRecursiv(string sourcePath, ArrayList excludeDirs)
        {
            FileSize fs = new FileSize(sourcePath, excludeDirs);
            fs.ExecuteRecursiv(sourcePath);
            return fs.Size;
        }

        /// <summary>
        /// Gets file size recursively.
        /// </summary>
        /// <param name="sourcePath">Source Path at which to start to calculate filesize.</param>
        /// <param name="searchPattern">Only files that match this pattern are counted.</param>
        /// <param name="excludeDirs">Directories to exclude from filesize.</param>
        /// <returns>Returns size of directory tree.</returns>
        public static long GetSizeRecursiv(string sourcePath, string searchPattern, ArrayList excludeDirs)
        {
            FileSize fs = new FileSize(sourcePath, searchPattern, excludeDirs);
            fs.ExecuteRecursiv(sourcePath);
            return fs.Size;
        }

        /// <summary>
        /// Override this method to execute the action.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public abstract void FileAction(string fileName);

        /// <summary>
        /// Called for each file in the source path and each underling directory <see cref="FileAction"/>.
        /// This is the recursive method.
        /// </summary>
        /// <param name="path">The full path..</param>
        public virtual void ExecuteRecursiv(string path)
        {
            string[] files = Directory.GetFiles(path, this.searchPattern);
            foreach (string file in files)
            {
                this.FileAction(file);
                this.OnFileActionExecuted(file);
            }

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                string nextDir = dir.Substring(path.Length);
                if (!this.excludeDirs.Contains(nextDir))
                {
                    string newPath = path + nextDir;
                    this.ExecuteRecursiv(newPath);
                }
            }
        }

        /// <summary>
        /// Returns the difference between the to given path.
        /// </summary>
        /// <param name="path">The path to compare against.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>The path difference.</returns>
        protected static string GetDiffPath(string path, string filename)
        {
            string dir1 = Path.GetDirectoryName(path);
            string dir2 = Path.GetDirectoryName(filename);
            if (dir1 == dir2)
            {
                // file is in the root of the path
                return Path.GetFileName(filename);
            }
            else 
            {
                string diff = dir2.Substring(dir1.Length + 1);
                return Path.Combine(diff, Path.GetFileName(filename));
            }
        }

        /// <summary>
        /// Fires <see cref="FileActionExecuted"/> event.
        /// </summary>
        /// <param name="fileName">Name of the file that has been processed.</param>
        protected virtual void OnFileActionExecuted(string fileName)
        {
            if (this.FileActionExecuted != null)
            {
                this.FileActionExecuted(this, new FileActionExecutedEventArgs(fileName));
            }
        }
    }
}
