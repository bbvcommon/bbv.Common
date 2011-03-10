//-------------------------------------------------------------------------------
// <copyright file="FileResourceLoader.cs" company="bbv Software Services AG">
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

namespace bbv.Common.IO.Resources
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.XPath;
    using IO;

    /// <summary>
    /// Implements the <see cref="IResourceLoader"/> to load resources from file system
    /// </summary>
    public class FileResourceLoader : IResourceLoader
    {
        #region IResourceLoader Members

        /// <summary>
        /// Loads an XML file into an <see cref="XmlNode"/>
        /// </summary>
        /// <param name="type">The type to find the assembly and namespace containing the resource</param>
        /// <param name="resourceName">The name of the resource relative to the namespace of <paramref name="type"/>.</param>
        /// <returns>
        /// A <see cref="IXPathNavigable"/> containing the contents of the embedded XML file
        /// </returns>
        public IXPathNavigable LoadResourceAsXml(Type type, string resourceName)
        {
            return this.LoadResourceAsXml(type.Assembly, resourceName);
        }

        /// <summary>
        /// Loads an embedded XML file into a<see cref="XmlNode"/>
        /// </summary>
        /// <param name="assembly">The Assembly to look for the resource.</param>
        /// <param name="resourceName">The full name including the namespace of the resource.</param>
        /// <returns>
        /// A<see cref="IXPathNavigable"/>containing the contents of the embedded XML file
        /// </returns>
        public IXPathNavigable LoadResourceAsXml(Assembly assembly, string resourceName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            string filepath = Path.Combine(Path.GetDirectoryName(assembly.Location), resourceName);
            xmlDocument.PreserveWhitespace = true;
            xmlDocument.Load(filepath);
            return xmlDocument.DocumentElement;
        }

        /// <summary>
        /// Loads an embedded file into a <see cref="string"/>
        /// </summary>
        /// <param name="type">The type to find the assembly containing the resource</param>
        /// <param name="resourceName">The name of the resource relative to the namespace of <paramref name="type"/>.</param>
        /// <returns>
        /// A <see cref="string"/> containing the contents of the embedded resource file
        /// </returns>
        public string LoadResourceAsString(Type type, string resourceName)
        {
            return this.LoadResourceAsString(type.Assembly, resourceName);
        }

        /// <summary>
        /// Loads an embedded file into a <see cref="string"/>
        /// </summary>
        /// <param name="assembly">The Assembly to look for the resource.</param>
        /// <param name="resourceName">The full name including the namespace of the resource.</param>
        /// <returns>
        /// A <see cref="string"/> containing the contents of the embedded resource file
        /// </returns>
        public string LoadResourceAsString(Assembly assembly, string resourceName)
        {
            string filepath = Path.Combine(Path.GetDirectoryName(assembly.Location), resourceName);
            using (StreamReader reader = new StreamReader(filepath))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Loads an embedded file into a <see cref="Stream"/>
        /// </summary>
        /// <param name="type">The type to find the assembly containing the resource</param>
        /// <param name="resourceName">The name of the resource relative to the namespace of <paramref name="type"/>.///</param>
        /// <returns>
        /// A <see cref="Stream"/> containing the contents of the embedded resource file
        /// </returns>
        public Stream LoadResourceAsStream(Type type, string resourceName)
        {
            return this.LoadResourceAsStream(type.Assembly, resourceName);
        }

        /// <summary>
        /// Loads an embedded file into a <see cref="Stream"/>
        /// </summary>
        /// <param name="assembly">The Assembly to look for the resource.</param>
        /// <param name="resourceName">The full name including the namespace of the resource.</param>
        /// <returns>
        /// A <see cref="Stream"/> containing the contents of the embedded resource file
        /// </returns>
        public Stream LoadResourceAsStream(Assembly assembly, string resourceName)
        {
            Stream result = new MemoryStream();
            string filepath = Path.Combine(Path.GetDirectoryName(assembly.Location), resourceName);
            using (FileStream fs = File.OpenRead(filepath))
            {
                StreamHelper.CopyStream(fs, result);
                fs.Close();
            }

            return result;
        }

        #endregion
    }
}