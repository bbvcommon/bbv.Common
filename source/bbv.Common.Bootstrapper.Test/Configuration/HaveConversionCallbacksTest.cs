//-------------------------------------------------------------------------------
// <copyright file="HaveConversionCallbacksTest.cs" company="bbv Software Services AG">
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

    public class HaveConversionCallbacksTest
    {
        [Fact]
        public void ConversionCallbacks_ExtensionNotIHaveConversionCallbacks_ShouldUseEmptyOne()
        {
            var extension = new Mock<IExtension>();

            var testee = new HaveConversionCallbacks(extension.Object);
            testee.ConversionCallbacks.Should().BeEmpty();
        }

        [Fact]
        public void ConversionCallbacks_ExtensionIsIHaveConversionCallbacks_ShouldAcquireCallbacksFromExtension()
        {
            var extension = new Mock<IExtension>();
            var consumer = extension.As<IHaveConversionCallbacks>();
            var expected = new KeyValuePair<string, Func<string, PropertyInfo, object>>("Value", (value, info) => new object());

            consumer.Setup(n => n.ConversionCallbacks).Returns(new Dictionary<string, Func<string, PropertyInfo, object>> { { expected.Key, expected.Value } });

            var testee = new HaveConversionCallbacks(extension.Object);
            testee.ConversionCallbacks.Should().Contain(expected);
        }

        [Fact]
        public void DefaultConversionCallback_ExtensionNotIHaveConversionCallbacks_ShouldUseEmptyOne()
        {
            var extension = new Mock<IExtension>();

            var testee = new HaveConversionCallbacks(extension.Object);

            testee.DefaultConversionCallback.Should().Be(HaveConversionCallbacks.DefaultCallback);
        }

        [Fact]
        public void DefaultConversionCallback_ExtensionIsIHaveConversionCallbacks_ShouldAcquireDefaultCallbackFromExtension()
        {
            var extension = new Mock<IExtension>();
            var consumer = extension.As<IHaveConversionCallbacks>();
            Func<string, PropertyInfo, object> expected = (value, info) => new object();

            consumer.Setup(n => n.DefaultConversionCallback).Returns(expected);

            var testee = new HaveConversionCallbacks(extension.Object);
            testee.DefaultConversionCallback.Should().Be(expected);
        }
    }
}