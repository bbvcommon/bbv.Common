//-------------------------------------------------------------------------------
// <copyright file="IFolderWatcherExtension.cs" company="bbv Software Services AG">
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
    /// <summary>
    /// Extension for <see cref="IFolderWatcher"/>.
    /// </summary>
    public interface IFolderWatcherExtension
    {
        /// <summary>
        /// Called when observation was started.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filter">The filter.</param>
        void ObservationStarted(string folder, string filter);

        /// <summary>
        /// Called when observation was stopped.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filter">The filter.</param>
        void ObservationStopped(string folder, string filter);

        /// <summary>
        /// Called when a changed or added file is detected.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="path">The path of the file.</param>
        void FileAddedOrChanged(string folder, string filter, string path);
    }
}