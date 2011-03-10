//-------------------------------------------------------------------------------
// <copyright file="DefinitionHostTest.cs" company="bbv Software Services AG">
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
    using System;
    using System.Reflection;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class DefinitionHostTest
    {
        private readonly DefinitionHost testee;

        public DefinitionHostTest()
        {
            this.testee = new DefinitionHost();
        }

        [Fact]
        public void FindDefinitionWhenNoMatchingDefinitionWasAddedThenNullIsReturned()
        {
            this.AddSomeNonMatchingDefinitions();

            var definition = this.testee.FindDefinition<string>(typeof(TestQuestion));

            definition.Should().BeNull();
        }

        [Fact]
        public void FindDefinitionWhenMatchingDefinitionWasAddedThenItIsReturned()
        {
            this.AddSomeNonMatchingDefinitions();
            var definition = AddDefinition<TestQuestion>();

            var returnedDefinition = this.testee.FindDefinition<string>(typeof(TestQuestion));

            returnedDefinition.Should().BeSameAs(definition);
        }

        [Fact]
        public void FindInHierarchyWhenMatchingDefinitionThenReturnsClone()
        {
            var definition = this.AddDefinition<TestQuestion>();

            var result = this.testee.FindInHierarchyAndCloneDefinition(new TestQuestion());

            result
                .ShouldHave().AllProperties().EqualTo(definition);

            result
                .Should().NotBeSameAs(definition, "a clone should be returned so that the original object cannot be changed.");
        }

        [Fact]
        public void FindInHierarchyWhenNoMatchingDefinitionThenReturnsNull()
        {
            var result = this.testee.FindInHierarchyAndCloneDefinition(new TestQuestion());

            result.Should().BeNull();
        }

        [Fact]
        public void AddDefinitionWhenADefinitionForTheSameQuestionTypeIsAlreadyPresentThenAnExceptionIsThrown()
        {
            var definition1 = new Definition<TestQuestion, string, Missing, string>();
            var definition2 = new Definition<TestQuestion, string, Missing, string>();

            this.testee.AddDefinition(definition1);

            Action action = () => this.testee.AddDefinition(definition2);

            action.ShouldThrow<InvalidOperationException>();
        }

        private Definition<TQuestion, string, Missing, string> AddDefinition<TQuestion>() where TQuestion : IQuestion<string>
        {
            var definition = new Definition<TQuestion, string, Missing, string>
                {
                    Strategy = new Mock<IStrategy<string, Missing>>().Object,
                    Aggregator = new Mock<IAggregator<string, string, Missing>>().Object
                };

            this.testee.AddDefinition(definition);
            
            return definition;
        }

        private void AddSomeNonMatchingDefinitions()
        {
            this.AddDefinition<AnotherQuestion>();
            this.AddDefinition<YetAnotherQuestion>();
        }

        private class TestQuestion : Question<string>
        {
        }

        private class AnotherQuestion : Question<string>
        {
        }

        private class YetAnotherQuestion : Question<string>
        {
        }
    }
}