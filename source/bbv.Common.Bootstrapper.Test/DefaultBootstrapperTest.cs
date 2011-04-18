//-------------------------------------------------------------------------------
// <copyright file="DefaultBootstrapperTest.cs" company="bbv Software Services AG">
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
    using System;
    using System.Collections.Generic;

    using bbv.Common.Bootstrapper.Syntax;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class DefaultBootstrapperTest
    {
        private readonly Mock<IExtensionHost<IExtension>> extensionHost;

        private readonly Mock<IExecutor<IExtension>> runExecutor;

        private readonly DefaultBootstrapper<IExtension> testee;

        public DefaultBootstrapperTest()
        {
            this.extensionHost = new Mock<IExtensionHost<IExtension>>();
            this.runExecutor = new Mock<IExecutor<IExtension>>();

            this.testee = new DefaultBootstrapper<IExtension>(this.extensionHost.Object, this.runExecutor.Object);
        }

        [Fact]
        public void Initialize_MultipleTimes_ShouldThrowException()
        {
            this.testee.Initialize(Mock.Of<IStrategy<IExtension>>());

            this.testee.Invoking(x => x.Initialize(Mock.Of<IStrategy<IExtension>>())).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void AddExtension_ShouldTrackExtension()
        {
            var extension = Mock.Of<IExtension>();

            this.testee.AddExtension(extension);

            this.extensionHost.Verify(x => x.AddExtension(extension));
        }

        [Fact]
        public void Run_ShouldBuildRunSyntax()
        {
            var strategy = new Mock<IStrategy<IExtension>>();
            this.testee.Initialize(strategy.Object);

            this.testee.Run();

            strategy.Verify(s => s.BuildRunSyntax());
        }

        [Fact]
        public void Run_ShouldExecuteSyntaxAndExtensionsOnRunExecutor()
        {
            var runSyntax = new Mock<ISyntax<IExtension>>();
            var strategy = new Mock<IStrategy<IExtension>>();
            strategy.Setup(s => s.BuildRunSyntax()).Returns(runSyntax.Object);

            var extensions = new List<IExtension> { Mock.Of<IExtension>(), };
            this.extensionHost.Setup(e => e.Extensions).Returns(extensions);

            this.testee.Initialize(strategy.Object);

            this.testee.Run();

            this.runExecutor.Verify(r => r.Execute(runSyntax.Object, extensions));
        }
    }
}