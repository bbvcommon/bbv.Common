//-------------------------------------------------------------------------------
// <copyright file="EventFilterTest.cs" company="bbv Software Services AG">
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
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class EventFilterTest
    {
        [Test]
        [Ignore("unstable - investigate")]
        public void FilterFastEvent()
        {
            int count = 0;

            EventFilter<EventArgs> filter = new EventFilter<EventArgs>(100);

            AutoResetEvent finished = new AutoResetEvent(false);

            filter.FilteredEventRaised += delegate
                                              {
                                                  count++;
                                                  finished.Set();
                                              };

            filter.HandleOriginalEvent(this, EventArgs.Empty);
            filter.HandleOriginalEvent(this, EventArgs.Empty);
            filter.HandleOriginalEvent(this, EventArgs.Empty);

            finished.WaitOne(200, false);

            Assert.AreEqual(1, count, "There should be one filtered event");
        }

        [Test]
        public void FilterMixedEvents()
        {
            int count = 0;

            EventFilter<EventArgs> filter = new EventFilter<EventArgs>(100);

            AutoResetEvent finished = new AutoResetEvent(false);

            filter.FilteredEventRaised += delegate
                                              {
                                                  count++;
                                                  finished.Set();
                                              };

            filter.HandleOriginalEvent(this, EventArgs.Empty);
            filter.HandleOriginalEvent(this, EventArgs.Empty);
            finished.WaitOne(200, false);
            filter.HandleOriginalEvent(this, EventArgs.Empty);
            finished.WaitOne(200, false);

            Assert.AreEqual(2, count, "There should be 2 filtered events");
        }

        [Test]
        public void FilterSlowEvents()
        {
            int count = 0;

            EventFilter<EventArgs> filter = new EventFilter<EventArgs>(100);

            AutoResetEvent finished = new AutoResetEvent(false);

            filter.FilteredEventRaised += delegate
                                              {
                                                  count++;
                                                  finished.Set();
                                              };

            filter.HandleOriginalEvent(this, EventArgs.Empty);
            finished.WaitOne(200, false);
            filter.HandleOriginalEvent(this, EventArgs.Empty);
            finished.WaitOne(200, false);
            filter.HandleOriginalEvent(this, EventArgs.Empty);
            finished.WaitOne(200, false);

            Assert.AreEqual(3, count, "There should be 3 filtered events");
        }
    }
}
