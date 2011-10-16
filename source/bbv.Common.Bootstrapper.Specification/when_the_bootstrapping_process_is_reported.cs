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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using bbv.Common.Bootstrapper.Reporting;
    using bbv.Common.Bootstrapper.Specification.Helpers;
    using FluentAssertions;
    using Machine.Specifications;
    using Machine.Specifications.Runner.Impl;
    using Xunit;

    [Subject(Concern)]
    public class when_the_bootstrapping_process_is_reported : BootstrapperReportingSpecification
    {
        private static StringReporter ExpectedContextReporter;
        private static StringReporter InterceptingContextReporter;

        Establish context = () =>
        {
            ExpectedContextReporter = new StringReporter();
            InterceptingContextReporter = new StringReporter();

            Bootstrapper.Initialize(Strategy);
            Bootstrapper.AddExtension(First);
            Bootstrapper.AddExtension(Second);

            RegisterReporter(InterceptingContextReporter);
        };

        Because of = () =>
        {
            Bootstrapper.Run();
            Bootstrapper.Shutdown();
            Bootstrapper.Dispose();
        };

        It should_report_complete_bootstrapping_process = () =>
            {
                Assert.True(true);

                const string ActionExecutableCustomExtension = "bbv.Common.Bootstrapper.Syntax.Executables.ActionExecutable<bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string ActionOnExtensionExecutableCustomExtension = "bbv.Common.Bootstrapper.Syntax.Executables.ActionOnExtensionExecutable<bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string ActionExecutableWithDictionaryContextCustomExtension = "bbv.Common.Bootstrapper.Syntax.Executables.ActionOnExtensionWithInitializerExecutable<System.Collections.Generic.IDictionary<System.String,System.String>,bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string ActionExecutableWithStringContextCustomExtension = "bbv.Common.Bootstrapper.Syntax.Executables.ActionOnExtensionWithInitializerExecutable<System.String,bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>";

                const string BehaviorCustomExtension = "bbv.Common.Bootstrapper.Specification.Dummies.Behavior";
                const string LazyBehaviorCustomExtension = "bbv.Common.Bootstrapper.Behavior.LazyBehavior<bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>";
                const string BehaviorWithConfigurationContextCustomExtesion = "bbv.Common.Bootstrapper.Specification.Dummies.BehaviorWithConfigurationContext";
                const string BehaviorWithStringContextCustomExtesion = "bbv.Common.Bootstrapper.Specification.Dummies.BehaviorWithStringContext";

                var expectedContext = ReportingContextBuilder.Create()
                    .Extension("bbv.Common.Bootstrapper.Specification.Dummies.FirstExtension", "First Extension")
                    .Extension("bbv.Common.Bootstrapper.Specification.Dummies.SecondExtension", "Second Extension")
                    .Run("bbv.Common.Bootstrapper.Execution.SynchronousExecutor<bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>", "Runs all executables synchronously on the extensions in the order which they were added.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.BeginWith)\" during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, "Behaves on all extensions by dumping \"run first beginning\" on the extensions.")
                            .Behavior(LazyBehaviorCustomExtension, "Behaves by creating the behavior with () => new Behavior(\"run second beginning\") and executing behave on the lazy initialized behavior.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => DumpAction(\"CustomRun\")\" during bootstrapping.")
                        .Executable(ActionOnExtensionExecutableCustomExtension, "Executes \"extension => extension.Start()\" on each extension during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, "Behaves on all extensions by dumping \"run first start\" on the extensions.")
                            .Behavior(LazyBehaviorCustomExtension, "Behaves by creating the behavior with () => new Behavior(\"run second start\") and executing behave on the lazy initialized behavior.")
                        .Executable(ActionExecutableWithDictionaryContextCustomExtension, "Initializes the context once with \"() => value(bbv.Common.Bootstrapper.Specification.Dummies.CustomExtensionWithBehaviorStrategy).RunInitializeConfiguration()\" and executes \"(extension, dictionary) => extension.Configure(dictionary)\" on each extension during bootstrapping.")
                            .Behavior(BehaviorWithConfigurationContextCustomExtesion, "Behaves on all extensions by dumping the key \"RunFirstValue\" and value \"RunTestValue\" and modifying the configuration with it.")
                            .Behavior(BehaviorWithConfigurationContextCustomExtesion, "Behaves on all extensions by dumping the key \"RunSecondValue\" and value \"RunTestValue\" and modifying the configuration with it.")
                        .Executable(ActionOnExtensionExecutableCustomExtension, "Executes \"extension => extension.Initialize()\" on each extension during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, "Behaves on all extensions by dumping \"run first initialize\" on the extensions.")
                            .Behavior(LazyBehaviorCustomExtension, "Behaves by creating the behavior with () => new Behavior(\"run second initialize\") and executing behave on the lazy initialized behavior.")
                        .Executable(ActionExecutableWithStringContextCustomExtension, "Initializes the context once with \"() => \"RunTest\"\" and executes \"(extension, context) => extension.Register(context)\" on each extension during bootstrapping.")
                            .Behavior(BehaviorWithStringContextCustomExtesion, "Behaves on all extensions by dumping \"RunTestValueFirst\" on the extensions.")
                            .Behavior(BehaviorWithStringContextCustomExtesion, "Behaves on all extensions by dumping \"RunTestValueSecond\" on the extensions.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.EndWith)\" during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, "Behaves on all extensions by dumping \"run first end\" on the extensions.")
                            .Behavior(LazyBehaviorCustomExtension, "Behaves by creating the behavior with () => new Behavior(\"run second end\") and executing behave on the lazy initialized behavior.")
                    .Shutdown("bbv.Common.Bootstrapper.Execution.SynchronousReverseExecutor<bbv.Common.Bootstrapper.Specification.Dummies.ICustomExtension>", "Runs all executables synchronously on the extensions in the reverse order which they were added.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.BeginWith)\" during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, "Behaves on all extensions by dumping \"shutdown first beginning\" on the extensions.")
                            .Behavior(LazyBehaviorCustomExtension, "Behaves by creating the behavior with () => new Behavior(\"shutdown second beginning\") and executing behave on the lazy initialized behavior.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => DumpAction(\"CustomShutdown\")\" during bootstrapping.")
                        .Executable(ActionExecutableWithStringContextCustomExtension, "Initializes the context once with \"() => \"ShutdownTest\"\" and executes \"(extension, ctx) => extension.Unregister(ctx)\" on each extension during bootstrapping.")
                            .Behavior(BehaviorWithStringContextCustomExtesion, "Behaves on all extensions by dumping \"ShutdownTestValueFirst\" on the extensions.")
                            .Behavior(BehaviorWithStringContextCustomExtesion, "Behaves on all extensions by dumping \"ShutdownTestValueSecond\" on the extensions.")
                        .Executable(ActionExecutableWithDictionaryContextCustomExtension, "Initializes the context once with \"() => value(bbv.Common.Bootstrapper.Specification.Dummies.CustomExtensionWithBehaviorStrategy).ShutdownInitializeConfiguration()\" and executes \"(extension, dictionary) => extension.DeConfigure(dictionary)\" on each extension during bootstrapping.")
                            .Behavior(BehaviorWithConfigurationContextCustomExtesion, "Behaves on all extensions by dumping the key \"ShutdownFirstValue\" and value \"ShutdownTestValue\" and modifying the configuration with it.")
                            .Behavior(BehaviorWithConfigurationContextCustomExtesion, "Behaves on all extensions by dumping the key \"ShutdownSecondValue\" and value \"ShutdownTestValue\" and modifying the configuration with it.")
                        .Executable(ActionOnExtensionExecutableCustomExtension, "Executes \"extension => extension.Stop()\" on each extension during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, "Behaves on all extensions by dumping \"shutdown first stop\" on the extensions.")
                            .Behavior(LazyBehaviorCustomExtension, "Behaves by creating the behavior with () => new Behavior(\"shutdown second stop\") and executing behave on the lazy initialized behavior.")
                        .Executable(ActionExecutableCustomExtension, "Executes \"() => Invoke(SyntaxBuilder`1.EndWith)\" during bootstrapping.")
                            .Behavior(BehaviorCustomExtension, "Behaves on all extensions by dumping \"shutdown first end\" on the extensions.")
                            .Behavior(LazyBehaviorCustomExtension, "Behaves by creating the behavior with () => new Behavior(\"shutdown second end\") and executing behave on the lazy initialized behavior.")
                            .Behavior("bbv.Common.Bootstrapper.Behavior.DisposeExtensionBehavior", "Behaves on all extensions by checking whether they implement IDisposable and disposing them if this is the case.")
                    .Build();

                ExpectedContextReporter.Report(expectedContext);

                InterceptingContextReporter.ToString().Should().Be(ExpectedContextReporter.ToString());
            };

        private class StringReporter : IReporter
        {
            private IReportingContext context;

            public void Report(IReportingContext context)
            {
                this.context = context;
            }

            public override string ToString()
            {
                return Dump(this.context);
            }

            private static string Dump(IReportingContext context)
            {
                var builder = new StringBuilder();

                context.Extensions.ForEach(e => Dump(e.Name, e.Description, builder, 0));

                Dump(context.Run, builder);
                Dump(context.Shutdown, builder);

                return builder.ToString();
            }

            private static void Dump(IExecutionContext executionContext, StringBuilder sb)
            {
                Dump(executionContext.Name, executionContext.Description, sb, 3);

                Dump(executionContext.Executables, sb);
            }

            private static void Dump(IEnumerable<IExecutableContext> executableContexts, StringBuilder sb)
            {
                foreach (IExecutableContext executableContext in executableContexts)
                {
                    Dump(executableContext.Name, executableContext.Description, sb, 6);

                    executableContext.Behaviors.ForEach(b => Dump(b.Name, b.Description, sb, 9));
                }
            }

            private static void Dump(string name, string description, StringBuilder sb, int indent)
            {
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}[Name = {1}, Description = {2}]", string.Empty.PadLeft(indent), name, description));
            }
        }
    }
}