//-------------------------------------------------------------------------------
// <copyright file="IEventFilter.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Events
{
    using System;

    /// <summary>
    /// Defines functions for a filter that filters events who occurs consecutive in short time, only the last one 
    /// of the series is important.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
    public interface IEventFilter<TEventArgs> where TEventArgs : EventArgs
    {
        /// <summary>
        /// This event accurs if in the defined time window no new 'original' event occurs.
        /// </summary>
        event EventHandler<TEventArgs> FilteredEventRaised;

        /// <summary>
        /// Receiver of the to filter event
        /// </summary>
        /// <param name="sender">Source of the Event.</param>
        /// <param name="e">The Event Args.</param>
        /// <remarks>
        /// The method can direct used as the event handler method, but it can also called from the original event
        /// handler method
        /// </remarks>
        void HandleOriginalEvent(object sender, TEventArgs e);
    }
}
