//-------------------------------------------------------------------------------
// <copyright file="SingleArgumentGuardHolder.cs" company="bbv Software Services AG">
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
    /// Holds a single argument guard.
    /// </summary>
    /// <typeparam name="T">Type of the argument of the guard.</typeparam>
    public class SingleArgumentGuardHolder<T> : IGuardHolder
    {
        private readonly Func<T, bool> guard;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleArgumentGuardHolder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="guard">The guard.</param>
        public SingleArgumentGuardHolder(Func<T, bool> guard)
        {
            this.guard = guard;
        }

        /// <summary>
        /// Executes the guard.
        /// </summary>
        /// <param name="arguments">The state machine event arguments.</param>
        /// <returns>Result of the guard execution.</returns>
        public bool Execute(object[] arguments)
        {
            Ensure.ArgumentNotNull(arguments, "arguments");

            if (arguments.Length != 1)
            {
                throw new ArgumentException(ExceptionMessages.CannotPassMultipleArgumentsToSingleArgumentGuard(arguments, this.Describe()));
            }

            if (!typeof(T).IsAssignableFrom(arguments[0].GetType()))
            {
                throw new ArgumentException(ExceptionMessages.CannotCastArgumentToGuardArgument(arguments[0], this.Describe()));
            }

            return this.guard((T)arguments[0]);
        }

        /// <summary>
        /// Describes the guard.
        /// </summary>
        /// <returns>Description of the guard.</returns>
        public string Describe()
        {
            return this.guard.Method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any() ? "anonymous" : this.guard.Method.Name;
        }
    }
}