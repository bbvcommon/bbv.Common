//-------------------------------------------------------------------------------
// <copyright file="ThreadHelper.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Threading
{
    using System;
    using System.Threading;

    /// <summary>
    /// Provides functionality to use usage of <see cref="Thread"/>s.
    /// </summary>
    public static class ThreadHelper
    {
        /// <summary>
        /// Random is not thread safe. The thread static attribute must be used. 
        /// But it is considered good practice to have static random instance.
        /// </summary>
        [ThreadStatic]
        private static readonly Random SleepRandomlyGenerator = new Random();

        /// <summary>
        /// Sleeps a random time but max. the specified number of milliSeconds
        /// </summary>
        /// <param name="maxMilliseconds">Number of milliseconds to sleep.</param>
        public static void SleepRandomly(int maxMilliseconds)
        {
            Thread.Sleep(SleepRandomlyGenerator.Next(maxMilliseconds));
        }
    }
}