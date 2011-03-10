//-------------------------------------------------------------------------------
// <copyright file="IPathAccessExtension.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;

    /// <summary>
    /// Interface for path access extensions
    /// </summary>
    public interface IPathAccessExtension
    {
        /// <see cref="IPathAccess.GetDirectoryName"/>
        void BeginGetDirectoryName(string path);

        /// <see cref="IPathAccess.GetDirectoryName"/>
        void EndGetDirectoryName(string result, string path);

        /// <see cref="IPathAccess.GetDirectoryName"/>
        void FailGetDirectoryName(ref Exception exception);

        /// <see cref="IPathAccess.GetFileName"/>
        void BeginGetFileName(string path);

        /// <see cref="IPathAccess.GetFileName"/>
        void EndGetFileName(string result, string path);

        /// <see cref="IPathAccess.GetFileName"/>
        void FailGetFileName(ref Exception exception);

        /// <see cref="IPathAccess.GetFileNameWithoutExtension"/>
        void BeginGetFileNameWithoutExtension(string path);

        /// <see cref="IPathAccess.GetFileNameWithoutExtension"/>
        void EndGetFileNameWithoutExtension(string result, string path);

        /// <see cref="IPathAccess.GetFileNameWithoutExtension"/>
        void FailGetFileNameWithoutExtension(ref Exception exception);

        /// <see cref="IPathAccess.Combine"/>
        void BeginCombine(string path1, string path2);

        /// <see cref="IPathAccess.Combine"/>
        void EndCombine(string result, string path1, string path2);

        /// <see cref="IPathAccess.Combine"/>
        void FailCombine(ref Exception exception);

        /// <see cref="IPathAccess.GetRandomFileName"/>
        void BeginGetRandomFileName();

        /// <see cref="IPathAccess.GetRandomFileName"/>
        void EndGetRandomFileName(string result);

        /// <see cref="IPathAccess.GetRandomFileName"/>
        void FailGetRandomFileName(ref Exception exception);

        /// <see cref="IPathAccess.ChangeExtension"/>
        void BeginChangeExtension(string path, string extension);

        /// <see cref="IPathAccess.ChangeExtension"/>
        void EndChangeExtension(string result, string path, string extension);

        /// <see cref="IPathAccess.ChangeExtension"/>
        void FailChangeExtension(ref Exception exception);

        /// <see cref="IPathAccess.GetExtension"/>
        void BeginGetExtension(string path);

        /// <see cref="IPathAccess.GetExtension"/>
        void EndGetExtension(string result, string path);

        /// <see cref="IPathAccess.GetExtension"/>
        void FailGetExtension(ref Exception exception);

        /// <see cref="IPathAccess.GetFullPath"/>
        void BeginGetFullPath(string path);

        /// <see cref="IPathAccess.GetFullPath"/>
        void EndGetFullPath(string result, string path);

        /// <see cref="IPathAccess.GetFullPath"/>
        void FailGetFullPath(ref Exception exception);

        /// <see cref="IPathAccess.GetInvalidFileNameChars"/>
        void BeginGetInvalidFileNameChars();

        /// <see cref="IPathAccess.GetInvalidFileNameChars"/>
        void EndGetInvalidFileNameChars(char[] result);

        /// <see cref="IPathAccess.GetInvalidFileNameChars"/>
        void FailGetInvalidFileNameChars(ref Exception exception);

        /// <see cref="IPathAccess.GetInvalidPathChars"/>
        void BeginGetInvalidPathChars();

        /// <see cref="IPathAccess.GetInvalidPathChars"/>
        void EndGetInvalidPathChars(char[] result);

        /// <see cref="IPathAccess.GetInvalidPathChars"/>
        void FailGetInvalidPathChars(ref Exception exception);

        /// <see cref="IPathAccess.GetPathRoot"/>
        void BeginGetPathRoot(string path);

        /// <see cref="IPathAccess.GetPathRoot"/>
        void EndGetPathRoot(string result, string path);

        /// <see cref="IPathAccess.GetPathRoot"/>
        void FailGetPathRoot(ref Exception exception);

        /// <see cref="IPathAccess.GetTempFileName"/>
        void BeginGetTempFileName();

        /// <see cref="IPathAccess.GetTempFileName"/>
        void EndGetTempFileName(string result);

        /// <see cref="IPathAccess.GetTempFileName"/>
        void FailGetTempFileName(ref Exception exception);

        /// <see cref="IPathAccess.GetTempPath"/>
        void BeginGetTempPath();

        /// <see cref="IPathAccess.GetTempPath"/>
        void EndGetTempPath(string result);

        /// <see cref="IPathAccess.GetTempPath"/>
        void FailGetTempPath(ref Exception exception);

        /// <see cref="IPathAccess.HasExtension"/>
        void BeginHasExtension(string path);

        /// <see cref="IPathAccess.HasExtension"/>
        void EndHasExtension(bool result, string path);

        /// <see cref="IPathAccess.HasExtension"/>
        void FailHasExtension(ref Exception exception);

        /// <see cref="IPathAccess.IsPathRooted"/>
        void BeginIsPathRooted(string path);

        /// <see cref="IPathAccess.IsPathRooted"/>
        void EndIsPathRooted(bool result, string path);

        /// <see cref="IPathAccess.IsPathRooted"/>
        void FailIsPathRooted(ref Exception exception);
    }
}