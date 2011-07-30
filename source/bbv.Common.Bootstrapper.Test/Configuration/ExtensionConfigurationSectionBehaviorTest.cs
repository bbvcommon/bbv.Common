//-------------------------------------------------------------------------------
// <copyright file="ExtensionConfigurationSectionBehaviorTest.cs" company="bbv Software Services AG">
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
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class ExtensionConfigurationSectionBehaviorTest
    {
        private readonly Mock<IHaveConversionCallbacks> conversionCallbacks;

        private readonly Mock<ILoadConfigurationSection> sectionProvider;

        private readonly Mock<IConsumeConfiguration> consumer;

        private readonly Mock<IHaveConfigurationSectionName> sectionNameProvider;

        private readonly List<IExtension> extensions;

        private readonly Mock<IExtensionPropertyReflector> extensionPropertyReflector;

        private readonly ExtensionConfigurationSectionBehavior testee;

        public ExtensionConfigurationSectionBehaviorTest()
        {
            this.extensionPropertyReflector = new Mock<IExtensionPropertyReflector>();
            this.consumer = new Mock<IConsumeConfiguration>();
            this.sectionNameProvider = new Mock<IHaveConfigurationSectionName>();
            this.sectionProvider = new Mock<ILoadConfigurationSection>();
            this.conversionCallbacks = new Mock<IHaveConversionCallbacks>();

            this.extensions = new List<IExtension> { Mock.Of<IExtension>(), Mock.Of<IExtension>(), };

            this.testee = new TestableExtensionConfigurationSectionBehavior(this.extensionPropertyReflector.Object, this.consumer.Object, this.sectionNameProvider.Object, this.sectionProvider.Object, this.conversionCallbacks.Object);
        }

        [Fact]
        public void Behave_ShouldConsumeSectionFromProvider()
        {
            var expectedConfiguration = new Dictionary<string, string> { { "AnyKey", "AnyValue" } };
            var configuration = new Dictionary<string, string>();

            var configurationSection = ExtensionConfigurationSectionHelper.CreateSection(expectedConfiguration);

            this.sectionProvider.Setup(p => p.GetSection(It.IsAny<string>())).Returns(configurationSection);
            this.consumer.Setup(c => c.Configuration).Returns(configuration);

            this.testee.Behave(new List<IExtension> { this.extensions.First() });

            configuration.Should().BeEquivalentTo(expectedConfiguration);
        }

        [Fact]
        public void Behave_ShouldAcquireSectionName()
        {
            this.testee.Behave(this.extensions);

            this.sectionNameProvider.Verify(p => p.SectionName);
        }

        [Fact]
        public void Behave_ShouldAcquireSectionByName()
        {
            const string AnySectionName = "SectionName";

            this.sectionNameProvider.Setup(p => p.SectionName).Returns(AnySectionName);

            this.testee.Behave(this.extensions);

            this.sectionProvider.Verify(p => p.GetSection(AnySectionName));
        }

        private class TestableExtensionConfigurationSectionBehavior : ExtensionConfigurationSectionBehavior
        {
            private readonly IConsumeConfiguration consumer;

            private readonly IHaveConfigurationSectionName sectionNameProvider;

            private readonly ILoadConfigurationSection sectionProvider;

            private readonly IHaveConversionCallbacks conversionCallbacks;

            public TestableExtensionConfigurationSectionBehavior(
                IExtensionPropertyReflector extensionPropertyReflector,
                IConsumeConfiguration consumer,
                IHaveConfigurationSectionName sectionNameProvider,
                ILoadConfigurationSection sectionProvider,
                IHaveConversionCallbacks conversionCallbacks)
                : base(extensionPropertyReflector)
            {
                this.conversionCallbacks = conversionCallbacks;
                this.sectionProvider = sectionProvider;
                this.sectionNameProvider = sectionNameProvider;
                this.consumer = consumer;
            }

            protected override IHaveConfigurationSectionName CreateHaveConfigurationSectionName(IExtension extension)
            {
                return this.sectionNameProvider;
            }

            protected override ILoadConfigurationSection CreateLoadConfigurationSection(IExtension extension)
            {
                return this.sectionProvider;
            }

            protected override IConsumeConfiguration CreateConsumeConfiguration(IExtension extension)
            {
                return this.consumer;
            }

            protected override IHaveConversionCallbacks CreateHaveConversionCallbacks(IExtension extension)
            {
                return this.conversionCallbacks;
            }
        }
    }
}