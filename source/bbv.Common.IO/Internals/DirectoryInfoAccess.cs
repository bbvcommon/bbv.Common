//-------------------------------------------------------------------------------
// <copyright file="DirectoryInfoAccess.cs" company="bbv Software Services AG">
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
    using System.Runtime.Serialization;
    using System.Security;
    using System.Security.Permissions;

    /// <summary>
    /// Wrapper class which simplifies the access to directory information.
    /// </summary>
    [Serializable]
    public sealed class DirectoryInfoAccess : FileSystemInfoAccess<DirectoryInfo>, IDirectoryInfoAccess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryInfoAccess"/> class.
        /// </summary>
        /// <param name="directoryInfo">The directory info.</param>
        public DirectoryInfoAccess(DirectoryInfo directoryInfo)
            : base(directoryInfo)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryInfoAccess"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        private DirectoryInfoAccess(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the parent directory of a specified subdirectory.
        /// </summary>
        /// <value>The parent directory, or null if the path is null or if the file path
        /// denotes a root (such as "\", "C:", or * "\\server\share").</value>
        /// <exception cref="SecurityException">The caller does not have the required
        /// permission.</exception>
        public IDirectoryInfoAccess Parent
        {
            get { return this.FileSystemInfo != null ? new DirectoryInfoAccess(this.FileSystemInfo.Parent) : null; }
        }

        /// <summary>
        /// Gets the root portion of a path.
        /// </summary>
        /// <value>A <see cref="IDirectoryInfoAccess"/> object representing the root of a path.</value>
        /// <exception cref="SecurityException">The caller does not have the required
        /// permission.</exception>
        public IDirectoryInfoAccess Root
        {
            get
            {
                return this.FileSystemInfo != null ? new DirectoryInfoAccess(this.FileSystemInfo.Root) : null;
            }
        }

        /// <summary>
        /// Creates a directory.
        /// </summary>
        /// <exception cref="IOException">The directory cannot be created.</exception>
        public void Create()
        {
            this.FileSystemInfo.Create();
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