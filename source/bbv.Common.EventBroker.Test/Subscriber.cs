//-------------------------------------------------------------------------------
// <copyright file="Subscriber.cs" company="bbv Software Services AG">
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

namespace bbv.Common.EventBroker
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    /// <summary>
    /// Subscriber used in tests.
    /// </summary>
    public class Subscriber
    {
        /// <summary>
        /// Track thread id.
        /// </summary>
        private int threadId = -1;

        /// <summary>
        /// Signal for thread synchronization.
        /// </summary>
        private AutoResetEvent signal = new AutoResetEvent(false);

        /// <summary>
        /// Gets or sets the number of times <see cref="CountHandler"/> was called.
        /// </summary>
        public static int Count { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="SimpleEvent"/> was called.
        /// </summary>
        public bool SimpleEventCalled { get; set; }

        /// <summary>
        /// Gets or sets the received custom event arguments.
        /// </summary>
        public CustomEventArguments ReceivedCustomEventArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="MultiplePublicationTokens1"/> was called.
        /// </summary>
        public bool MultiplePublicationToken1Called { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="MultiplePublicationTokens2"/> was called.
        /// </summary>
        public bool MultiplePublicationToken2Called { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="MultiplePublicationTokens3"/> was called.
        /// </summary>
        public bool MultiplePublicationToken3Called { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="MultipleSubscriptionTokens"/> was called.
        /// </summary>
        public int MultipleSubscriptionTokensCount { get; set; }

        /// <summary>
        /// Gets or sets the thread id.
        /// </summary>
        /// <value>The thread id.</value>
        public int ThreadId
        {
            get { return this.threadId; }
            set { this.threadId = value; }
        }

        /// <summary>
        /// Gets or sets the signal.
        /// </summary>
        /// <value>The signal.</value>
        public AutoResetEvent Signal
        {
            get { return this.signal; }
            set { this.signal = value; }
        }

        /// <summary>
        /// Simple event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [EventSubscription(EventTopics.SimpleEvent, typeof(Handlers.Publisher))]
        public void SimpleEvent(object sender, EventArgs e)
        {
            this.SimpleEventCalled = true;
        }

        /// <summary>
        /// Event handler with custom event arguments.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        [EventSubscription(EventTopics.CustomEventArgs, typeof(Handlers.Publisher))]
        public void CustomEventArgs(object sender, CustomEventArguments e)
        {
            this.ReceivedCustomEventArguments = e;
        }

        /// <summary>
        /// Event handler running on background thread.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [EventSubscription(EventTopics.BackgroundThread, typeof(Handlers.Background))]
        public void BackgroundThread(object sender, EventArgs e)
        {
            this.threadId = Thread.CurrentThread.ManagedThreadId;
            this.signal.Set();
        }

        /// <summary>
        /// For counting test.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [EventSubscription(EventTopics.Count, typeof(Handlers.Publisher))]
        public void CountHandler(object sender, EventArgs e)
        {
            Count++;
        }

        /// <summary>
        /// For multiple publications test.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [EventSubscription(EventTopics.MultiplePublicationTokens + "1", typeof(Handlers.Publisher))]
        public void MultiplePublicationTokens1(object sender, EventArgs e)
        {
            this.MultiplePublicationToken1Called = true;
        }

        /// <summary>
        /// For multiple publications test.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [EventSubscription(EventTopics.MultiplePublicationTokens + "2", typeof(Handlers.Publisher))]
        public void MultiplePublicationTokens2(object sender, EventArgs e)
        {
            this.MultiplePublicationToken2Called = true;
        }

        /// <summary>
        /// For multiple publications test.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [EventSubscription(EventTopics.MultiplePublicationTokens + "3", typeof(Handlers.Publisher))]
        public void MultiplePublicationTokens3(object sender, EventArgs e)
        {
            this.MultiplePublicationToken3Called = true;
        }

        /// <summary>
        /// Handler for multiple subscriptions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [EventSubscription(EventTopics.MultipleSubscriptionTokens + "1", typeof(Handlers.Publisher))]
        [EventSubscription(EventTopics.MultipleSubscriptionTokens + "2", typeof(Handlers.Publisher))]
        [EventSubscription(EventTopics.MultipleSubscriptionTokens + "3", typeof(Handlers.Publisher))]
        public void MultipleSubscriptionTokens(object sender, EventArgs e)
        {
            this.MultipleSubscriptionTokensCount++;
        }

        /// <summary>
        /// For cancel event arguments test.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        [EventSubscription(EventTopics.CancelEventArgs, typeof(Handlers.Publisher))]
        public void CancelEventArgs(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}