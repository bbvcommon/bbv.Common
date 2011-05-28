//-------------------------------------------------------------------------------
// <copyright file="HaveExtensionConfigurationSectionNameTest.cs" company="bbv Software Services AG">
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
    using bbv.Common.Bootstrapper.Configuration.Internals;
    using bbv.Common.Bootstrapper.Dummies;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class HaveExtensionConfigurationSectionNameTest
    {
        private const string AnyName = "AnyName";

        [Fact]
        public void SectionName_ExtensionNotIHaveExtensionConfigurationSectionName_ShouldUseTypeName()
        {
            var extension = new Mock<IExtension>();
            var expected = extension.Object.GetType().Name;

            var testee = new HaveExtensionConfigurationSectionName(extension.Object);
            testee.SectionName.Should().Be(expected);
        }

        [Fact]
        public void SectionName_ExtensionIHaveExtensionConfigurationSectionName_ShouldAcquireNameFromExtension()
        {
            var extension = new Mock<IExtension>();
            var namer = extension.As<IHaveExtensionConfigurationSectionName>();
            namer.Setup(n => n.SectionName).Returns(AnyName);

            var testee = new HaveExtensionConfigurationSectionName(extension.Object);
            testee.SectionName.Should().Be(AnyName);
        }
    }
}