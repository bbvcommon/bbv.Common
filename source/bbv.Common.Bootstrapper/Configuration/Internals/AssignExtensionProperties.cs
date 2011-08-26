//-------------------------------------------------------------------------------
// <copyright file="AssignExtensionProperties.cs" company="bbv Software Services AG">
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
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Default IAssignExtensionProperties
    /// </summary>
    public class AssignExtensionProperties : IAssignExtensionProperties
    {
        /// <inheritdoc />
        public void Assign(IReflectExtensionProperties reflector, IExtension extension, IConsumeConfiguration consumer, IHaveConversionCallbacks callbackProvider)
        {
            Ensure.ArgumentNotNull(reflector, "reflector");
            Ensure.ArgumentNotNull(consumer, "consumer");
            Ensure.ArgumentNotNull(callbackProvider, "callbackProvider");

            IEnumerable<PropertyInfo> properties = reflector.Reflect(extension);
            IDictionary<string, Func<string, PropertyInfo, object>> conversionCallbacks = callbackProvider.ConversionCallbacks;
            Func<string, PropertyInfo, object> defaultCallback = callbackProvider.DefaultConversionCallback;

            foreach (KeyValuePair<string, string> keyValuePair in consumer.Configuration)
            {
                KeyValuePair<string, string> pair = keyValuePair;

                var matchedProperty = properties.Where(property => property.Name.Equals(pair.Key, StringComparison.OrdinalIgnoreCase))
                    .SingleOrDefault();

                if (matchedProperty == null)
                {
                    continue;
                }

                Func<string, PropertyInfo, object> conversionCallback;
                if (!conversionCallbacks.TryGetValue(pair.Key, out conversionCallback))
                {
                    conversionCallback = defaultCallback;
                }

                matchedProperty.SetValue(extension, conversionCallback(pair.Value, matchedProperty), null);
            }
        }
    }
}