//-------------------------------------------------------------------------------
// <copyright file="TransitionsTest.cs" company="bbv Software Services AG">
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
    using Xunit;

    /// <summary>
    /// Tests transition behavior.
    /// </summary>
    public class TransitionsTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private readonly StateMachine<States, Events> testee;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionsTest"/> class.
        /// </summary>
        public TransitionsTest()
        {
            this.testee = new StateMachine<States, Events>();
        }

        /// <summary>
        /// When no transition for the fired event can be found in the entire
        /// hierarchy up from the current state then the transition declined event is fired and 
        /// the state machine remains in its current state.
        /// </summary>
        [Fact]
        public void MissingTransition()
        {
            this.testee.In(States.A)
                .On(Events.B).Goto(States.B);

            bool declined = false;

            this.testee.TransitionDeclined += (sender, e) =>
                                                  {
                                                      declined = true;
                                                  };

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.C);

            Assert.True(declined, "Declined event was not fired");
            Assert.Equal(States.A, this.testee.CurrentStateId);
        }

        /// <summary>
        /// Actions on transitions are performed and the event arguments are passed to them.
        /// </summary>
        [Fact]
        public void ExecuteActions()
        {
            object[] action1Arguments = null;
            object[] action2Arguments = null;

            this.testee.In(States.A)
                .On(Events.B).Goto(States.B).Execute(
                arguments => { action1Arguments = arguments; },
                arguments => { action2Arguments = arguments; });

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            var eventArguments = new object[] { 1, 2, 3, "test" };
            this.testee.Fire(Events.B, eventArguments);

            Assert.Equal(eventArguments, action1Arguments);
            Assert.Equal(eventArguments, action2Arguments);
        }

        /// <summary>
        /// Internal transitions can be executed 
        /// (internal transition = transition that remains in the same state and does not execute exit
        /// and entry actions.
        /// </summary>
        [Fact]
        public void InternalTransition()
        {
            bool executed = false;

            this.testee.In(States.A)
                .On(Events.A).Execute(eventArguments => executed = true);
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.A);

            Assert.True(executed, "internal transition was not executed.");
            Assert.Equal(States.A, this.testee.CurrentStateId);
        }

        [Fact]
        public void ActionsWithoutArguments()
        {
            bool executed = false;

            this.testee.In(States.A)
                .On(Events.B).Execute(() => executed = true);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B);

            Assert.True(executed);
        }

        [Fact]
        public void ActionsWithOneArgument()
        {
            const int ExpectedValue = 1;
            int value = 0;

            this.testee.In(States.A)
                .On(Events.B).Execute<int>(v => value = v);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B, new object[] { ExpectedValue });

            Assert.Equal(value, ExpectedValue);
        }
    }
}