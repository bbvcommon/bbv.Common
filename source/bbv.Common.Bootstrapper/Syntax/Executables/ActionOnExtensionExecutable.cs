//-------------------------------------------------------------------------------
// <copyright file="ActionOnExtensionExecutable.cs" company="bbv Software Services AG">
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
    /// The executable which executes an action on an extension.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class ActionOnExtensionExecutable<TExtension> : IExecutable<TExtension>
        where TExtension : IExtension
    {
        private readonly Action<TExtension> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionOnExtensionExecutable{TExtension}"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public ActionOnExtensionExecutable(Action<TExtension> action)
        {
            this.action = action;
        }

        /// <summary>
        /// Executes an operation on the specified extensions.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public void Execute(IEnumerable<TExtension> extensions)
        {
            Ensure.ArgumentNotNull(extensions, "extensions");

            foreach (TExtension extension in extensions)
            {
                this.action(extension);
            }
        }
    }
}