//-------------------------------------------------------------------------------
// <copyright file="ValidationAcceptanceTest.cs" company="bbv Software Services AG">
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
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    /// <summary>
    /// Acceptance test (test of complete component) for validation rules on rule engine.
    /// </summary>
    [TestFixture]
    public class ValidationAcceptanceTest : IRulesProviderFinder
    {
        /// <summary>Rule engine instance</summary>
        private RuleEngine ruleEngine;

        /// <summary>we need two rules provider for demonstration</summary>
        private IRulesProvider globalRulesProvider;

        /// <summary>we need two rules provider for demonstration</summary>
        private IRulesProvider pluginRulesProvider;

        /// <summary>
        /// Initializes the whole rule engine component
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            IRulesProviderFinder rulesProviderFinder = this;
            this.ruleEngine = new RuleEngine(rulesProviderFinder);

            this.globalRulesProvider = new GlobalRulesProvider();
            this.pluginRulesProvider = new PlugInRulesProvider();
        }

        /// <summary>
        /// Tests the good case when everything is valid.
        /// </summary>
        [Test]
        public void ValidationSucceeds()
        {
            IValidationResult validationResult = this.ruleEngine.Evaluate(new TestRuleSetDescriptor("bla"));

            Assert.IsTrue(validationResult.Valid);
        }

        /// <summary>
        /// Tests the bad case when rules fail.
        /// </summary>
        [Test]
        public void ValidationFails()
        {
            IValidationResult validationResult = this.ruleEngine.Evaluate(new TestRuleSetDescriptor("hm"));

            Assert.IsFalse(validationResult.Valid);
            Assert.AreEqual(2, validationResult.Violations.Count);
        }

        /// <summary>
        /// Finds the rules providers.
        /// </summary>
        /// <typeparam name="TRule">The type of the rule.</typeparam>
        /// <typeparam name="TAggregationResult">The type of the aggregation result.</typeparam>
        /// <param name="ruleSetDescriptor">The rule set descriptor.</param>
        /// <returns>Rules providers</returns>
        public ICollection<IRulesProvider> FindRulesProviders<TRule, TAggregationResult>(
            IRuleSetDescriptor<TRule, TAggregationResult> ruleSetDescriptor)
        {
            return new List<IRulesProvider> { this.globalRulesProvider, this.pluginRulesProvider };
        }

        /// <summary>
        /// Descriptor for rules to check.
        /// </summary>
        private class TestRuleSetDescriptor : ValidationRuleSetDescriptor
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ValidationAcceptanceTest.TestRuleSetDescriptor"/> class.
            /// </summary>
            /// <param name="contextInfo">The context info.</param>
            public TestRuleSetDescriptor(string contextInfo)
            {
                this.ContextInfo = contextInfo;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ValidationAcceptanceTest.TestRuleSetDescriptor"/> class. 
            /// </summary>
            /// <param name="breakOnFirstViolation">
            /// if set to <c>true</c> [break on first violation].
            /// </param>
            /// <param name="contextInfo">
            /// The context info.
            /// </param>
            public TestRuleSetDescriptor(bool breakOnFirstViolation, string contextInfo)
                : base(breakOnFirstViolation)
            {
                this.ContextInfo = contextInfo;
            }

            /// <summary>
            /// Gets the context info.
            /// </summary>
            /// <value>The context info.</value>
            public string ContextInfo { get; private set; }
        }

        /// <summary>
        /// The global rules provider provides <see cref="RuleLength"/>.
        /// </summary>
        private class GlobalRulesProvider : RulesProviderBase
        {
            /// <summary>
            /// Gets the rules.
            /// </summary>
            /// <param name="descriptor">The descriptor.</param>
            /// <returns><see cref="RuleSet{TRule}"/> with <see cref="RuleLength"/>.</returns>
            [RuleProvider]
            public IRuleSet<IValidationRule> GetRules(TestRuleSetDescriptor descriptor)
            {
                return new RuleSet<IValidationRule> { new RuleLength(descriptor.ContextInfo, descriptor.Factory) };
            }
        }

        /// <summary>
        /// The plug-in rules provider provides <see cref="RuleContainsA"/>.
        /// </summary>
        private class PlugInRulesProvider : RulesProviderBase
        {
            /// <summary>
            /// Gets the rules.
            /// </summary>
            /// <param name="descriptor">The descriptor.</param>
            /// <returns><see cref="RuleSet{TRule}"/> with <see cref="RuleContainsA"/>.</returns>
            [RuleProvider]
            public IRuleSet<IValidationRule> GetRules(TestRuleSetDescriptor descriptor)
            {
                return new RuleSet<IValidationRule> { new RuleContainsA(descriptor.ContextInfo, descriptor.Factory) };
            }
        }

        /// <summary>
        /// Rule checking the length of the context info.
        /// </summary>
        private class RuleLength : ValidationRuleBase
        {
            /// <summary>the context info passed to this rule</summary>
            private readonly string contextInfo;

            /// <summary>
            /// Initializes a new instance of the <see cref="ValidationAcceptanceTest.RuleLength"/> class.
            /// </summary>
            /// <param name="contextInfo">The context info.</param>
            /// <param name="validationFactory">The validation factory.</param>
            public RuleLength(string contextInfo, IValidationFactory validationFactory)
                : base(validationFactory)
            {
                this.contextInfo = contextInfo;
            }

            /// <summary>
            /// Validates this rule.
            /// </summary>
            /// <returns>The result of the validation.</returns>
            public override IValidationResult Evaluate()
            {
                bool valid = this.contextInfo.Length > 2;

                IValidationResult validationResult = this.ValidationFactory.CreateValidationResult(valid);
                if (!valid)
                {
                    validationResult.Violations.Add(this.ValidationFactory.CreateValidationViolation("too short"));
                }

                return validationResult;
            }
        }

        /// <summary>
        /// Rule validating the context info for an 'a'.
        /// </summary>
        private class RuleContainsA : ValidationRuleBase
        {
            /// <summary>the context info passed to this rule</summary>
            private readonly string contextInfo;

            /// <summary>
            /// Initializes a new instance of the <see cref="ValidationAcceptanceTest.RuleContainsA"/> class.
            /// </summary>
            /// <param name="contextInfo">The context info.</param>
            /// <param name="validationFactory">The validation factory.</param>
            public RuleContainsA(string contextInfo, IValidationFactory validationFactory)
                : base(validationFactory)
            {
                this.contextInfo = contextInfo;
            }

            /// <summary>
            /// Validates this rule.
            /// </summary>
            /// <returns>The result of the validation.</returns>
            public override IValidationResult Evaluate()
            {
                bool valid = this.contextInfo.Contains("a");

                IValidationResult validationResult = this.ValidationFactory.CreateValidationResult(valid);
                if (!valid)
                {
                    validationResult.Violations.Add(this.ValidationFactory.CreateValidationViolation("no a contained"));
                }

                return validationResult;
            }
        }
    }
}