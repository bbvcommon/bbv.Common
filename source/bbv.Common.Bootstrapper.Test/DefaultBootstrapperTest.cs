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

        private readonly Mock<IExecutor<IExtension>> shutdownExecutor;

        private readonly DefaultBootstrapper<IExtension> testee;

        public DefaultBootstrapperTest()
        {
            this.extensionHost = new Mock<IExtensionHost<IExtension>>();
            this.runExecutor = new Mock<IExecutor<IExtension>>();
            this.shutdownExecutor = new Mock<IExecutor<IExtension>>();

            this.testee = new DefaultBootstrapper<IExtension>(this.extensionHost.Object, this.runExecutor.Object, this.shutdownExecutor.Object);
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

        [Fact]
        public void Run_ShouldThrowExceptionWhenNotInitialized()
        {
            this.testee.Invoking(t => t.Run())
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Shutdown_ShouldBuildShutdownSyntax()
        {
            this.ShouldBuildShutdownSyntax(() => this.testee.Shutdown());
        }

        [Fact]
        public void Shutdown_ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor()
        {
            this.ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor(() => this.testee.Shutdown());
        }

        [Fact]
        public void Shutdown_ShouldThrowExceptionWhenNotInitialized()
        {
            this.testee.Invoking(t => t.Shutdown())
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Dispose_ShouldBuildShutdownSyntax()
        {
            this.ShouldBuildShutdownSyntax(() => this.testee.Dispose());
        }

        [Fact]
        public void Dispose_ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor()
        {
            this.ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor(() => this.testee.Dispose());
        }

        [Fact]
        public void Dispose_ShouldDisposeStrategy()
        {
            var strategy = new Mock<IStrategy<IExtension>>();
            this.testee.Initialize(strategy.Object);

            this.testee.Dispose();

            strategy.Verify(s => s.Dispose());
        }

        private void ShouldBuildShutdownSyntax(Action executionAction)
        {
            var strategy = new Mock<IStrategy<IExtension>>();
            this.testee.Initialize(strategy.Object);

            executionAction();

            strategy.Verify(s => s.BuildShutdownSyntax());
        }

        private void ShouldExecuteSyntaxAndExtensionsOnShutdownExecutor(Action executionAction)
        {
            var shutdownSyntax = new Mock<ISyntax<IExtension>>();
            var strategy = new Mock<IStrategy<IExtension>>();
            strategy.Setup(s => s.BuildShutdownSyntax()).Returns(shutdownSyntax.Object);

            var extensions = new List<IExtension> { Mock.Of<IExtension>(), };
            this.extensionHost.Setup(e => e.Extensions).Returns(extensions);

            this.testee.Initialize(strategy.Object);

            executionAction();

            this.shutdownExecutor.Verify(r => r.Execute(shutdownSyntax.Object, extensions));
        }
    }
}