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
// Project:     bbv.Common.AsyncModule
// ------------------------------------------------
// File:        ScheduledMessageMatcher.cs
// Author:      Adrian Krummenacher / AK
//
// History:
// --------
// Date          Name   Version   Description
// 23-AUG-2006   AK		1.0       Creation     
//
/***************************************************************************/

using System;
using NMock2;

namespace bbv.Common.AsyncModule
{
    /// <summary>
    /// The parameters of the type ScheduledMessage cannot be matched directly 
    /// because the due time is calculated from DateTime.Now. Therefore, we 
    /// need our own matcher.
    /// </summary>
    public class ScheduledMessageMatcher : Matcher
    {
        /// <summary>
        /// The ScheduledMessage matches if it contains this module name.
        /// </summary>
        private string m_moduleName;

        /// <summary>
        /// The ScheduledMessage matches if it contains this message.
        /// </summary>
        private object m_message;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="moduleName">
        /// The ScheduledMessage matches if it contains this module name.
        /// </param>
        /// <param name="message">
        /// The ScheduledMessage matches if it contains this message.
        /// </param>
        public ScheduledMessageMatcher(string moduleName, object message)
        {
            m_moduleName = moduleName;
            m_message = message;
        }

        /// <summary>
        /// NMock uses this method to log the match.
        /// </summary>
        /// <param name="writer">
        /// The log has to be written to this writer.
        /// </param>
        public override void DescribeTo(System.IO.TextWriter writer)
        {
            writer.Write(string.Format("ScheduledMessage({0},{1})"), m_moduleName,
                m_message.ToString());
        }

        /// <summary>
        /// True if the object is of the type ScheduledMessage and the 
        /// module name and the contained message match the values 
        /// this matcher was initialized with.
        /// </summary>
        /// <param name="obj">
        /// The object to compare.
        /// </param>
        /// <returns>
        /// See above.
        /// </returns>
        public override bool Matches(object obj)
        {
            if (!(obj is ScheduledMessage))
            {
                return (false);
            }
            ScheduledMessage message = (ScheduledMessage)obj;
            return ((m_moduleName == message.ModuleName) &&
                (m_message == message.Message));
        }
    }
}
