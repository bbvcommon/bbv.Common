//-------------------------------------------------------------------------------
// <copyright file="RulesProviderBaseExceptionTest.cs" company="bbv Software Services AG">
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
    using NUnit.Framework;

    /// <summary>
    /// Tests exception behavior of the <see cref="RulesProviderBase"/> class.
    /// </summary>
    [TestFixture]
    public class RulesProviderBaseExceptionTest
    {
        /// <summary>
        /// Static methods cannot be marked with RuleProvider attribute.
        /// </summary>
        [Test]
        public void StaticRuleProviderMethod()
        {
            RuleEngineException exception = Assert.Throws<RuleEngineException>(
                () => new StaticRulesProvider());

            Assert.AreEqual("Do not mark static methods as RuleProvider.", exception.Message);
        }

        /// <summary>
        /// Private methods cannot be marked with RuleProvider attribute.
        /// </summary>
        [Test]
        public void PrivateRuleProviderMethod()
        {
            RuleEngineException exception = Assert.Throws<RuleEngineException>(
                () => new PrivateRulesProvider());

            Assert.AreEqual("Do not mark private methods as RuleProvider.", exception.Message);
        }

        /// <summary>
        /// The rules provider defining an illegal static rule provider method.
        /// </summary>
        private class StaticRulesProvider : RulesProviderBase
        {
            /// <summary>
            /// Rule provider method.
            /// </summary>
            /// <param name="ruleSetDescriptor">the rule set descriptor this method returns rules for.</param>
            /// <returns>A rule set.</returns>
            [RuleProvider]
            public static IRuleSet<IValidationRule> GetRules(TestRuleSetDescriptor ruleSetDescriptor)
            {
                return null;
            }
        }

        /// <summary>
        /// The rules provider defining an illegal private rule provider method.
        /// </summary>
        private class PrivateRulesProvider : RulesProviderBase
        {
            /// <summary>
            /// Rule provider method.
            /// </summary>
            /// <param name="ruleSetDescriptor">the rule set descriptor this method returns rules for.</param>
            /// <returns>A rule set.</returns>
            [RuleProvider]
            private IRuleSet<IValidationRule> GetRules(TestRuleSetDescriptor ruleSetDescriptor)
            {
                return null;
            }
        }

        /// <summary>
        /// Rule set descriptor for this unit test.
        /// </summary>
        private class TestRuleSetDescriptor : ValidationRuleSetDescriptor
        {
        }
    }
}