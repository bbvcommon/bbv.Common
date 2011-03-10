//-------------------------------------------------------------------------------
// <copyright file="AsyncWorker.cs" company="bbv Software Services AG">
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
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using log4net;

    /// <summary>
    /// The <see cref="AsyncWorker"/> is a small wrapper around the <see cref="BackgroundWorker"/> for easier usage
    /// outside of UI components.
    /// The async worker has to be created on the UI thread.
    /// </summary>
    public class AsyncWorker : IAsyncWorker
    {
        /// <summary>The completed delegate.</summary>
        private readonly RunWorkerCompletedEventHandler completed;

        /// <summary>
        /// The name of this worker. Used in log messages.
        /// </summary>
        private readonly string name;

        /// <summary>The background worker to execute the operation.</summary>
        private readonly BackgroundWorker backgroundWorker;

        /// <summary>The worker delegate.</summary>
        private readonly DoWorkEventHandler worker;

        /// <summary>The progress delegate.</summary>
        private readonly ProgressChangedEventHandler progress;

        /// <summary>
        /// Logger of this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorker"/> class.
        /// </summary>
        /// <param name="worker">The worker delegate.</param>
        public AsyncWorker(DoWorkEventHandler worker)
            : this(null, worker, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorker"/> class.
        /// </summary>
        /// <param name="name">The name used in log messages.</param>
        /// <param name="worker">The worker delegate.</param>
        public AsyncWorker(string name, DoWorkEventHandler worker)
            : this(name, worker, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorker"/> class.
        /// </summary>
        /// <param name="worker">The worker delegate.</param>
        /// <param name="completed">The completed delegate.</param>
        public AsyncWorker(
            DoWorkEventHandler worker,
            RunWorkerCompletedEventHandler completed)
            : this(null, worker, null, completed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorker"/> class.
        /// </summary>
        /// <param name="name">The name used in log messages.</param>
        /// <param name="worker">The worker delegate.</param>
        /// <param name="completed">The completed delegate.</param>
        public AsyncWorker(
            string name,
            DoWorkEventHandler worker,
            RunWorkerCompletedEventHandler completed)
            : this(null, worker, null, completed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorker"/> class.
        /// </summary>
        /// <param name="worker">The worker delegate.</param>
        /// <param name="progress">The progress delegate.</param>
        /// <param name="completed">The completed delegate.</param>
        public AsyncWorker(
            DoWorkEventHandler worker,
            ProgressChangedEventHandler progress,
            RunWorkerCompletedEventHandler completed)
            : this(null, worker, progress, completed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncWorker"/> class.
        /// </summary>
        /// <param name="name">The name used in log messages.</param>
        /// <param name="worker">The worker delegate.</param>
        /// <param name="progress">The progress delegate.</param>
        /// <param name="completed">The completed delegate.</param>
        public AsyncWorker(
            string name,
            DoWorkEventHandler worker,
            ProgressChangedEventHandler progress,
            RunWorkerCompletedEventHandler completed)
        {
            this.name = name;

            this.worker = worker;
            this.progress = progress;
            this.completed = completed;

            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += this.DoWork;
            this.backgroundWorker.ProgressChanged += this.ProgressChanged;
            this.backgroundWorker.RunWorkerCompleted += this.Completed;
        }

        /// <summary>
        /// Gets a value indicating whether an asynchronous operation is running.
        /// </summary>
        public bool IsBusy
        {
            get { return this.backgroundWorker.IsBusy; }
        }

        /// <summary>
        /// Gets a value indicating whether the operation should be canceled.
        /// </summary>
        public bool CancellationPending
        {
            get { return this.backgroundWorker.CancellationPending; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether progress can be reported.
        /// </summary>
        public bool WorkerReportsProgress
        {
            get { return this.backgroundWorker.WorkerReportsProgress; }
            set { this.backgroundWorker.WorkerReportsProgress = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether cancellation of asynchronous operation is supported.
        /// </summary>
        public bool WorkerSupportsCancellation
        {
            get { return this.backgroundWorker.WorkerSupportsCancellation; }
            set { this.backgroundWorker.WorkerSupportsCancellation = value; }
        }

        /// <summary>
        /// Starts execution of a background operation.
        /// </summary>
        public void RunWorkerAsync()
        {
            this.RunWorkerAsync(null);
        }

        /// <summary>
        /// Starts execution of a background operation.
        /// </summary>
        /// <param name="argument">The argument passed to the worker delegate.</param>
        public void RunWorkerAsync(object argument)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info(string.Format(
                             CultureInfo.InvariantCulture,
                             "{0} executes asynchronous operation {1}.{2}({3})",
                             this,
                             this.worker.Method.DeclaringType.FullName,
                             this.worker.Method.Name,
                             argument));
            }

            this.backgroundWorker.RunWorkerAsync(argument);
        }

        /// <summary>
        /// Cancels the operation.
        /// </summary>
        public void CancelAsync()
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info(string.Format(
                             CultureInfo.InvariantCulture,
                             "{0} cancels asynchronous operation {1}.{2}()",
                             this,
                             this.worker.Method.DeclaringType.FullName,
                             this.worker.Method.Name));
            }

            this.backgroundWorker.CancelAsync();
        }

        /// <summary>
        /// Raises the <see cref="ProgressChanged"/> event.
        /// </summary>
        /// <param name="percentProgress">The percent progress.</param>
        /// <param name="userState">State of the user.</param>
        public void ReportProgress(int percentProgress, object userState)
        {
            if (Log.IsInfoEnabled)
            {
                Log.Info(string.Format(
                             CultureInfo.InvariantCulture,
                             "{0} reports progress for operation {1}.{2}()",
                             this,
                             this.worker.Method.DeclaringType.FullName,
                             this.worker.Method.Name));
            }

            this.backgroundWorker.ReportProgress(percentProgress, userState);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.name ?? this.GetType().FullName;
        }

        /// <summary>
        /// Calls the completed delegate if there is any. If no completed delegate is defined and there exists an exception then an exception is thrown.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        private void Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            this.LogOperationCompleted(e);

            if (this.completed != null)
            {
                this.completed(this, e);
            }
            else if (e.Error != null)
            {
                // For some unknown reason the ApplicationException will be 
                // swallowed by the framework, but by rethrowing the Error, 
                // the ApplicationException handler can catch and react on 
                // the Exception that occured in the background thread.
                throw new AsyncWorkerException("An error occurred in a background operation.", e.Error);
            }
        }

        /// <summary>
        /// Calls the worker delegate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            this.worker(this, e);
        }

        /// <summary>
        /// Calls the progress delegate.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progress(this, e);
        }

        /// <summary>
        /// Logs that the operation completed.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void LogOperationCompleted(RunWorkerCompletedEventArgs e)
        {
            if (Log.IsInfoEnabled)
            {
                if (e.Error == null)
                {
                    this.LogOperationCompletedWithoutException();
                }
                else
                {
                    this.LogOperationCompletedWithException(e.Error);
                }
            }
        }

        /// <summary>
        /// Logs that the operation completed without an exception.
        /// </summary>
        private void LogOperationCompletedWithoutException()
        {
            Log.Info(string.Format(
                         CultureInfo.InvariantCulture,
                         "{0} completes asynchronous operation {1}.{2}().",
                         this,
                         this.worker.Method.DeclaringType.FullName,
                         this.worker.Method.Name));
        }

        /// <summary>
        /// Logs that the operation completed with an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private void LogOperationCompletedWithException(Exception exception)
        {
            Log.Info(string.Format(
                         CultureInfo.InvariantCulture,
                         "{0} completes asynchronous operation {1}.{2}() with exception = {3}",
                         this,
                         this.worker.Method.DeclaringType.FullName,
                         this.worker.Method.Name,
                         exception));
        }
    }
}