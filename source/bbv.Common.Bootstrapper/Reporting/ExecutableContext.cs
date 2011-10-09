//-------------------------------------------------------------------------------
// <copyright file="ExecutableContext.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Reporting
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class ExecutableContext : IExecutableContext
    {
        private ICollection<IBehaviorContext> behaviors;

        public ExecutableContext(IDescribable describable)
        {
            Ensure.ArgumentNotNull(describable, "describable");

            this.behaviors = new Collection<IBehaviorContext>();

            this.Name = describable.Name;
            this.Description = describable.Describe();
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<IBehaviorContext> Behaviors
        {
            get
            {
                return this.behaviors;
            }
        }

        public IBehaviorContext CreateBehaviorContext(IDescribable describable)
        {
            var behaviorInfo = new BehaviorContext(describable);

            this.behaviors.Add(behaviorInfo);

            return behaviorInfo;
        }
    }
}