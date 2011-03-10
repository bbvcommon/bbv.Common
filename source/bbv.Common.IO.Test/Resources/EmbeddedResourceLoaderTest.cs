//-------------------------------------------------------------------------------
// <copyright file="EmbeddedResourceLoaderTest.cs" company="bbv Software Services AG">
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
    using System.Xml.XPath;
    using NUnit.Framework;

    /// <summary>
    /// Tests the embedded resource loader
    /// </summary>
    [TestFixture]
    public class EmbeddedResourceLoaderTest
    {
        /// <summary>
        /// Name for a non-existing text resource
        /// </summary>
        private const string NoTextResourceName = "EmbeddedTestResources.NoResource.txt";

        /// <summary>
        /// Name for a non-existing XML resource
        /// </summary>
        private const string NoXmlResourceName = "EmbeddedTestResources.NoResource.xml";

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
        private EmbeddedResourceLoader testee;

        #region Setup/Teardown

        /// <summary>
        /// Sets up each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new EmbeddedResourceLoader();
        }

        #endregion

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Test]
        public void LoadNotExistingStreamResourceFromAssembly()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsString(
                          Assembly.GetExecutingAssembly(),
                          string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, NoTextResourceName)));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Test]
        public void LoadNotExistingStreamResourceFromType()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsXml(typeof(EmbeddedResourceLoaderTest), NoTextResourceName));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Test]
        public void LoadNotExistingStringResourceFromAssembly()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsString(
                          Assembly.GetExecutingAssembly(),
                          string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, NoTextResourceName)));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Test]
        public void LoadNotExistingStringResourceFromType()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsString(typeof(EmbeddedResourceLoaderTest), NoTextResourceName));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Test]
        public void LoadNotExistingXmlResourceFromAssembly()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsXml(
                    Assembly.GetExecutingAssembly(),
                    string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, NoTextResourceName)));
        }

        /// <summary>
        /// When a non-existing resource is loaded an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Test]
        public void LoadNotExistingXmlResourceFromType()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsXml(typeof(EmbeddedResourceLoaderTest), NoXmlResourceName));
        }

        /// <summary>
        /// Loads a resource from assembly and verifies the size of the resulting stream
        /// </summary>
        [Test]
        public void LoadStreamResourceFromAssembly()
        {
            Stream stream = this.testee.LoadResourceAsStream(
                Assembly.GetExecutingAssembly(),
                string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, XmlResourceName));

            Assert.AreEqual(209, stream.Length);
            Assert.AreEqual(0, stream.Position);
        }

        /// <summary>
        /// Loads a resource from type and verifies the size of the resulting stream
        /// </summary>
        [Test]
        public void LoadStreamResourceFromType()
        {
            Stream stream = this.testee.LoadResourceAsStream(typeof(EmbeddedResourceLoaderTest), XmlResourceName);

            Assert.AreEqual(209, stream.Length);
            Assert.AreEqual(0, stream.Position);
        }

        /// <summary>
        /// When a non-existing resource is loaded as a strema an <see cref="ArgumentException"/> is thrown.
        /// </summary>
        [Test]
        public void LoadNotExistingResourceAsStream()
        {
            Assert.Throws<ArgumentException>(
                () => this.testee.LoadResourceAsStream(typeof(EmbeddedResourceLoaderTest), NoTextResourceName));
        }

        /// <summary>
        /// Loads a string resource from assembly and verifies the string is correct
        /// </summary>
        [Test]
        public void LoadStringResourceFromAssembly()
        {
            string result = this.testee.LoadResourceAsString(
                Assembly.GetExecutingAssembly(),
                string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, TextResourceName));

            Assert.AreEqual("MyString", result);
        }

        /// <summary>
        /// Loads a string resource from type and verifies the string is correct
        /// </summary>
        [Test]
        public void LoadStringResourceFromType()
        {
            string result = this.testee.LoadResourceAsString(typeof(EmbeddedResourceLoaderTest), TextResourceName);

            Assert.AreEqual("MyString", result);
        }

        /// <summary>
        /// Loads a XML resource from assembly and verifies it is correct.
        /// </summary>
        [Test]
        public void LoadXmlResourceFromAssembly()
        {
            IXPathNavigable xml =
                this.testee.LoadResourceAsXml(
                    Assembly.GetExecutingAssembly(),
                    string.Format("{0}.{1}", typeof(EmbeddedResourceLoaderTest).Namespace, XmlResourceName));

            Assert.IsTrue(xml.CreateNavigator().HasChildren);
        }

        /// <summary>
        /// Loads a XML resource from type and verifies it is correct
        /// </summary>
        [Test]
        public void LoadXmlResourceFromType()
        {
            IXPathNavigable xmlNode =
                this.testee.LoadResourceAsXml(typeof(EmbeddedResourceLoaderTest), XmlResourceName);
            Assert.IsTrue(xmlNode.CreateNavigator().HasChildren);
        }
    }
}