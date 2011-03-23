//-------------------------------------------------------------------------------
// <copyright file="FolderWatcher.cs" company="bbv Software Services AG">
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
    using Events;

    /// <summary>
    /// Observes a folder for file changes. Filters frequent file changed event from file system with an <see cref="IEventFilter{TEventArgs}"/>.
    /// </summary>
    public class FolderWatcher : IFolderWatcher
    {
        /// <summary>
        /// The <see cref="EventFilter{TEventArgs}"/> timeout.
        /// </summary>
        private const int FilterTimeWindow = 100;
        
        /// <summary>
        /// Filters the original <see cref="FileSystemWatcher"/> events to prevent to much events for the user of the <see cref="FolderWatcher"/>.
        /// </summary>
        private readonly EventFilter<FileSystemEventArgs> eventFilter;
        
        private readonly List<IFolderWatcherExtension> extensions;

        private FileSystemWatcher filesystemwatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderWatcher"/> class.
        /// </summary>
        /// <remarks>Folder and Filter must be set from outside.</remarks>
        public FolderWatcher()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderWatcher"/> class.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filter">The filter.</param>
        public FolderWatcher(string folder, string filter)
        {
            this.Folder = folder;
            this.Filter = filter;

            this.extensions = new List<IFolderWatcherExtension>();
            this.eventFilter = new EventFilter<FileSystemEventArgs>(FilterTimeWindow);
            this.eventFilter.FilteredEventRaised += this.HandleEventFilterFilteredEventRaised;
        }

        /// <summary>
        /// Occurs when a new file is recognized. The event arguments contains the found filename.
        /// </summary>
        public event EventHandler<EventArgs<string>> FileChanged;

        /// <summary>
        /// Gets or sets the folder which will be observed.
        /// </summary>
        /// <value>The folder.</value>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the filter. It is a file system filter string like "*.txt".
        /// </summary>
        /// <value>The filter.</value>
        public string Filter { get; set; }

        /// <summary>
        /// Adds the extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public void AddExtension(IFolderWatcherExtension extension)
        {
            this.extensions.Add(extension);
        }

        /// <summary>
        /// Removes the extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public void RemoveExtension(IFolderWatcherExtension extension)
        {
            this.extensions.Remove(extension);
        }

        /// <summary>
        /// Clears all extensions.
        /// </summary>
        public void ClearExtensions()
        {
            this.extensions.Clear();
        }

        /// <summary>
        /// Initialize the file watcher who are observing the given directory for new files
        /// </summary>
        public void StartObservation()
        {
            this.CheckNotAlreadyRunning();

            if (this.filesystemwatcher == null)
            {
                this.filesystemwatcher = new FileSystemWatcher(this.Folder, this.Filter);
            }

            this.filesystemwatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName |
                                             NotifyFilters.DirectoryName;

            this.filesystemwatcher.IncludeSubdirectories = true;
            this.filesystemwatcher.Changed += this.eventFilter.HandleOriginalEvent;
            this.filesystemwatcher.EnableRaisingEvents = true;

            this.extensions.ForEach(extension => extension.ObservationStarted(this.Folder, this.Filter));
        }

        /// <summary>
        /// Stops the observation.
        /// </summary>
        public void StopObservation()
        {
            if (this.filesystemwatcher != null)
            {
                this.filesystemwatcher.EnableRaisingEvents = false;
                this.filesystemwatcher.Changed -= this.eventFilter.HandleOriginalEvent;
                this.filesystemwatcher = null;
            }

            this.extensions.ForEach(extension => extension.ObservationStopped(this.Folder, this.Filter));
        }

        /// <summary>
        /// Gets the current available files stored in the defined folder and matching to the filter criteria.
        /// </summary>
        /// <returns>A list of all available files in the monitored folder and matching to the given filter</returns>
        public IList<string> GetCurrentAvailableFiles()
        {
            return Directory.GetFiles(this.Folder, this.Filter);
        }

        private void HandleEventFilterFilteredEventRaised(object sender, FileSystemEventArgs e)
        {
            this.extensions.ForEach(extension => extension.FileAddedOrChanged(this.Folder, this.Filter, e.FullPath));
            
            if (this.FileChanged == null)
            {
                return;
            }

            this.FileChanged(this, new EventArgs<string>(e.FullPath));
        }

        private void CheckNotAlreadyRunning()
        {
            if (this.filesystemwatcher != null)
            {
                throw new InvalidOperationException("Observation is already started.");
            }
        }
    }
}
