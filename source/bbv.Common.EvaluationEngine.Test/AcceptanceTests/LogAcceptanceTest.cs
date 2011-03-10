//-------------------------------------------------------------------------------
// <copyright file="LogAcceptanceTest.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2010 bbv Software Services AG
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

namespace bbv.Common.EvaluationEngine.AcceptanceTests
{
    using System;

    using bbv.Common.EvaluationEngine.Extensions;
    using bbv.Common.TestUtilities;

    using Xunit;

    public class LogAcceptanceTest
    {
        private readonly Log4netHelper log4Net;

        public LogAcceptanceTest()
        {
            this.log4Net = new Log4netHelper();
        }

        [Fact]
        public void Log()
        {
            var engine = new EvaluationEngine();

            engine.SetLogExtension(new Log4NetExtension());

            engine.Solve<TestQuestion, string>()
                .AggregateWithExpressionAggregator(string.Empty, (aggregate, value) => aggregate + value)
                .ByEvaluating(question => new TestExpression("one"))
                .ByEvaluating(question => new TestExpression("two"));

            engine.Answer(new TestQuestion());

            this.log4Net.DumpAllMessagesToConsole();
        }

        private class TestQuestion : IQuestion<string>
        {
            public string Describe()
            {
                return "What is the answer to the test question?";
            }
        }

        private class TestExpression : NoParameterExpression<string>
        {
            private readonly string value;

            public TestExpression(string value)
            {
                this.value = value;
            }

            public override string Describe()
            {
                return "Test expression with value " + this.value;
            }

            protected override string Evaluate()
            {
                return this.value;
            }
        }
    }
}