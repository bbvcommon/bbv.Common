//-------------------------------------------------------------------------------
// <copyright file="IReportingContext.cs" company="bbv Software Services AG">
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

    public interface IReportingContext : IRunExecutionContextFactory, IShutdownExecutionContextFactory, IExtensionContextFactory
    {
        IExecutionContext Run { get; }

        IExecutionContext Shutdown { get; }

        IEnumerable<IExtensionContext> Extensions { get; }
    }

    public interface IExecutableContextFactory
    {
        IExecutableContext CreateExecutableContext(IDescribable describable);
    }

    public interface IRunExecutionContextFactory
    {
        IExecutionContext CreateRunExecutionContext(IDescribable describable);
    }

    public interface IShutdownExecutionContextFactory
    {
        IExecutionContext CreateShutdownExecutionContext(IDescribable describable);
    }

    public interface IExtensionContextFactory
    {
        IExtensionContext CreateExtensionContext(IDescribable describable);
    }

    public interface IExtensionContext
    {
        string Name { get; }

        string Description { get; }
    }

    public interface IExecutionContext : IExecutableContextFactory
    {
        string Name { get; }

        string Description { get; }

        IEnumerable<IExecutableContext> Executables { get; }
    }

    public interface IExecutableContext : IBehaviorContextFactory
    {
        string Name { get; }

        string Description { get; }

        IEnumerable<IBehaviorContext> Behaviors { get; }
    }

    public interface IBehaviorContextFactory
    {
        IBehaviorContext CreateBehaviorContext(IDescribable describable);
    }

    public interface IBehaviorContext
    {
        string Name { get; }

        string Description { get; }
    }

    public interface IDescribable
    {
        string Name { get; }

        string Describe();
    }
}