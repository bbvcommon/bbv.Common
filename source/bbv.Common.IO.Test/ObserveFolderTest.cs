//-------------------------------------------------------------------------------
// <copyright file="ObserveFolderTest.cs" company="bbv Software Services AG">
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

    using bbv.Common.Events;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class ObserveFolderTest
    {
        private readonly Mock<IFolderWatcher> folderWatcher;

        private readonly ObserveFolder testee;

        public ObserveFolderTest()
        {
            this.folderWatcher = new Mock<IFolderWatcher>();

            this.testee = new ObserveFolder(() => this.folderWatcher.Object);
        }

        [Fact]
        public void Start_MustSetFolder()
        {
            this.Start("FakeFolder", string.Empty);

            this.folderWatcher.VerifySet(watcher => watcher.Folder = "FakeFolder");
        }

        [Fact]
        public void Start_MustSetFilter()
        {
            this.Start(string.Empty, "FakeFilter");

            this.folderWatcher.VerifySet(watcher => watcher.Filter = "FakeFilter");
        }

        [Fact]
        public void Start_MustStartObservation()
        {
            this.Start("FakeFolder", string.Empty);

            this.folderWatcher.Verify(watcher => watcher.StartObservation());
        }

        [Fact]
        public void Start_OnFileChanged_MustStopObservation()
        {
            var task = this.StartTask(string.Empty, string.Empty);

            this.RaiseFileChanged(string.Empty);

            task.Wait();

            this.folderWatcher.Verify(watcher => watcher.StopObservation());
        }

        [Fact]
        public void Start_OnFileChanged_MustReturnPath()
        {
            const string ExpectedPath = "FakePath";

            var task = this.StartTask(string.Empty, string.Empty);

            this.RaiseFileChanged(ExpectedPath);

            task.Result.Should().Be(ExpectedPath);
        }

        [Fact]
        public void Start_OnCancel_MustStopObservation()
        {
            var tokenSource = new CancellationTokenSource();

            var task = this.testee.Start(string.Empty, string.Empty, tokenSource.Token);

            tokenSource.Cancel();

            Assert.Throws<AggregateException>(() => task.Wait());

            this.folderWatcher.Verify(watcher => watcher.StopObservation());
        }

        private void Start(string folder, string filter)
        {
            var task = this.StartTask(folder, filter);

            this.RaiseFileChanged(null);

            task.Wait();
        }

        private Task<string> StartTask(string folder, string filter)
        {
            var source = new CancellationTokenSource();

            var task = this.testee.Start(folder, filter, source.Token);

            return task;
        }

        private void RaiseFileChanged(string path)
        {
            this.folderWatcher.Raise(watcher => watcher.FileChanged += null, new EventArgs<string>(path));
        }
    }
}