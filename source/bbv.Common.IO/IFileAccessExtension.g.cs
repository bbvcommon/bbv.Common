//-------------------------------------------------------------------------------
// <copyright file="IFileAccessExtension.cs" company="bbv Software Services AG">
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
    using System.IO;
    using System.Security.AccessControl;
    using System.Text;

    /// <summary>
    /// Interface for file access extensions
    /// </summary>
    public interface IFileAccessExtension
    {
        /// <see cref="IFileAccess.GetLastWriteTime(System.String)" />
        void BeginGetLastWriteTime(string path);

        /// <see cref="IFileAccess.GetLastWriteTime(System.String)" />
        void EndGetLastWriteTime(DateTime result, string path);

        /// <see cref="IFileAccess.GetLastWriteTime(System.String)" />
        void FailGetLastWriteTime(ref Exception exception);

        /// <see cref="IFileAccess.GetLastWriteTimeUtc(System.String)" />
        void BeginGetLastWriteTimeUtc(string path);

        /// <see cref="IFileAccess.GetLastWriteTimeUtc(System.String)" />
        void EndGetLastWriteTimeUtc(DateTime result, string path);

        /// <see cref="IFileAccess.GetLastWriteTimeUtc(System.String)" />
        void FailGetLastWriteTimeUtc(ref Exception exception);

        /// <see cref="IFileAccess.Move(System.String,System.String)" />
        void BeginMove(string sourceFileName, string destFileName);

        /// <see cref="IFileAccess.Move(System.String,System.String)" />
        void EndMove(string sourceFileName, string destFileName);

        /// <see cref="IFileAccess.Move(System.String,System.String)" />
        void FailMove(ref Exception exception);

        /// <see cref="IFileAccess.OpenRead(System.String)" />
        void BeginOpenRead(string path);

        /// <see cref="IFileAccess.OpenRead(System.String)" />
        void EndOpenRead(FileStream result, string path);

        /// <see cref="IFileAccess.OpenRead(System.String)" />
        void FailOpenRead(ref Exception exception);

        /// <see cref="IFileAccess.OpenText(System.String)" />
        void BeginOpenText(string path);

        /// <see cref="IFileAccess.OpenText(System.String)" />
        void EndOpenText(StreamReader result, string path);

        /// <see cref="IFileAccess.OpenText(System.String)" />
        void FailOpenText(ref Exception exception);

        /// <see cref="IFileAccess.OpenWrite(System.String)" />
        void BeginOpenWrite(string path);

        /// <see cref="IFileAccess.OpenWrite(System.String)" />
        void EndOpenWrite(FileStream result, string path);

        /// <see cref="IFileAccess.OpenWrite(System.String)" />
        void FailOpenWrite(ref Exception exception);

        /// <see cref="IFileAccess.Replace(System.String,System.String,System.String)" />
        void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName);

        /// <see cref="IFileAccess.Replace(System.String,System.String,System.String)" />
        void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName);

        /// <see cref="IFileAccess.Replace(System.String,System.String,System.String)" />
        void FailReplace(ref Exception exception);

        /// <see cref="IFileAccess.Replace(System.String,System.String,System.String,System.Boolean)" />
        void BeginReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);

        /// <see cref="IFileAccess.Replace(System.String,System.String,System.String,System.Boolean)" />
        void EndReplace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);

        /// <see cref="IFileAccess.SetAccessControl(System.String,System.Security.AccessControl.FileSecurity)" />
        void BeginSetAccessControl(string path, FileSecurity fileSecurity);

        /// <see cref="IFileAccess.SetAccessControl(System.String,System.Security.AccessControl.FileSecurity)" />
        void EndSetAccessControl(string path, FileSecurity fileSecurity);

        /// <see cref="IFileAccess.SetAccessControl(System.String,System.Security.AccessControl.FileSecurity)" />
        void FailSetAccessControl(ref Exception exception);

        /// <see cref="IFileAccess.SetCreationTime(System.String,System.DateTime)" />
        void BeginSetCreationTime(string path, DateTime creationTime);

        /// <see cref="IFileAccess.SetCreationTime(System.String,System.DateTime)" />
        void EndSetCreationTime(string path, DateTime creationTime);

        /// <see cref="IFileAccess.SetCreationTime(System.String,System.DateTime)" />
        void FailSetCreationTime(ref Exception exception);

        /// <see cref="IFileAccess.SetCreationTimeUtc(System.String,System.DateTime)" />
        void BeginSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <see cref="IFileAccess.SetCreationTimeUtc(System.String,System.DateTime)" />
        void EndSetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <see cref="IFileAccess.SetCreationTimeUtc(System.String,System.DateTime)" />
        void FailSetCreationTimeUtc(ref Exception exception);

        /// <see cref="IFileAccess.SetLastAccessTime(System.String,System.DateTime)" />
        void BeginSetLastAccessTime(string path, DateTime lastAccessTime);

        /// <see cref="IFileAccess.SetLastAccessTime(System.String,System.DateTime)" />
        void EndSetLastAccessTime(string path, DateTime lastAccessTime);

        /// <see cref="IFileAccess.SetLastAccessTime(System.String,System.DateTime)" />
        void FailSetLastAccessTime(ref Exception exception);

        /// <see cref="IFileAccess.SetLastAccessTimeUtc(System.String,System.DateTime)" />
        void BeginSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <see cref="IFileAccess.SetLastAccessTimeUtc(System.String,System.DateTime)" />
        void EndSetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <see cref="IFileAccess.SetLastAccessTimeUtc(System.String,System.DateTime)" />
        void FailSetLastAccessTimeUtc(ref Exception exception);

        /// <see cref="IFileAccess.SetLastWriteTimeUtc(System.String,System.DateTime)" />
        void BeginSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <see cref="IFileAccess.SetLastWriteTimeUtc(System.String,System.DateTime)" />
        void EndSetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <see cref="IFileAccess.SetLastWriteTimeUtc(System.String,System.DateTime)" />
        void FailSetLastWriteTimeUtc(ref Exception exception);

        /// <see cref="IFileAccess.Delete(System.String)" />
        void BeginDelete(string path);

        /// <see cref="IFileAccess.Delete(System.String)" />
        void EndDelete(string path);

        /// <see cref="IFileAccess.Delete(System.String)" />
        void FailDelete(ref Exception exception);

        /// <see cref="IFileAccess.Copy(System.String,System.String)" />
        void BeginCopy(string sourceFileName, string destFileName);

        /// <see cref="IFileAccess.Copy(System.String,System.String)" />
        void EndCopy(string sourceFileName, string destFileName);

        /// <see cref="IFileAccess.Copy(System.String,System.String)" />
        void FailCopy(ref Exception exception);

        /// <see cref="IFileAccess.Copy(System.String,System.String,System.Boolean)" />
        void BeginCopy(string sourceFileName, string destFileName, bool overwrite);

        /// <see cref="IFileAccess.Copy(System.String,System.String,System.Boolean)" />
        void EndCopy(string sourceFileName, string destFileName, bool overwrite);

        /// <see cref="IFileAccess.CreateText(System.String)" />
        void BeginCreateText(string path);

        /// <see cref="IFileAccess.CreateText(System.String)" />
        void EndCreateText(StreamWriter result, string path);

        /// <see cref="IFileAccess.CreateText(System.String)" />
        void FailCreateText(ref Exception exception);

        /// <see cref="IFileAccess.GetAttributes(System.String)" />
        void BeginGetAttributes(string path);

        /// <see cref="IFileAccess.GetAttributes(System.String)" />
        void EndGetAttributes(FileAttributes result, string path);

        /// <see cref="IFileAccess.GetAttributes(System.String)" />
        void FailGetAttributes(ref Exception exception);

        /// <see cref="IFileAccess.SetLastWriteTime(System.String,System.DateTime)" />
        void BeginSetLastWriteTime(string path, DateTime lastWriteTime);

        /// <see cref="IFileAccess.SetLastWriteTime(System.String,System.DateTime)" />
        void EndSetLastWriteTime(string path, DateTime lastWriteTime);

        /// <see cref="IFileAccess.SetLastWriteTime(System.String,System.DateTime)" />
        void FailSetLastWriteTime(ref Exception exception);

        /// <see cref="IFileAccess.SetAttributes(System.String,System.IO.FileAttributes)" />
        void BeginSetAttributes(string path, FileAttributes fileAttributes);

        /// <see cref="IFileAccess.SetAttributes(System.String,System.IO.FileAttributes)" />
        void EndSetAttributes(string path, FileAttributes fileAttributes);

        /// <see cref="IFileAccess.SetAttributes(System.String,System.IO.FileAttributes)" />
        void FailSetAttributes(ref Exception exception);

        /// <see cref="IFileAccess.Exists(System.String)" />
        void BeginExists(string path);

        /// <see cref="IFileAccess.Exists(System.String)" />
        void EndExists(bool result, string path);

        /// <see cref="IFileAccess.Exists(System.String)" />
        void FailExists(ref Exception exception);

        /// <see cref="IFileAccess.ReadAllBytes(System.String)" />
        void BeginReadAllBytes(string path);

        /// <see cref="IFileAccess.ReadAllBytes(System.String)" />
        void EndReadAllBytes(byte[] result, string path);

        /// <see cref="IFileAccess.ReadAllBytes(System.String)" />
        void FailReadAllBytes(ref Exception exception);

        /// <see cref="IFileAccess.ReadAllLines(System.String,System.Text.Encoding)" />
        void BeginReadAllLines(string path, Encoding encoding);

        /// <see cref="IFileAccess.ReadAllLines(System.String,System.Text.Encoding)" />
        void EndReadAllLines(string[] result, string path, Encoding encoding);

        /// <see cref="IFileAccess.ReadAllLines(System.String,System.Text.Encoding)" />
        void FailReadAllLines(ref Exception exception);

        /// <see cref="IFileAccess.ReadAllLines(System.String)" />
        void BeginReadAllLines(string path);

        /// <see cref="IFileAccess.ReadAllLines(System.String)" />
        void EndReadAllLines(string[] result, string path);

        /// <see cref="IFileAccess.ReadAllText(System.String,System.Text.Encoding)" />
        void BeginReadAllText(string path, Encoding encoding);

        /// <see cref="IFileAccess.ReadAllText(System.String,System.Text.Encoding)" />
        void EndReadAllText(string result, string path, Encoding encoding);

        /// <see cref="IFileAccess.ReadAllText(System.String,System.Text.Encoding)" />
        void FailReadAllText(ref Exception exception);

        /// <see cref="IFileAccess.ReadAllText(System.String)" />
        void BeginReadAllText(string path);

        /// <see cref="IFileAccess.ReadAllText(System.String)" />
        void EndReadAllText(string result, string path);

        /// <see cref="IFileAccess.WriteAllLines(System.String,System.Collections.Generic.IEnumerable{string},System.Text.Encoding)" />
        void BeginWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        /// <see cref="IFileAccess.WriteAllLines(System.String,System.Collections.Generic.IEnumerable{string},System.Text.Encoding)" />
        void EndWriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        /// <see cref="IFileAccess.WriteAllLines(System.String,System.Collections.Generic.IEnumerable{string},System.Text.Encoding)" />
        void FailWriteAllLines(ref Exception exception);

        /// <see cref="IFileAccess.WriteAllLines(System.String,System.Collections.Generic.IEnumerable{string})" />
        void BeginWriteAllLines(string path, IEnumerable<string> contents);

        /// <see cref="IFileAccess.WriteAllLines(System.String,System.Collections.Generic.IEnumerable{string})" />
        void EndWriteAllLines(string path, IEnumerable<string> contents);

        /// <see cref="IFileAccess.WriteAllText(System.String,System.String)" />
        void BeginWriteAllText(string path, string contents);

        /// <see cref="IFileAccess.WriteAllText(System.String,System.String)" />
        void EndWriteAllText(string path, string contents);

        /// <see cref="IFileAccess.WriteAllText(System.String,System.String)" />
        void FailWriteAllText(ref Exception exception);

        /// <see cref="IFileAccess.WriteAllText(System.String,System.String,System.Text.Encoding)" />
        void BeginWriteAllText(string path, string contents, Encoding encoding);

        /// <see cref="IFileAccess.WriteAllText(System.String,System.String,System.Text.Encoding)" />
        void EndWriteAllText(string path, string contents, Encoding encoding);

        /// <see cref="IFileAccess.WriteAllBytes(System.String,System.Collections.Generic.IEnumerable{byte})" />
        void BeginWriteAllBytes(string path, IEnumerable<byte> bytes);

        /// <see cref="IFileAccess.WriteAllBytes(System.String,System.Collections.Generic.IEnumerable{byte})" />
        void EndWriteAllBytes(string path, IEnumerable<byte> bytes);

        /// <see cref="IFileAccess.WriteAllBytes(System.String,System.Collections.Generic.IEnumerable{byte})" />
        void FailWriteAllBytes(ref Exception exception);

        /// <see cref="IFileAccess.Open(System.String,System.IO.FileMode)" />
        void BeginOpen(string path, FileMode mode);

        /// <see cref="IFileAccess.Open(System.String,System.IO.FileMode)" />
        void EndOpen(FileStream result, string path, FileMode mode);

        /// <see cref="IFileAccess.Open(System.String,System.IO.FileMode)" />
        void FailOpen(ref Exception exception);

        /// <see cref="IFileAccess.Open(System.String,System.IO.FileMode,System.IO.FileAccess)" />
        void BeginOpen(string path, FileMode mode, FileAccess access);

        /// <see cref="IFileAccess.Open(System.String,System.IO.FileMode,System.IO.FileAccess)" />
        void EndOpen(FileStream result, string path, FileMode mode, FileAccess access);

        /// <see cref="IFileAccess.Open(System.String,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)" />
        void BeginOpen(string path, FileMode mode, FileAccess access, FileShare share);

        /// <see cref="IFileAccess.Open(System.String,System.IO.FileMode,System.IO.FileAccess,System.IO.FileShare)" />
        void EndOpen(FileStream result, string path, FileMode mode, FileAccess access, FileShare share);

        /// <see cref="IFileAccess.AppendAllText(System.String,System.String)" />
        void BeginAppendAllText(string path, string contents);

        /// <see cref="IFileAccess.AppendAllText(System.String,System.String)" />
        void EndAppendAllText(string path, string contents);

        /// <see cref="IFileAccess.AppendAllText(System.String,System.String)" />
        void FailAppendAllText(ref Exception exception);

        /// <see cref="IFileAccess.AppendAllText(System.String,System.String,System.Text.Encoding)" />
        void BeginAppendAllText(string path, string contents, Encoding encoding);

        /// <see cref="IFileAccess.AppendAllText(System.String,System.String,System.Text.Encoding)" />
        void EndAppendAllText(string path, string contents, Encoding encoding);

        /// <see cref="IFileAccess.AppendText(System.String)" />
        void BeginAppendText(string path);

        /// <see cref="IFileAccess.AppendText(System.String)" />
        void EndAppendText(StreamWriter result, string path);

        /// <see cref="IFileAccess.AppendText(System.String)" />
        void FailAppendText(ref Exception exception);

        /// <see cref="IFileAccess.Create(System.String)" />
        void BeginCreate(string path);

        /// <see cref="IFileAccess.Create(System.String)" />
        void EndCreate(FileStream result, string path);

        /// <see cref="IFileAccess.Create(System.String)" />
        void FailCreate(ref Exception exception);

        /// <see cref="IFileAccess.Create(System.String,System.Int32)" />
        void BeginCreate(string path, int bufferSize);

        /// <see cref="IFileAccess.Create(System.String,System.Int32)" />
        void EndCreate(FileStream result, string path, int bufferSize);

        /// <see cref="IFileAccess.Create(System.String,System.Int32,System.IO.FileOptions)" />
        void BeginCreate(string path, int bufferSize, FileOptions options);

        /// <see cref="IFileAccess.Create(System.String,System.Int32,System.IO.FileOptions)" />
        void EndCreate(FileStream result, string path, int bufferSize, FileOptions options);

        /// <see cref="IFileAccess.Create(System.String,System.Int32,System.IO.FileOptions,System.Security.AccessControl.FileSecurity)" />
        void BeginCreate(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity);

        /// <see cref="IFileAccess.Create(System.String,System.Int32,System.IO.FileOptions,System.Security.AccessControl.FileSecurity)" />
        void EndCreate(FileStream result, string path, int bufferSize, FileOptions options, FileSecurity fileSecurity);

        /// <see cref="IFileAccess.Decrypt(System.String)" />
        void BeginDecrypt(string path);

        /// <see cref="IFileAccess.Decrypt(System.String)" />
        void EndDecrypt(string path);

        /// <see cref="IFileAccess.Decrypt(System.String)" />
        void FailDecrypt(ref Exception exception);

        /// <see cref="IFileAccess.Encrypt(System.String)" />
        void BeginEncrypt(string path);

        /// <see cref="IFileAccess.Encrypt(System.String)" />
        void EndEncrypt(string path);

        /// <see cref="IFileAccess.Encrypt(System.String)" />
        void FailEncrypt(ref Exception exception);

        /// <see cref="IFileAccess.GetAccessControl(System.String)" />
        void BeginGetAccessControl(string path);

        /// <see cref="IFileAccess.GetAccessControl(System.String)" />
        void EndGetAccessControl(FileSecurity result, string path);

        /// <see cref="IFileAccess.GetAccessControl(System.String)" />
        void FailGetAccessControl(ref Exception exception);

        /// <see cref="IFileAccess.GetAccessControl(System.String,System.Security.AccessControl.AccessControlSections)" />
        void BeginGetAccessControl(string path, AccessControlSections includeSections);

        /// <see cref="IFileAccess.GetAccessControl(System.String,System.Security.AccessControl.AccessControlSections)" />
        void EndGetAccessControl(FileSecurity result, string path, AccessControlSections includeSections);

        /// <see cref="IFileAccess.GetCreationTime(System.String)" />
        void BeginGetCreationTime(string path);

        /// <see cref="IFileAccess.GetCreationTime(System.String)" />
        void EndGetCreationTime(DateTime result, string path);

        /// <see cref="IFileAccess.GetCreationTime(System.String)" />
        void FailGetCreationTime(ref Exception exception);

        /// <see cref="IFileAccess.GetCreationTimeUtc(System.String)" />
        void BeginGetCreationTimeUtc(string path);

        /// <see cref="IFileAccess.GetCreationTimeUtc(System.String)" />
        void EndGetCreationTimeUtc(DateTime result, string path);

        /// <see cref="IFileAccess.GetCreationTimeUtc(System.String)" />
        void FailGetCreationTimeUtc(ref Exception exception);

        /// <see cref="IFileAccess.GetLastAccessTime(System.String)" />
        void BeginGetLastAccessTime(string path);

        /// <see cref="IFileAccess.GetLastAccessTime(System.String)" />
        void EndGetLastAccessTime(DateTime result, string path);

        /// <see cref="IFileAccess.GetLastAccessTime(System.String)" />
        void FailGetLastAccessTime(ref Exception exception);

        /// <see cref="IFileAccess.GetLastAccessTimeUtc(System.String)" />
        void BeginGetLastAccessTimeUtc(string path);

        /// <see cref="IFileAccess.GetLastAccessTimeUtc(System.String)" />
        void EndGetLastAccessTimeUtc(DateTime result, string path);

        /// <see cref="IFileAccess.GetLastAccessTimeUtc(System.String)" />
        void FailGetLastAccessTimeUtc(ref Exception exception);
    }
}