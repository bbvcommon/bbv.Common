//-------------------------------------------------------------------------------
// <copyright file="Log4NetExtensionTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.StateMachine.Extensions
{
    using System;
    using Internals;
    using log4net.Core;
    using Moq;
    using TestUtilities;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="Log4NetExtension{TState,TEvent}"/>
    /// </summary>
    public class Log4NetExtensionTest : IDisposable
    {
        private readonly Log4NetExtension<States, Events> testee;
        private readonly Log4netHelper log4Net;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetExtensionTest"/> class.
        /// </summary>
        public Log4NetExtensionTest()
        {
            this.testee = new Log4NetExtension<States, Events>();

            this.log4Net = new Log4netHelper();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.log4Net.Dispose();
        }

        /// <summary>
        /// When an event is fired
        /// then the state machine name and the performed transition path is logged.
        /// </summary>
        [Fact]
        public void FiredEvent()
        {
            const string StateMachineName = "test machine";
            const string Records = "test records";

            Mock<IStateMachineInformation<States, Events>> stateMachineInformation = this.CreateStateMachineInformation(StateMachineName, States.C);

            var transitionContext = new Mock<ITransitionContext<States, Events>>();
            transitionContext.Setup(x => x.GetRecords()).Returns(Records);

            this.testee.FiredEvent(stateMachineInformation.Object, transitionContext.Object);

            this.log4Net.LogContains(string.Format("State machine {0} performed {1}.", StateMachineName, Records));
        }

        /// <summary>
        /// When an event is firing
        /// then the state machine name, current state and the firing event is logged.
        /// </summary>
        [Fact]
        public void FiringEvent()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            Events eventId = Events.A;
            object[] eventArguments = new object[] { "A", "B" };

            var stateMachineInformation = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);

            this.testee.FiringEvent(stateMachineInformation.Object, ref eventId, ref eventArguments);

            this.log4Net.LogContains(string.Format(
                "Fire event {0} on state machine {1} with current state {2} and event arguments A, B.",
                eventId,
                StateMachineName,
                CurrentStateId));
        }

        /// <summary>
        /// When an exception occurs in an entry action
        /// then the state machine name, the state and the exception are logged.
        /// </summary>
        [Fact]
        public void HandlingEntryActionException()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            const string ExceptionMessage = "test exception";
            Exception exception = new Exception(ExceptionMessage);

            var stateMachineInformationMock = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);
            var stateMock = this.CreateStateMock(CurrentStateId);
            var stateContext = new StateContext<States, Events>();

            this.testee.HandlingEntryActionException(stateMachineInformationMock.Object, stateMock.Object, stateContext, ref exception);

            this.log4Net.LogContains(
                Level.Error, 
                "Exception in entry action of state A of state machine test machine: System.Exception: " + ExceptionMessage);
        }

        /// <summary>
        /// When an exception occurs in an exit action
        /// then the state machine name, the state and the exception are logged.
        /// </summary>
        [Fact]
        public void HandlingExitActionException()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            var stateMachineInformationMock = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);
            var stateMock = this.CreateStateMock(CurrentStateId);
            var stateContext = new StateContext<States, Events>();
            var exception = new Exception("test exception");

            this.testee.HandlingExitActionException(stateMachineInformationMock.Object, stateMock.Object, stateContext, ref exception);

            this.log4Net.LogContains(
                Level.Error,
                "Exception in exit action of state A of state machine test machine: System.Exception: test exception");
        }

        /// <summary>
        /// When an exception is thrown within a guard
        /// then the state machine name, the transition and the exception are logged.
        /// </summary>
        [Fact]
        public void HandlingGuardException()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            var stateMachineInformationMock = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);
            var transitionMock = new Mock<ITransition<States, Events>>();
            var stateMock = this.CreateStateMock(CurrentStateId);
            var transitionContext = new TransitionContext<States, Events>(stateMock.Object, Events.B, null, null);
            var exception = new Exception("test exception");

            this.testee.HandlingGuardException(stateMachineInformationMock.Object, transitionMock.Object, transitionContext, ref exception);

            this.log4Net.LogMatch(
                Level.Error,
                "Exception in guard of transition .* of state machine test machine: System.Exception: test exception");
        }

        /// <summary>
        /// When an exception is thrown within an transition
        /// then the state machine name, the transition and the exception are logged.
        /// </summary>
        [Fact]
        public void HandlingTransitionException()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            var stateMachineInformationMock = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);
            var transitionMock = new Mock<ITransition<States, Events>>();
            var stateMock = this.CreateStateMock(CurrentStateId);
            var transitionContext = new TransitionContext<States, Events>(stateMock.Object, Events.B, null, null);
            var exception = new Exception("test exception");

            this.testee.HandlingTransitionException(stateMachineInformationMock.Object, transitionMock.Object, transitionContext, ref exception);

            this.log4Net.LogMatch(
                Level.Error,
                "Exception in action of transition .* of state machine test machine: System.Exception: test exception");
        }

        private Mock<IStateMachineInformation<States, Events>> CreateStateMachineInformation(string stateMachineName, States currentStateId)
        {
            var stateMachineInformationMock = new Mock<IStateMachineInformation<States, Events>>();
            stateMachineInformationMock.Setup(information => information.Name).Returns(stateMachineName);
            stateMachineInformationMock.Setup(information => information.CurrentStateId).Returns(currentStateId);
            
            return stateMachineInformationMock;
        }

        private Mock<IState<States, Events>> CreateStateMock(States stateId)
        {
            var stateMock = new Mock<IState<States, Events>>();
            stateMock.Setup(state => state.Id).Returns(stateId);

            return stateMock;
        }
    }
}