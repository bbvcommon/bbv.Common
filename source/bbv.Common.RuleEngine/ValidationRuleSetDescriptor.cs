//-------------------------------------------------------------------------------
// <copyright file="ValidationRuleSetDescriptor.cs" company="bbv Software Services AG">
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

namespace bbv.Common.RuleEngine
{
    /// <summary>
    /// A rule set descriptor for validation rules.
    /// </summary>
    public class ValidationRuleSetDescriptor : IValidationRuleSetDescriptor
    {
        /// <summary>The validation factory.</summary>
        private readonly IValidationFactory factory;

        /// <summary>The validation aggregator.</summary>
        private readonly IAggregator<IValidationRule, IValidationResult> aggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleSetDescriptor"/> class
        /// that does not stop on the first violation, all rules are evaluated.
        /// </summary>
        public ValidationRuleSetDescriptor()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleSetDescriptor"/> class.
        /// </summary>
        /// <param name="breakOnFirstViolation">if set to <c>true</c> the aggregation of rules is 
        /// stopped when the first violation occurs, all remaining rules are not evaluated.].</param>
        public ValidationRuleSetDescriptor(bool breakOnFirstViolation)
        {
            this.factory = new ValidationFactory();
            this.aggregator = new ValidationAggregator(this.factory, breakOnFirstViolation);
        }

        /// <summary>
        /// Gets the factory used to create needed instances of rule engine related classes.
        /// </summary>
        /// <value>The factory.</value>
        public IValidationFactory Factory
        {
            get { return this.factory; }
        }

        /// <summary>
        /// Gets the factory used to create needed instances of rule engine related classes.
        /// </summary>
        /// <value>The factory.</value>
        IFactory<IValidationRule> IRuleSetDescriptor<IValidationRule, IValidationResult>.Factory
        {
            get { return this.Factory; }
        }

        /// <summary>
        /// Gets the aggregator that is used to combine all rules taking part in the evaluation.
        /// </summary>
        /// <value>The aggregator.</value>
        public IAggregator<IValidationRule, IValidationResult> Aggregator
        {
            get { return this.aggregator; }
        }
    }
}