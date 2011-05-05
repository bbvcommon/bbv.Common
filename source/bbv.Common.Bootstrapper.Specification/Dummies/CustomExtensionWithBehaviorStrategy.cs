//-------------------------------------------------------------------------------
// <copyright file="CustomExtensionWithBehaviorStrategy.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Specification.Dummies
{
    using System.Collections.Generic;

    using bbv.Common.Bootstrapper.Syntax;

    public class CustomExtensionWithBehaviorStrategy : AbstractStrategy<ICustomExtension>
    {
        public int ConfigurationInitializerAccessCounter
        {
            get;
            private set;
        }

        protected override void DefineRunSyntax(ISyntaxBuilder<ICustomExtension> builder)
        {
            builder
                    .With(new Behavior("first beginning"))
                    .With(new Behavior("second beginning"))
                .Execute(extension => extension.Start())
                    .With(new Behavior("first start"))
                    .With(new Behavior("second start"))
                .Execute(this.InitializeConfiguration, (extension, dictionary) => extension.Configure(dictionary))
                    .With(dictionary => new BehaviorWithConfigurationContext(dictionary, "FirstValue", "TestValue"))
                    .With(dictionary => new BehaviorWithConfigurationContext(dictionary, "SecondValue", "TestValue"))
                .Execute(extension => extension.Initialize())
                    .With(new Behavior("first initialize"))
                    .With(new Behavior("second initialize"))
                .Execute(() => "Test", (extension, context) => extension.Inject(context))
                    .With(context => new BehaviorWithStringContext(context, "TestValue"))
                    .With(context => new BehaviorWithStringContext(context, "TestValue"));
        }

        protected override void DefineShutdownSyntax(ISyntaxBuilder<ICustomExtension> syntax)
        {
            syntax
                .Execute(extension => extension.Stop());
        }

        private IDictionary<string, string> InitializeConfiguration()
        {
            this.ConfigurationInitializerAccessCounter++;

            return new Dictionary<string, string> { { "Test", "TestValue" } };
        }
    }
}