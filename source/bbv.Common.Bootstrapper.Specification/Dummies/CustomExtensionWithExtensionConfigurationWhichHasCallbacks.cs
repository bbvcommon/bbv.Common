//-------------------------------------------------------------------------------
// <copyright file="CustomExtensionWithExtensionConfigurationWhichHasCallbacks.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Specification.Dummies
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Reflection;

    using bbv.Common.Bootstrapper.Configuration;
    using bbv.Common.Formatters;

    public class CustomExtensionWithExtensionConfigurationWhichHasCallbacks : ICustomExtensionWithExtensionConfiguration,
        IHaveConversionCallbacks, ILoadConfigurationSection
    {
        public Func<string, PropertyInfo, object> DefaultConversionCallback
        {
            get
            {
                return (value, info) => string.Format(CultureInfo.InvariantCulture, "{0}. Modified by Default!", value);
            }
        }

        public IDictionary<string, Func<string, PropertyInfo, object>> ConversionCallbacks
        {
            get
            {
                return new Dictionary<string, Func<string, PropertyInfo, object>>
                    {
                        { "SomeInt", (value, info) => { return Convert.ToInt32(value); } },
                        { "SomeString", (value, info) => { return string.Format(CultureInfo.InvariantCulture, "{0}. Modified by Callback!", value); } },
                    };
            }
        }

        public int SomeInt { get; set; }

        public string SomeString { get; set; }

        public string SomeStringWithDefault { get; set; }

        public string SomeStringWhichIsIgnored { get; set; }

        public string SectionAcquired { get; private set; }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.GetType().FullNameToString();
            }
        }

        public string Describe()
        {
            return "Custom extension which defines conversion callbacks";
        }

        public ConfigurationSection GetSection(string sectionName)
        {
            this.SectionAcquired = sectionName;

            return ExtensionConfigurationSectionHelper.CreateSection(
                new KeyValuePair<string, string>("SomeInt", "1"),
                new KeyValuePair<string, string>("SomeString", "SomeString"),
                new KeyValuePair<string, string>("SomeStringWithDefault", "SomeStringWithDefault"));
        }
    }
}