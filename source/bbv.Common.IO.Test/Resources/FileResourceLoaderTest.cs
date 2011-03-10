//-------------------------------------------------------------------------------
// <copyright file="FileResourceLoaderTest.cs" company="bbv Software Services AG">
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
    using System.IO;
    using System.Xml.XPath;
    using IO;
    using NUnit.Framework;
    using TestUtilities;

    /// <summary>
    /// Tests the <see cref="FileResourceLoader"/>
    /// </summary>
    [TestFixture]
    public class FileResourceLoaderTest
    {
        /// <summary>
        /// File name for a parameters XML file
        /// </summary>
        private const string FileName = "EmbeddedTestResources.Parameters.xml";

        /// <summary>
        /// Name for an XML resource
        /// </summary>
        private const string XmlResourceName = "EmbeddedTestResources.XmlResource.xml";

        /// <summary>
        /// Name for a text resource
        /// </summary>
        private const string TextResourceName = "EmbeddedTestResources.StringResource.txt";

        /// <summary>
        /// The object under test
        /// </summary>
        private FileResourceLoader testee;

        /// <summary>
        /// The filepath to hold the temporary files
        /// </summary>
        private string filepath;

        /// <summary>
        /// The embedded resource loader to load the resources from the test assembly
        /// </summary>
        private EmbeddedResourceLoader resourceLoader;

        #region Setup/Teardown

        /// <summary>
        /// Initializes the unit test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new FileResourceLoader();
            this.resourceLoader = new EmbeddedResourceLoader();
            this.filepath =
                Path.Combine(Path.GetDirectoryName(typeof(FileResourceLoaderTest).Assembly.Location), FileName);
        }

        #endregion

        /// <summary>
        /// Loads the resource as stream from file. Ensures that the stream contents are identical.
        /// </summary>
        [Test]
        public void LoadResourceAsStreamFromAssembly()
        {
            Stream expected = this.resourceLoader.LoadResourceAsStream(typeof(FileResourceLoaderTest), XmlResourceName);

            using (new TemporaryFileHolder(this.filepath, expected))
            {
                Stream resource = this.testee.LoadResourceAsStream(typeof(FileResourceLoaderTest).Assembly, FileName);

                Assert.IsTrue(StreamHelper.CompareStreamContents(expected, resource));
            }
        }

        /// <summary>
        /// Loads the resource as stream from file. Ensures that the stream contents are identical.
        /// </summary>
        [Test]
        public void LoadResourceAsStreamFromType()
        {
            Stream expected = this.resourceLoader.LoadResourceAsStream(typeof(FileResourceLoaderTest), XmlResourceName);

            using (new TemporaryFileHolder(this.filepath, expected))
            {
                Stream resource = this.testee.LoadResourceAsStream(typeof(FileResourceLoaderTest), FileName);

                Assert.IsTrue(StreamHelper.CompareStreamContents(expected, resource));
            }
        }

        /// <summary>
        /// Loads the resource as string from file. Ensures that the string contents are identical.
        /// </summary>
        [Test]
        public void LoadResourceAsStringFromAssembly()
        {
            string expected = this.resourceLoader.LoadResourceAsString(typeof(FileResourceLoaderTest), TextResourceName);
            using (new TemporaryFileHolder(this.filepath, expected))
            {
                string resource = this.testee.LoadResourceAsString(typeof(FileResourceLoaderTest).Assembly, FileName);
                Assert.AreEqual(expected, resource);
            }
        }

        /// <summary>
        /// Loads the resource as string from file. Ensures that the string contents are identical.
        /// </summary>
        [Test]
        public void LoadResourceAsStringFromType()
        {
            string expected = this.resourceLoader.LoadResourceAsString(typeof(FileResourceLoaderTest), TextResourceName);
            using (new TemporaryFileHolder(this.filepath, expected))
            {
                string resource = this.testee.LoadResourceAsString(typeof(FileResourceLoaderTest), FileName);
                Assert.AreEqual(expected, resource);
            }
        }

        /// <summary>
        /// Loads the resource as XML from file. Ensures that the xml contents are identical.
        /// </summary>
        [Test]
        public void LoadResourceAsXmlFromAssembly()
        {
            Stream resourceStream = this.resourceLoader.LoadResourceAsStream(typeof(FileResourceLoaderTest), XmlResourceName);

            using (new TemporaryFileHolder(this.filepath, resourceStream))
            {
                IXPathNavigable expected = this.resourceLoader.LoadResourceAsXml(typeof(FileResourceLoaderTest), XmlResourceName);
                IXPathNavigable resource = this.testee.LoadResourceAsXml(typeof(FileResourceLoaderTest).Assembly, FileName);

                Assert.AreEqual(expected.CreateNavigator().InnerXml, resource.CreateNavigator().InnerXml);
            }
        }

        /// <summary>
        /// Loads the resource as XML from file. Ensures that the xml contents are identical.
        /// </summary>
        [Test]
        public void LoadResourceAsXmlFromType()
        {
            Stream resourceStream = this.resourceLoader.LoadResourceAsStream(typeof(FileResourceLoaderTest), XmlResourceName);

            using (new TemporaryFileHolder(this.filepath, resourceStream))
            {
                IXPathNavigable expected = this.resourceLoader.LoadResourceAsXml(typeof(FileResourceLoaderTest), XmlResourceName);
                IXPathNavigable resource = this.testee.LoadResourceAsXml(typeof(FileResourceLoaderTest), FileName);

                Assert.AreEqual(expected.CreateNavigator().InnerXml, resource.CreateNavigator().InnerXml);
            }
        }
    }
}