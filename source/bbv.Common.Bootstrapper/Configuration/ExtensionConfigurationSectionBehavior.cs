//-------------------------------------------------------------------------------
// <copyright file="ExtensionConfigurationSectionBehavior.cs" company="bbv Software Services AG">
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
    using System.Linq;
    using System.Reflection;

    using bbv.Common.Bootstrapper.Configuration.Internals;

    /// <summary>
    /// Behavior which automatically loads extension configuration sections.
    /// </summary>
    public class ExtensionConfigurationSectionBehavior : IBehavior<IExtension>
    {
        private readonly IExtensionConfigurationSectionBehaviorFactory factory;

        private readonly IReflectExtensionProperties reflectExtensionProperties;

        private readonly IAssignExtensionProperties assignExtensionProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionConfigurationSectionBehavior"/> class.
        /// </summary>
        /// <remarks>Uses the default <see cref="ReflectExtensionPublicProperties"/>.</remarks>
        public ExtensionConfigurationSectionBehavior()
            : this(new DefaultExtensionConfigurationSectionBehaviorFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionConfigurationSectionBehavior"/> class.
        /// </summary>
        /// <param name="factory">The factory which creates the necessary dependencies.</param>
        public ExtensionConfigurationSectionBehavior(IExtensionConfigurationSectionBehaviorFactory factory)
        {
            Ensure.ArgumentNotNull(factory, "factory");

            this.factory = factory;

            this.reflectExtensionProperties = this.factory.CreateReflectExtensionProperties();
            this.assignExtensionProperties = this.factory.CreateAssignExtensionProperties();
        }

        /// <summary>
        /// Applies the extension configuration section loading behavior to the extensions.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public void Behave(IEnumerable<IExtension> extensions)
        {
            Ensure.ArgumentNotNull(extensions, "extensions");

            foreach (IExtension extension in extensions)
            {
                IHaveConfigurationSectionName sectionNameProvider = this.factory.CreateHaveConfigurationSectionName(extension);
                ILoadConfigurationSection sectionProvider = this.factory.CreateLoadConfigurationSection(extension);
                IConsumeConfiguration consumer = this.factory.CreateConsumeConfiguration(extension);
                IHaveConversionCallbacks callbackProvider = this.factory.CreateHaveConversionCallbacks(extension);

                string sectionName = sectionNameProvider.SectionName;
                ExtensionConfigurationSection section = sectionProvider.GetSection(sectionName) as ExtensionConfigurationSection ?? 
                    ExtensionConfigurationSectionHelper.CreateSection(new Dictionary<string, string>());

                FillConsumerConfiguration(section, consumer);

                this.assignExtensionProperties.Assign(this.reflectExtensionProperties, extension, consumer, callbackProvider);
            }
        }

        private static void FillConsumerConfiguration(ExtensionConfigurationSection section, IConsumeConfiguration consumer)
        {
            foreach (ExtensionSettingsElement settingsElement in section.Configuration)
            {
                string key = settingsElement.Key;
                string value = settingsElement.Value;

                consumer.Configuration.Add(key, value);
            }
        }
    }
}