//-------------------------------------------------------------------------------
// <copyright file="TransitionDictionaryTest.cs" company="bbv Software Services AG">
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

    public class TransitionDictionaryTest
    {
        private readonly Mock<IState<States, Events>> stateMock;

        private readonly TransitionDictionary<States, Events> testee;

        public TransitionDictionaryTest()
        {
            this.stateMock = new Mock<IState<States, Events>>();

            this.testee = new TransitionDictionary<States, Events>(this.stateMock.Object);
        }

        [Fact]
        public void TransitionWhenTransitionIsAlreadyUsedForAnotherStateThenThrowException()
        {
            var transition = Mock.Of<ITransition<States, Events>>();

            this.testee.Add(Events.A, transition);

            Action action = () => this.testee.Add(Events.B, transition);

            action
                .ShouldThrow<InvalidOperationException>().WithMessage(ExceptionMessages.TransitionDoesAlreadyExist(transition, this.stateMock.Object));
        }
    }
}