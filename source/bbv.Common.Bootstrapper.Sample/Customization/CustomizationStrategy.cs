//-------------------------------------------------------------------------------
// <copyright file="CustomizationStrategy.cs" company="bbv Software Services AG">
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
    using bbv.Common.Bootstrapper.Sample.Complex;

    /// <summary>
    /// Strategy which inherits from <see cref="ComplexStrategy"/> but customizes the core infrastructure
    /// </summary>
    public class CustomizationStrategy : ComplexStrategy
    {
        /// <inheritdoc />
        /// <remarks>Creates a <see cref="CustomExtensionResolver"/></remarks>
        public override IExtensionResolver<IComplexExtension> CreateExtensionResolver()
        {
            return new CustomExtensionResolver();
        }

        /// <inheritdoc />
        /// <remarks>Creates a <see cref="AsynchronousRunExecutor"/></remarks>
        public override IExecutor<IComplexExtension> CreateRunExecutor()
        {
            return new AsynchronousRunExecutor();
        }

        /// <inheritdoc />
        /// <remarks>Creates a <see cref="AsynchronousShutdownExecutor"/></remarks>
        public override IExecutor<IComplexExtension> CreateShutdownExecutor()
        {
            return new AsynchronousShutdownExecutor();
        }
    }
}