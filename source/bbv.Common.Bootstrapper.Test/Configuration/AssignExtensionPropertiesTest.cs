//-------------------------------------------------------------------------------
// <copyright file="AssignExtensionPropertiesTest.cs" company="bbv Software Services AG">
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
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using bbv.Common.Bootstrapper.Configuration.Internals;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class AssignExtensionPropertiesTest
    {
        private const string SomeExtensionPropertyName = "SomeProperty";

        private const string SomeExtensionPropertyValue = "AnyValue";

        private readonly Mock<IConsumeConfiguration> consumer;

        private readonly Mock<IHaveConversionCallbacks> conversionCallbacks;

        private readonly Mock<IReflectExtensionProperties> extensionPropertyReflector;

        private readonly AssignExtensionProperties testee;

        public AssignExtensionPropertiesTest()
        {
            this.consumer = new Mock<IConsumeConfiguration>();
            this.extensionPropertyReflector = new Mock<IReflectExtensionProperties>();
            this.conversionCallbacks = new Mock<IHaveConversionCallbacks>();

            this.testee = new AssignExtensionProperties();
        }

        [Fact]
        public void Assign_ShouldReflectPropertiesOfExtensions()
        {
            this.SetupEmptyConsumerConfiguration();

            this.testee.Assign(this.extensionPropertyReflector.Object, Mock.Of<IExtension>(), this.consumer.Object, this.conversionCallbacks.Object);

            this.extensionPropertyReflector.Verify(r => r.Reflect(It.IsAny<IExtension>()));
        }

        [Fact]
        public void Assign_ShouldAcquireConversionCallbacks()
        {
            this.SetupEmptyConsumerConfiguration();

            this.testee.Assign(this.extensionPropertyReflector.Object, Mock.Of<IExtension>(), this.consumer.Object, this.conversionCallbacks.Object);

            this.conversionCallbacks.VerifyGet(x => x.ConversionCallbacks);
            this.conversionCallbacks.VerifyGet(x => x.DefaultConversionCallback);
        }

        [Fact]
        public void Assign_WhenReflectedPropertyMatchesConfiguration_ShouldAcquireCallback()
        {
            bool wasCalled = false;

            var dictionary = new Dictionary<string, Func<string, PropertyInfo, object>>
                {
                    {
                        SomeExtensionPropertyName, (value, info) =>
                            {
                                wasCalled = true;
                                return value;
                            }
                        }
                };

            this.conversionCallbacks.Setup(x => x.ConversionCallbacks).Returns(dictionary);

            this.extensionPropertyReflector.Setup(x => x.Reflect(It.IsAny<IExtension>())).Returns(new List<PropertyInfo> { GetSomePropertyPropertyInfo() });
            this.consumer.Setup(x => x.Configuration).Returns(new Dictionary<string, string> { { SomeExtensionPropertyName, SomeExtensionPropertyValue } });

            var someExtension = new SomeExtension();
            this.testee.Assign(this.extensionPropertyReflector.Object, someExtension, this.consumer.Object, this.conversionCallbacks.Object);

            wasCalled.Should().BeTrue();
            someExtension.SomeProperty.Should().Be(SomeExtensionPropertyValue);
        }

        [Fact]
        public void Assign_WhenNoConversionCallbackFound_ShouldUseDefaultCallback()
        {
            this.conversionCallbacks.Setup(x => x.ConversionCallbacks).Returns(
                new Dictionary<string, Func<string, PropertyInfo, object>>());

            bool wasCalled = false;
            Func<string, PropertyInfo, object> defaultCallback = (value, info) =>
            {
                wasCalled = true;
                return value;
            };
            this.conversionCallbacks.Setup(x => x.DefaultConversionCallback).Returns(defaultCallback);

            this.extensionPropertyReflector.Setup(x => x.Reflect(It.IsAny<IExtension>())).Returns(new List<PropertyInfo> { GetSomePropertyPropertyInfo() });
            this.consumer.Setup(x => x.Configuration).Returns(new Dictionary<string, string> { { SomeExtensionPropertyName, SomeExtensionPropertyValue } });

            var someExtension = new SomeExtension();
            this.testee.Assign(this.extensionPropertyReflector.Object, someExtension, this.consumer.Object, this.conversionCallbacks.Object);

            wasCalled.Should().BeTrue();
            someExtension.SomeProperty.Should().Be(SomeExtensionPropertyValue);
        }

        private static PropertyInfo GetSomePropertyPropertyInfo()
        {
            return typeof(SomeExtension).GetProperty(SomeExtensionPropertyName);
        }

        private void SetupEmptyConsumerConfiguration()
        {
            this.consumer.Setup(x => x.Configuration).Returns(new Dictionary<string, string>());
        }

        private class SomeExtension : IExtension
        {
            public string SomeProperty { get; private set; }
        }
    }
}