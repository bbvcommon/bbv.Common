//-------------------------------------------------------------------------------
// <copyright file="Context.cs" company="bbv Software Services AG">
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

    /// <summary>
    /// Contains all information needed for logging.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        public Context()
        {
            this.Expressions = new List<ExpressionInfo>();
        }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>The question.</value>
        public IDescriptionProvider Question { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        /// <value>The answer.</value>
        public object Answer { get; set; }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public IDescriptionProvider Strategy { get; set; }

        /// <summary>
        /// Gets or sets the aggregator.
        /// </summary>
        /// <value>The aggregator.</value>
        public IDescriptionProvider Aggregator { get; set; }

        /// <summary>
        /// Gets or sets the parameter passed along with the question.
        /// </summary>
        /// <value>The parameter.</value>
        public object Parameter { get; set; }

        /// <summary>
        /// Gets or sets the expressions.
        /// </summary>
        /// <value>The expressions.</value>
        public IList<ExpressionInfo> Expressions { get; set; }

        /// <summary>
        /// Combines an expression with the result that it returned.
        /// </summary>
        public class ExpressionInfo
        {
            /// <summary>
            /// Gets or sets the expression.
            /// </summary>
            /// <value>The expression.</value>
            public IDescriptionProvider Expression { get; set; }

            /// <summary>
            /// Gets or sets the expression result.
            /// </summary>
            /// <value>The expression result.</value>
            public object ExpressionResult { get; set; }
        }
    }
}