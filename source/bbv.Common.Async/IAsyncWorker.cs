//-------------------------------------------------------------------------------
// <copyright file="IAsyncWorker.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Async
{
    using System.ComponentModel;

    /// <summary>
    /// The <see cref="AsyncWorker"/> is a small wrapper around the <see cref="BackgroundWorker"/> for easier usage
    /// outside of UI components.
    /// </summary>
    public interface IAsyncWorker
    {
        /// <summary>
        /// Gets a value indicating whether an asynchronous operation is running.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Gets a value indicating whether the operation should be canceled.
        /// </summary>
        bool CancellationPending { get; }

        /// <summary>
        /// Gets or sets a value indicating whether progress can be reported.
        /// </summary>
        bool WorkerReportsProgress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether cancellation of asynchronous operation is supported.
        /// </summary>
        bool WorkerSupportsCancellation { get; set; }

        /// <summary>
        /// Starts execution of a background operation.
        /// </summary>
        void RunWorkerAsync();

        /// <summary>
        /// Starts execution of a background operation.
        /// </summary>
        /// <param name="argument">The argument passed to the worker.</param>
        void RunWorkerAsync(object argument);

        /// <summary>
        /// Cancels the operation.
        /// </summary>
        void CancelAsync();

        /// <summary>
        /// Raises the <see cref="AsyncWorker.ProgressChanged"/> event.
        /// </summary>
        /// <param name="percentProgress">The percent progress.</param>
        /// <param name="userState">State of the user.</param>
        void ReportProgress(int percentProgress, object userState);

        /// <summary>
        /// Adds the extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        void AddExtension(IAsyncWorkerExtension extension);

        /// <summary>
        /// Removes the extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        void RemoveExtension(IAsyncWorkerExtension extension);

        /// <summary>
        /// Clears all extensions.
        /// </summary>
        void ClearExtensions();

        /// <summary>
        /// Initializes the <see cref="IAsyncWorker"/>.
        /// </summary>
        /// <param name="worker">The worker delegate.</param>
        void Initialize(DoWorkEventHandler worker);

        /// <summary>
        /// Initializes the <see cref="IAsyncWorker"/>.
        /// </summary>
        /// <param name="worker">The worker delegate.</param>
        /// <param name="completed">The completed delegate.</param>
        void Initialize(DoWorkEventHandler worker, RunWorkerCompletedEventHandler completed);

        /// <summary>
        /// Initializes the <see cref="IAsyncWorker"/>.
        /// </summary>
        /// <param name="name">The name used in log messages.</param>
        /// <param name="worker">The worker delegate.</param>
        void Initialize(string name, DoWorkEventHandler worker);

        /// <summary>
        /// Initializes the <see cref="IAsyncWorker"/>.
        /// </summary>
        /// <param name="name">The name used in log messages.</param>
        /// <param name="worker">The worker delegate.</param>
        /// <param name="completed">The completed delegate.</param>
        void Initialize(
            string name,
            DoWorkEventHandler worker, 
            RunWorkerCompletedEventHandler completed);

        /// <summary>
        /// Initializes the <see cref="IAsyncWorker"/>.
        /// </summary>
        /// <param name="worker">The worker delegate.</param>
        /// <param name="progress">The progress delegate.</param>
        /// <param name="completed">The completed delegate.</param>
        void Initialize(
            DoWorkEventHandler worker,
            ProgressChangedEventHandler progress,
            RunWorkerCompletedEventHandler completed);

        /// <summary>
        /// Initializes the <see cref="IAsyncWorker"/>.
        /// </summary>
        /// <param name="name">The name used in log messages.</param>
        /// <param name="worker">The worker delegate.</param>
        /// <param name="progress">The progress delegate.</param>
        /// <param name="completed">The completed delegate.</param>
        void Initialize(
            string name,
            DoWorkEventHandler worker,
            ProgressChangedEventHandler progress,
            RunWorkerCompletedEventHandler completed);
    }
}