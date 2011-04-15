//-------------------------------------------------------------------------------
// <copyright file="AbstractStrategy.cs" company="bbv Software Services AG">
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

    using bbv.Common.Bootstrapper.Syntax;

    /// <summary>
    /// Abstract strategy definition.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public abstract class AbstractStrategy<TExtension> : IStrategy
        where TExtension : IExtension
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Will be used later.")]
        private readonly ISyntaxBuilder<TExtension> runSyntaxBuilder;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Will be used later.")]
        private readonly ISyntaxBuilder<TExtension> shutdownSyntaxBuilder;

        private bool runSyntaxBuilded;

        private bool shutdownSyntaxBuilded;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractStrategy&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <remarks>Uses the default syntax builder.</remarks>
        protected AbstractStrategy()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractStrategy&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <param name="runSyntaxBuilder">The run syntax builder.</param>
        /// <param name="shutdownSyntaxBuilder">The shutdown syntax builder.</param>
        protected AbstractStrategy(ISyntaxBuilder<TExtension> runSyntaxBuilder, ISyntaxBuilder<TExtension> shutdownSyntaxBuilder)
        {
            this.shutdownSyntaxBuilder = shutdownSyntaxBuilder;
            this.runSyntaxBuilder = runSyntaxBuilder;
        }

        /// <summary>
        /// Builds the run syntax.
        /// </summary>
        /// <returns>
        /// The run syntax.
        /// </returns>
        public ISyntax BuildRunSyntax()
        {
            this.AssertRunSyntaxAvailable();

            return default(ISyntax);
        }

        /// <summary>
        /// Builds the shutdown syntax.
        /// </summary>
        /// <returns>
        /// The shutdown syntax.
        /// </returns>
        public ISyntax BuildShutdownSyntax()
        {
            this.AssertShutdownSyntaxAvailable();

            return default(ISyntax);
        }

        /// <summary>
        /// Fluently defines the run syntax on the specified builder.
        /// </summary>
        /// <param name="builder">The syntax builder</param>
        protected abstract void DefineRunSyntax(ISyntaxBuilder<TExtension> builder);

        /// <summary>
        /// Fluently defines the shutdown syntax on the specified builder.
        /// </summary>
        /// <param name="builder">The syntax builder</param>
        protected abstract void DefineShutdownSyntax(ISyntaxBuilder<TExtension> builder);

        private void AssertRunSyntaxAvailable()
        {
            if (this.runSyntaxBuilded)
            {
                throw new InvalidOperationException("The run syntax can only be acquired once.");
            }

            this.runSyntaxBuilded = true;
        }

        private void AssertShutdownSyntaxAvailable()
        {
            if (this.shutdownSyntaxBuilded)
            {
                throw new InvalidOperationException("The shutdown syntax can only be acquired once.");
            }

            this.shutdownSyntaxBuilded = true;
        }
    }
}