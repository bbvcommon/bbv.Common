//-------------------------------------------------------------------------------
// <copyright file="UnitTestStateMachineTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.StateMachine
{
    using System;
    using Xunit;

    /// <summary>
    /// Tests the special state machine for unit testing that throws all exceptions directly instead of
    /// firing events.
    /// </summary>
    public class UnitTestStateMachineTest
    {
        private readonly UnitTestStateMachine<States, Events> testee;

        /// <summary>
        /// The expected stack trace pointing to the method originally throwing the exception.
        /// </summary>
        private const string ExpectedStackTrace = "at bbv.Common.StateMachine.UnitTestStateMachineTest.ExceptionThrower()";

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestStateMachineTest"/> class.
        /// </summary>
        public UnitTestStateMachineTest()
        {
            this.testee = new UnitTestStateMachine<States, Events>();
            this.testee.Initialize(States.A);
            this.testee.Start();
        }

        /// <summary>
        /// When an exception is thrown in a transition action then the exception is thrown back to the caller of the 
        /// Fire method to allow easier unit testing of state machines.
        /// </summary>
        [Fact(Skip = "Platform dependent test. Investigate")]
        public void TransitionActionExceptionsAreThrown()
        {
            this.testee.In(States.A)
                .On(Events.B).Goto(States.B).Execute(arguments => this.ExceptionThrower());

            TestException exception = Assert.Throws<TestException>(() => this.testee.Fire(Events.B));
            Assert.Contains(ExpectedStackTrace, exception.StackTrace);
        }

        /// <summary>
        /// When an exception is thrown in an entry action then the exception is thrown back to the caller of the
        /// Fire method to allow easier unit testing of the state machines.
        /// </summary>
        [Fact]
        public void EntryActionExceptionsAreThrown()
        {
            this.testee.In(States.A)
                .On(Events.B).Goto(States.B);
            this.testee.In(States.B)
                .ExecuteOnEntry(this.ExceptionThrower);

            TestException exception = Assert.Throws<TestException>(() => this.testee.Fire(Events.B));
            Assert.Contains(ExpectedStackTrace, exception.StackTrace);
        }

        /// <summary>
        /// When an exception is thrown in an exit action then the exception is thrown back to the caller of the
        /// Fire method to allow easier unit testing of the state machines.
        /// </summary>
        [Fact]
        public void ExitActionExceptionsAreThrown()
        {
            this.testee.In(States.A)
                .ExecuteOnExit(this.ExceptionThrower)
                .On(Events.B).Goto(States.B);

            TestException exception = Assert.Throws<TestException>(() => this.testee.Fire(Events.B));
            Assert.Contains(ExpectedStackTrace, exception.StackTrace);
        }

        /// <summary>
        /// When a transition is declined then an <see cref="InvalidOperationException"/> is thrown directly to the
        /// caller of he state machine.
        /// </summary>
        [Fact]
        public void TransitionDeclineException()
        {
            Assert.Throws<InvalidOperationException>(() => this.testee.Fire(Events.B));
        }

        /// <summary>
        /// Throws an exception. Used to check that the stack trace points to this method here and not to the
        /// throw in the state machine.
        /// </summary>
        private void ExceptionThrower()
        {
            throw new TestException();
        }

        /// <summary>
        /// The exception used in the tests.
        /// </summary>
        private class TestException : Exception
        {
        }
    }
}