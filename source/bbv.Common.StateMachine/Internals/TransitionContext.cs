//-------------------------------------------------------------------------------
// <copyright file="TransitionContext.cs" company="bbv Software Services AG">
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
    using System.Diagnostics;

    /// <summary>
    /// Provides context information during a transition.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    [DebuggerDisplay("State = {state} Event = {eventId} EventArguments = {eventArguments}")]
    public class TransitionContext<TState, TEvent> : StateContext<TState, TEvent>, ITransitionContext<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// The event that causes the transition.
        /// </summary>
        private readonly TEvent eventId;

        /// <summary>
        /// The event arguments.
        /// </summary>
        private readonly object[] eventArguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionContext{TState,TEvent}"/> class.
        /// </summary>
        /// <param name="state">The source state.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventArguments">The event arguments.</param>
        /// <param name="notifier">The notifier to fire events about transition events and exceptions.</param>
        public TransitionContext(IState<TState, TEvent> state, TEvent eventId, object[] eventArguments, INotifier<TState, TEvent> notifier)
            : base(state, notifier)
        {
            this.eventId = eventId;
            this.eventArguments = eventArguments;
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        public TEvent EventId
        {
            get { return this.eventId; }
        }

        /// <summary>
        /// Gets the event arguments.
        /// </summary>
        /// <value>The event arguments.</value>
        public object[] EventArguments
        {
            get { return this.eventArguments; }
        }

        /// <summary>
        /// Called when an exception should be notified.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public override void OnExceptionThrown(Exception exception)
        {
            this.AddException(exception);
            this.Notifier.OnExceptionThrown(this, exception);
        }

        /// <summary>
        /// Called when a transition beginning should be notified.
        /// </summary>
        public void OnTransitionBegin()
        {
            this.Notifier.OnTransitionBegin(this);
        }
    }
}