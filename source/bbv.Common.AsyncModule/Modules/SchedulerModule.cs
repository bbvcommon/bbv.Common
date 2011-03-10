/***************************************************************************/
// Copyright 2007 bbv Software Services AG
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
/***************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using bbv.Common.AsyncModule.Extensions;

namespace bbv.Common.AsyncModule.Modules
{
    /// <summary>
    /// The default scheduler module. Consumes ScheduledMessages and 
    /// posts the contained message at the requested time. Needs a
    /// TimedTriggerExtension.
    /// </summary>
    public class SchedulerModule
    {
        /// <summary>
        /// The messages to post sorted by the due time.
        /// </summary>
        private readonly List<ScheduledMessage> scheduledMessages;

        /// <summary>
        /// The module needs the controller to get the retry extension.
        /// </summary>
        private IModuleController moduleController;
        
        /// <summary>
        /// The module needs the coordinator to post the scheduled message.
        /// </summary>
        private IModuleCoordinator moduleCoordinator;

        /// <summary>
        /// The module needs the controller to get the retry extension.
        /// </summary>
        [ModuleController]
        public IModuleController ModuleController
        {
            set { moduleController = value; }
        }

        /// <summary>
        /// The module needs the coordinator to post the scheduled message.
        /// </summary>
        [ModuleCoordinator]
        public IModuleCoordinator ModuleCoordinator
        {
            set { moduleCoordinator = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SchedulerModule()
        {
            scheduledMessages = new List<ScheduledMessage>();
        }

        /// <summary>
        /// Compares the due times of the message.
        /// </summary>
        /// <param name="messageA">
        /// Left side of comparison.
        /// </param>
        /// <param name="messageB">
        /// Right side of comparison.
        /// </param>
        /// <returns>
        /// Result of dueTimeA.CompareTo(dueTimeB);
        /// </returns>
        private int CompareDueTimeOfScheduledMessage(ScheduledMessage messageA, ScheduledMessage messageB)
        {
            return messageA.DueTime.CompareTo(messageB.DueTime);
        }

        /// <summary>
        /// Sets the timed trigger extension of the module to the due time 
        /// of the given message.
        /// </summary>
        /// <param name="message">
        /// See above.
        /// </param>
        private void SetTimedTriggerForMessage(ScheduledMessage message)
        {
            // The time in ms we have to wait until the next 
            // scheduled message has to be posted.
            int nextMessageWaitTime = (message.DueTime - DateTime.Now).Milliseconds;

            // If the message is to late, schedule it immediately.
            if (nextMessageWaitTime < 0)
            {
                nextMessageWaitTime = 0;
            }

            // Set the timed trigger.
            moduleController.Extensions.Get<TimedTriggerExtension>().ChangeTimer(nextMessageWaitTime, Timeout.Infinite);
        }

        /// <summary>
        /// Consumes a scheduled message. Puts the message into 
        /// the waiting queue and sets the timed trigger.
        /// </summary>
        /// <param name="message">
        /// Contains the message to be posted at the given time.
        /// </param>
        [MessageConsumer]
        public void ConsumeScheduledMessage(ScheduledMessage message)
        {
            scheduledMessages.Add(message);
            scheduledMessages.Sort(CompareDueTimeOfScheduledMessage);
            SetTimedTriggerForMessage(scheduledMessages[0]);
        }

        /// <summary>
        /// The trigger to post the next message.
        /// </summary>
        /// <param name="message">
        /// Ignored.
        /// </param>
        [MessageConsumer]
        public void ConsumeTimedTrigger(TimedTriggerMessage message)
        {
            bool foundMessageToSchedule = false;
            DateTime now = DateTime.Now;

            // Post all messages, which are ready.
            while ((scheduledMessages.Count != 0) && !foundMessageToSchedule)
            {
                // Get the next message.
                ScheduledMessage scheduledMessage = scheduledMessages[0];

                // Is the message ready?
                if (scheduledMessage.DueTime <= now)
                {
                    // Yes -> Post it.
                    moduleCoordinator.PostMessage(scheduledMessage.ModuleName,
                        scheduledMessage.Message);
                    scheduledMessages.RemoveAt(0);
                }
                else
                {
                    // There is a message, which is not ready. We have 
                    // to set up the scheduler.
                    foundMessageToSchedule = true;
                }
            }

            // If there is still a message, which is not ready, schedule it.
            if (foundMessageToSchedule)
            {
                SetTimedTriggerForMessage(scheduledMessages[0]);
            }
        }
    }
}
