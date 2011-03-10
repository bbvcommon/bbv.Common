//-------------------------------------------------------------------------------
// <copyright file="EventTester.cs" company="bbv Software Services AG">
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

namespace bbv.Common.TestUtilities
{
    using System;

    /// <summary>
    /// This class can be used for testing if an event occurred and had the correct event arguments.
    /// The event is registered in the constructor and released when the instance is disposed.
    /// </summary>
    /// <example>
    /// Usage:
    /// <code>
    /// using (new EventHelper(testee, myEventName)
    /// {
    ///     Do();
    ///     Assert.IsTrue(eventHelper.WasFired, "MyEvent was not fired!");
    /// }
    /// </code>
    /// </example>
    public class EventTester : AbstractEventTester<EventHandler, EventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventTester"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        public EventTester(object sender, string eventName)
            : base(sender, eventName, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTester"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventMatcher">The event matcher that is used to determine whether the event shall be taken into account.</param>
        /// <param name="eventMatcherDescriptor">The event matcher descriptor which is used to annotate the event with matching information.</param>
        public EventTester(object sender, string eventName, Func<object, EventArgs, bool> eventMatcher, string eventMatcherDescriptor)
            : base(sender, eventName, null, eventMatcher, eventMatcherDescriptor, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTester"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="expectedInvocationCount">
        /// The expected number of invocations. An <see cref="EventTesterException"/> is thrown at disposal if the
        /// current invocation count does not match this value.
        /// </param>
        public EventTester(object sender, string eventName, int expectedInvocationCount)
            : base(sender, eventName, null, expectedInvocationCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTester"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventMatcher">The event matcher that is used to determine whether the event shall be taken into account.</param>
        /// <param name="eventMatcherDescriptor">The event matcher descriptor which is used to annotate the event with matching information.</param>
        /// <param name="expectedInvocationCount">
        /// The expected number of invocations. An <see cref="EventTesterException"/> is thrown at disposal if the
        /// current invocation count does not match this value.
        /// </param>
        public EventTester(object sender, string eventName, Func<object, EventArgs, bool> eventMatcher, string eventMatcherDescriptor, int expectedInvocationCount)
            : base(sender, eventName, null, eventMatcher, eventMatcherDescriptor, expectedInvocationCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTester"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventHandler">The event handler that is executed when the event occurs.</param>
        public EventTester(object sender, string eventName, EventHandler eventHandler)
            : base(sender, eventName, eventHandler, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTester"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventHandler">The event handler that is executed when the event occurs.</param>
        /// <param name="eventMatcher">The event matcher that is used to determine whether the event shall be taken into account.</param>
        /// <param name="eventMatcherDescriptor">The event matcher descriptor which is used to annotate the event with matching information.</param>
        public EventTester(object sender, string eventName, EventHandler eventHandler, Func<object, EventArgs, bool> eventMatcher, string eventMatcherDescriptor)
            : base(sender, eventName, eventHandler, eventMatcher, eventMatcherDescriptor, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTester"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventHandler">The event handler that is executed when the event occurs.</param>
        /// <param name="expectedInvocationCount">
        /// The expected number of invocations. An <see cref="EventTesterException"/> is thrown at disposal if the
        /// current invocation count does not match this value.
        /// </param>
        public EventTester(object sender, string eventName, EventHandler eventHandler, int expectedInvocationCount)
            : base(sender, eventName, eventHandler, expectedInvocationCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTester"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventHandler">The event handler that is executed when the event occurs.</param>
        /// <param name="eventMatcher">The event matcher that is used to determine whether the event shall be taken into account.</param>
        /// <param name="eventMatcherDescriptor">The event matcher descriptor which is used to annotate the event with matching information.</param>
        /// <param name="expectedInvocationCount">
        /// The expected number of invocations. An <see cref="EventTesterException"/> is thrown at disposal if the
        /// current invocation count does not match this value.
        /// </param>
        public EventTester(object sender, string eventName, EventHandler eventHandler, Func<object, EventArgs, bool> eventMatcher, string eventMatcherDescriptor, int expectedInvocationCount)
            : base(sender, eventName, eventHandler, eventMatcher, eventMatcherDescriptor, expectedInvocationCount)
        {
        }

        /// <summary>
        /// Fires the event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void Fire(object sender, EventArgs e)
        {
            this.EventHandler(sender, e);
        }
    }
}