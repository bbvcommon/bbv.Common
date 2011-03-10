//-------------------------------------------------------------------------------
// <copyright file="EventFilter.cs" company="bbv Software Services AG">
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
    using System.Timers;

    /// <summary>
    /// Filter for events that occurs consecutive in short time and only the last one of the series is important.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
    public class EventFilter<TEventArgs> : IDisposable, IEventFilter<TEventArgs> where TEventArgs : EventArgs
    {
        /// <summary>
        /// Default time to wait since last event from observee until event is fired to observer
        /// </summary>
        private const int DefaultTimeWindow = 150;

        /// <summary>
        /// Timer used to wait for timeout.
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// Last received sender.
        /// </summary>
        private object pendingSender;

        /// <summary>
        /// Last received event args.
        /// </summary>
        private TEventArgs pendingEventArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFilter{TEventArgs}"/> class.
        /// </summary>
        public EventFilter() : this(DefaultTimeWindow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFilter{TEventArgs}"/> class.
        /// </summary>
        /// <param name="timeWindow">The filter time window.</param>
        public EventFilter(int timeWindow)
        {
            this.timer = new Timer();
            this.timer.Interval = timeWindow;
            this.timer.Elapsed += this.OnTimerElapsed;
        }

        /// <summary>
        /// This event accurs if in the defined time window no new 'original' event occurs.
        /// </summary>
        public event EventHandler<TEventArgs> FilteredEventRaised;

        /// <summary>
        /// Receiver of the to filter event
        /// </summary>
        /// <param name="sender">Source of the Event.</param>
        /// <param name="e">The Event Args.</param>
        /// <remarks>
        /// The method can direct used as the event handler method, but it can also called from the original event
        /// handler method
        /// </remarks>
        public void HandleOriginalEvent(object sender, TEventArgs e)
        {
            this.timer.Stop();
            this.pendingSender = sender;
            this.pendingEventArgs = e;
            this.timer.Start();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.timer.Elapsed -= this.OnTimerElapsed;
                this.timer.Dispose();
            }
        }

        /// <summary>
        /// Called when timer elapsed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnTimerElapsed(object sender, EventArgs e)
        {
            this.timer.Stop();

            EventHandler<TEventArgs> handler = this.FilteredEventRaised;

            if (handler != null)
            {
                handler(this.pendingSender, this.pendingEventArgs);
            }

            this.pendingSender = null;
            this.pendingEventArgs = null;
        }
    }
}
