//-------------------------------------------------------------------------------
// <copyright file="SingleArgumentGuardHolderTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.StateMachine.Internals
{
    using System;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class SingleArgumentGuardHolderTest
    {
        private readonly SingleArgumentGuardHolder<IBase> testee;

        private bool guardExecuted;

        public SingleArgumentGuardHolderTest()
        {
            this.guardExecuted = false;
            Func<IBase, bool> guard = v => this.guardExecuted = true;
            this.testee = new SingleArgumentGuardHolder<IBase>(guard);
        }

        [Fact]
        public void Execute()
        {
            this.testee.Execute(new object[] { Mock.Of<IBase>() });

            this.guardExecuted
                .Should().BeTrue();
        }

        [Fact]
        public void ExecuteWhenPassingADerivedClassThenGuardGetsExecuted()
        {
            this.testee.Execute(new object[] { Mock.Of<IDerived>() });

            this.guardExecuted
                .Should().BeTrue();
        }

        [Fact]
        public void ExecuteWhenPassingWrongTypeThenException()
        {
            var arguments = new object[] { 4 };

            Action action = () => this.testee.Execute(arguments);

            action
                .ShouldThrow<ArgumentException>()
                .WithMessage(ExceptionMessages.CannotCastArgumentToGuardArgument(arguments[0], this.testee.Describe()));
        }

        [Fact]
        public void ExecuteWhenPassingWrongNumberOfValuesThenException()
        {
            var arguments = new object[] { Mock.Of<IBase>(), Mock.Of<IBase>() };

            Action action = () => this.testee.Execute(arguments);

            action
                .ShouldThrow<ArgumentException>()
                .WithMessage(ExceptionMessages.CannotPassMultipleArgumentsToSingleArgumentGuard(arguments, this.testee.Describe()));
        }

        public interface IBase
        {
        }

        public interface IDerived : IBase
        {
        }
    }
}