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
    using System.Collections.Generic;

    using bbv.Common.Bootstrapper.Configuration.Internals;

    /// <summary>
    /// Behavior which automatically loads extension configuration sections.
    /// </summary>
    public class ExtensionConfigurationSectionBehavior : IBehavior<IExtension>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used after refactoring.")]
        private readonly IExtensionPropertyReflector extensionPropertyReflector;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionConfigurationSectionBehavior"/> class.
        /// </summary>
        /// <remarks>Uses the default <see cref="ExtensionPublicPropertyReflector"/>.</remarks>
        public ExtensionConfigurationSectionBehavior()
            : this(new ExtensionPublicPropertyReflector())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionConfigurationSectionBehavior"/> class.
        /// </summary>
        /// <param name="extensionPropertyReflector">The extension property reflector.</param>
        public ExtensionConfigurationSectionBehavior(IExtensionPropertyReflector extensionPropertyReflector)
        {
            this.extensionPropertyReflector = extensionPropertyReflector;
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
                IHaveConfigurationSectionName sectionNameProvider = this.CreateHaveConfigurationSectionName(extension);
                ILoadConfigurationSection sectionProvider = this.CreateLoadConfigurationSection(extension);
                IConsumeConfiguration consumer = this.CreateConsumeConfiguration(extension);

                string sectionName = sectionNameProvider.SectionName;
                ExtensionConfigurationSection section = sectionProvider.GetSection(sectionName) as ExtensionConfigurationSection ?? 
                    ExtensionConfigurationSectionHelper.CreateSection(new Dictionary<string, string>());

                foreach (ExtensionSettingsElement settingsElement in section.Configuration)
                {
                    consumer.Configuration.Add(settingsElement.Key, settingsElement.Value);
                }
            }
        }

        /// <summary>
        /// Creates the consume configuration instance.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The instance.</returns>
        protected virtual IConsumeConfiguration CreateConsumeConfiguration(IExtension extension)
        {
            return new ConsumeConfiguration(extension);
        }

        /// <summary>
        /// Creates the instance which knows the section name.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The istance.</returns>
        protected virtual IHaveConfigurationSectionName CreateHaveConfigurationSectionName(IExtension extension)
        {
            return new HaveConfigurationSectionName(extension);
        }

        /// <summary>
        /// Creates the instance which loads configuration sections.
        /// </summary>
        /// <param name="extension">The extensions.</param>
        /// <returns>The instance.</returns>
        protected virtual ILoadConfigurationSection CreateLoadConfigurationSection(IExtension extension)
        {
            return new LoadConfigurationSection(extension);
        }

        /// <summary>
        /// Creates the instance which has conversion callbacks.
        /// </summary>
        /// <param name="extension">The extensions.</param>
        /// <returns>The instance.</returns>
        protected virtual IHaveConversionCallbacks CreateHaveConversionCallbacks(IExtension extension)
        {
            return new HaveConversionCallbacks(extension);
        }
    }
}