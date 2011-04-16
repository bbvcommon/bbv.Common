//-------------------------------------------------------------------------------
// <copyright file="ActionOnExtensionWithInitializerExecutable.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Syntax.Executables
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Executable which executes an initializer and passes the initialized
    /// context to the action which operates on the extension.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class ActionOnExtensionWithInitializerExecutable<TContext, TExtension> : IExecutable<TExtension>
        where TExtension : IExtension
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Will be used later.")]
        private readonly Func<TContext> initializer;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Will be used later.")]
        private readonly Action<TExtension, TContext> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionOnExtensionWithInitializerExecutable{TContext, TExtension}"/> class.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        /// <param name="action">The action.</param>
        public ActionOnExtensionWithInitializerExecutable(Func<TContext> initializer, Action<TExtension, TContext> action)
        {
            this.action = action;
            this.initializer = initializer;
        }

        /// <summary>
        /// Executes an operation on the specified extensions.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public void Execute(IEnumerable<TExtension> extensions)
        {
        }
    }
}