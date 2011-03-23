//-------------------------------------------------------------------------------
// <copyright file="Log4NetAsyncWorkerExtension.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Async.Log4Net
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

    using log4net;

    /// <summary>
    /// Logger for <see cref="AsyncWorker"/> using log4net.
    /// </summary>
    public class Log4NetAsyncWorkerExtension : IAsyncWorkerExtension
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetAsyncWorkerExtension"/> class.
        /// </summary>
        public Log4NetAsyncWorkerExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetAsyncWorkerExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Log4NetAsyncWorkerExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Called when an operation is started.
        /// </summary>
        /// <param name="asyncWorker">The async worker.</param>
        /// <param name="worker">The worker.</param>
        /// <param name="argument">The argument.</param>
        public void StartedExecution(AsyncWorker asyncWorker, DoWorkEventHandler worker, object argument)
        {
            if (this.log.IsInfoEnabled)
            {
                this.log.Info(string.Format(
                             CultureInfo.InvariantCulture,
                             "{0} executes asynchronous operation {1}.{2}({3})",
                             asyncWorker,
                             worker.Method.DeclaringType.FullName,
                             worker.Method.Name,
                             argument));
            }
        }

        /// <summary>
        /// Called when an operation is cancelled.
        /// </summary>
        /// <param name="asyncWorker">The async worker.</param>
        /// <param name="worker">The worker.</param>
        public void CancellingExecution(AsyncWorker asyncWorker, DoWorkEventHandler worker)
        {
            if (this.log.IsInfoEnabled)
            {
                this.log.Info(string.Format(
                             CultureInfo.InvariantCulture,
                             "{0} cancels asynchronous operation {1}.{2}()",
                             asyncWorker,
                             worker.Method.DeclaringType.FullName,
                             worker.Method.Name));
            }
        }

        /// <summary>
        /// Called when an operation reports progress.
        /// </summary>
        /// <param name="asyncWorker">The async worker.</param>
        /// <param name="worker">The worker.</param>
        /// <param name="progress">The progress.</param>
        /// <param name="userState">State of the user.</param>
        public void ReportProgress(AsyncWorker asyncWorker, DoWorkEventHandler worker, ProgressChangedEventHandler progress, object userState)
        {
            if (this.log.IsInfoEnabled)
            {
                this.log.Info(string.Format(
                             CultureInfo.InvariantCulture,
                             "{0} reports progress for operation {1}.{2}()",
                             asyncWorker,
                             worker.Method.DeclaringType.FullName,
                             worker.Method.Name));
            }
        }

        /// <summary>
        /// Called when an operation was completed.
        /// </summary>
        /// <param name="asyncWorker">The async worker.</param>
        /// <param name="worker">The worker.</param>
        /// <param name="completed">The completed handler.</param>
        /// <param name="runWorkerCompletedEventArgs">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        public void CompletedExecution(AsyncWorker asyncWorker, DoWorkEventHandler worker, RunWorkerCompletedEventHandler completed, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (this.log.IsInfoEnabled)
            {
                if (runWorkerCompletedEventArgs.Error == null)
                {
                    this.LogOperationCompletedWithoutException(asyncWorker, worker);
                }
                else
                {
                    this.LogOperationCompletedWithException(asyncWorker, worker, runWorkerCompletedEventArgs.Error);
                }
            }
        }

        private void LogOperationCompletedWithoutException(AsyncWorker asyncWorker, DoWorkEventHandler worker)
        {
            this.log.Info(string.Format(
                         CultureInfo.InvariantCulture,
                         "{0} completes asynchronous operation {1}.{2}().",
                         asyncWorker,
                         worker.Method.DeclaringType.FullName,
                         worker.Method.Name));
        }

        private void LogOperationCompletedWithException(AsyncWorker asyncWorker, DoWorkEventHandler worker, Exception exception)
        {
            this.log.Info(string.Format(
                CultureInfo.InvariantCulture,
                "{0} completes asynchronous operation {1}.{2}() with exception = {3}",
                asyncWorker,
                worker.Method.DeclaringType.FullName,
                worker.Method.Name,
                exception));
        }
    }
}