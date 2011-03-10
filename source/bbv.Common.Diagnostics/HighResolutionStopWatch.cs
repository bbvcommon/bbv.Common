//-------------------------------------------------------------------------------
// <copyright file="HighResolutionStopWatch.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Diagnostics
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// The <see cref="HighResolutionStopWatch"/> is used to meassure time between the calls to Start and Stop.
    /// </summary>
    /// <example>
    /// <code>
    /// PerformanceTimer pt = new PerformanceTimer();
    /// pt.Start();
    /// //do something
    /// pt.Stop();
    /// Console.WriteLine(pt.ElapsedTimeMilliseconds);
    /// </code>
    /// </example>
    public class HighResolutionStopWatch
    {
        private readonly long frequency;

        private long startCount;

        private long stopCount;

        private long elapsedCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="HighResolutionStopWatch"/> class and initializes 
        /// the <see cref="Frequency"/> of the high-resolution performance counter.
        /// </summary>
        public HighResolutionStopWatch()
        {
            this.frequency = 0;
            QueryPerformanceFrequency(ref this.frequency);
        }

        /// <summary>
        /// Gets the elapsed counts of the last performance measure.
        /// </summary>
        public long Elapsed
        {
            get { return this.elapsedCount; }
        }

        /// <summary>
        /// Gets the elapsed seconds of the last performance measure.
        /// </summary>
        public double Seconds
        {
            get { return (double)this.elapsedCount / this.frequency; }
        }

        /// <summary>
        /// Gets the elapsed milliseconds of the last performance measure.
        /// </summary>
        public double Milliseconds
        {
            get { return (1000 * (double)this.elapsedCount) / this.frequency; }
        }

        /// <summary>
        /// Gets the Frequency of of the high-resolution performance counter.
        /// </summary>
        public long Frequency
        {
            get { return this.frequency; }
        }

        /// <summary>
        /// Starts the performance measuring.
        /// </summary>
        public void Start()
        {
            this.startCount = 0;
            QueryPerformanceCounter(ref this.startCount);
        }

        /// <summary>
        /// Stops the performance measuring.
        /// </summary>
        public void Stop()
        {
            this.stopCount = 0;
            QueryPerformanceCounter(ref this.stopCount);
            this.elapsedCount = this.stopCount - this.startCount;
        }

        /// <summary>
        /// Resets all internal variables.
        /// </summary>
        public void Reset()
        {
            this.startCount = 0;
            this.stopCount = 0;
            this.elapsedCount = 0;
        }

        [DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(ref long performanceCount);

        [DllImport("KERNEL32")]
        private static extern bool QueryPerformanceFrequency(ref long frequency);
    }
}
