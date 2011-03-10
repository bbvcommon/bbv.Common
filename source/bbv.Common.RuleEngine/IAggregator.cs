//-------------------------------------------------------------------------------
// <copyright file="IAggregator.cs" company="bbv Software Services AG">
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
    /// An aggregator is used to combine the results of all rules taking part in the evaluation into a single result.
    /// </summary>
    /// <typeparam name="TRule">The type of the rule.</typeparam>
    /// <typeparam name="TAggregationResult">The type of the aggregation result.</typeparam>
    public interface IAggregator<TRule, TAggregationResult>
    {
        /// <summary>
        /// Aggregates the specified rule set.
        /// </summary>
        /// <param name="ruleSet">The rule set.</param>
        /// <param name="logInfo">The log info. The aggregator should provide information about the results of the different rules and how they
        /// influenced the overall result. This info is written to the log by the rule engine.</param>
        /// <returns>The aggregated result of all rules takingpart in the evaluation.</returns>
        TAggregationResult Aggregate(IRuleSet<TRule> ruleSet, out string logInfo);
    }
}