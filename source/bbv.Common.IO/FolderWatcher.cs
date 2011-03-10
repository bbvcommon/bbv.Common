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
    using System.Reflection;
    using Events;
    using log4net;

    /// <summary>
    /// Implements the FolderWatcher class. Offers function to observe folders for file changes.
    /// </summary>
    public class FolderWatcher : IFolderWatcher
    {
        #region members

        /// <summary>
        /// The <see cref="EventFilter{TEventArgs}"/> timeout.
        /// </summary>
        private const int FilterTimeWindow = 100; // filter time window is 100ms
        
        /// <summary>
        /// Filters the original <see cref="FileSystemWatcher"/> events to prevent to much events for the user of the <see cref="FolderWatcher"/>.
        /// </summary>
        private readonly EventFilter<FileSystemEventArgs> eventFilter;
        
        /// <summary>
        /// log4net Logger
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// .NET file system watcher
        /// </summary>
        private FileSystemWatcher filesystemwatcher;
        
        /// <summary>
        /// File filter - it is a file system filter string like "*.txt".
        /// </summary>
        private string filter;
        
        /// <summary>
        /// The folder who is monitored from the <see cref="FileSystemWatcher"/>
        /// </summary>
        private string watchedFolder;

        #endregion

        #region CTOR

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
            this.eventFilter = new EventFilter<FileSystemEventArgs>(FilterTimeWindow);
            this.eventFilter.FilteredEventRaised += this.EventFilterOnFilteredEventRaised;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Occurs when a new file is recognized. The event arguments contains the found filename.
        /// </summary>
        public event EventHandler<EventArgs<string>> FileChanged;

        /// <summary>
        /// Gets or sets the folder which will be observed.
        /// </summary>
        /// <value>The folder.</value>
        public string Folder
        {
            get { return this.watchedFolder; }
            set { this.watchedFolder = value; }
        }

        /// <summary>
        /// Gets or sets the filter it is a file system filter string like "*.txt".
        /// </summary>
        /// <value>The filter.</value>
        public string Filter
        {
            get { return this.filter; }
            set { this.filter = value; }
        }

        /// <summary>
        /// Initialize the file watcher who are observing the given directory for new files
        /// </summary>
        public void StartObservation()
        {
            if (this.filesystemwatcher == null)
            {
                this.filesystemwatcher = new FileSystemWatcher(this.Folder, this.Filter);
            }

            this.filesystemwatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName |
                                             NotifyFilters.DirectoryName;

            this.filesystemwatcher.IncludeSubdirectories = true;

            // init the events
            this.filesystemwatcher.Changed += this.eventFilter.HandleOriginalEvent;

            // start watching
            this.filesystemwatcher.EnableRaisingEvents = true;

            this.log.Debug("Observation started");
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
            }

            this.log.Debug("Observation stopped");
        }

        /// <summary>
        /// Gets the current available files stored in the defined folder and matching to the filter criteria.
        /// </summary>
        /// <returns>A list of all available files in the monitored folder and matching to the given filter</returns>
        public IList<string> GetCurrentAvailableFiles()
        {
            string[] files = Directory.GetFiles(this.Folder, this.Filter);

            foreach (string file in files)
            {
                this.log.DebugFormat("File {0} in folder {1}", file, this.Folder);
            }

            return files;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Handles events raised from the Event filter. The event is the last original event.
        /// </summary>
        /// <param name="sender">The original sender of the event</param>
        /// <param name="e">Event arguments in this case the <see cref="FileSystemEventArgs"/></param>
        private void EventFilterOnFilteredEventRaised(object sender, FileSystemEventArgs e)
        {
            this.log.DebugFormat("New file recognized {0}", e.FullPath);

            if (this.FileChanged == null)
            {
                return;
            }

            this.FileChanged(this, new EventArgs<string>(e.FullPath));
        }

        #endregion
    }
}
