//-------------------------------------------------------------------------------
// <copyright file="StateTest.cs" company="bbv Software Services AG">
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

    public class StateTest
    {
        private readonly Mock<IStateMachineInformation<States, Events>> stateMachineInformationMock;

        private readonly Mock<IExtensionHost<States, Events>> extensionHostMock;

        private readonly State<States, Events> testee;

        public StateTest()
        {
            this.stateMachineInformationMock = new Mock<IStateMachineInformation<States, Events>>();
            this.extensionHostMock = new Mock<IExtensionHost<States, Events>>();

            this.testee = new State<States, Events>(States.A, this.stateMachineInformationMock.Object, this.extensionHostMock.Object);
        }

        [Fact]
        public void HierarchyWhenDefiningAStateAsItsOwnSuperStateThenAnExceptionIsThrown()
        {
            Action action = () => this.testee.SuperState = this.testee;

            action
                .ShouldThrow<ArgumentException>().WithMessage(ExceptionMessages.StateCannotBeItsOwnSuperState(this.testee.ToString()));
        }

        [Fact]
        public void HierarchyWhenSettingLevelThenTheLevelOfAllChildrenIsUpdated()
        {
            const int Level = 2;

            var subState = Mock.Of<IState<States, Events>>();

            this.testee.SubStates.Add(subState);

            this.testee.Level = Level;

            subState.Level
                .Should().Be(Level + 1);
        }
    }
}