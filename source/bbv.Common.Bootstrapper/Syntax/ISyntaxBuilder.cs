//-------------------------------------------------------------------------------
// <copyright file="ISyntaxBuilder.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Syntax
{
    using System;

    /// <summary>
    /// Syntax builder.
    /// </summary>
    /// <typeparam name="TExtension">The extension.</typeparam>
    public interface ISyntaxBuilder<TExtension> : ISyntax<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Adds an execution action to the currently built syntax.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current syntax builder.</returns>
        ISyntaxBuilder<TExtension> Execute(Action action);

        /// <summary>
        /// Adds an context initializer and an execution action which gets
        /// access to the context to the currently built syntax.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="initializer">The context initializer.</param>
        /// <param name="action">The action with access to the context.</param>
        /// <returns>
        /// The current syntax builder.
        /// </returns>
        ISyntaxBuilder<TExtension> Execute<TContext>(Func<TContext> initializer, Action<TExtension, TContext> action);

        /// <summary>
        /// Adds an execution action which operates on the extension to the
        /// currently built syntax.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current syntax builder.</returns>
        ISyntaxBuilder<TExtension> Execute(Action<TExtension> action);
    }
}