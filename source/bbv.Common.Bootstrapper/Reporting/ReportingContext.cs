//-------------------------------------------------------------------------------
// <copyright file="ReportingContext.cs" company="bbv Software Services AG">
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

    public class ReportingContext : IReportingContext
    {
        private Collection<IExtensionContext> extensions;

        public ReportingContext()
        {
            this.extensions = new Collection<IExtensionContext>();
        }

        public IExecutionContext CreateRunExecutionContext(IDescribable describable)
        {
            this.Run = new ExecutionContext(describable);
            return this.Run;
        }

        public IExecutionContext CreateShutdownExecutionContext(IDescribable describable)
        {
            this.Shutdown = new ExecutionContext(describable);
            return this.Shutdown;
        }

        public IExecutionContext Run { get; private set; }

        public IExecutionContext Shutdown { get; private set; }

        public IEnumerable<IExtensionContext> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        public IExtensionContext CreateExtensionContext(IDescribable describable)
        {
            var extensionInfo = new ExtensionContext(describable);
            this.extensions.Add(extensionInfo);
            return extensionInfo;
        }
    }
}