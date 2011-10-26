//-------------------------------------------------------------------------------
// <copyright file="AsynchronousRunExecutor.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Sample.Customization
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using bbv.Common.Bootstrapper.Reporting;
    using bbv.Common.Bootstrapper.Sample.Complex;
    using bbv.Common.Bootstrapper.Syntax;
    using bbv.Common.Formatters;

    /// <summary>
    /// Very naive implementation of an asynchronous executor.
    /// </summary>
    public class AsynchronousRunExecutor : IExecutor<IComplexExtension>
    {
        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.GetType().FullNameToString();
            }
        }

        /// <inheritdoc />
        public string Describe()
        {
            return "Runs all executables asynchronously on the extensions in the order which they were added.";
        }

        /// <inheritdoc />
        public void Execute(ISyntax<IComplexExtension> syntax, IEnumerable<IComplexExtension> extensions, IExecutionContext executionContext)
        {
            Ensure.ArgumentNotNull(syntax, "syntax");

            foreach (IExecutable<IComplexExtension> executable in syntax)
            {
                using (var worker = new Task(
                    state =>
                    {
                        var e = (IExecutable<IComplexExtension>)state;
                        IExecutableContext executableContext = executionContext.CreateExecutableContext(e);

                        e.Execute(extensions, executableContext);
                    },
                    executable))
                {
                    worker.Start();
                    worker.Wait();
                }
            }
        }
    }
}