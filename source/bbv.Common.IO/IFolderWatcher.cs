//-------------------------------------------------------------------------------
// <copyright file="IFolderWatcher.cs" company="bbv Software Services AG">
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
    using Events;

    /// <summary>
    /// Interface for the Folder Watcher class. Offers function to observe folders for file changes.
    /// </summary>
    public interface IFolderWatcher
    {
        /// <summary>
        /// Occurs when a new file is recognized or a file is changed. The event arguments contains the found path.
        /// </summary>
        event EventHandler<EventArgs<string>> FileChanged;

        /// <summary>
        /// Gets or sets the folder which will be observed.
        /// </summary>
        /// <value>The folder.</value>
        string Folder { get; set; }

        /// <summary>
        /// Gets or sets the filter it is a normal file system filter string like "*.txt".
        /// </summary>
        /// <value>The filter.</value>
        string Filter { get; set; }

        /// <summary>
        /// Initialize the file watcher who are observing the given directory for new files
        /// </summary>
        void StartObservation();

        /// <summary>
        /// Stops the observation.
        /// </summary>
        void StopObservation();

        /// <summary>
        /// Gets the current available files stored in the defined folder and matching to the filter criteria.
        /// </summary>
        /// <returns>A list of all available files in the monitored folder and matching to the given filter</returns>
        IList<string> GetCurrentAvailableFiles();
    }
}
