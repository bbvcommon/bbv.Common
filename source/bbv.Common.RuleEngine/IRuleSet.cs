//-------------------------------------------------------------------------------
// <copyright file="IRuleSet.cs" company="bbv Software Services AG">
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
    /// A <see cref="IRuleSet{TRule}"/> defines a set of rules.
    /// </summary>
    /// <typeparam name="TRule">The type of the rule.</typeparam>
    public interface IRuleSet<TRule> : IList<TRule>
    {
        /// <summary>
        /// Adds several rules at once.
        /// </summary>
        /// <param name="rules">The rules.</param>
        void AddRange(IEnumerable<TRule> rules);
    }
}
