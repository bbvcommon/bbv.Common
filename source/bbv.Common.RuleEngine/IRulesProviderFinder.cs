//-------------------------------------------------------------------------------
// <copyright file="IRulesProviderFinder.cs" company="bbv Software Services AG">
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

    /// <summary>
    /// The <see cref="RuleEngine"/> uses a <see cref="IRulesProviderFinder"/> to get
    /// all <see cref="IRulesProvider"/> relevant for a <see cref="IRuleSetDescriptor{TRule, TAggregationResult}"/>.
    /// </summary>
    public interface IRulesProviderFinder
    {
        /// <summary>
        /// Finds the rule providers.
        /// </summary>
        /// <typeparam name="TRule">The type of the rule.</typeparam>
        /// <typeparam name="TAggregationResult">The type of the aggregation result.</typeparam>
        /// <param name="ruleSetDescriptor">The rule set descriptor.</param>
        /// <returns>
        /// List of rules providers to be asked for rules for the rule descriptor.
        /// </returns>
        ICollection<IRulesProvider> FindRulesProviders<TRule, TAggregationResult>(IRuleSetDescriptor<TRule, TAggregationResult> ruleSetDescriptor);
    }
}
