//-------------------------------------------------------------------------------
// <copyright file="IExtension.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper
{
    using System;

    /// <summary>
    /// Interface for bootstrapper extensions
    /// </summary>
    /// <remarks>The extensions are executed the following way: 
    /// 0. Start --> Possible to influence starting procedure without having any prerequisites.
    /// ...
    /// (n-2). Finished --> Bootstrapping is finished.
    /// (n-1). Shutdown --> Bootstrapper shuts down.
    /// (n). Dispose --> Bootstraper is being disposed.
    /// </remarks>
    public interface IExtension : IDisposable
    {
        /// <summary>
        /// Called when the bootstrapping mechanism is starting.
        /// </summary>
        /// <exception cref="BootstrapperException">When an exception occurred during bootstrapping.</exception>
        void Start();

        /// <summary>
        /// Called when the bootstrapping mechanism is finished.
        /// </summary>
        /// <exception cref="BootstrapperException">When an exception occurred during bootstrapping.</exception>
        void Finished();

        /// <summary>
        /// Called when the bootstrapping mechanism is shutting down.
        /// </summary>
        /// <exception cref="BootstrapperException">When an exception occurred during bootstrapping.</exception>
        void Shutdown();
    }
}