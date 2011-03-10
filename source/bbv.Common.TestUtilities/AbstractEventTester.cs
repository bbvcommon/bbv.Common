//-------------------------------------------------------------------------------
// <copyright file="AbstractEventTester.cs" company="bbv Software Services AG">
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
    using System.Globalization;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Abstract event tester to handle vent handlers and event arguments of any type.
    /// </summary>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    public abstract class AbstractEventTester<TEventHandler, TEventArgs> : IEventTester
        where TEventHandler : class
        where TEventArgs : EventArgs
    {
        /// <summary>
        /// The event that is monitored by this event helper instance.
        /// </summary>
        private readonly EventInfo eventInfo;

        /// <summary>
        /// The object that is monitored by this event helper instance.
        /// </summary>
        private readonly object sender;

        /// <summary>
        /// Delegate that is registered with the event.
        /// </summary>
        private readonly Delegate handlerDelegate;

        /// <summary>
        /// Event matcher that is used to determine whether the event must be fired.
        /// </summary>
        private readonly Func<object, TEventArgs, bool> eventMatcher;

        /// <summary>
        /// The matcher descriptor which is used to give the event tester a meaningful name.
        /// </summary>
        private readonly string matcherDescriptor;

        /// <summary>
        /// The expected number of invocations.
        /// </summary>
        private int? expectedInvocationCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractEventTester{TEventHandler, TEventArgs}"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        protected AbstractEventTester(object sender, string eventName)
            : this(sender, eventName, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractEventTester{TEventHandler, TEventArgs}"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventHandler">The event handler that is executed when the event occurs.</param>
        /// <param name="expectedInvocationCount">
        /// The expected number of invocations. An <see cref="EventTesterException"/> is thrown at disposal if the
        /// current invocation count does not match this value. If null is passed no check is performed.
        /// </param>
        protected AbstractEventTester(object sender, string eventName, TEventHandler eventHandler, int? expectedInvocationCount)
            : this(sender, eventName, eventHandler, (s, e) => true, null, expectedInvocationCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractEventTester{TEventHandler, TEventArgs}"/> class.
        /// </summary>
        /// <param name="sender">The object that contains the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="eventHandler">The event handler that is executed when the event occurs.</param>
        /// <param name="eventMatcher">The event matcher that is used to determine whether the event shall be taken into account.</param>
        /// <param name="eventMatcherDescriptor">The event matcher descriptor which is used to annotate the event with matching information.</param>
        /// <param name="expectedInvocationCount">The expected number of invocations. An <see cref="EventTesterException"/> is thrown at disposal if the
        /// current invocation count does not match this value. If null is passed no check is performed.</param>
        protected AbstractEventTester(object sender, string eventName, TEventHandler eventHandler, Func<object, TEventArgs, bool> eventMatcher, string eventMatcherDescriptor, int? expectedInvocationCount)
        {
            this.handlerDelegate = Delegate.CreateDelegate(typeof(TEventHandler), this, "Handler");

            this.EventHandler = eventHandler;
            this.eventInfo = sender.GetType().GetEvent(eventName);
            this.eventInfo.AddEventHandler(sender, this.handlerDelegate);
            this.sender = sender;
            this.expectedInvocationCount = expectedInvocationCount;
            this.eventMatcher = eventMatcher;
            this.matcherDescriptor = eventMatcherDescriptor;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AbstractEventTester{TEventHandler, TEventArgs}"/> class.
        /// </summary>
        ~AbstractEventTester()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Occurs when the monitored event is fired.
        /// </summary>
        public event EventHandler Invocation;

        /// <summary>
        /// Gets or sets the number of invocation of the monitored event.
        /// </summary>
        /// <value>The number of invocation of the monitored event.</value>
        public int InvocationCount
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a value indicating whether the event was fired.
        /// </summary>
        /// <value><c>true</c> if the event was fired; otherwise, <c>false</c>.</value>
        public bool WasFired
        {
            get
            {
                return this.InvocationCount > 0;
            }
        }

        /// <summary>
        /// Gets or sets the event handler that is called when the event occurs.
        /// </summary>
        protected TEventHandler EventHandler { get; set; }

        /// <summary>
        /// Gets the event matcher.
        /// </summary>
        /// <value>The event matcher.</value>
        protected Func<object, TEventArgs, bool> EventMatcher
        {
            get { return this.eventMatcher; }
        }

        /// <summary>
        /// Asserts that the event was fired.
        /// </summary>
        public void AssertWasFired()
        {
            if (!this.WasFired)
            {
                string errorMessage = string.Format(
                    CultureInfo.InvariantCulture,
                    "The event {0} was not fired.",
                    this.eventInfo.Name);
                throw new EventTesterException(errorMessage);
            }
        }

        /// <summary>
        /// Asserts that the event was fired exactly as often as specified by <paramref name="expectedInvocationCount"/>.
        /// </summary>
        /// <param name="expectedInvocationCount">The expected number of invocations.</param>
        public void AssertInvocationCount(int expectedInvocationCount)
        {
            string message = string.Format(
                CultureInfo.InvariantCulture,
                "The event {0} was fired {1} times instead of {2}.",
                this.eventInfo.Name,
                this.InvocationCount,
                expectedInvocationCount);

            if (this.InvocationCount != expectedInvocationCount)
            {
                throw new EventTesterException(message);
            }
        }

        /// <summary>
        /// Asserts that all expectations of the testers are met.
        /// Used for ordered event expectations in the <see cref="EventTestList"/>.
        /// </summary>
        public void AssertComplete()
        {
            if (this.expectedInvocationCount.HasValue)
            {
                this.AssertInvocationCount(this.expectedInvocationCount.Value);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder(this.eventInfo.Name);

            if (!string.IsNullOrEmpty(this.matcherDescriptor))
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, " matching {0}", this.matcherDescriptor);
            }
            
            return sb.ToString();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            this.AssertComplete();
        }

        /// <summary>
        /// Determines whether the <paramref name="sender"/> and the 
        /// <paramref name="eventArgs"/> are matching a given criteria.
        /// </summary>
        /// <param name="sender">The sender which fired the event.</param>
        /// <param name="eventArgs">The event arguments.</param>
        /// <returns><c>true</c> when the criteria matches; otherwise <c>false</c>
        /// </returns>
        protected virtual bool IsMatch(object sender, TEventArgs eventArgs)
        {
            return this.EventMatcher(sender, eventArgs);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; 
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.eventInfo.RemoveEventHandler(this.sender, this.handlerDelegate);
            }
        }

        /// <summary>
        /// Fires the event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments instance containing the event data.</param>
        protected abstract void Fire(object sender, TEventArgs e);

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments passed with the event.</param>
        protected virtual void Handler(object sender, TEventArgs e)
        {
            if (this.IsMatch(sender, e))
            {
                this.IncrementInvocationCount();

                this.OnInvocation();

                this.OnFire(sender, e);
            }
        }

        /// <summary>
        /// Invokes the fire method when an event handler is present.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments used in the event.</param>
        protected void OnFire(object sender, TEventArgs e)
        {
            if (this.EventHandler != null)
            {
                this.Fire(sender, e);
            }
        }

        /// <summary>
        /// Invokes the invocation event.
        /// </summary>
        protected void OnInvocation()
        {
            if (this.Invocation != null)
            {
                this.Invocation(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Increments the invocation counter.
        /// </summary>
        protected void IncrementInvocationCount()
        {
            this.InvocationCount++;
        }
    }
}