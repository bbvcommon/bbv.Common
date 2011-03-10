//-------------------------------------------------------------------------------
// <copyright file="HierarchicalDefinitionHostTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.EvaluationEngine.Internals
{
    using System.Reflection;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class HierarchicalDefinitionHostTest
    {
        private readonly DefinitionHost testee;
        private readonly DefinitionHost parent;

        public HierarchicalDefinitionHostTest()
        {
            this.parent = new DefinitionHost();
            this.testee = new DefinitionHost(this.parent);
        }

        [Fact]
        public void FindInHierarchyWhenMatchingDefinitionInParentAndNoMatchingDefinitionInTesteeThenReturnsCloneOfDefinitionOfParent()
        {
            var definitionOfParent = CreateDefinition();
            this.parent.AddDefinition(definitionOfParent);

            var definition = this.testee.FindInHierarchyAndCloneDefinition(new TestQuestion());

            AssertThatItIsAClone(definitionOfParent, definition);
        }

        [Fact]
        public void FindInHierarchyWhenNoMatchingDefinitionInParentAndNoMatchingDefinitionInTesteeThenNullIsReturned()
        {
            var definition = this.testee.FindInHierarchyAndCloneDefinition(new TestQuestion());

            definition.Should().BeNull();
        }

        [Fact]
        public void FindInHierarchyWhenNoMatchingDefinitionInParentAndMatchingDefinitionInTesteeThenReturnsCloneOfDefinitionOfTestee()
        {
            var definitionOfTestee = CreateDefinition();
            this.testee.AddDefinition(definitionOfTestee);

            var definition = this.testee.FindInHierarchyAndCloneDefinition(new TestQuestion());
            
            AssertThatItIsAClone(definitionOfTestee, definition);
        }

        [Fact]
        public void FindInHierarchyWhenMatchingDefinitionInParentAndMatchingDefinitionInTesteeThenReturnsMergedAndClonedDefinition()
        {
            var definitionOfParent = CreateDefinition();
            this.parent.AddDefinition(definitionOfParent);

            var definitionOfTestee = CreateDefinition();
            this.testee.AddDefinition(definitionOfTestee);

            var definition = this.testee.FindInHierarchyAndCloneDefinition(new TestQuestion());

            AssertThatItIsAClone(definitionOfTestee, definition);
        }

        private static void AssertThatItIsAClone(Definition<TestQuestion, string, Missing, string> originalDefinition, IDefinition actualDefinition)
        {
            actualDefinition
                .ShouldHave().AllProperties().EqualTo(originalDefinition);

            actualDefinition
                .Should().NotBeSameAs(originalDefinition, "a clone is expected so that original definition cannot be modified.");
        }

        private static Definition<TestQuestion, string, Missing, string> CreateDefinition()
        {
            var definition = new Definition<TestQuestion, string, Missing, string>
                {
                    Strategy = new Mock<IStrategy<string, Missing>>().Object,
                    Aggregator = new Mock<IAggregator<string, string, Missing>>().Object
                };

            return definition;
        }

        private class TestQuestion : Question<string>
        {
        }
    }
}