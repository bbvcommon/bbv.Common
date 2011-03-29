//-------------------------------------------------------------------------------
// <copyright file="Log4NetFolderWatcherExtension.cs" company="bbv Software Services AG">
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
    using System.Globalization;
    using System.Reflection;

    using log4net;

    /// <summary>
    /// <see cref="IFolderWatcherExtension"/> that uses log4net to log messages about the folder watcher.
    /// </summary>
    public class Log4NetFolderWatcherExtension : IFolderWatcherExtension
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetFolderWatcherExtension"/> class
        /// with standard logger name (this class's name).
        /// </summary>
        public Log4NetFolderWatcherExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetFolderWatcherExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Log4NetFolderWatcherExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Called when observation was started.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filter">The filter.</param>
        public void ObservationStarted(string folder, string filter)
        {
            this.log.InfoFormat(CultureInfo.InvariantCulture, "Observation started on folder {0} with filter {1}.", folder, filter);
        }

        /// <summary>
        /// Called when observation was stopped.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filter">The filter.</param>
        public void ObservationStopped(string folder, string filter)
        {
            this.log.InfoFormat(CultureInfo.InvariantCulture, "Observation stopped on folder {0} with filter {1}.", folder, filter);
        }

        /// <summary>
        /// Called when a changed or added file is detected.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="path">The path of the file.</param>
        public void FileAddedOrChanged(string folder, string filter, string path)
        {
            this.log.DebugFormat(CultureInfo.InvariantCulture, "New or changed file {0} in folder {1}.", path, folder);
        }
    }
}