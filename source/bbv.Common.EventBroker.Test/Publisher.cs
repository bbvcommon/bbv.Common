//-------------------------------------------------------------------------------
// <copyright file="Publisher.cs" company="bbv Software Services AG">
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

    /// <summary>
    /// Publisher providing scenarios needed in tests.
    /// </summary>
    public class Publisher
    {
        /// <summary>
        /// A simple event.
        /// </summary>
        [EventPublication(EventTopics.SimpleEvent)]
        public event EventHandler SimpleEvent;

        /// <summary>
        /// Event with custom event arguments.
        /// </summary>
        [EventPublication(EventTopics.CustomEventArgs)]
        public event EventHandler<CustomEventArguments> CustomEventArgs;

        /// <summary>
        /// Event to test background handling.
        /// </summary>
        [EventPublication(EventTopics.BackgroundThread)]
        public event EventHandler BackgroundThread;

        /// <summary>
        /// Event to test several subscribers.
        /// </summary>
        [EventPublication(EventTopics.Count)]
        public event EventHandler Count;

        /// <summary>
        /// Event with several publications.
        /// </summary>
        [EventPublication(EventTopics.MultiplePublicationTokens + "1")]
        [EventPublication(EventTopics.MultiplePublicationTokens + "2")]
        [EventPublication(EventTopics.MultiplePublicationTokens + "3")]
        public event EventHandler MultiplePublicationTokens;

        /// <summary>
        /// Event to test subscriptions to several publications.
        /// </summary>
        [EventPublication(EventTopics.MultipleSubscriptionTokens + "1")]
        public event EventHandler MultipleSubscriptionTokens1;

        /// <summary>
        /// Event to test subscriptions to several publications.
        /// </summary>
        [EventPublication(EventTopics.MultipleSubscriptionTokens + "2")]
        public event EventHandler MultipleSubscriptionTokens2;

        /// <summary>
        /// Event to test subscriptions to several publications.
        /// </summary>
        [EventPublication(EventTopics.MultipleSubscriptionTokens + "3")]
        public event EventHandler MultipleSubscriptionTokens3;

        /// <summary>
        /// Event with cancel event arguments.
        /// </summary>
        [EventPublication(EventTopics.CancelEventArgs)]
        public event EventHandler<CancelEventArgs> CancelEvent;

        /// <summary>
        /// Calls <see cref="SimpleEvent"/>.
        /// </summary>
        public void CallSimpleEvent()
        {
            this.SimpleEvent(this, EventArgs.Empty);
        }

        /// <summary>
        /// Calls the custom event arguments event.
        /// </summary>
        /// <param name="value">The value.</param>
        public void CallCustomEventArgs(string value)
        {
            this.CustomEventArgs(this, new CustomEventArguments(value));
        }

        /// <summary>
        /// Calls the <see cref="BackgroundThread"/> event.
        /// </summary>
        public void CallBackgroundThread()
        {
            this.BackgroundThread(this, EventArgs.Empty);
        }

        /// <summary>
        /// Calls the <see cref="Count"/> event.
        /// </summary>
        public void CallCount()
        {
            this.Count(this, EventArgs.Empty);
        }

        /// <summary>
        /// Calls the <see cref="MultiplePublicationTokens"/> event.
        /// </summary>
        public void CallMultiplePublicationTokens()
        {
            this.MultiplePublicationTokens(this, EventArgs.Empty);
        }

        /// <summary>
        /// Calls the multiple subscription tokens event.
        /// </summary>
        public void CallMultipleSubscriptionTokens()
        {
            this.MultipleSubscriptionTokens1(this, EventArgs.Empty);
            this.MultipleSubscriptionTokens2(this, EventArgs.Empty);
            this.MultipleSubscriptionTokens3(this, EventArgs.Empty);
        }

        /// <summary>
        /// Calls the cancel event.
        /// </summary>
        /// <returns>Whether the event was canceled.</returns>
        public bool CallCancelEvent()
        {
            CancelEventArgs e = new CancelEventArgs(false);
            this.CancelEvent(this, e);

            return e.Cancel;
        }
    }
}