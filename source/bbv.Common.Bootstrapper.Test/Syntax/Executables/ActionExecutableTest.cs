//-------------------------------------------------------------------------------
// <copyright file="ActionExecutableTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Syntax.Executables
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class ActionExecutableTest
    {
        private bool wasExecuted;

        private ActionExecutable<IExtension> testee;

        public ActionExecutableTest()
        {
            this.testee = new ActionExecutable<IExtension>(() => this.wasExecuted = true);
        }

        [Fact]
        public void Execute_ShouldExecuteAction()
        {
            this.testee.Execute(Enumerable.Empty<IExtension>());

            this.wasExecuted.Should().BeTrue();
        }

        [Fact]
        public void Execute_ShouldNotProcessExtensions()
        {
            var enumerator = new Mock<IEnumerable<IExtension>>();

            this.testee.Execute(enumerator.Object);

            enumerator.Verify(e => e.GetEnumerator(), Times.Never());
        }

        [Fact]
        public void Execute_ShouldExecuteBehavior()
        {
            var first = new Mock<IBehavior<IExtension>>();
            var second = new Mock<IBehavior<IExtension>>();
            var extensions = Enumerable.Empty<IExtension>();

            this.testee.Add(first.Object);
            this.testee.Add(second.Object);

            this.testee.Execute(extensions);

            first.Verify(b => b.Behave(extensions));
            second.Verify(b => b.Behave(extensions));
        }
    }
}