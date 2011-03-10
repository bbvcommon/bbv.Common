//-------------------------------------------------------------------------------
// <copyright file="IRuleSetDescriptor.cs" company="bbv Software Services AG">
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
    /// All rule set descriptors have to implement this interface.
    /// </summary>
    /// <typeparam name="TRule">The type of the rule.</typeparam>
    /// <typeparam name="TAggregationResult">The type of the aggregation result.</typeparam>
    /// <remarks>
    /// A rule set descriptor is used to define a set of rules.
    /// On one side it is used to define what should be validated (caller of rule engine), on the other
    /// side it defines what rules are part of the evaluation (rules provider).
    /// </remarks>
    public interface IRuleSetDescriptor<TRule, TAggregationResult>
    {
        /// <summary>
        /// Gets the factory used to create needed instances of rule engine related classes.
        /// </summary>
        /// <value>The factory.</value>
        IFactory<TRule> Factory { get; }

        /// <summary>
        /// Gets the aggregator that is used to combine all rules taking part in the evaluation.
        /// </summary>
        /// <value>The aggregator.</value>
        IAggregator<TRule, TAggregationResult> Aggregator { get; }
    }
}
