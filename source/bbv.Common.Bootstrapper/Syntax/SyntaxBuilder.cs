//-------------------------------------------------------------------------------
// <copyright file="SyntaxBuilder.cs" company="bbv Software Services AG">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The syntax builder.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class SyntaxBuilder<TExtension> : ISyntaxBuilder<TExtension>
        where TExtension : IExtension
    {
        private static readonly Action DoNothing = () => { };

        private readonly Queue<IExecutable<TExtension>> executables;

        private readonly IExecutableFactory<TExtension> executableFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxBuilder&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <param name="executableFactory">The executable factory.</param>
        public SyntaxBuilder(IExecutableFactory<TExtension> executableFactory)
        {
            this.executableFactory = executableFactory;
            this.executables = new Queue<IExecutable<TExtension>>();
        }

        /// <summary>
        /// Gets the currently built executable
        /// </summary>
        protected IExecutable<TExtension> BuiltExecutable { get; private set; }

        /// <summary>
        /// Attaches a behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        public IWithBehavior<TExtension> With(IBehavior<TExtension> behavior)
        {
            if (!this.executables.Any())
            {
                this.WithAction(DoNothing);
            }

            this.BuiltExecutable.Add(behavior);

            return this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IExecutable<TExtension>> GetEnumerator()
        {
            return this.executables.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds an execution action to the currently built syntax.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current syntax builder.</returns>
        public IWithBehavior<TExtension> Execute(Action action)
        {
            return this.WithAction(action);
        }

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
        public IWithBehaviorOnContext<TExtension, TContext> Execute<TContext>(Func<TContext> initializer, Action<TExtension, TContext> action)
        {
            return this.WithInitializerAndActionOnExtension(initializer, action);
        }

        /// <summary>
        /// Adds an execution action which operates on the extension to the
        /// currently built syntax.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current syntax builder.</returns>
        public IWithBehavior<TExtension> Execute(Action<TExtension> action)
        {
            return this.WithActionOnExtension(action);
        }

        private ISyntaxBuilder<TExtension> WithAction(Action action)
        {
            var executable = this.executableFactory.CreateExecutable(action);

            this.executables.Enqueue(executable);
            this.BuiltExecutable = executable;

            return this;
        }

        private ISyntaxBuilder<TExtension> WithActionOnExtension(Action<TExtension> action)
        {
            var executable = this.executableFactory.CreateExecutable(action);

            this.executables.Enqueue(executable);
            this.BuiltExecutable = executable;

            return this;
        }

        private IWithBehaviorOnContext<TExtension, TContext> WithInitializerAndActionOnExtension<TContext>(Func<TContext> initializer, Action<TExtension, TContext> action)
        {
            var providerQueue = new Queue<Func<TContext, IBehavior<TExtension>>>();

            var executable = this.executableFactory.CreateExecutable(
                behaviorAware =>
                    {
                        var context = initializer();

                        foreach (Func<TContext, IBehavior<TExtension>> provider in providerQueue)
                        {
                            behaviorAware.Add(provider(context));
                        }

                    return context;
                },
                action);

            this.executables.Enqueue(executable);
            this.BuiltExecutable = executable;

            return new SyntaxBuilderWithContext<TContext>(this, providerQueue);
        }

        private class SyntaxBuilderWithContext<TContext> : IWithBehaviorOnContext<TExtension, TContext>
        {
            private readonly Queue<Func<TContext, IBehavior<TExtension>>> behaviorProviders;

            private readonly SyntaxBuilder<TExtension> syntaxBuilder;

            public SyntaxBuilderWithContext(SyntaxBuilder<TExtension> syntaxBuilder, Queue<Func<TContext, IBehavior<TExtension>>> behaviorProviders)
            {
                this.syntaxBuilder = syntaxBuilder;
                this.behaviorProviders = behaviorProviders;
            }

            public IWithBehavior<TExtension> Execute(Action action)
            {
                return this.syntaxBuilder.Execute(action);
            }

            public IWithBehavior<TExtension> Execute(Action<TExtension> action)
            {
                return this.syntaxBuilder.Execute(action);
            }

            public IWithBehaviorOnContext<TExtension, TChainedContext> Execute<TChainedContext>(Func<TChainedContext> initializer, Action<TExtension, TChainedContext> action)
            {
                return this.syntaxBuilder.Execute(initializer, action);
            }

            public IWithBehaviorOnContext<TExtension, TContext> With(Func<TContext, IBehavior<TExtension>> provider)
            {
                this.behaviorProviders.Enqueue(provider);

                return this;
            }

            public IEnumerator<IExecutable<TExtension>> GetEnumerator()
            {
                return this.syntaxBuilder.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}