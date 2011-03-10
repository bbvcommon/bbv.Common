//-------------------------------------------------------------------------------
// <copyright file="InlineExpressionProviderTest.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Xunit;

    public class InlineExpressionProviderTest
    {
        private readonly InlineExpressionProvider<TestQuestion, string, string, string> testee;

        public InlineExpressionProviderTest()
        {
            this.testee = new InlineExpressionProvider<TestQuestion, string, string, string>((q, p) => q.Value + p);
        }

        [Fact]
        public void F()
        {
            IEnumerable<IExpression<string, string>> expressions = this.testee.GetExpressions(new TestQuestion { Value = "Q" });

            expressions
                .Should().HaveCount(1);

            expressions.ElementAt(0).Evaluate("P").Should().Be("QP", "question and parameter must be passed to inline expression.");
        }

        private class TestQuestion : Question<string, string>
        {
            public string Value { get; set; }
        }
    }
}