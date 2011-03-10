//-------------------------------------------------------------------------------
// <copyright file="IRuleEngine.cs" company="bbv Software Services AG">
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
    /// The rule engine is the central component for checking of logical rules.
    /// </summary>
    public interface IRuleEngine
    {
        /// <summary>
        /// Evaluates the rules specified by the rule set descriptor.
        /// </summary>
        /// <typeparam name="TRule">The type of the rule.</typeparam>
        /// <typeparam name="TAggregationResult">The type of the aggregation result.</typeparam>
        /// <param name="ruleSetDescriptor">The rule set descriptor.</param>
        /// <returns>The result of the validation.</returns>
        TAggregationResult Evaluate<TRule, TAggregationResult>(IRuleSetDescriptor<TRule, TAggregationResult> ruleSetDescriptor);
    }
}
