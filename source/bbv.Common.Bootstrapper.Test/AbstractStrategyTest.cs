//-------------------------------------------------------------------------------
// <copyright file="AbstractStrategyTest.cs" company="bbv Software Services AG">
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

    public class AbstractStrategyTest
    {
        private readonly Mock<ISyntaxBuilder<IExtension>> runSyntaxBuilder;

        private readonly Mock<ISyntaxBuilder<IExtension>> shutdownSyntaxBuilder;

        private readonly TestableAbstractStrategy testee;

        public AbstractStrategyTest()
        {
            this.runSyntaxBuilder = new Mock<ISyntaxBuilder<IExtension>>();
            this.shutdownSyntaxBuilder = new Mock<ISyntaxBuilder<IExtension>>();

            this.testee = new TestableAbstractStrategy(this.runSyntaxBuilder.Object, this.shutdownSyntaxBuilder.Object);
        }

        [Fact]
        public void BuildRunSyntax_WhenCalledMultipleTimes_ShouldThrowException()
        {
            this.testee.BuildRunSyntax();

            this.testee.Invoking(x => x.BuildRunSyntax()).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BuildRunSyntax_ShouldReturnDefinedRunSyntax()
        {
            this.runSyntaxBuilder.Setup(x => x.GetEnumerator())
                .Returns(new List<IExecutable<IExtension>>().GetEnumerator());

            var syntax = this.testee.BuildRunSyntax();

            syntax.Should().BeEmpty();
            this.runSyntaxBuilder.Verify(x => x.Execute(It.IsAny<Action>()));
        }

        [Fact]
        public void BuildShutdownSyntax_WhenCalledMultipleTimes_ShouldThrowException()
        {
            this.testee.BuildShutdownSyntax();

            this.testee.Invoking(x => x.BuildShutdownSyntax()).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BuildShutdownSyntax_ShouldReturnDefinedRunSyntax()
        {
            this.shutdownSyntaxBuilder.Setup(x => x.GetEnumerator())
                .Returns(new List<IExecutable<IExtension>>().GetEnumerator());

            var syntax = this.testee.BuildShutdownSyntax();

            syntax.Should().BeEmpty();
            this.shutdownSyntaxBuilder.Verify(x => x.Execute(It.IsAny<Action>()));
        }

        private class TestableAbstractStrategy : AbstractStrategy<IExtension>
        {
            public TestableAbstractStrategy(ISyntaxBuilder<IExtension> runSyntaxBuilder, ISyntaxBuilder<IExtension> shutdownSyntaxBuilder)
                : base(runSyntaxBuilder, shutdownSyntaxBuilder)
            {
            }

            protected override void DefineRunSyntax(ISyntaxBuilder<IExtension> builder)
            {
                builder.Execute(() => { });
            }

            protected override void DefineShutdownSyntax(ISyntaxBuilder<IExtension> builder)
            {
                builder.Execute(() => { });
            }
        }
    }
}