//-------------------------------------------------------------------------------
// <copyright file="InlineExpressionTest.cs" company="bbv Software Services AG">
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
    using System.Linq.Expressions;
    using System.Reflection;

    using FluentAssertions;

    using Xunit;

    public class InlineExpressionTest
    {
        private const int Result = 3;

        private readonly InlineExpression<TestQuestion, string, int> testee;

        private readonly TestQuestion question;

        private readonly Expression<Func<TestQuestion, string, int>> inlineExpression;

        private TestQuestion receivedQuestion;

        private string receivedParameter;

        public InlineExpressionTest()
        {
            this.question = new TestQuestion();
            this.inlineExpression = (q, p) => this.StoreReceivedQuestionAndParameterAndReturnResult(q, p);

            this.testee = new InlineExpression<TestQuestion, string, int>(this.question, this.inlineExpression);
        }

        [Fact]
        public void Evaluate()
        {
            const string Parameter = "how_long_is_this_text";

            int answer = this.testee.Evaluate(Parameter);

            answer.Should().Be(Result);
            this.receivedQuestion.Should().BeSameAs(this.question);
            this.receivedParameter.Should().Be(Parameter);
        }

        [Fact]
        public void Describe()
        {
            string description = this.testee.Describe();

            description.Should().StartWith("inline expression = ");
        }

        private int StoreReceivedQuestionAndParameterAndReturnResult(TestQuestion question, string parameter)
        {
            this.receivedQuestion = question;
            this.receivedParameter = parameter;

            return Result;
        }

        private class TestQuestion : Question<string>
        {
        }
    }
}