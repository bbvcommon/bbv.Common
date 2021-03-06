//-------------------------------------------------------------------------------
// <copyright file="StaticSubscriberHandlerException.cs" company="bbv Software Services AG">
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

namespace bbv.Common.EventBroker.Exceptions
{
    using System.Reflection;

    /// <summary>
    /// An <see cref="EventBrokerException"/> thrown when a static subscription handler is found.
    /// </summary>
    public class StaticSubscriberHandlerException : EventBrokerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticSubscriberHandlerException"/> class.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        public StaticSubscriberHandlerException(MethodInfo methodInfo)
            : base("Subscriber handler must not be static: '{0}'", methodInfo.DeclaringType.FullName, methodInfo.Name)
        {
        }
    }
}
