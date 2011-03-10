//-------------------------------------------------------------------------------
// <copyright file="PathAccess.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Wrapper class which simplifies access to paths.
    /// </summary>
    public class PathAccess : IPathAccess, IExtensionProvider<IPathAccessExtension>
    {
        private readonly List<IPathAccessExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathAccess"/> class.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public PathAccess(IEnumerable<IPathAccessExtension> extensions)
        {
            this.extensions = extensions.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<IPathAccessExtension> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <inheritdoc />
        public string GetDirectoryName(string path)
        {
            return this.SurroundWithExtension(() => Path.GetDirectoryName(path), path);
        }

        /// <inheritdoc />
        public string GetFileName(string path)
        {
            return this.SurroundWithExtension(() => Path.GetFileName(path), path);
        }

        /// <inheritdoc />
        public string GetFileNameWithoutExtension(string path)
        {
            return this.SurroundWithExtension(() => Path.GetFileNameWithoutExtension(path), path);
        }

        /// <inheritdoc />
        public string Combine(string path1, string path2)
        {
            return this.SurroundWithExtension(() => Path.Combine(path1, path2), path1, path2);
        }

        /// <inheritdoc />
        public string GetRandomFileName()
        {
            return this.SurroundWithExtension(() => Path.GetRandomFileName());
        }

        /// <inheritdoc />
        public string ChangeExtension(string path, string extension)
        {
            return this.SurroundWithExtension(() => Path.ChangeExtension(path, extension), path, extension);
        }

        /// <inheritdoc />
        public string GetExtension(string path)
        {
            return this.SurroundWithExtension(() => Path.GetExtension(path), path);
        }

        /// <inheritdoc />
        public string GetFullPath(string path)
        {
            return this.SurroundWithExtension(() => Path.GetFullPath(path), path);
        }

        /// <inheritdoc />
        public IEnumerable<char> GetInvalidFileNameChars()
        {
            return this.SurroundWithExtension(() => Path.GetInvalidFileNameChars());
        }

        /// <inheritdoc />
        public IEnumerable<char> GetInvalidPathChars()
        {
            return this.SurroundWithExtension(() => Path.GetInvalidPathChars());
        }

        /// <inheritdoc />
        public string GetPathRoot(string path)
        {
            return this.SurroundWithExtension(() => Path.GetPathRoot(path), path);
        }

        /// <inheritdoc />
        public string GetTempFileName()
        {
            return this.SurroundWithExtension(() => Path.GetTempFileName());
        }

        /// <inheritdoc />
        public string GetTempPath()
        {
            return this.SurroundWithExtension(() => Path.GetTempPath());
        }

        /// <inheritdoc />
        public bool HasExtension(string path)
        {
            return this.SurroundWithExtension(() => Path.HasExtension(path), path);
        }

        /// <inheritdoc />
        public bool IsPathRooted(string path)
        {
            return this.SurroundWithExtension(() => Path.IsPathRooted(path), path);
        }
    }
}