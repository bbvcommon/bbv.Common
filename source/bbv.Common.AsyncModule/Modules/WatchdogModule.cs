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
using bbv.Common.AsyncModule.Extensions;

namespace bbv.Common.AsyncModule.Modules
{
    /// <summary>
    /// The watch dog module checks periodically if the other 
    /// modules are still alive and restarts them if they die.
    /// The time the module tries to do this can be configured.
    /// </summary>
    public class WatchdogModule
    {
        /// <summary>
        /// A module is only restarted this number of times.
        /// </summary>
        private readonly int maxRestartCount;
        
        /// <summary>
        /// The restart count for each module.
        /// </summary>
        private readonly Dictionary<string, int> restartCounts;

        /// <summary>
        /// The watch dog accesses the other module controllers through 
        /// the coordinator to check if they are alive.
        /// </summary>
        private IModuleCoordinator moduleCoordinator;
                       
        /// <summary>
        /// The watch dog accesses the other module controllers through 
        /// the coordinator to check if they are alive.
        /// </summary>
        [ModuleCoordinator]
        public IModuleCoordinator ModuleCoordinator
        {
            set { moduleCoordinator = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxRestartCount">
        /// A module is only restarted this number of times.
        /// </param>
        public WatchdogModule(int maxRestartCount)
        {
            this.maxRestartCount = maxRestartCount;
            restartCounts = new Dictionary<string, int>();
        }

        /// <summary>
        /// The watch dog module has to be set up with the TimedTriggerExtension
        /// This method is called when a timed trigger event occurs.
        /// </summary>
        /// <param name="message">
        /// See above.
        /// </param>
        [MessageConsumer]
        public void ConsumeTimedTrigger(TimedTriggerMessage message)
        {
            // Check all modules.
            foreach (string moduleName in moduleCoordinator.ModuleControllers.Keys)
            {
                IModuleController moduleController = moduleCoordinator.ModuleControllers[moduleName];
                
                // Check if the module is alive.
                if (!moduleController.IsAlive)
                {
                    if (!restartCounts.ContainsKey(moduleName))
                    {
                        restartCounts.Add(moduleName, 0);
                    }

                    if (restartCounts[moduleName] < maxRestartCount)
                    {
                        // Restart module.
                        moduleController.Stop();
                        moduleController.Start();
                        restartCounts[moduleName]++;
                    }
                    else
                    {
                        throw new Exception(string.Format("Module {0} died and could not be restarted.", moduleName));
                    }
                }
            }
        }
    }
}
