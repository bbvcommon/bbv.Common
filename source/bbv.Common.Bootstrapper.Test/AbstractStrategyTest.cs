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

    using bbv.Common.Bootstrapper.Syntax;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class AbstractStrategyTest
    {
        private readonly TestableAbstractStrategy testee;

        public AbstractStrategyTest()
        {
            this.testee = new TestableAbstractStrategy(Mock.Of<ISyntaxBuilder<IExtension>>(), Mock.Of<ISyntaxBuilder<IExtension>>());
        }

        [Fact]
        public void BuildRunSyntax_WhenCalledMultipleTimes_ShouldThrowException()
        {
            this.testee.BuildRunSyntax();

            this.testee.Invoking(x => x.BuildRunSyntax()).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void BuildShutdownSyntax_WhenCalledMultipleTimes_ShouldThrowException()
        {
            this.testee.BuildShutdownSyntax();

            this.testee.Invoking(x => x.BuildShutdownSyntax()).ShouldThrow<InvalidOperationException>();
        }

        private class TestableAbstractStrategy : AbstractStrategy<IExtension>
        {
            public TestableAbstractStrategy(ISyntaxBuilder<IExtension> runSyntaxBuilder, ISyntaxBuilder<IExtension> shutdownSyntaxBuilder)
                : base(runSyntaxBuilder, shutdownSyntaxBuilder)
            {
            }

            protected override void DefineRunSyntax(ISyntaxBuilder<IExtension> builder)
            {
            }

            protected override void DefineShutdownSyntax(ISyntaxBuilder<IExtension> syntax)
            {
            }
        }
    }
}