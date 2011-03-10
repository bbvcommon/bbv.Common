//-------------------------------------------------------------------------------
// <copyright file="EngineSolveTest.cs" company="bbv Software Services AG">
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
    using bbv.Common.EvaluationEngine.Syntax;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class EngineSolveTest
    {
        private readonly Engine testee;

        private readonly Mock<IDefinitionHost> definitionHostMock;
        private readonly Mock<IDefinitionSyntaxFactory> syntaxFactoryMock;
        private readonly Mock<IDefinitionFactory> definitionFactoryMock;

        public EngineSolveTest()
        {
            this.definitionHostMock = new Mock<IDefinitionHost>();
            this.syntaxFactoryMock = new Mock<IDefinitionSyntaxFactory>(MockBehavior.Strict);
            this.definitionFactoryMock = new Mock<IDefinitionFactory>();

            this.testee = new Engine(this.definitionHostMock.Object, this.syntaxFactoryMock.Object, this.definitionFactoryMock.Object);
        }

        [Fact]
        public void SolveWhenThereExistsAlreadyADefinitionForTheQuestionThenTheDefinitionIsPassedToTheBuilder()
        {
            var builderMock = new Mock<IDefinitionSyntax<TestQuestion, string, int, string>>();
            var definition = new Definition<TestQuestion, string, int, string>();

            this.definitionHostMock.Setup(host => host.FindDefinition<string>(typeof(TestQuestion))).Returns(definition);
            this.syntaxFactoryMock.Setup(factory => factory.CreateDefinitionSyntax(definition)).Returns(builderMock.Object);
            
            var builder = this.testee.Solve<TestQuestion, string, int>();

            builder.Should().BeSameAs(builderMock.Object);
        }

        [Fact]
        public void SolveWhenThereDoesNotExistADefinitionForTheQuestionThenANewDefinitionIsPassedToTheBuilderAndStoredOnTheHost()
        {
            var builderMock = new Mock<IDefinitionSyntax<TestQuestion, string, int, string>>();
            var definition = new Definition<TestQuestion, string, int, string>();

            this.definitionHostMock.Setup(host => host.FindDefinition<string>(typeof(TestQuestion))).Returns((IDefinition)null);
            this.definitionFactoryMock.Setup(factory => factory.CreateDefinition<TestQuestion, string, int, string>())
                .Returns(definition);
            this.syntaxFactoryMock
                .Setup(factory => factory.CreateDefinitionSyntax(definition))
                .Returns(() => builderMock.Object);

            this.testee.Solve<TestQuestion, string, int>();

            this.definitionHostMock.Verify(host => host.AddDefinition(definition));
        }

        public class TestQuestion : Question<string, int>
        {
        }
    }
}