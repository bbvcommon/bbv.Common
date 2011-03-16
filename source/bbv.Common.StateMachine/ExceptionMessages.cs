//-------------------------------------------------------------------------------
// <copyright file="ExceptionMessages.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using bbv.Common.StateMachine.Internals;

    /// <summary>
    /// Holds all exception messages
    /// </summary>
    public static class ExceptionMessages
    {
        /// <summary>
        /// Value is not initialized.
        /// </summary>
        public const string ValueNotInitialized = "Value is not initialized";

        /// <summary>
        /// State machine is already initialized.
        /// </summary>
        public const string StateMachineIsAlreadyInitialized = "state machine is already initialized";

        /// <summary>
        /// State machine is not initialized.
        /// </summary>
        public const string StateMachineNotInitialized = "state machine is not initialized";

        /// <summary>
        /// State machine has not yet entered initial state.
        /// </summary>
        public const string StateMachineHasNotYetEnteredInitialState = "Initial state is not yet entered.";

        /// <summary>
        /// State cannot be its own super-state..
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>error message</returns>
        public static string StateCannotBeItsOwnSuperState(string state)
        {
            return string.Format(CultureInfo.InvariantCulture, "State {0} cannot be its own super-state.", state);
        }

        /// <summary>
        /// State cannot be the initial sub-state to itself.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>error message</returns>
        public static string StateCannotBeTheInitialSubStateToItself(string state)
        {
            return string.Format(
                CultureInfo.InvariantCulture, "State {0} cannot be the initial sub-state to itself.", state);
        }

        /// <summary>
        /// State cannot be the initial state of super state because it is not a direct sub-state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="superState">State of the super.</param>
        /// <returns>error message</returns>
        public static string StateCannotBeTheInitialStateOfSuperStateBecauseItIsNotADirectSubState(
            string state, string superState)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "State {0} cannot be the initial state of super state {1} because it is not a direct sub-state.",
                state,
                superState);
        }

        /// <summary>
        /// Cannot set state as a super state because the children states do already have a super state.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="newSuperStateId">The new super state id.</param>
        /// <param name="statesAlreadyHavingASuperState">State of the states already having A super.</param>
        /// <returns>error message</returns>
        public static string CannotSetStateAsASuperStateBecauseASuperStateIsAlreadySet<TState, TEvent>(TState newSuperStateId, IEnumerable<IState<TState, TEvent>> statesAlreadyHavingASuperState)
            where TState : IComparable
            where TEvent : IComparable
        {
            var statesWithSuperStates = from m in statesAlreadyHavingASuperState select new { m.Id, SuperStateId = m.SuperState.Id };
            string message = statesWithSuperStates.Aggregate(string.Empty, (acc, info) => acc + " state = " + info.Id + " super state = " + info.SuperStateId + ";");

            return string.Format(
                CultureInfo.InvariantCulture,
                "Cannot set state {0} as a super state because the following states do already have a super state: {1}.",
                newSuperStateId,
                message);
        }

        /// <summary>
        /// Transition cannot be added to the state because it has already been added to the state.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="transition">The transition.</param>
        /// <param name="state">The state.</param>
        /// <returns>error message</returns>
        public static string TransitionDoesAlreadyExist<TState, TEvent>(ITransition<TState, TEvent> transition, IState<TState, TEvent> state)
            where TState : IComparable
            where TEvent : IComparable
        {
            Ensure.ArgumentNotNull(transition, "transition");

            return string.Format(
                        CultureInfo.InvariantCulture,
                        "Transition {0} cannot be added to the state {1} because it has already been added to the state {2}.",
                        transition,
                        state,
                        transition.Source);
        }

        /// <summary>
        /// Transition declined.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="stateId">The state id.</param>
        /// <param name="eventId">The event id.</param>
        /// <returns>error message</returns>
        public static string TransitionDeclined<TState, TEvent>(TState stateId, TEvent eventId)
        {
            return string.Format(
                    CultureInfo.InvariantCulture,
                    "Transition declined: state = {0} event = {1}",
                    stateId,
                    eventId);
        }
    }
}