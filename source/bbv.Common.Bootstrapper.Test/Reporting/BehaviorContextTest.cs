//-------------------------------------------------------------------------------
// <copyright file="BehaviorContextTest.cs" company="bbv Software Services AG">
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
    using FluentAssertions;

    using Moq;

    using Xunit;

    public class BehaviorContextTest
    {
        private readonly Mock<IDescribable> describable;

        public BehaviorContextTest()
        {
            this.describable = new Mock<IDescribable>();
        }

        [Fact]
        public void Constructor_ShouldDescribe()
        {
            const string ExpectedDescription = "TestDescription";

            this.describable.Setup(d => d.Describe()).Returns(ExpectedDescription);

            BehaviorContext testee = CreateTestee(this.describable.Object);

            testee.Description.Should().Be(ExpectedDescription);
            this.describable.Verify(d => d.Describe());
        }

        private static BehaviorContext CreateTestee(IDescribable describable)
        {
            return new BehaviorContext(describable);
        }
    }
}