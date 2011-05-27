//-------------------------------------------------------------------------------
// <copyright file="ExtensionSettingsElementCollection.cs" company="bbv Software Services AG">
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
    /// Represents the bootstrapper extension settings configuration element collection.
    /// </summary>
    public sealed class ExtensionSettingsElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets or sets the
        /// <see cref="ExtensionSettingsElement"/> at the
        /// specified index.
        /// </summary>
        /// <param name="index">The index which is used for item retrieval.</param>
        /// <value>The <see cref="ExtensionSettingsElement"/> at the specified index.</value>
        public ExtensionSettingsElement this[int index]
        {
            get
            {
                return (ExtensionSettingsElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="ExtensionSettingsElement"/>
        /// with the specified alias.
        /// </summary>
        /// <param name="key">The alias which is used for item retrieval.</param>
        /// <value>The <see cref="ExtensionSettingsElement"/> with the specified key.</value>
        public new ExtensionSettingsElement this[string key]
        {
            get
            {
                return (ExtensionSettingsElement)BaseGet(key);
            }

            set
            {
                BaseAdd(value);
            }
        }

        /// <summary>
        /// When overridden in a derived class, creates a new 
        /// <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExtensionSettingsElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a
        /// derived class.
        /// </summary>
        /// <param name="element">The 
        /// <see cref="T:System.Configuration.ConfigurationElement"/> to return the key
        /// for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified 
        /// <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionSettingsElement)element).Key;
        }
    }
}