//-------------------------------------------------------------------------------
// <copyright file="CustomExtensionStrategy.cs" company="bbv Software Services AG">
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
}