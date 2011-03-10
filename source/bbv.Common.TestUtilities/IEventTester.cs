//-------------------------------------------------------------------------------
// <copyright file="IEventTester.cs" company="bbv Software Services AG">
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

namespace bbv.Common.TestUtilities
{
    using System;

    /// <summary>
    /// The checks that can be performed with an event tester.
    /// </summary>
    public interface IEventTester : IDisposable
    {
        /// <summary>
        /// Occurs when the monitored event is fired.
        /// </summary>
        event EventHandler Invocation;

        /// <summary>
        /// Gets the number of invocation of the monitored event.
        /// </summary>
        /// <value>The number of invocation of the monitored event.</value>
        int InvocationCount { get; }

        /// <summary>
        /// Gets a value indicating whether the event was fired.
        /// </summary>
        /// <value><c>true</c> if the event was fired; otherwise, <c>false</c>.</value>
        bool WasFired { get; }

        /// <summary>
        /// Asserts that the event was fired.
        /// </summary>
        void AssertWasFired();

        /// <summary>
        /// Asserts that the event was fired exactly as often as specified by <paramref name="expectedInvocationCount"/>.
        /// </summary>
        /// <param name="expectedInvocationCount">The expected number of invocations.</param>
        void AssertInvocationCount(int expectedInvocationCount);

        /// <summary>
        /// Asserts that all expectations of the testers are met.
        /// Used for ordered event expectations in the <see cref="EventTestList"/>.
        /// </summary>
        void AssertComplete();
    }
}
