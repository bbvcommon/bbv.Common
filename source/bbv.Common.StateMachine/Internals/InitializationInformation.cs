//-------------------------------------------------------------------------------
// <copyright file="InitializationInformation.cs" company="bbv Software Services AG">
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
    /// <summary>
    /// Holds information about the state into which to initialize the state machine on first Start.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    public class InitializationInformation<TState>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationInformation&lt;TState&gt;"/> class.
        /// </summary>
        /// <param name="initialState">Initial state.</param>
        public InitializationInformation(TState initialState)
        {
            this.InitialState = initialState;
        }

        /// <summary>
        /// Gets the initial state.
        /// </summary>
        /// <value>The initial state.</value>
        public TState InitialState { get; private set; }
    }
}