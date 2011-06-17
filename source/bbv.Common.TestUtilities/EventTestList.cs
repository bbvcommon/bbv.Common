//-------------------------------------------------------------------------------
// <copyright file="EventTestList.cs" company="bbv Software Services AG">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// This list can be used to check several events using <see cref="EventTester"/> without having to use nested
    /// usings. Additionally it can be used to enforce an ordered occurrence of the events.
    /// </summary>
    /// <example>
    /// Usage:
    /// <code>
    /// using (new EventTestList(true)
    /// {
    ///     new EventTester(this.testInterface, "NoneGenericEvent", 1), 
    ///     new EventTester&lt;EventArgs&gt;(this.testInterface, "GenericEvent", 1)
    ///    })
    /// {
    ///     DoSomethingThatTriggersTheEventsInOrderedOccurrence();
    /// }
    /// </code>
    /// </example>
    public class EventTestList : IEnumerable<IEventTester>, IDisposable
    {
        /// <summary>
        /// The list of <see cref="IEventTester"/> in this list.
        /// </summary>
        private readonly List<IEventTester> eventTesterList = new List<IEventTester>();

        /// <summary>
        /// Indicated if an event invocation has failed.
        /// </summary>
        private bool failed;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTestList"/> class.
        /// </summary>
        public EventTestList() : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTestList"/> class.
        /// </summary>
        /// <param name="ordered">if set to <c>true</c> it is checked that the events occur in the order they are added.</param>
        public EventTestList(bool ordered)
        {
            this.Ordered = ordered;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EventTestList"/> is ordered.
        /// </summary>
        /// <value><c>true</c> if ordered; otherwise, <c>false</c>.</value>
        public bool Ordered { get; protected set; }

        /// <summary>
        /// Gets number of IEventTesters in this list.
        /// </summary>
        /// <value>The number of IEventTesters in this list.</value>
        public int Count
        {
            get { return this.eventTesterList.Count; }
        }

        /// <summary>
        /// Gets the <see cref="bbv.Common.TestUtilities.IEventTester"/> at the specified index.
        /// </summary>
        /// <value>The <see cref="bbv.Common.TestUtilities.IEventTester"/> at the specified index.</value>
        /// <param name="index">index of the element that shall be returned.</param>
        /// <returns><see cref="bbv.Common.TestUtilities.IEventTester"/> at the specified index.</returns>
        public IEventTester this[int index]
        {
            get { return this.eventTesterList[index]; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// Disposes all <see cref="IEventTester"/> in this list.
        /// </summary>
        public void Dispose()
        {
            Exception exception = null;
            foreach (IEventTester eventTester in this)
            {
                try
                {
                    eventTester.Dispose();
                }
                catch (EventTesterException e)
                {
                    if (!this.failed)
                    {
                        this.failed = true;
                        exception = new EventTesterException("One of the event testers failed.", e);
                    }
                }
            }

            if (exception != null)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Adds the specified event tester to the list.
        /// </summary>
        /// <param name="eventTester">The event tester to the list..</param>
        public void Add(IEventTester eventTester)
        {
            this.eventTesterList.Add(eventTester);
            eventTester.Invocation += this.OnEventInvoked;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IEventTester> GetEnumerator()
        {
            return this.eventTesterList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.eventTesterList.GetEnumerator();
        }

        /// <summary>
        /// Called when the event monitored by one of the event testers in the list is fired.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnEventInvoked(object sender, EventArgs e)
        {
            if (!this.Ordered)
            {
                return;
            }

            foreach (IEventTester eventTester in this)
            {
                if (eventTester.Equals(sender))
                {
                    return;
                }

                try
                {
                    eventTester.AssertComplete();
                }
                catch (EventTesterException exception)
                {
                    this.failed = true;
                    throw new EventTesterException(
                        string.Format(
                            CultureInfo.InvariantCulture, 
                            "The event {0} was fired before event {1}. But they are expected to occur in the opposite order.",
                            sender,
                            eventTester), 
                        exception);
                }
            }
        }
    }
}
