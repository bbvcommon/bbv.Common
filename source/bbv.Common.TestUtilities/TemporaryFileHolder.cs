//-------------------------------------------------------------------------------
// <copyright file="TemporaryFileHolder.cs" company="bbv Software Services AG">
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

namespace bbv.Common.TestUtilities
{
    using System;
    using System.IO;
    using IO;

    /// <summary>
    /// Helper class to create (and destroy) a temporary file for UnitTests that depend on file system operations
    /// </summary>
    public class TemporaryFileHolder : IDisposable
    {
        /// <summary>
        /// path of the file.
        /// </summary>
        private readonly string filepath;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryFileHolder"/> class.
        /// </summary>
        /// <param name="filepath">The path were the file should be created.</param>
        /// <param name="fileContent">Content of the file.</param>
        public TemporaryFileHolder(string filepath, Stream fileContent)
        {
            this.filepath = filepath;
            using (FileStream fileStream = File.Create(filepath))
            {
                StreamHelper.CopyStream(fileContent, fileStream);
                fileStream.Flush();
                fileStream.Close();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryFileHolder"/> class.
        /// </summary>
        /// <param name="filepath">The path were the file should be created.</param>
        /// <param name="fileContent">Content of the file.</param>
        public TemporaryFileHolder(string filepath, string fileContent)
        {
            this.filepath = filepath;
            using (StreamWriter fileStream = File.CreateText(filepath))
            {
                fileStream.Write(fileContent);
                fileStream.Flush();
                fileStream.Close();
            }
        }

        /// <summary>
        /// Gets the file info.
        /// </summary>
        /// <value>The file info.</value>
        public FileInfo FileInfo
        {
            get { return new FileInfo(this.filepath); }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            try
            {
                File.Delete(this.filepath);
            }
            catch
            {
                // No chance to delete the file
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}