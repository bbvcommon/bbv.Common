//-------------------------------------------------------------------------------
// <copyright file="EventTopics.cs" company="bbv Software Services AG">
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
    /// <summary>
    /// Contains event topics used in tests.
    /// </summary>
    public static class EventTopics
    {
        /// <summary>Simple event</summary>
        public const string SimpleEvent = "topic://EventBrokerTest/SimpleEvent";

        /// <summary>Event with custom event arguments</summary>
        public const string CustomEventArgs = "topic://EventBrokerTest/CustomEventArgs";

        /// <summary>subscription on background thread</summary>
        public const string BackgroundThread = "topic://EventBrokerTest/BackgroundThread";

        /// <summary>Count test</summary>
        public const string Count = "topic://EventBrokerTest/Count";

        /// <summary>Multiple publications on a single event</summary>
        public const string MultiplePublicationTokens = "topic://EventBroker/MultiplePublicationTokens";

        /// <summary>Multiple subscriptions on a single handler method.</summary>
        public const string MultipleSubscriptionTokens = "topic://EventBroker/MultipleSubscriptionTokens";

        /// <summary>Cancel event arguments.</summary>
        public const string CancelEventArgs = "topic://EventBrokerTest/CancelEventArgs";
    }
}