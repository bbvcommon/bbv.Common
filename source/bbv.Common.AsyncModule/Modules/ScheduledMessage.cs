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

namespace bbv.Common.AsyncModule.Modules
{
    /// <summary>
    /// This message is sent to a scheduler module. The scheduler module 
    /// has to post the message contained in the SchedulerMessage
    /// at the DueTime.
    /// </summary>
    public class ScheduledMessage
    {
        /// <summary>
        /// To this module the contained message has to be sent.
        /// </summary>
        private readonly string moduleName;

        /// <summary>
        /// This message has to be posted at the DueTime.
        /// </summary>
        private readonly object message;
        
        /// <summary>
        /// At this time the contained message has to be posted.
        /// </summary>
        private readonly DateTime dueTime;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="moduleName">
        /// To this module the contained message has to be sent.
        /// </param>
        /// <param name="message">
        /// This message has to be posted after the dueTime.
        /// </param>
        /// <param name="dueTime">
        /// After this time in ms the contained message has to be posted.
        /// </param>
        public ScheduledMessage(string moduleName, object message, double dueTime)
        {
            this.moduleName = moduleName;
            this.message = message;
            this.dueTime = DateTime.Now.AddMilliseconds(dueTime);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="moduleName">
        /// To this module the contained message has to be sent.
        /// </param>
        /// <param name="message">
        /// This message has to be posted at the dueTime.
        /// </param>
        /// <param name="dueTime">
        /// At this time the contained message has to be posted.
        /// </param>
        public ScheduledMessage(string moduleName, object message, DateTime dueTime)
        {
            this.moduleName = moduleName;
            this.message = message;
            this.dueTime = dueTime;
        }

        /// <summary>
        /// To this module the contained message has to be sent.
        /// </summary>
        public string ModuleName
        {
            get { return moduleName; }
        }

        /// <summary>
        /// This message has to be posted at the dueTime.
        /// </summary>
        public object Message
        {
            get { return message; }
        }

        /// <summary>
        /// At this time the contained message has to be posted.
        /// </summary>
        public DateTime DueTime
        {
            get { return dueTime; }
        }
    }
}
