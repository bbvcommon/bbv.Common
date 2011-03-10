//-------------------------------------------------------------------------------
// <copyright file="IDirectoryAccessExtension.cs" company="bbv Software Services AG">
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
    using System.Security.AccessControl;

    /// <summary>
    /// Interface for directory access extensions
    /// </summary>
    public interface IDirectoryAccessExtension
    {
        /// <see cref="IDirectoryAccess.Exists(System.String)" />
        void BeginExists(string path);

        /// <see cref="IDirectoryAccess.Exists(System.String)" />
        void EndExists(bool result, string path);

        /// <see cref="IDirectoryAccess.Exists(System.String)" />
        void FailExists(ref Exception exception);

        /// <see cref="IDirectoryAccess.CreateDirectory(System.String)" />
        void BeginCreateDirectory(string path);

        /// <see cref="IDirectoryAccess.CreateDirectory(System.String)" />
        void EndCreateDirectory(DirectoryInfo result, string path);

        /// <see cref="IDirectoryAccess.CreateDirectory(System.String)" />
        void FailCreateDirectory(ref Exception exception);

        /// <see cref="IDirectoryAccess.CreateDirectory(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void BeginCreateDirectory(string path, DirectorySecurity directorySecurity);

        /// <see cref="IDirectoryAccess.CreateDirectory(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void EndCreateDirectory(DirectoryInfo result, string path, DirectorySecurity directorySecurity);

        /// <see cref="IDirectoryAccess.Delete(System.String,System.Boolean)" />
        void BeginDelete(string path, bool recursive);

        /// <see cref="IDirectoryAccess.Delete(System.String,System.Boolean)" />
        void EndDelete(string path, bool recursive);

        /// <see cref="IDirectoryAccess.Delete(System.String,System.Boolean)" />
        void FailDelete(ref Exception exception);

        /// <see cref="IDirectoryAccess.Delete(System.String)" />
        void BeginDelete(string path);

        /// <see cref="IDirectoryAccess.Delete(System.String)" />
        void EndDelete(string path);

        /// <see cref="IDirectoryAccess.GetFiles(System.String)" />
        void BeginGetFiles(string path);

        /// <see cref="IDirectoryAccess.GetFiles(System.String)" />
        void EndGetFiles(string[] result, string path);

        /// <see cref="IDirectoryAccess.GetFiles(System.String)" />
        void FailGetFiles(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetFiles(System.String,System.String)" />
        void BeginGetFiles(string path, string searchPattern);

        /// <see cref="IDirectoryAccess.GetFiles(System.String,System.String)" />
        void EndGetFiles(string[] result, string path, string searchPattern);

        /// <see cref="IDirectoryAccess.GetFiles(System.String,System.String,System.IO.SearchOption)" />
        void BeginGetFiles(string path, string searchPattern, SearchOption searchOption);

        /// <see cref="IDirectoryAccess.GetFiles(System.String,System.String,System.IO.SearchOption)" />
        void EndGetFiles(string[] result, string path, string searchPattern, SearchOption searchOption);

        /// <see cref="IDirectoryAccess.GetDirectories(System.String)" />
        void BeginGetDirectories(string path);

        /// <see cref="IDirectoryAccess.GetDirectories(System.String)" />
        void EndGetDirectories(string[] result, string path);

        /// <see cref="IDirectoryAccess.GetDirectories(System.String)" />
        void FailGetDirectories(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetDirectories(System.String,System.String)" />
        void BeginGetDirectories(string path, string searchPattern);

        /// <see cref="IDirectoryAccess.GetDirectories(System.String,System.String)" />
        void EndGetDirectories(string[] result, string path, string searchPattern);

        /// <see cref="IDirectoryAccess.GetDirectories(System.String,System.String,System.IO.SearchOption)" />
        void BeginGetDirectories(string path, string searchPattern, SearchOption searchOption);

        /// <see cref="IDirectoryAccess.GetDirectories(System.String,System.String,System.IO.SearchOption)" />
        void EndGetDirectories(string[] result, string path, string searchPattern, SearchOption searchOption);

        /// <see cref="IDirectoryAccess.GetAccessControl(System.String)" />
        void BeginGetAccessControl(string path);

        /// <see cref="IDirectoryAccess.GetAccessControl(System.String)" />
        void EndGetAccessControl(DirectorySecurity result, string path);

        /// <see cref="IDirectoryAccess.GetAccessControl(System.String)" />
        void FailGetAccessControl(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetAccessControl(System.String,System.Security.AccessControl.AccessControlSections)" />
        void BeginGetAccessControl(string path, AccessControlSections includeSections);

        /// <see cref="IDirectoryAccess.GetAccessControl(System.String,System.Security.AccessControl.AccessControlSections)" />
        void EndGetAccessControl(DirectorySecurity result, string path, AccessControlSections includeSections);

        /// <see cref="IDirectoryAccess.GetCreationTime(System.String)" />
        void BeginGetCreationTime(string path);

        /// <see cref="IDirectoryAccess.GetCreationTime(System.String)" />
        void EndGetCreationTime(DateTime result, string path);

        /// <see cref="IDirectoryAccess.GetCreationTime(System.String)" />
        void FailGetCreationTime(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetCreationTimeUtc(System.String)" />
        void BeginGetCreationTimeUtc(string path);

        /// <see cref="IDirectoryAccess.GetCreationTimeUtc(System.String)" />
        void EndGetCreationTimeUtc(DateTime result, string path);

        /// <see cref="IDirectoryAccess.GetCreationTimeUtc(System.String)" />
        void FailGetCreationTimeUtc(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetCurrentDirectory" />
        void BeginGetCurrentDirectory();

        /// <see cref="IDirectoryAccess.GetCurrentDirectory" />
        void EndGetCurrentDirectory(string result);

        /// <see cref="IDirectoryAccess.GetCurrentDirectory" />
        void FailGetCurrentDirectory(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetDirectoryRoot(System.String)" />
        void BeginGetDirectoryRoot(string path);

        /// <see cref="IDirectoryAccess.GetDirectoryRoot(System.String)" />
        void EndGetDirectoryRoot(string result, string path);

        /// <see cref="IDirectoryAccess.GetDirectoryRoot(System.String)" />
        void FailGetDirectoryRoot(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetFileSystemEntries(System.String)" />
        void BeginGetFileSystemEntries(string path);

        /// <see cref="IDirectoryAccess.GetFileSystemEntries(System.String)" />
        void EndGetFileSystemEntries(string[] result, string path);

        /// <see cref="IDirectoryAccess.GetFileSystemEntries(System.String)" />
        void FailGetFileSystemEntries(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetFileSystemEntries(System.String,System.String)" />
        void BeginGetFileSystemEntries(string path, string searchPattern);

        /// <see cref="IDirectoryAccess.GetFileSystemEntries(System.String,System.String)" />
        void EndGetFileSystemEntries(string[] result, string path, string searchPattern);

        /// <see cref="IDirectoryAccess.GetLastAccessTime(System.String)" />
        void BeginGetLastAccessTime(string path);

        /// <see cref="IDirectoryAccess.GetLastAccessTime(System.String)" />
        void EndGetLastAccessTime(DateTime result, string path);

        /// <see cref="IDirectoryAccess.GetLastAccessTime(System.String)" />
        void FailGetLastAccessTime(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetLastAccessTimeUtc(System.String)" />
        void BeginGetLastAccessTimeUtc(string path);

        /// <see cref="IDirectoryAccess.GetLastAccessTimeUtc(System.String)" />
        void EndGetLastAccessTimeUtc(DateTime result, string path);

        /// <see cref="IDirectoryAccess.GetLastAccessTimeUtc(System.String)" />
        void FailGetLastAccessTimeUtc(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetLastWriteTime(System.String)" />
        void BeginGetLastWriteTime(string path);

        /// <see cref="IDirectoryAccess.GetLastWriteTime(System.String)" />
        void EndGetLastWriteTime(DateTime result, string path);

        /// <see cref="IDirectoryAccess.GetLastWriteTime(System.String)" />
        void FailGetLastWriteTime(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetLastWriteTimeUtc(System.String)" />
        void BeginGetLastWriteTimeUtc(string path);

        /// <see cref="IDirectoryAccess.GetLastWriteTimeUtc(System.String)" />
        void EndGetLastWriteTimeUtc(DateTime result, string path);

        /// <see cref="IDirectoryAccess.GetLastWriteTimeUtc(System.String)" />
        void FailGetLastWriteTimeUtc(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetLogicalDrives" />
        void BeginGetLogicalDrives();

        /// <see cref="IDirectoryAccess.GetLogicalDrives" />
        void EndGetLogicalDrives(string[] result);

        /// <see cref="IDirectoryAccess.GetLogicalDrives" />
        void FailGetLogicalDrives(ref Exception exception);

        /// <see cref="IDirectoryAccess.GetParent(System.String)" />
        void BeginGetParent(string path);

        /// <see cref="IDirectoryAccess.GetParent(System.String)" />
        void EndGetParent(DirectoryInfo result, string path);

        /// <see cref="IDirectoryAccess.GetParent(System.String)" />
        void FailGetParent(ref Exception exception);

        /// <see cref="IDirectoryAccess.Move(System.String,System.String)" />
        void BeginMove(string sourceDirName, string destDirName);

        /// <see cref="IDirectoryAccess.Move(System.String,System.String)" />
        void EndMove(string sourceDirName, string destDirName);

        /// <see cref="IDirectoryAccess.Move(System.String,System.String)" />
        void FailMove(ref Exception exception);

        /// <see cref="IDirectoryAccess.SetAccessControl(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void BeginSetAccessControl(string path, DirectorySecurity directorySecurity);

        /// <see cref="IDirectoryAccess.SetAccessControl(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void EndSetAccessControl(string path, DirectorySecurity directorySecurity);

        /// <see cref="IDirectoryAccess.SetAccessControl(System.String,System.Security.AccessControl.DirectorySecurity)" />
        void FailSetAccessControl(ref Exception exception);

        /// <see cref="IDirectoryAccess.SetCreationTime(System.String,System.DateTime)" />
        void BeginSetCreationTime(string path, DateTime creationTime);

        /// <see cref="IDirectoryAccess.SetCreationTime(System.String,System.DateTime)" />
        void EndSetCreationTime(string path, DateTime creationTime);

        /// <see cref="IDirectoryAccess.SetCreationTime(System.String,System.DateTime)" />
        void FailSetCreationTime(ref Exception exception);

        /// <see cref="IDirectoryAccess.SetCreationTimeUtc(System.String,System.DateTime)" />
        void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <see cref="IDirectoryAccess.SetCreationTimeUtc(System.String,System.DateTime)" />
        void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <see cref="IDirectoryAccess.SetCreationTimeUtc(System.String,System.DateTime)" />
        void FailSetCreationTimeUtc(ref Exception exception);

        /// <see cref="IDirectoryAccess.SetCurrentDirectory(System.String)" />
        void BeginSetCurrentDirectory(string path);

        /// <see cref="IDirectoryAccess.SetCurrentDirectory(System.String)" />
        void EndSetCurrentDirectory(string path);

        /// <see cref="IDirectoryAccess.SetCurrentDirectory(System.String)" />
        void FailSetCurrentDirectory(ref Exception exception);

        /// <see cref="IDirectoryAccess.SetLastAccessTime(System.String,System.DateTime)" />
        void BeginSetLastAccessTime(string path, DateTime lastAccessTime);

        /// <see cref="IDirectoryAccess.SetLastAccessTime(System.String,System.DateTime)" />
        void EndSetLastAccessTime(string path, DateTime lastAccessTime);

        /// <see cref="IDirectoryAccess.SetLastAccessTime(System.String,System.DateTime)" />
        void FailSetLastAccessTime(ref Exception exception);

        /// <see cref="IDirectoryAccess.SetLastAccessTimeUtc(System.String,System.DateTime)" />
        void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <see cref="IDirectoryAccess.SetLastAccessTimeUtc(System.String,System.DateTime)" />
        void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <see cref="IDirectoryAccess.SetLastAccessTimeUtc(System.String,System.DateTime)" />
        void FailSetLastAccessTimeUtc(ref Exception exception);

        /// <see cref="IDirectoryAccess.SetLastWriteTime(System.String,System.DateTime)" />
        void BeginSetLastWriteTime(string path, DateTime lastWriteTime);

        /// <see cref="IDirectoryAccess.SetLastWriteTime(System.String,System.DateTime)" />
        void EndSetLastWriteTime(string path, DateTime lastWriteTime);

        /// <see cref="IDirectoryAccess.SetLastWriteTime(System.String,System.DateTime)" />
        void FailSetLastWriteTime(ref Exception exception);

        /// <see cref="IDirectoryAccess.SetLastWriteTimeUtc(System.String,System.DateTime)" />
        void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <see cref="IDirectoryAccess.SetLastWriteTimeUtc(System.String,System.DateTime)" />
        void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <see cref="IDirectoryAccess.SetLastWriteTimeUtc(System.String,System.DateTime)" />
        void FailSetLastWriteTimeUtc(ref Exception exception);
    }
}