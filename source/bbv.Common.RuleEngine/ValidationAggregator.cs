//-------------------------------------------------------------------------------
// <copyright file="ValidationAggregator.cs" company="bbv Software Services AG">
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
    using System.Reflection;
    using System.Text;
    using Formatters;
    using log4net;

    /// <summary>
    /// The <see cref="ValidationAggregator"/> is an aggregator that combines the result of <see cref="IValidationRule"/>s
    /// into a single <see cref="IValidationResult"/>.
    /// The result is valid if all evaluated rules are valid and the list of violations of the result is the sum of all violations of all 
    /// evaluated rules.
    /// </summary>
    public class ValidationAggregator : IAggregator<IValidationRule, IValidationResult>
    {
        /// <summary>The logger for the validation aggregator.</summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>The validation factory used to create needed instances.</summary>
        private readonly IValidationFactory validationFactory;

        /// <summary>Indicates whether the evaluation of rules should be stopped on the first violation.</summary>
        private readonly bool breakOnFirstViolation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAggregator"/> class.
        /// </summary>
        /// <param name="validationFactory">The validation factory.</param>
        /// <param name="breakOnFirstViolation">if set to <c>true</c> [break on first violation].</param>
        public ValidationAggregator(IValidationFactory validationFactory, bool breakOnFirstViolation)
        {
            this.validationFactory = validationFactory;
            this.breakOnFirstViolation = breakOnFirstViolation;
        }

        /// <summary>
        /// Aggregates the specified rule set.
        /// The result is valid if all rules are valid and it contains all violations of all rules.
        /// </summary>
        /// <param name="ruleSet">The rule set.</param>
        /// <param name="logInfo">The log info. The aggregator should provide information about the results of the different rules and how they
        /// influenced the overall result. This info is written to the log by the rule engine.</param>
        /// <returns>
        /// The aggregated result of all rules taking part in the evaluation.
        /// </returns>
        public IValidationResult Aggregate(IRuleSet<IValidationRule> ruleSet, out string logInfo)
        {
            StringBuilder sb = new StringBuilder();

            IValidationResult aggregatedResults = this.validationFactory.CreateValidationResult(true);

            foreach (IValidationRule rule in ruleSet)
            {
                IValidationResult result = rule.Evaluate();

                aggregatedResults.Valid &= result.Valid;

                if (!result.Valid && result.Violations != null && result.Violations.Count > 0)
                {
                    log.WarnFormat("Rule '{0}' was valid but returned violations '{1}'.", rule, FormatHelper.ConvertToString(result.Violations, ", "));
                }

                if (result.Violations != null)
                {
                    foreach (IValidationViolation validationViolation in result.Violations)
                    {
                        aggregatedResults.Violations.Add(validationViolation);
                    }
                }

                sb.AppendFormat("Rule '{0}' returned '{1}' with violations '{2}'. ", rule, result.Valid, FormatHelper.ConvertToString(result.Violations, ", "));

                if (!result.Valid && this.breakOnFirstViolation)
                {
                    sb.Append("Rule validation was stopped after first violation. ");
                    break;
                }
            }

            logInfo = sb.ToString();
            return aggregatedResults;
        }
    }
}