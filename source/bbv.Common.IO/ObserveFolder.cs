//-------------------------------------------------------------------------------
// <copyright file="ObserveFolder.cs" company="bbv Software Services AG">
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
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The folder observer.
    /// </summary>
    public class ObserveFolder : IObserveFolder
    {
        private readonly Func<IFolderWatcher> folderWatcherFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObserveFolder"/> class.
        /// </summary>
        public ObserveFolder()
            : this(() => new FolderWatcher())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObserveFolder"/> class.
        /// </summary>
        /// <param name="folderWatcherFactory">The folder watcher factory.</param>
        public ObserveFolder(Func<IFolderWatcher> folderWatcherFactory)
        {
            this.folderWatcherFactory = folderWatcherFactory;
        }

        /// <summary>
        /// Starts the observation on the given folder with the specified filter. When a file changed in the observed folder
        /// the observation is stopped and the file's path is returned as result of the task.
        /// </summary>
        /// <param name="folder">The folder to be observed.</param>
        /// <param name="filter">The filter to be used.</param>
        /// <param name="cancellationToken">The cancellation token which allows to stop the observation.</param>
        /// <returns>
        /// The fully qualified path to the changed file.
        /// </returns>
        public Task<string> Start(string folder, string filter, CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            var folderWatcher = this.folderWatcherFactory();

            folderWatcher.FileChanged += (sender, args) => taskCompletionSource.TrySetResult(args.Value);
            cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());

            folderWatcher.Folder = folder;
            folderWatcher.Filter = filter;

            folderWatcher.StartObservation();

            return taskCompletionSource.Task
                .ContinueWith(
                    antecedent =>
                    {
                        folderWatcher.StopObservation();
                        return antecedent.Result;
                    },
                    TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}