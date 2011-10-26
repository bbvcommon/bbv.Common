//-------------------------------------------------------------------------------
// <copyright file="IExtensionResolver.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper
{
    /// <summary>
    /// The extension resolver can resolve extensions and add it to the provided extension point.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IExtensionResolver<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Resolves extensions.
        /// </summary>
        /// <remarks>All extensions resolved through the extension resolver are added 
        /// to the bootstrapper before manual adding of extensions occurs.</remarks>
        /// <param name="extensionPoint">The extension point which allows to add the resolved extensions.</param>
        void Resolve(IExtensionPoint<TExtension> extensionPoint);
    }
}