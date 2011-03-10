//-------------------------------------------------------------------------------
// <copyright file="DirectoryAccess.cs" company="bbv Software Services AG">
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

namespace bbv.Common.IO.Internals
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.AccessControl;

    /// <summary>
    /// Wrapper class which simplifies the access to directories.
    /// </summary>
    public class DirectoryAccess : IDirectoryAccess, IExtensionProvider<IDirectoryAccessExtension>
    {
        private readonly List<IDirectoryAccessExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryAccess"/> class.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public DirectoryAccess(IEnumerable<IDirectoryAccessExtension> extensions)
        {
            this.extensions = extensions.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<IDirectoryAccessExtension> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <inheritdoc />
        public bool Exists(string path)
        {
            return this.SurroundWithExtension(() => Directory.Exists(path), path);
        }

        /// <inheritdoc />
        public IDirectoryInfoAccess CreateDirectory(string path)
        {
            var directoryInfo = this.SurroundWithExtension(() => Directory.CreateDirectory(path), path);
            return new DirectoryInfoAccess(directoryInfo);
        }

        /// <inheritdoc />
        public IDirectoryInfoAccess CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            var directoryInfo = this.SurroundWithExtension(() => Directory.CreateDirectory(path, directorySecurity), path, directorySecurity);
            return new DirectoryInfoAccess(directoryInfo);
        }

        /// <inheritdoc />
        public void Delete(string path, bool recursive)
        {
            this.SurroundWithExtension(() => Directory.Delete(path, recursive), path, recursive);
        }

        /// <inheritdoc />
        public void Delete(string path)
        {
            this.SurroundWithExtension(() => Directory.Delete(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFiles(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetFiles(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFiles(string path, string searchPattern)
        {
            return this.SurroundWithExtension(() => Directory.GetFiles(path, searchPattern), path, searchPattern);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return this.SurroundWithExtension(() => Directory.GetFiles(path, searchPattern, searchOption), path, searchPattern, searchOption);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetDirectories(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetDirectories(path), path);
        }

        /// <inheritdoc />
        public DirectorySecurity GetAccessControl(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetAccessControl(path), path);
        }

        /// <inheritdoc />
        public DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return this.SurroundWithExtension(() => Directory.GetAccessControl(path, includeSections), path, includeSections);
        }

        /// <inheritdoc />
        public DateTime GetCreationTime(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetCreationTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetCreationTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetCreationTimeUtc(path), path);
        }

        /// <inheritdoc />
        public string GetCurrentDirectory()
        {
            return this.SurroundWithExtension(() => Directory.GetCurrentDirectory());
        }

        /// <inheritdoc />
        public IEnumerable<string> GetDirectories(string path, string searchPattern)
        {
            return this.SurroundWithExtension(() => Directory.GetDirectories(path, searchPattern), path, searchPattern);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return this.SurroundWithExtension(() => Directory.GetDirectories(path, searchPattern, searchOption), path, searchPattern, searchOption);
        }

        /// <inheritdoc />
        public string GetDirectoryRoot(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetDirectoryRoot(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFileSystemEntries(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetFileSystemEntries(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFileSystemEntries(string path, string searchPattern)
        {
            return this.SurroundWithExtension(() => Directory.GetFileSystemEntries(path, searchPattern), path, searchPattern);
        }

        /// <inheritdoc />
        public DateTime GetLastAccessTime(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetLastAccessTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastAccessTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetLastAccessTimeUtc(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastWriteTime(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetLastWriteTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastWriteTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => Directory.GetLastWriteTimeUtc(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> GetLogicalDrives()
        {
            return this.SurroundWithExtension(() => Directory.GetLogicalDrives());
        }

        /// <inheritdoc />
        public IDirectoryInfoAccess GetParent(string path)
        {
            var directoryInfo = this.SurroundWithExtension(() => Directory.GetParent(path));
            return new DirectoryInfoAccess(directoryInfo);
        }

        /// <inheritdoc />
        public void Move(string sourceDirName, string destDirName)
        {
            this.SurroundWithExtension(() => Directory.Move(sourceDirName, destDirName), sourceDirName, destDirName);
        }

        /// <inheritdoc />
        public void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            this.SurroundWithExtension(() => Directory.SetAccessControl(path, directorySecurity), path, directorySecurity);
        }

        /// <inheritdoc />
        public void SetCreationTime(string path, DateTime creationTime)
        {
            this.SurroundWithExtension(() => Directory.SetCreationTime(path, creationTime), path, creationTime);
        }

        /// <inheritdoc />
        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            this.SurroundWithExtension(() => Directory.SetCreationTimeUtc(path, creationTimeUtc), path, creationTimeUtc);
        }

        /// <inheritdoc />
        public void SetCurrentDirectory(string path)
        {
            this.SurroundWithExtension(() => Directory.SetCurrentDirectory(path), path);
        }

        /// <inheritdoc />
        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            this.SurroundWithExtension(() => Directory.SetLastAccessTime(path, lastAccessTime), path, lastAccessTime);
        }

        /// <inheritdoc />
        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            this.SurroundWithExtension(() => Directory.SetLastAccessTimeUtc(path, lastAccessTimeUtc), path, lastAccessTimeUtc);
        }

        /// <inheritdoc />
        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            this.SurroundWithExtension(() => Directory.SetLastWriteTime(path, lastWriteTime), path, lastWriteTime);
        }

        /// <inheritdoc />
        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            this.SurroundWithExtension(() => Directory.SetLastWriteTimeUtc(path, lastWriteTimeUtc), path, lastWriteTimeUtc);
        }
    }
}