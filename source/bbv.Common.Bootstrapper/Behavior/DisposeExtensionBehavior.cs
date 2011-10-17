//-------------------------------------------------------------------------------
// <copyright file="DisposeExtensionBehavior.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Behavior
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using bbv.Common.Formatters;

    /// <summary>
    /// Behavior which disposes all extensions which implement IDisposable
    /// </summary>
    public class DisposeExtensionBehavior : IBehavior<IExtension>
    {
        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.GetType().FullNameToString();
            }
        }

        /// <summary>
        /// Diposes all extensions which implement IDisposable.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public void Behave(IEnumerable<IExtension> extensions)
        {
            foreach (IDisposable extension in extensions.OfType<IDisposable>())
            {
                extension.Dispose();
            }
        }

        /// <inheritdoc />
        public string Describe()
        {
            return
                "Behaves on all extensions by checking whether they implement IDisposable and disposing them if this is the case.";
        }
    }
}