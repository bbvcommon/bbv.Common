//-------------------------------------------------------------------------------
// <copyright file="IWithBehavior.cs" company="bbv Software Services AG">
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
    /// Fluent definition syntax interface for behaviors.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IWithBehavior<TExtension> : IExecuteAction<TExtension>, IExecuteActionOnExtension<TExtension>, IExecuteActionOnExtensionWithContext<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Attaches a behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        IWithBehavior<TExtension> With(IBehavior<TExtension> behavior);

        /// <summary>
        /// Attaches a lazy behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        IWithBehavior<TExtension> With(Func<IBehavior<TExtension>> behavior);
    }
}