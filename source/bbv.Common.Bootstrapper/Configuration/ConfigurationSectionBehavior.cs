//-------------------------------------------------------------------------------
// <copyright file="ConfigurationSectionBehavior.cs" company="bbv Software Services AG">
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
    using System.Configuration;

    using bbv.Common.Bootstrapper.Configuration.Internals;

    /// <summary>
    /// Adds behavior to the IBootstrapper to load configuration sections.
    /// </summary>
    public class ConfigurationSectionBehavior : IBehavior<IExtension>
    {
        /// <summary>
        /// Behaves the specified extensions.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public void Behave(IEnumerable<IExtension> extensions)
        {
            Ensure.ArgumentNotNull(extensions, "extensions");

            foreach (IExtension extension in extensions)
            {
                IConsumeConfigurationSection consumer = this.CreateConsumeConfigurationSection(extension);
                IHaveConfigurationSectionName sectionNameProvider = this.CreateHaveConfigurationSectionName(extension);
                ILoadConfigurationSection sectionProvider = this.CreateLoadConfigurationSection(extension);

                string sectionName = sectionNameProvider.SectionName;
                ConfigurationSection section = sectionProvider.GetSection(sectionName);

                consumer.Apply(section);
            }
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
        /// Creates the instance which consumes a configuration section.
        /// </summary>
        /// <param name="extension">The extensions.</param>
        /// <returns>
        /// The instance.
        /// </returns>
        protected virtual IConsumeConfigurationSection CreateConsumeConfigurationSection(IExtension extension) 
        {
            return new ConsumeConfigurationSection(extension);
        }
    }
}