//-------------------------------------------------------------------------------
// <copyright file="HaveConversionCallbacks.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Configuration.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Default IHaveConversionCallbacks
    /// </summary>
    public class HaveConversionCallbacks : IHaveConversionCallbacks
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HaveConversionCallbacks"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public HaveConversionCallbacks(IExtension extension)
        {
            var callbacksProvider = extension as IHaveConversionCallbacks;

            this.ConversionCallbacks = callbacksProvider != null
                ? callbacksProvider.ConversionCallbacks
                : new Dictionary<string, Func<string, PropertyInfo, object>>();

            this.DefaultConversionCallback = callbacksProvider != null
                ? callbacksProvider.DefaultConversionCallback
                : DefaultCallback;
        }

        /// <summary>
        /// Gets the default conversion callback
        /// </summary>
        public static Func<string, PropertyInfo, object> DefaultCallback
        {
            get { return (value, info) => Convert.ChangeType(value, info.PropertyType, CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Gets the conversion callback which is used as fallback when no suitable conversion
        /// callback can be found in <see cref="ConversionCallbacks"/>
        /// </summary>
        public Func<string, PropertyInfo, object> DefaultConversionCallback { get; private set; }

        /// <summary>
        /// Gets the conversion callbacks
        /// </summary>
        public IDictionary<string, Func<string, PropertyInfo, object>> ConversionCallbacks { get; private set; }
    }
}