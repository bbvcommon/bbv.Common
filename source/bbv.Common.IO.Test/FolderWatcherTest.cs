//-------------------------------------------------------------------------------
// <copyright file="FolderWatcherTest.cs" company="bbv Software Services AG">
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
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using bbv.Common.TestUtilities;

    using NUnit.Framework;

    [TestFixture]
    public class FolderWatcherTest
    {
        private readonly string tempPath1 = Path.Combine(Path.GetTempPath(), "WorkOrderFileWatcherTest1\\");

        private readonly string tempPath2 = Path.Combine(Path.GetTempPath(), "WorkOrderFileWatcherTest2\\");

        private Log4netHelper log4Net;

        private FolderWatcher testee;

        [SetUp]
        public void SetUp()
        {
            this.log4Net = new Log4netHelper();

            this.CreateFolders();
            this.testee = new FolderWatcher(this.tempPath1, "*.*");
        }

        [TearDown]
        public void TearDown()
        {
            this.testee.StopObservation();
            this.testee = null;

            this.DeleteFolders();
            Thread.Sleep(100);

            this.log4Net.DumpAllMessagesToConsole();
            this.log4Net.Dispose();
        }

        [Test]
        public void Filter()
        {
            this.testee.Filter = "filter";
            Assert.AreEqual("filter", this.testee.Filter);
        }

        [Test]
        public void Folder()
        {
            this.testee.Folder = "foldername";
            Assert.AreEqual("foldername", this.testee.Folder);
        }

        [Test]
        public void GetCurrrentAvailableFilesNoFiles()
        {
            IList<string> files = this.testee.GetCurrentAvailableFiles();
            Assert.AreEqual(0, files.Count, "There sould be no files available");
        }

        [Test]
        public void GetCurrrentAvailableFilesTwoFiles()
        {
            FileStream stream = File.Create(Path.Combine(this.tempPath1, Path.GetRandomFileName()));
            stream.Close();
            stream = File.Create(Path.Combine(this.tempPath1, Path.GetRandomFileName()));
            stream.Close();

            IList<string> files = this.testee.GetCurrentAvailableFiles();

            Assert.AreEqual(2, files.Count, "There sould be no files available");
        }

        [Test]
        [Ignore("fragile test")]
        public void StartObservationOneFile()
        {
            AutoResetEvent signal = new AutoResetEvent(false);

            int found = 0;
            this.testee.FileChanged += delegate
                                      {
                                          found++;
                                          signal.Set();
                                      };
            this.testee.StartObservation();

            this.CreateAndCopyFile();

            Assert.IsTrue(signal.WaitOne(5000, false), "signal not received.");
            Assert.AreEqual(1, found, "There should be one new file detected");
        }

        [Test]
        public void StopObservation()
        {
            this.testee.StartObservation();
            this.testee.StopObservation();
        }

        private void CreateAndCopyFile()
        {
            // The file watcher doesn't work if the own process do file actions - therefor a new process is 
            // started to create file system events.
            string filename = Path.GetRandomFileName();
            FileStream fileStream = File.Create(Path.Combine(this.tempPath2, filename));
            fileStream.Write(new byte[1024 * 1024], 0, 1024 * 1024);
            fileStream.Close();

            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
                {
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    WorkingDirectory = this.tempPath1
                };

            Process proc = Process.Start(psi);
            if (proc == null)
            {
                throw new ApplicationException("Cannot start process for testing");
            }

            proc.StandardInput.WriteLine(
                string.Format("copy \"{0}\" \"{1}\"", Path.Combine(this.tempPath2, filename), filename));
            Thread.Sleep(300);
            proc.Kill();
            proc.Close();
        }

        private void CreateFolders()
        {
            this.DeleteFolders();
            
            Directory.CreateDirectory(this.tempPath1);
            Directory.CreateDirectory(this.tempPath2);
        }

        private void DeleteFolders()
        {
            if (Directory.Exists(this.tempPath1))
            {
                Directory.Delete(this.tempPath1, true);
            }
            
            if (Directory.Exists(this.tempPath2))
            {
                Directory.Delete(this.tempPath2, true);
            }
        }
    }
}
