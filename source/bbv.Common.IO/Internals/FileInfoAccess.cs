//-------------------------------------------------------------------------------
// <copyright file="FileInfoAccess.cs" company="bbv Software Services AG">
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
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security.AccessControl;
    using System.Security.Permissions;

    /// <summary>
    /// Wrapper class which simplifies the access to file information.
    /// </summary>
    [Serializable]
    public sealed class FileInfoAccess : FileSystemInfoAccess<FileInfo>, IFileInfoAccess
    {
        /// <summary>
        /// The directory info.
        /// </summary>
        private readonly IDirectoryInfoAccess directoryInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoAccess"/> class.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        public FileInfoAccess(FileInfo fileInfo)
            : base(fileInfo)
        {
            this.directoryInfo = new DirectoryInfoAccess(fileInfo.Directory);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoAccess"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private FileInfoAccess(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <inheritdoc />
        public IDirectoryInfoAccess Directory
        {
            get { return this.directoryInfo; }
        }

        /// <inheritdoc />
        public string DirectoryName
        {
            get { return this.FileSystemInfo.DirectoryName; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return this.FileSystemInfo.IsReadOnly; }
            set { this.FileSystemInfo.IsReadOnly = value; }
        }

        /// <inheritdoc />
        public long Length
        {
            get { return this.FileSystemInfo.Length; }
        }

        /// <inheritdoc />
        public StreamWriter AppendText()
        {
            return this.FileSystemInfo.AppendText();
        }

        /// <inheritdoc />
        public IFileInfoAccess CopyTo(string destFileName)
        {
            return new FileInfoAccess(this.FileSystemInfo.CopyTo(destFileName));
        }

        /// <inheritdoc />
        public IFileInfoAccess CopyTo(string destFileName, bool overwrite)
        {
            return new FileInfoAccess(this.FileSystemInfo.CopyTo(destFileName, overwrite));
        }

        /// <inheritdoc />
        public Stream Create()
        {
            return this.FileSystemInfo.Create();
        }

        /// <inheritdoc />
        public StreamWriter CreateText()
        {
            return this.FileSystemInfo.CreateText();
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public void Decrypt()
        {
            this.FileSystemInfo.Decrypt();
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public void Encrypt()
        {
            this.FileSystemInfo.Encrypt();
        }

        /// <inheritdoc />
        public FileSecurity GetAccessControl()
        {
            return this.FileSystemInfo.GetAccessControl();
        }

        /// <inheritdoc />
        public FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            return this.FileSystemInfo.GetAccessControl(includeSections);
        }

        /// <inheritdoc />
        public void MoveTo(string destFileName)
        {
            this.FileSystemInfo.MoveTo(destFileName);
        }

        /// <inheritdoc />
        public Stream Open(FileMode mode)
        {
            return this.FileSystemInfo.Open(mode);
        }

        /// <inheritdoc />
        public Stream Open(FileMode mode, System.IO.FileAccess access)
        {
            return this.FileSystemInfo.Open(mode, access);
        }

        /// <inheritdoc />
        public Stream Open(FileMode mode, System.IO.FileAccess access, FileShare share)
        {
            return this.FileSystemInfo.Open(mode, access, share);
        }

        /// <inheritdoc />
        public Stream OpenRead()
        {
            return this.FileSystemInfo.OpenRead();
        }

        /// <inheritdoc />
        public StreamReader OpenText()
        {
            return this.FileSystemInfo.OpenText();
        }

        /// <inheritdoc />
        public Stream OpenWrite()
        {
            return this.FileSystemInfo.OpenWrite();
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public IFileInfoAccess Replace(string destinationFileName, string destinationBackupFileName)
        {
            return new FileInfoAccess(this.FileSystemInfo.Replace(destinationFileName, destinationBackupFileName));
        }

        /// <inheritdoc />
        [ComVisible(false)]
        public IFileInfoAccess Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            return new FileInfoAccess(this.FileSystemInfo.Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors));
        }

        /// <inheritdoc />
        public void SetAccessControl(FileSecurity fileSecurity)
        {
            this.FileSystemInfo.SetAccessControl(fileSecurity);
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with
        /// the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The
        /// <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with
        /// data.</param>
        /// <param name="context">The destination (see
        /// <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this
        /// serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">The caller does
        /// not have the required permission.
        /// </exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}