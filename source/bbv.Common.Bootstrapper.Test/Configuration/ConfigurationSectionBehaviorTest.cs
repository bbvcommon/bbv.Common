//-------------------------------------------------------------------------------
// <copyright file="ConfigurationSectionBehaviorTest.cs" company="bbv Software Services AG">
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

    using Moq;

    using Xunit;

    public class ConfigurationSectionBehaviorTest
    {
        private readonly Mock<IConsumeConfigurationSection> consumer;

        private readonly Mock<IHaveConfigurationSectionName> sectionNameProvider;

        private readonly Mock<ILoadConfigurationSection> sectionProvider;

        private readonly List<IExtension> extensions;

        private readonly ConfigurationSectionBehavior testee;

        public ConfigurationSectionBehaviorTest()
        {
            this.consumer = new Mock<IConsumeConfigurationSection>();
            this.sectionNameProvider = new Mock<IHaveConfigurationSectionName>();
            this.sectionProvider = new Mock<ILoadConfigurationSection>();

            this.extensions = new List<IExtension> { Mock.Of<IExtension>(), Mock.Of<IExtension>(), };

            this.testee = new TestableConfigurationSectionBehavior(this.consumer.Object, this.sectionNameProvider.Object, this.sectionProvider.Object);
        }

        [Fact]
        public void Behave_ShouldApply()
        {
            this.testee.Behave(this.extensions);

            this.consumer.Verify(c => c.Apply(It.IsAny<ConfigurationSection>()));
        }

        [Fact]
        public void Behave_ShouldApplySectionFromProvider()
        {
            var configurationSection = Mock.Of<ConfigurationSection>();
            this.sectionProvider.Setup(p => p.GetSection(It.IsAny<string>())).Returns(configurationSection);

            this.testee.Behave(this.extensions);

            this.consumer.Verify(c => c.Apply(configurationSection));
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

        private class TestableConfigurationSectionBehavior : ConfigurationSectionBehavior
        {
            private readonly IConsumeConfigurationSection consumer;

            private readonly IHaveConfigurationSectionName sectionNameProvider;

            private readonly ILoadConfigurationSection sectionProvider;

            public TestableConfigurationSectionBehavior(
                IConsumeConfigurationSection consumer,
                IHaveConfigurationSectionName sectionNameProvider,
                ILoadConfigurationSection sectionProvider)
            {
                this.sectionProvider = sectionProvider;
                this.sectionNameProvider = sectionNameProvider;
                this.consumer = consumer;
            }

            protected override IConsumeConfigurationSection CreateConsumeConfigurationSection(IExtension extension)
            {
                return this.consumer;
            }

            protected override IHaveConfigurationSectionName CreateHaveConfigurationSectionName(IExtension extension)
            {
                return this.sectionNameProvider;
            }

            protected override ILoadConfigurationSection CreateLoadConfigurationSection(IExtension extension)
            {
                return this.sectionProvider;
            }
        }
    }
}