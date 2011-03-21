//-------------------------------------------------------------------------------
// <copyright file="SingleArgumentTransitionActionHolderTest.cs" company="bbv Software Services AG">
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

    public class SingleArgumentTransitionActionHolderTest
    {
        public interface IBase
        {
        }

        public interface IDerived : IBase
        {
        }

        [Fact]
        public void MatchingType()
        {
            var testee = new SingleArgumentTransitionActionHolder<IBase>(BaseAction);
            
            testee.Execute(new[] { Mock.Of<IBase>() });
        }

        [Fact]
        public void DerivedType()
        {
            var testee = new SingleArgumentTransitionActionHolder<IBase>(BaseAction);

            testee.Execute(new[] { Mock.Of<IDerived>() });
        }

        [Fact]
        public void NonMatchingType()
        {
            var testee = new SingleArgumentTransitionActionHolder<IBase>(BaseAction);

            Action action = () => { testee.Execute(new object[] { 3 }); };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void TooManyArguments()
        {
            var testee = new SingleArgumentTransitionActionHolder<IBase>(BaseAction);

            Action action = () => { testee.Execute(new object[] { 3, 4 }); };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void TooFewArguments()
        {
            var testee = new SingleArgumentTransitionActionHolder<IBase>(BaseAction);

            Action action = () => { testee.Execute(new object[] { }); };

            action.ShouldThrow<ArgumentException>();
        }

        private static void BaseAction(IBase b)
        {
        }
    }
}