//-------------------------------------------------------------------------------
// <copyright file="DisposeExtensionBehaviorTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Behavior
{
    using System.Collections.Generic;

    using bbv.Common.Bootstrapper.Dummies;

    using Moq;

    using Xunit;

    public class DisposeExtensionBehaviorTest
    {
        private readonly DisposeExtensionBehavior testee;

        public DisposeExtensionBehaviorTest()
        {
            this.testee = new DisposeExtensionBehavior();
        }

        [Fact]
        public void Behave_ShouldDisposeDisposableExtensions()
        {
            var notDisposableExtension = new Mock<INonDisposableExtension>();
            var disposableExtension = new Mock<IDisposableExtension>();

            this.testee.Behave(new List<IExtension> { notDisposableExtension.Object, disposableExtension.Object });

            notDisposableExtension.Verify(e => e.Dispose(), Times.Never());
            disposableExtension.Verify(e => e.Dispose(), Times.Once());
        }
    }
}