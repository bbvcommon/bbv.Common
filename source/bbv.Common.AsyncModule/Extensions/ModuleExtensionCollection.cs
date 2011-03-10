//-------------------------------------------------------------------------------
// <copyright file="ModuleExtensionCollection.cs" company="bbv Software Services AG">
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

namespace bbv.Common.AsyncModule.Extensions
{
    using System;
    using System.Collections;

    /// <summary>
    /// This collection contains module extensions.
    /// </summary>
    public class ModuleExtensionCollection : Hashtable
    {
        /// <summary>
        /// Adds the call to Attach to the standard behavior of the method Add.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// An element with the same key already exists in the <see cref="T:System.Collections.Hashtable"/>.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Hashtable"/> is read-only.
        /// -or-
        /// The <see cref="T:System.Collections.Hashtable"/> has a fixed size.
        /// </exception>
        public override void Add(object key, object value)
        {
            this.AddAndAttach(key, value);
        }

        /// <summary>
        /// Adds the call to Detach and the tolerance for not existing keys to
        /// the standard behavior of the method Remove.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Hashtable"/> is read-only.
        /// -or-
        /// The <see cref="T:System.Collections.Hashtable"/> has a fixed size.
        /// </exception>
        public override void Remove(object key)
        {
            if (this.ContainsKey(key))
            {
                this.RemoveAndDetach(key);
            }
        }

        /// <summary>
        /// Adds a new extension.
        /// </summary>
        /// <typeparam name="TExtension">
        /// With this type the extension is retrieved by GetExtension().
        /// </typeparam>
        /// <param name="extensionInstance">
        /// The actual extension instance.
        /// </param>
        public void Add<TExtension>(object extensionInstance)
        {
            if (this.ContainsKey(typeof(TExtension)))
            {
                this.RemoveAndDetach(typeof(TExtension));
            }

            this.AddAndAttach(typeof(TExtension), extensionInstance);
        }

        /// <summary>
        /// Gets the extension from the module which was registered 
        /// with the type TExtensionType.
        /// </summary>
        /// <typeparam name="TExtension">
        /// The type identifying the extension to get.
        /// </typeparam>
        /// <returns>
        /// See above.
        /// </returns>
        public TExtension Get<TExtension>()
        {
            foreach (Type extensionType in this.Keys)
            {
                if (typeof(TExtension).IsAssignableFrom(extensionType))
                {
                    return (TExtension)base[extensionType];
                }
            }

            return default(TExtension);
        }

        /// <summary>
        /// Removes and detaches an extension.
        /// </summary>
        /// <param name="extensionType">
        /// The type of the extension to remove.
        /// </param>
        private void RemoveAndDetach(object extensionType)
        {
            object extensionInstance = this[extensionType];
            
            // Detach before removing the extension
            if (extensionInstance is IModuleExtension)
            {
                ((IModuleExtension)extensionInstance).Detach();
            }
            
            base.Remove(extensionType);
        }

        /// <summary>
        /// Add the extension to the dictionary. If the extension is of the 
        /// type IModuleExtension, the Attach method of the extension is called, 
        /// so that the extension can add event handlers to the extension points.
        /// </summary>
        /// <param name="extensionType">
        /// Extension type to add.
        /// </param>
        /// <param name="extensionInstance">
        /// The actual extension.
        /// </param>
        private void AddAndAttach(object extensionType, object extensionInstance)
        {
            base.Add(extensionType, extensionInstance);
            if (extensionInstance is IModuleExtension)
            {
                ((IModuleExtension)extensionInstance).Attach();
            }
        }
    }
}
