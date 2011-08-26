//-------------------------------------------------------------------------------
// <copyright file="ExecutableFactory.cs" company="bbv Software Services AG">
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

    using bbv.Common.Bootstrapper.Syntax.Executables;

    /// <summary>
    /// Factory which creates the set of executables.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class ExecutableFactory<TExtension> : IExecutableFactory<TExtension>
        where TExtension : IExtension
    {
        /// <inheritdoc />
        public IExecutable<TExtension> CreateExecutable(Action action)
        {
            return new ActionExecutable<TExtension>(action);
        }

        /// <inheritdoc />
        public IExecutable<TExtension> CreateExecutable<TContext>(Func<TContext> initializer, Action<TExtension, TContext> action, Action<IBehaviorAware<TExtension>, TContext> contextInterceptor)
        {
            return new ActionOnExtensionWithInitializerExecutable<TContext, TExtension>(initializer, action, contextInterceptor);
        }

        /// <inheritdoc />
        public IExecutable<TExtension> CreateExecutable(Action<TExtension> action)
        {
            return new ActionOnExtensionExecutable<TExtension>(action);
        }
    }
}