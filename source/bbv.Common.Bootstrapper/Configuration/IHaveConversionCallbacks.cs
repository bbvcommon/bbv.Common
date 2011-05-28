//-------------------------------------------------------------------------------
// <copyright file="IHaveConversionCallbacks.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Identifies the implemenator as conversion callback provider
    /// </summary>
    public interface IHaveConversionCallbacks
    {
        /// <summary>
        /// Gets the conversion callback which is used as fallback when no suitable conversion 
        /// callback can be found in <see cref="ConversionCallbacks"/>
        /// </summary>
        Func<string, PropertyInfo, object> DefaultConversionCallback { get; }

        /// <summary>
        /// Gets the conversion callbacks
        /// </summary>
        IDictionary<string, Func<string, PropertyInfo, object>> ConversionCallbacks { get; }
    }
}