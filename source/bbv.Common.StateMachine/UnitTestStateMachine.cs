//-------------------------------------------------------------------------------
// <copyright file="UnitTestStateMachine.cs" company="bbv Software Services AG">
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
    using Internals;

    /// <summary>
    /// Special state machine that can be used in unit tests for easier exception testing.
    /// This state machine does not only signal exception cases and declined transitions with events
    /// but throws exceptions, too. This guarantees that these exception cases don't stay hidden in
    /// unit tests.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public class UnitTestStateMachine<TState, TEvent> : PassiveStateMachine<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestStateMachine{TState, TEvent}"/> class.
        /// </summary>
        public UnitTestStateMachine()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestStateMachine{TState, TEvent}"/> class.
        /// </summary>
        /// <param name="name">The name of the state machine. Used in log messages.</param>
        public UnitTestStateMachine(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestStateMachine{TState, TEvent}"/> class.
        /// </summary>
        /// <param name="name">The name of the state machine. Used in log messages.</param>
        /// <param name="factory">The factory.</param>
        public UnitTestStateMachine(string name, IFactory<TState, TEvent> factory)
            : base(name, factory)
        {
            this.ExceptionThrown += HandleExceptionThrown;
            this.TransitionDeclined += HandleTransitionDeclined;
            this.TransitionExceptionThrown += HandleTranistionExceptionThrown;
        }
        
        /// <summary>
        /// Throws an exception when a transition declined event is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="bbv.Common.StateMachine.TransitionEventArgs&lt;TState,TEvent&gt;"/> instance containing the event data.</param>
        private static void HandleTransitionDeclined(object sender, TransitionEventArgs<TState, TEvent> e)
        {
            throw new InvalidOperationException(ExceptionMessages.TransitionDeclined(e.StateId, e.EventId));
        }

        /// <summary>
        /// Throws the exception received in the event arguments. The stack trace of the original exception is restored.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="bbv.Common.StateMachine.ExceptionEventArgs&lt;TState,TEvent&gt;"/> instance containing the event data.</param>
        private static void HandleExceptionThrown(object sender, ExceptionEventArgs<TState, TEvent> e)
        {
            throw e.Exception.PreserveStackTrace();
        }

        /// <summary>
        /// Throws the exception received in the event arguments. The stack trace of the original exception is restored.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="bbv.Common.StateMachine.ExceptionEventArgs&lt;TState,TEvent&gt;"/> instance containing the event data.</param>
        private static void HandleTranistionExceptionThrown(object sender, TransitionExceptionEventArgs<TState, TEvent> e)
        {
            throw e.Exception.PreserveStackTrace();
        }
    }
}