//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapper_is_run.cs" company="bbv Software Services AG">
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
    using System.Reflection;
    using System.Text;

    using bbv.Common.Bootstrapper.Syntax;

    using Machine.Specifications;

    public interface ICustomExtension : IExtension
    {
        void Start();

        void Configure(IDictionary<string, string> configuration);

        void Stop();
    }

    public class When_the_bootstrapper_is_run
    {
        private static IStrategy<ICustomExtension> Strategy;

        private static CustomExtensionBase First;

        private static CustomExtensionBase Second;

        private static IBootstrapper<ICustomExtension> Bootstrapper;

        Establish context = () =>
            {
                Bootstrapper = new DefaultBootstrapper<ICustomExtension>();

                Strategy = new CustomExtensionStrategy();
                First = new FirstExtension();
                Second = new SecondExtension();

                Bootstrapper.Initialize(Strategy);
                Bootstrapper.AddExtension(First);
                Bootstrapper.AddExtension(Second);
            };

        Because of = () =>
            {
                Bootstrapper.Run();
            };

        It should_execute_the_extensions_in_the_correct_order;

        It should_pass_the_initialized_values_to_the_extension;

        It should_execute_the_extension_point_according_to_the_strategy_defined_order;
    }

    public class CustomExtensionStrategy : AbstractStrategy<ICustomExtension>
    {
        protected override void DefineRunSyntax(ISyntaxBuilder<ICustomExtension> builder)
        {
            builder
                .Execute(extension => extension.Start())
                .Execute(InitializeConfiguration, (extension, dictionary) => extension.Configure(dictionary));
        }

        protected override void DefineShutdownSyntax(ISyntaxBuilder<ICustomExtension> syntax)
        {
            syntax
                .Execute(extension => extension.Stop());
        }

        private static IDictionary<string, string> InitializeConfiguration()
        {
            return new Dictionary<string, string> { { "Test", "TestValue" } };
        }
    }

    public class CustomExtensionBase : ICustomExtension 
    {
        private readonly StringBuilder stringBuilder;

        public CustomExtensionBase()
        {
            this.stringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        public void Start()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        public void Configure(IDictionary<string, string> configuration)
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        public void Stop()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        private void Dump(string methodName)
        {
            this.stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", this.GetType().Name, methodName));
        }
    }

    public class FirstExtension : CustomExtensionBase
    {
    }

    public class SecondExtension : CustomExtensionBase
    {
    }
}