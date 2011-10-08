//-------------------------------------------------------------------------------
// <copyright file="SynchronousExecutorTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Execution
{
    using System.Collections.Generic;
    using System.Linq;

    using bbv.Common.Bootstrapper.Reporting;
    using bbv.Common.Bootstrapper.Syntax;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class SynchronousExecutorTest
    {
        private readonly Mock<IExecutionContext> executionContext;

        private readonly SynchronousExecutor<IExtension> testee;

        public SynchronousExecutorTest()
        {
            this.executionContext = new Mock<IExecutionContext>();

            this.testee = new SynchronousExecutor<IExtension>();
        }

        [Fact]
        public void Execute_ShouldExecuteSyntaxWithExtensions()
        {
            var firstExecutable = new Mock<IExecutable<IExtension>>();
            var secondExecutable = new Mock<IExecutable<IExtension>>();
            var syntax = new Mock<ISyntax<IExtension>>();
            var extensions = new List<IExtension> { Mock.Of<IExtension>(), };

            syntax.Setup(s => s.GetEnumerator())
                .Returns(new List<IExecutable<IExtension>> { firstExecutable.Object, secondExecutable.Object }
                .GetEnumerator());

            this.testee.Execute(syntax.Object, extensions, this.executionContext.Object);

            firstExecutable.Verify(e => e.Execute(extensions));
            secondExecutable.Verify(e => e.Execute(extensions));
        }

        [Fact]
        public void Execute_ShouldExecuteSyntaxWithExtensionsInOrderOfAppearance()
        {
            var executable = new Mock<IExecutable<IExtension>>();
            var syntax = new Mock<ISyntax<IExtension>>();
            var firstExtension = Mock.Of<IExtension>();
            var secondExtension = Mock.Of<IExtension>();

            var extensions = new List<IExtension> { secondExtension, firstExtension, };

            IEnumerable<IExtension> passedExtensions = Enumerable.Empty<IExtension>();

            executable.Setup(e => e.Execute(It.IsAny<IEnumerable<IExtension>>()))
                .Callback<IEnumerable<IExtension>>(ext => passedExtensions = ext);

            syntax.Setup(s => s.GetEnumerator())
                .Returns(new List<IExecutable<IExtension>> { executable.Object }
                .GetEnumerator());

            this.testee.Execute(syntax.Object, extensions, this.executionContext.Object);

            passedExtensions.Should().ContainInOrder(extensions);
        }

        [Fact]
        public void ShouldDescribeItself()
        {
            this.testee.Describe().Should().Be("Runs all executables synchronously on the extensions in the order which they were added.");
        }
    }
}