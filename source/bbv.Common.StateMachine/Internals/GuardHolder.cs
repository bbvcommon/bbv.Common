//-------------------------------------------------------------------------------
// <copyright file="GuardHolder.cs" company="bbv Software Services AG">
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
    /// Holds a guard.
    /// </summary>
    public class GuardHolder : IGuardHolder
    {
        private readonly Func<object[], bool> guard;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardHolder"/> class.
        /// </summary>
        /// <param name="guard">The guard.</param>
        public GuardHolder(Func<object[], bool> guard)
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
            return this.guard(arguments);
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