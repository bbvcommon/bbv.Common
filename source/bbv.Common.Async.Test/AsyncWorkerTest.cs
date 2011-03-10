//-------------------------------------------------------------------------------
// <copyright file="AsyncWorkerTest.cs" company="bbv Software Services AG">
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
    using System.Threading;
    using NUnit.Framework;

    /// <summary>
    /// Tests the <see cref="AsyncWorker"/> class.
    /// </summary>
    [TestFixture]
    public class AsyncWorkerTest
    {
        /// <summary>Time out for signals.</summary>
        private const int TimeOut = 1000;

        /// <summary>Exception that was caught on global exception handler.</summary>
        private Exception caughtException;

        /// <summary>Event that signals that an unhandled exception was caught globally.</summary>
        private AutoResetEvent caughtExceptionSignal;

        /// <summary>
        /// Sets up a test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.caughtException = null;
            this.caughtExceptionSignal = new AutoResetEvent(false);
        }

        /// <summary>
        /// An operation can be executed asynchronously.
        /// </summary>
        [Test]
        public void ExecuteOperation()
        {
            AutoResetEvent go = new AutoResetEvent(false);
            AutoResetEvent workerExecuted = new AutoResetEvent(false);

            DoWorkEventHandler worker = delegate
            {
                go.WaitOne();
                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker);

            testee.RunWorkerAsync();
            go.Set();

            Assert.IsTrue(workerExecuted.WaitOne(TimeOut), "worker did not execute.");
        }

        /// <summary>
        /// An operation can be executed asynchronously and arguments can be passed.
        /// </summary>
        [Test]
        public void ExecuteOperationAndPassArguments()
        {
            AutoResetEvent go = new AutoResetEvent(false);
            AutoResetEvent workerExecuted = new AutoResetEvent(false);

            const string Argument = "test";

            DoWorkEventHandler worker = delegate(object sender, DoWorkEventArgs e)
            {
                Assert.AreEqual(Argument, e.Argument, "argument is not passed.");
                go.WaitOne();
                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker);

            testee.RunWorkerAsync(Argument);
            go.Set();

            Assert.IsTrue(workerExecuted.WaitOne(TimeOut), "worker did not execute.");
        }

        /// <summary>
        /// The completed handler is called.
        /// </summary>
        [Test]
        public void ExecuteOperationWithCompletedHandler()
        {
            AutoResetEvent workerExecuted = new AutoResetEvent(false);

            const int Result = 3;

            DoWorkEventHandler worker = delegate(object sender, DoWorkEventArgs e)
            {
                e.Result = Result;
            };

            RunWorkerCompletedEventHandler completed = delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                Assert.IsNull(e.Error);
                Assert.AreEqual(Result, e.Result);

                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker, completed);

            testee.RunWorkerAsync();

            Assert.IsTrue(workerExecuted.WaitOne(TimeOut), "worker did not execute.");
        }

        /// <summary>
        /// When an exception is thrown in the asynchronous operation and no completed handler is passed to the generic worker
        /// then the exception can be caught on the AppDomain.CurrentDomain.UnhandledException handler.
        /// </summary>
        [Test]
        [Ignore("Cannot be executed by NUnit console because it'd crash the process.")]
        public void ExecuteFailingOperation()
        {
            AppDomain.CurrentDomain.UnhandledException += this.UnhandledException;

            DoWorkEventHandler worker = delegate
            {
                throw new InvalidOperationException("test");
            };

            AsyncWorker testee = new AsyncWorker(worker);

            testee.RunWorkerAsync();

            Assert.IsTrue(this.caughtExceptionSignal.WaitOne(TimeOut), "no exception caught");

            AppDomain.CurrentDomain.UnhandledException -= this.UnhandledException;
        }

        /// <summary>
        /// When an asynchronous operation throws an exception and there is a completed handler defined then
        /// the exception is passed to the completed handler.
        /// </summary>
        [Test]
        public void ExecuteFailingOperationWithCompletedHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += this.UnhandledException;

            AutoResetEvent workerExecuted = new AutoResetEvent(false);

            DoWorkEventHandler worker = delegate
            {
                throw new InvalidOperationException("test");
            };

            RunWorkerCompletedEventHandler completed = delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                Assert.IsInstanceOf<InvalidOperationException>(e.Error, "wrong exception");
                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker, completed);

            testee.RunWorkerAsync();

            Assert.IsTrue(workerExecuted.WaitOne(TimeOut), "worker did not execute.");

            Assert.IsNull(this.caughtException, "no exception should be handled globally.");
            AppDomain.CurrentDomain.UnhandledException -= this.UnhandledException;
        }

        /// <summary>
        /// An asynchronous operation can be canceled.
        /// </summary>
        [Test]
        public void CancelOperation()
        {
            AutoResetEvent workerStarted = new AutoResetEvent(false);
            AutoResetEvent workerExecuted = new AutoResetEvent(false);
            AutoResetEvent workerCancelled = new AutoResetEvent(false);
            AutoResetEvent allowTerminating = new AutoResetEvent(false);

            DoWorkEventHandler worker = delegate(object sender, DoWorkEventArgs e)
            {
                AsyncWorker genericWorker = (AsyncWorker)sender;
                genericWorker.WorkerSupportsCancellation = true;

                workerStarted.Set();
                while (!genericWorker.CancellationPending)
                {
                    Thread.Sleep(1);
                }

                e.Cancel = true;
                workerCancelled.Set();
                allowTerminating.WaitOne();
            };

            RunWorkerCompletedEventHandler completed = delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                Assert.IsTrue(e.Cancelled, "result does not reflect canceled state.");
                workerExecuted.Set();
            };

            AsyncWorker testee = new AsyncWorker(worker, completed);

            testee.RunWorkerAsync();
            Assert.IsTrue(workerStarted.WaitOne(TimeOut), "worker did not start.");

            testee.CancelAsync();
            Assert.IsTrue(workerCancelled.WaitOne(TimeOut), "worker did not cancel.");

            allowTerminating.Set();
            Assert.IsTrue(workerExecuted.WaitOne(TimeOut), "worker did not execute.");
        }

        /// <summary>
        /// The worker can notify about progress.
        /// </summary>
        [Test]
        public void Progress()
        {
            AutoResetEvent workerExecuted = new AutoResetEvent(false);

            DoWorkEventHandler worker = delegate(object sender, DoWorkEventArgs e)
            {
                AsyncWorker genericWorker = (AsyncWorker)sender;
                genericWorker.WorkerReportsProgress = true;

                for (int i = 0; i <= 100; i += 10)
                {
                    genericWorker.ReportProgress(i, null);
                }
            };

            int count = 0;
            ProgressChangedEventHandler progress = (sender, e) =>
            {
                count++;
                if (count == 10)
                {
                    workerExecuted.Set();
                }
            };

            AsyncWorker testee = new AsyncWorker(worker, progress, null);

            testee.RunWorkerAsync();

            Assert.IsTrue(workerExecuted.WaitOne(TimeOut), "worker did not execute.");
        }

        /// <summary>
        /// Handler for unhandled exceptions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.caughtException = (Exception)e.ExceptionObject;
            this.caughtExceptionSignal.Set();
        }
    }
}