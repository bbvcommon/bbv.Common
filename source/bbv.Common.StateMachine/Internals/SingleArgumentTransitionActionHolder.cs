//-------------------------------------------------------------------------------
// <copyright file="SingleArgumentTransitionActionHolder.cs" company="bbv Software Services AG">
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

namespace bbv.Common.StateMachine.Internals
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Holds a transition action with exactly one argument.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    public class SingleArgumentTransitionActionHolder<T> : ITransitionActionHolder
    {
        private readonly Action<T> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleArgumentTransitionActionHolder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public SingleArgumentTransitionActionHolder(Action<T> action)
        {
            this.action = action;
        }

        /// <summary>
        /// Executes the transition action.
        /// </summary>
        /// <param name="arguments">The state machine event arguments.</param>
        public void Execute(object[] arguments)
        {
            Ensure.ArgumentNotNull(arguments, "arguments");

            if (arguments.Length != 1)
            {
                throw new ArgumentException(ExceptionMessages.CannotPassMultipleArgumentsToSingleArgumentAction(arguments, this.Describe()));
            }

            if (!typeof(T).IsAssignableFrom(arguments[0].GetType()))
            {
                throw new ArgumentException(ExceptionMessages.CannotCastArgumentToActionArgument(arguments[0], this.Describe()));
            }

            var argument = (T)arguments[0];

            this.action(argument);
        }

        /// <summary>
        /// Describes the action.
        /// </summary>
        /// <returns>Description of the action.</returns>
        public string Describe()
        {
            return this.action.Method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any() ? "anonymous" : this.action.Method.Name;
        }
    }
}