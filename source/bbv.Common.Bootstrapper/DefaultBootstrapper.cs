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

    using bbv.Common.Bootstrapper.Reporting;

    /// <summary>
    /// The bootstrapper.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class DefaultBootstrapper<TExtension> : IBootstrapper<TExtension>
        where TExtension : IExtension
    {
        private readonly IExtensionHost<TExtension> extensionHost;

        private IStrategy<TExtension> strategy;

        private IReportingContext reportingContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBootstrapper{TExtension}"/> class.
        /// </summary>
        public DefaultBootstrapper()
            : this(new ExtensionHost<TExtension>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBootstrapper{TExtension}"/> class.
        /// </summary>
        /// <param name="extensionHost">The extension host.</param>
        public DefaultBootstrapper(IExtensionHost<TExtension> extensionHost)
        {
            this.extensionHost = extensionHost;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DefaultBootstrapper{TExtension}"/> class.
        /// </summary>
        ~DefaultBootstrapper()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// Adds the extension to the bootstrapping mechanism. The extensions are executed in the order which they were
        /// added.
        /// </summary>
        /// <param name="extension">The extension to be added.</param>
        public void AddExtension(TExtension extension)
        {
            this.extensionHost.AddExtension(extension);
        }

        /// <summary>
        /// Initializes the bootstrapper with the strategy.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        public void Initialize(IStrategy<TExtension> strategy)
        {
            Ensure.ArgumentNotNull(strategy, "strategy");

            this.CheckAlreadyInitialized();

            this.strategy = strategy;
            this.reportingContext = this.strategy.CreateReportingContext();
        }

        /// <summary>
        /// Runs the bootstrapper.
        /// </summary>
        /// <exception cref="BootstrapperException">When an exception occurred during bootstrapping.</exception>
        public void Run()
        {
            this.CheckIsInitialized();

            var syntax = this.strategy.BuildRunSyntax();

            IExecutor<TExtension> runExecutor = this.strategy.CreateRunExecutor();
            IExecutionContext runExecutionContext = this.reportingContext.CreateRunExecutionContext(runExecutor);

            runExecutor.Execute(syntax, this.extensionHost.Extensions, runExecutionContext);
        }

        /// <summary>
        /// Shutdowns the bootstrapper.
        /// </summary>
        /// <exception cref="BootstrapperException">When an exception occurred during bootstrapping.</exception>
        public void Shutdown()
        {
            this.CheckIsInitialized();

            this.Dispose();
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
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed && disposing)
            {
                var syntax = this.strategy.BuildShutdownSyntax();

                IExecutor<TExtension> shutdownExecutor = this.strategy.CreateShutdownExecutor();
                IExecutionContext shutdownExecutionContext = this.reportingContext.CreateShutdownExecutionContext(shutdownExecutor);

                shutdownExecutor.Execute(syntax, this.extensionHost.Extensions, shutdownExecutionContext);

                this.strategy.Dispose();

                this.IsDisposed = true;
            }
        }

        private void CheckIsInitialized()
        {
            if (this.strategy == null)
            {
                throw new InvalidOperationException("Bootstrapper must be initialized before run or shutdown.");
            }
        }

        private void CheckAlreadyInitialized()
        {
            if (this.strategy != null)
            {
                throw new InvalidOperationException("Bootstrapper can only be initialized once.");
            }
        }
    }
}