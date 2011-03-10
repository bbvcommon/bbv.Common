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
// File:        MockModuleImplementingInterface.cs
// Author:      Adrian Krummenacher / AK
//
// History:
// --------
// Date          Name   Version   Description
// 10-AUG-2006   AK		1.0       Creation     
//
/***************************************************************************/

using System;
using System.Collections.Generic;

namespace bbv.Common.AsyncModule
{
    /// <summary>
    /// Mock module to test a module, which implements IAsyncModule.
    /// </summary>
    public class MockModuleImplementingInterface : IAsyncModule
    {
        /// <summary>
        /// Implementation of IAsyncModule.
        /// </summary>
        private IModuleCoordinator m_moduleCoordinator;

        /// <summary>
        /// Implementation of IAsyncModule.
        /// </summary>
        public IModuleCoordinator ModuleCoordinator
        {
            get { return(m_moduleCoordinator); }
            set { m_moduleCoordinator = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MockModuleImplementingInterface()
        {
            ReceivedMessages = new List<object>();
        }

        /// <summary>
        /// List of messages received by the mock module.
        /// </summary>
        public List<object> ReceivedMessages;

        /// <summary>
        /// Implementation of IAsyncModule.
        /// </summary>
        public void ConsumeMessage(object message)
        {
            ReceivedMessages.Add(message);
            if (message is Exception)
            {
                Exception anException = (Exception)message;
                throw (anException);
            }
        }
    }
}
