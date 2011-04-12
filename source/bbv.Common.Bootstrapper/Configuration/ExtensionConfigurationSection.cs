//-------------------------------------------------------------------------------
// <copyright file="ExtensionConfigurationSection.cs" company="bbv Software Services AG">
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
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Configuration section for bootstrapper extensions.
    /// </summary>
    public class ExtensionConfigurationSection : ConfigurationSection
    {
        private const string ConfigurationKeyName = "Configuration";

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        [ConfigurationProperty(ConfigurationKeyName,
            IsDefaultCollection = false)]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Review.")]
        public ExtensionSettingsElementCollection Configuration
        {
            get
            {
                ExtensionSettingsElementCollection settingsElementCollection =
                (ExtensionSettingsElementCollection)base[ConfigurationKeyName];
                return settingsElementCollection;
            }

            set
            {
                base[ConfigurationKeyName] = value;
            }
        }
    }
}