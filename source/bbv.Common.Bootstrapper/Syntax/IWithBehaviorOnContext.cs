//-------------------------------------------------------------------------------
// <copyright file="IWithBehaviorOnContext.cs" company="bbv Software Services AG">
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
    /// Fluent definition syntax interface for behaviors which operate on contexts.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public interface IWithBehaviorOnContext<TExtension, out TContext> : IExecuteAction<TExtension>, IExecuteActionOnExtension<TExtension>, IExecuteActionOnExtensionWithContext<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Attaches a behavior which has access to the context to the currently built executable.
        /// </summary>
        /// <param name="provider">The behavior provider.</param>
        /// <returns>The syntax.</returns>
        IWithBehaviorOnContext<TExtension, TContext> With(Func<TContext, IBehavior<TExtension>> provider);
    }
}