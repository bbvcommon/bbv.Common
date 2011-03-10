//-------------------------------------------------------------------------------
// <copyright file="IMappingEventBrokerExtension.cs" company="bbv Software Services AG">
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

namespace bbv.Common.MappingEventBrokerExtension
{
    using System;

    using bbv.Common.EventBroker;
    using bbv.Common.EventBroker.Internals;

    /// <summary>
    /// Delegate which is called when a mapping could not be found.
    /// </summary>
    /// <param name="source">The source topic info.</param>
    /// <param name="destinationTopic">The destination topic.</param>
    /// <param name="publication">The publication.</param>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    public delegate void MissingMappingAction(IEventTopicInfo source, string destinationTopic, IPublication publication, object sender, EventArgs eventArgs);

    /// <summary>
    /// Interface which defines an auto mapper event broker extension.
    /// </summary>
    public interface IMappingEventBrokerExtension : IEventBrokerExtension, IManageEventBroker
    {
        /// <summary>
        /// Sets the missing mapping action which is called when no mapping was previously defined.
        /// </summary>
        /// <param name="action">The missing mapping action.</param>
        void SetMissingMappingAction(MissingMappingAction action);
    }
}