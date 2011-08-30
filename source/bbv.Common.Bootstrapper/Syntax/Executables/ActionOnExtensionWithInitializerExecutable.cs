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
    using System.Globalization;
    using System.Linq.Expressions;

    /// <summary>
    /// Executable which executes an initializer and passes the initialized
    /// context to the action which operates on the extension.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class ActionOnExtensionWithInitializerExecutable<TContext, TExtension> : IExecutable<TExtension>
        where TExtension : IExtension
    {
        private readonly Queue<IBehavior<TExtension>> behaviors;

        private readonly Expression<Func<TContext>> initializerExpression;
        private readonly Func<TContext> initializer;

        private readonly Expression<Action<TExtension, TContext>> actionExpression;
        private readonly Action<TExtension, TContext> action;

        private readonly Action<IBehaviorAware<TExtension>, TContext> contextInterceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionOnExtensionWithInitializerExecutable{TContext, TExtension}"/> class.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        /// <param name="action">The action.</param>
        /// <param name="contextInterceptor">The behavior aware.</param>
        public ActionOnExtensionWithInitializerExecutable(Expression<Func<TContext>> initializer, Expression<Action<TExtension, TContext>> action, Action<IBehaviorAware<TExtension>, TContext> contextInterceptor)
        {
            Ensure.ArgumentNotNull(action, "action");

            this.contextInterceptor = contextInterceptor;
            this.behaviors = new Queue<IBehavior<TExtension>>();

            this.actionExpression = action;
            this.initializerExpression = initializer;

            this.action = this.actionExpression.Compile();
            this.initializer = this.initializerExpression.Compile();
        }

        /// <summary>
        /// Executes an operation on the specified extensions.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public void Execute(IEnumerable<TExtension> extensions)
        {
            Ensure.ArgumentNotNull(extensions, "extensions");

            TContext context = this.initializer();

            this.contextInterceptor(this, context);

            foreach (IBehavior<TExtension> behavior in this.behaviors)
            {
                behavior.Behave(extensions);
            }

            foreach (TExtension extension in extensions)
            {
                this.action(extension, context);
            }
        }

        /// <summary>
        /// Adds the specified behavior.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public void Add(IBehavior<TExtension> behavior)
        {
            this.behaviors.Enqueue(behavior);
        }
    }
}