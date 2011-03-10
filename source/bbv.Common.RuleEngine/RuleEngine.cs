//-------------------------------------------------------------------------------
// <copyright file="RuleEngine.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using Formatters;
    using log4net;

    /// <summary>
    /// The rule engine is the central component for checking logical rules.
    /// </summary>
    /// <remarks>
    /// The rule engine takes a <see cref="IRuleSetDescriptor{TRule, TAggregationResult}"/> as a parameter to 
    /// the <see cref="Evaluate{TRule,TAggregationResult}"/>
    /// method. Then it uses the <see cref="IRulesProviderFinder"/> to get the rules providers relevant
    /// for this descriptor (this allows different rules providers in your system (for example for plug-ins)).
    /// To get all rules that need to be checked, the rule engine calls on all found rules provider the
    /// <see cref="IRulesProvider.GetRules{TRule, TResult}"/> method, which returns all rules relevant for the specified
    /// <see cref="IRuleSetDescriptor{TRule, TAggregationResult}"/>. Finally the rule engine aggregates the results of all checked rules together
    /// into one <see cref="IValidationResult"/> eventually containing a set of <see cref="IValidationViolation"/>s.
    /// </remarks>
    public class RuleEngine : IRuleEngine
    {
        /// <summary>
        /// The logger for the rule engine.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The rules provider finder that is asked for rule providers for a rule descriptor.
        /// </summary>
        private readonly IRulesProviderFinder rulesProviderFinder;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleEngine"/> class.
        /// </summary>
        /// <param name="rulesProviderFinder">The rules provider finder.</param>
        public RuleEngine(IRulesProviderFinder rulesProviderFinder)
        {
            this.rulesProviderFinder = rulesProviderFinder;
        }

        #endregion

        #region Evaluate

        /// <summary>
        /// Evaluates the rules specified by the rule set descriptor.
        /// </summary>
        /// <typeparam name="TRule">The type of the rule.</typeparam>
        /// <typeparam name="TAggregationResult">The type of the aggregation result.</typeparam>
        /// <param name="ruleSetDescriptor">The rule set descriptor describing which rules have to be evaluated.</param>
        /// <returns>The result of the validation.</returns>
        public TAggregationResult Evaluate<TRule, TAggregationResult>(IRuleSetDescriptor<TRule, TAggregationResult> ruleSetDescriptor)
        {
            IRuleSet<TRule> ruleSet = ruleSetDescriptor.Factory.CreateRuleSet();

            ICollection<IRulesProvider> providers = this.rulesProviderFinder.FindRulesProviders(ruleSetDescriptor);
            foreach (IRulesProvider rulesProvider in providers)
            {
                IRuleSet<TRule> rs = rulesProvider.GetRules(ruleSetDescriptor);
                if (rs != null)
                {
                    ruleSet.AddRange(rs);
                }
            }

            string logInfo;
            TAggregationResult result = ruleSetDescriptor.Aggregator.Aggregate(ruleSet, out logInfo);

            string logMessage = string.Format(
                CultureInfo.InvariantCulture,
                "Validated rule set '{0}' with rules from rules providers '{1}'. Validated Rules: '{2}'.",
                ruleSetDescriptor,
                FormatHelper.ConvertToString(providers, ", "),
                logInfo);

            log.DebugFormat(
                CultureInfo.InvariantCulture,
                logMessage);

            return result;
        }

        #endregion
    }
}
