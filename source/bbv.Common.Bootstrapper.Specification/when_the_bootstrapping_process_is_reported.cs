//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapping_process_is_reported.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Specification
{
    using bbv.Common.Bootstrapper.Specification.Helpers;
    using FluentAssertions;
    using Machine.Specifications;

    [Subject(Concern)]
    public class when_the_bootstrapping_process_is_reported : BootstrapperReportingSpecification
    {
        Establish context = () =>
        {
            Bootstrapper.Initialize(Strategy);
            Bootstrapper.AddExtension(First);
            Bootstrapper.AddExtension(Second);
        };

        Because of = () =>
        {
            Bootstrapper.Run();
            Bootstrapper.Shutdown();
            Bootstrapper.Dispose();
        };

        It should_report_complete_bootstrapping_process = () =>
            {
                const string ActionExecutableCustomExtension = "bbv.Common.Bootstrapper.Syntax.Executables.ActionExecutable<bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string ActionExecutableWithDictionaryContextCustomExtension = "bbv.Common.Bootstrapper.Syntax.Executables.ActionOnExtensionWithInitializerExecutable<System.Collections.Generic.IDictionary<System.String,System.String>,bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string ActionExecutableWithStringContextCustomExtension = "bbv.Common.Bootstrapper.Syntax.Executables.ActionOnExtensionWithInitializerExecutable<System.String,bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>";

                var context = ReportingContextBuilder.Create()
                    .Extension("bbv.Common.Bootstrapper.Specification.Dummies.FirstExtension", "FirstExtension")
                    .Extension("bbv.Common.Bootstrapper.Specification.Dummies.SecondExtension", "SecondExtension")
                    .Run("bbv.Common.Bootstrapper.Execution.SynchronousExecutor<bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>", "Runs all executables synchronously on the extensions in the order which they were added.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.BeginWith)\" during bootstrapping.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => DumpAction(\"CustomRun\")\" during bootstrapping.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"extension => extension.Start()\" on each extension during bootstrapping.")
                        .Executable(ActionExecutableWithDictionaryContextCustomExtension, "Initializes the context once with \"() => value(bbv.Common.Bootstrapper.Specification.Dummies.CustomExtensionWithBehaviorStrategy).RunInitializeConfiguration()\" and executes \"(extension, dictionary) => extension.Configure(dictionary)\" on each extension during bootstrapping.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"extension => extension.Initialize()\" on each extension during bootstrapping.")
                        .Executable(ActionExecutableWithStringContextCustomExtension, "Initializes the context once with \"() => \"RunTest\"\" and executes \"(extension, context) => extension.Register(context)\" on each extension during bootstrapping.")
                    .Shutdown("bbv.Common.Bootstrapper.Execution.SynchronousReverseExecutor<bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>", "Runs all executables synchronously on the extensions in the reverse order which they were added.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.BeginWith)\" during bootstrapping.")
                    .Build();

                ReportingContext.Should().NotBeNull();
            };
    }
}