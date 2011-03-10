//-------------------------------------------------------------------------------
// <copyright file="RulesProviderBaseTest.cs" company="bbv Software Services AG">
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
    using System;
    using System.Linq;
    using NUnit.Framework;

    /// <summary>
    /// Tests the <see cref="RulesProviderBase"/> class.
    /// </summary>
    [TestFixture]
    public class RulesProviderBaseTest
    {
        /// <summary>the object under test</summary>
        private RulesProviderBase testee;

        /// <summary>
        /// Common set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new TestRulesProvider();
        }

        /// <summary>
        /// The base class should relay the call to the correct GetRules method.
        /// </summary>
        [Test]
        public void GetRules()
        {
            IRuleSet<IValidationRule> ruleSet = this.testee.GetRules(new TestRuleSetDescriptor());

            Assert.IsNotNull(ruleSet);
            Assert.AreEqual(1, ruleSet.Count);
            Assert.IsInstanceOf(typeof(TestValidationRule), ruleSet.ElementAt(0));
        }

        /// <summary>
        /// If the specified descriptor can be assigned to multiple rules provider methods (derived classes)
        /// then the rules of all matching rules provider methods should be combined.
        /// </summary>
        [Test]
        public void GetRulesForDerivedDescriptor()
        {
            IRuleSet<IValidationRule> ruleSet = this.testee.GetRules(new DerivedTestRuleSetDescriptor());

            Assert.IsNotNull(ruleSet);
            Assert.AreEqual(2, ruleSet.Count);
            Assert.IsInstanceOf(typeof(TestValidationRule), ruleSet.ElementAt(0));
            Assert.IsInstanceOf(typeof(AnotherTestValidationRule), ruleSet.ElementAt(1));
        }

        /// <summary>
        /// Request for a descriptor with no matching method results in null.
        /// </summary>
        [Test]
        public void GetRulesForNotSupportedRuleSetDescriptor()
        {
            IRuleSet<IValidationRule> ruleSet = this.testee.GetRules(new NotSupportedRuleSetDescriptor());

            Assert.IsNull(ruleSet);
        }

        /// <summary>
        /// The rules provider for this unit test.
        /// </summary>
        private class TestRulesProvider : RulesProviderBase
        {
            /// <summary>
            /// Rule provider method.
            /// </summary>
            /// <param name="ruleSetDescriptor">the rule set descriptor this method returns rules for.</param>
            /// <returns>A rule set.</returns>
            [RuleProvider]
            public IRuleSet<IValidationRule> GetRules(TestRuleSetDescriptor ruleSetDescriptor)
            {
                return new ValidationRuleSet { new TestValidationRule(ruleSetDescriptor.Factory) };
            }

            /// <summary>
            /// Rule provider method.
            /// </summary>
            /// <param name="ruleSetDescriptor">the rule set descriptor this method returns rules for.</param>
            /// <returns>A rule set.</returns>
            [RuleProvider]
            public IRuleSet<IValidationRule> GetRules(DerivedTestRuleSetDescriptor ruleSetDescriptor)
            {
                return new ValidationRuleSet { new AnotherTestValidationRule(ruleSetDescriptor.Factory) };
            }

            /// <summary>
            /// Just a fake to test whether correct method is used.
            /// </summary>
            /// <param name="s">just a fake</param>
            /// <returns>Nothing useful.</returns>
            [RuleProvider]
            public int GetRules(string s)
            {
                return s.Length;
            }
        }

        /// <summary>
        /// Rule set descriptor for this unit test.
        /// </summary>
        private class TestRuleSetDescriptor : ValidationRuleSetDescriptor
        {
        }

        /// <summary>
        /// Derived rule set descriptor for this unit test.
        /// </summary>
        private class DerivedTestRuleSetDescriptor : TestRuleSetDescriptor
        {
        }

        /// <summary>
        /// A descriptor that is not used.
        /// </summary>
        private class NotSupportedRuleSetDescriptor : ValidationRuleSetDescriptor
        {
        }

        private class TestValidationRule : ValidationRuleBase
        {
            public TestValidationRule(IValidationFactory validationFactory) : base(validationFactory)
            {
            }

            public override IValidationResult Evaluate()
            {
                throw new NotImplementedException();
            }
        }

        private class AnotherTestValidationRule : ValidationRuleBase
        {
            public AnotherTestValidationRule(IValidationFactory validationFactory) : base(validationFactory)
            {
            }

            public override IValidationResult Evaluate()
            {
                throw new NotImplementedException();
            }
        }
    }
}
