//-------------------------------------------------------------------------------
// <copyright file="ConsumeConfigurationTest.cs" company="bbv Software Services AG">
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

    using bbv.Common.Bootstrapper.Configuration.Internals;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class ConsumeConfigurationTest
    {
        [Fact]
        public void Configuration_ExtensionNotIConsumeConfiguration_ShouldUseEmtpyOne()
        {
            var extension = new Mock<IExtension>();

            var testee = new ConsumeConfiguration(extension.Object);
            testee.Configuration.Should().BeEmpty();
        }

        [Fact]
        public void Configuration_ExtensionIHaveExtensionConfigurationSectionName_ShouldAcquireNameFromExtension()
        {
            var extension = new Mock<IExtension>();
            var consumer = extension.As<IConsumeConfiguration>();
            var expected = new Dictionary<string, string> { { "Value", "Key" } };

            consumer.Setup(n => n.Configuration).Returns(expected);

            var testee = new ConsumeConfiguration(extension.Object);
            testee.Configuration.Should().BeEquivalentTo(expected);
        }
    }
}