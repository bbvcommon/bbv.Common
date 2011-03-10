//-------------------------------------------------------------------------------
// <copyright file="FileAccess.cs" company="bbv Software Services AG">
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
    using System.Text;

    /// <summary>
    /// Wrapper class which simplifies the access to the file layer.
    /// </summary>
    public class FileAccess : IFileAccess, IExtensionProvider<IFileAccessExtension>
    {
        private readonly List<IFileAccessExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAccess"/> class.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public FileAccess(IEnumerable<IFileAccessExtension> extensions)
        {
            this.extensions = extensions.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<IFileAccessExtension> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <inheritdoc />
        public void Delete(string path)
        {
            this.SurroundWithExtension(() => File.Delete(path), path);
        }

        /// <inheritdoc />
        public void Copy(string sourceFileName, string destFileName)
        {
            this.SurroundWithExtension(() => File.Copy(sourceFileName, destFileName), sourceFileName, destFileName);
        }

        /// <inheritdoc />
        public void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            this.SurroundWithExtension(() => File.Copy(sourceFileName, destFileName, overwrite), sourceFileName, destFileName, overwrite);
        }

        /// <inheritdoc />
        public StreamWriter CreateText(string path)
        {
            return this.SurroundWithExtension(() => File.CreateText(path), path);
        }

        /// <inheritdoc />
        public FileAttributes GetAttributes(string path)
        {
            return this.SurroundWithExtension(() => File.GetAttributes(path), path);
        }

        /// <inheritdoc />
        public void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            this.SurroundWithExtension(() => File.SetLastWriteTime(path, lastWriteTime), path, lastWriteTime);
        }

        /// <inheritdoc />
        public void SetAttributes(string path, FileAttributes fileAttributes)
        {
            this.SurroundWithExtension(() => File.SetAttributes(path, fileAttributes), path, fileAttributes);
        }

        /// <inheritdoc />
        public bool Exists(string path)
        {
           return this.SurroundWithExtension(() => File.Exists(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<byte> ReadAllBytes(string path)
        {
            return this.SurroundWithExtension(() => File.ReadAllBytes(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<string> ReadAllLines(string path, Encoding encoding)
        {
            return this.SurroundWithExtension(() => File.ReadAllLines(path, encoding), path, encoding);
        }

        /// <inheritdoc />
        public IEnumerable<string> ReadAllLines(string path)
        {
             return this.SurroundWithExtension(() => File.ReadAllLines(path), path);
        }

        /// <inheritdoc />
        public string ReadAllText(string path, Encoding encoding)
        {
             return this.SurroundWithExtension(() => File.ReadAllText(path, encoding), path, encoding);
        }

        /// <inheritdoc />
        public string ReadAllText(string path)
        {
             return this.SurroundWithExtension(() => File.ReadAllText(path), path);
        }

        /// <inheritdoc />
        public void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            string[] contentAsArray = contents.ToArray();

            this.SurroundWithExtension(() => File.WriteAllLines(path, contentAsArray, encoding), path, contents, encoding);
        }

        /// <inheritdoc />
        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            string[] contentAsArray = contents.ToArray();

            this.SurroundWithExtension(() => File.WriteAllLines(path, contentAsArray), path, contents);
        }

        /// <inheritdoc />
        public void WriteAllText(string path, string contents)
        {
            this.SurroundWithExtension(() => File.WriteAllText(path, contents), path, contents);
        }

        /// <inheritdoc />
        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            this.SurroundWithExtension(() => File.WriteAllText(path, contents, encoding), path, contents, encoding);
        }

        /// <inheritdoc />
        public void WriteAllBytes(string path, IEnumerable<byte> bytes)
        {
            byte[] bytesAsArray = bytes.ToArray();

            this.SurroundWithExtension(() => File.WriteAllBytes(path, bytesAsArray), path, bytesAsArray);
        }

        /// <inheritdoc />
        public Stream Open(string path, FileMode mode)
        {
            return this.SurroundWithExtension(() => File.Open(path, mode), path, mode);
        }

        /// <inheritdoc />
        public Stream Open(string path, FileMode mode, System.IO.FileAccess access)
        {
            return this.SurroundWithExtension(() => File.Open(path, mode, access), path, mode, access);
        }

        /// <inheritdoc />
        public Stream Open(string path, FileMode mode, System.IO.FileAccess access, FileShare share)
        {
            return this.SurroundWithExtension(() => File.Open(path, mode, access, share), path, mode, access, share);
        }

        /// <inheritdoc />
        public void AppendAllText(string path, string contents)
        {
            this.SurroundWithExtension(() => File.AppendAllText(path, contents), path, contents);
        }

        /// <inheritdoc />
        public void AppendAllText(string path, string contents, Encoding encoding)
        {
            this.SurroundWithExtension(() => File.AppendAllText(path, contents, encoding), path, contents, encoding);
        }

        /// <inheritdoc />
        public StreamWriter AppendText(string path)
        {
            return this.SurroundWithExtension(() => File.AppendText(path), path);
        }

        /// <inheritdoc />
        public Stream Create(string path)
        {
            return this.SurroundWithExtension(() => File.Create(path), path);
        }

        /// <inheritdoc />
        public Stream Create(string path, int bufferSize)
        {
            return this.SurroundWithExtension(() => File.Create(path, bufferSize), path, bufferSize);
        }

        /// <inheritdoc />
        public Stream Create(string path, int bufferSize, FileOptions options)
        {
            return this.SurroundWithExtension(() => File.Create(path, bufferSize, options), path, bufferSize, options);
        }

        /// <inheritdoc />
        public Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            return this.SurroundWithExtension(() => File.Create(path, bufferSize, options, fileSecurity), path, bufferSize, options, fileSecurity);
        }

        /// <inheritdoc />
        public void Decrypt(string path)
        {
            this.SurroundWithExtension(() => File.Decrypt(path), path);
        }

        /// <inheritdoc />
        public void Encrypt(string path)
        {
            this.SurroundWithExtension(() => File.Encrypt(path), path);
        }

        /// <inheritdoc />
        public FileSecurity GetAccessControl(string path)
        {
            return this.SurroundWithExtension(() => File.GetAccessControl(path), path);
        }

        /// <inheritdoc />
        public FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return this.SurroundWithExtension(() => File.GetAccessControl(path, includeSections), path, includeSections);
        }

        /// <inheritdoc />
        public DateTime GetCreationTime(string path)
        {
            return this.SurroundWithExtension(() => File.GetCreationTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetCreationTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => File.GetCreationTimeUtc(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastAccessTime(string path)
        {
            return this.SurroundWithExtension(() => File.GetLastAccessTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastAccessTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => File.GetLastAccessTimeUtc(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastWriteTime(string path)
        {
            return this.SurroundWithExtension(() => File.GetLastWriteTime(path), path);
        }

        /// <inheritdoc />
        public DateTime GetLastWriteTimeUtc(string path)
        {
            return this.SurroundWithExtension(() => File.GetLastWriteTimeUtc(path), path);
        }

        /// <inheritdoc />
        public void Move(string sourceFileName, string destFileName)
        {
            this.SurroundWithExtension(() => File.Move(sourceFileName, destFileName), sourceFileName, destFileName);
        }

        /// <inheritdoc />
        public Stream OpenRead(string path)
        {
            return this.SurroundWithExtension(() => File.OpenRead(path), path);
        }

        /// <inheritdoc />
        public StreamReader OpenText(string path)
        {
            return this.SurroundWithExtension(() => File.OpenText(path), path);
        }

        /// <inheritdoc />
        public Stream OpenWrite(string path)
        {
            return this.SurroundWithExtension(() => File.OpenWrite(path), path);
        }

        /// <inheritdoc />
        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            this.SurroundWithExtension(() => File.Replace(sourceFileName, destinationFileName, destinationBackupFileName), sourceFileName, destinationFileName, destinationBackupFileName);
        }

        /// <inheritdoc />
        public void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            this.SurroundWithExtension(() => File.Replace(sourceFileName, destinationFileName, destinationBackupFileName), sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
        }

        /// <inheritdoc />
        public void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            this.SurroundWithExtension(() => File.SetAccessControl(path, fileSecurity), path, fileSecurity);
        }

        /// <inheritdoc />
        public void SetCreationTime(string path, DateTime creationTime)
        {
            this.SurroundWithExtension(() => File.SetCreationTime(path, creationTime), path, creationTime);
        }

        /// <inheritdoc />
        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            this.SurroundWithExtension(() => File.SetCreationTimeUtc(path, creationTimeUtc), path, creationTimeUtc);
        }

        /// <inheritdoc />
        public void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            this.SurroundWithExtension(() => File.SetLastAccessTime(path, lastAccessTime), path, lastAccessTime);
        }

        /// <inheritdoc />
        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            this.SurroundWithExtension(() => File.SetLastAccessTimeUtc(path, lastAccessTimeUtc), path, lastAccessTimeUtc);
        }

        /// <inheritdoc />
        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            this.SurroundWithExtension(() => File.SetLastWriteTimeUtc(path, lastWriteTimeUtc), path, lastWriteTimeUtc);
        }
    }
}