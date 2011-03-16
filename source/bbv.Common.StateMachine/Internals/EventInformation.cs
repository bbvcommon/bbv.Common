//-------------------------------------------------------------------------------
// <copyright file="EventInformation.cs" company="bbv Software Services AG">
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

    /// <summary>
    /// Provides information about an event: event-id and arguments.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public class EventInformation<TEvent>
        where TEvent : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventInformation&lt;TEvent&gt;"/> class.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventArguments">The event arguments.</param>
        public EventInformation(TEvent eventId, object[] eventArguments)
        {
            this.EventId = eventId;
            this.EventArguments = eventArguments;
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        public TEvent EventId { get; private set; }

        /// <summary>
        /// Gets the event arguments.
        /// </summary>
        /// <value>The event arguments.</value>
        public object[] EventArguments { get; private set; }
    }
}