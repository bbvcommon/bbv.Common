//-------------------------------------------------------------------------------
// <copyright file="IRulesProvider.cs" company="bbv Software Services AG">
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
    /// A rule provider provides set of rules that have to be checked.
    /// </summary>
    public interface IRulesProvider
    {
        /// <summary>
        /// Returns the rules described by the specified rule set descriptor.
        /// </summary>
        /// <typeparam name="TRule">The type of the rule.</typeparam>
        /// <typeparam name="TAggregationResult">The type of the aggregation result.</typeparam>
        /// <param name="ruleSetDescriptor">The rule set descriptor.</param>
        /// <returns>A set of rules to be validated.</returns>
        IRuleSet<TRule> GetRules<TRule, TAggregationResult>(IRuleSetDescriptor<TRule, TAggregationResult> ruleSetDescriptor);
    }
}
