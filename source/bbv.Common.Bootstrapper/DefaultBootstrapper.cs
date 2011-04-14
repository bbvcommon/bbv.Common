//-------------------------------------------------------------------------------
// <copyright file="DefaultBootstrapper.cs" company="bbv Software Services AG">
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
    /// The bootstrapper.
    /// </summary>
    public class DefaultBootstrapper : IBootstrapper
    {
        /// <summary>
        /// Adds the extension to the bootstrapping mechanism. The extensions are executed in the order which they were
        /// added.
        /// </summary>
        /// <param name="extension">The extension to be added.</param>
        public void AddExtension(IExtension extension)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initializes the bootstrapper with the strategy.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        public void Initialize(IStrategy strategy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Runs the bootstrapper.
        /// </summary>
        /// <exception cref="BootstrapperException">When an exception occurred during bootstrapping.</exception>
        public void Run()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shutdowns the bootstrapper.
        /// </summary>
        /// <exception cref="BootstrapperException">When an exception occurred during bootstrapping.</exception>
        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}