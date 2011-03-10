//-------------------------------------------------------------------------------
// <copyright file="RuleEngineTest.cs" company="bbv Software Services AG">
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

    using NMock2;

    using NUnit.Framework;

    using Is = NMock2.Is;
    using IValidationAggregator = IAggregator<IValidationRule, IValidationResult>;

    /// <summary>
    /// Tests for the <see cref="RuleEngine"/>.
    /// </summary>
    [TestFixture]
    public class RuleEngineTest
    {
        /// <summary>The object under test.</summary>
        private RuleEngine testee;

        /// <summary>Mock factory</summary>
        private Mockery mockery;

        /// <summary>Mock for the validation factory</summary>
        private IValidationFactory validationFactory;

        /// <summary>Mock for the rules provider</summary>
        private IRulesProvider defaultRulesProvider;

        /// <summary>Mock for the rules provider from a simulated plug-in</summary>
        private IRulesProvider pluginRulesProvider;

        /// <summary>Mock for the rules provider finder</summary>
        private IRulesProviderFinder rulesProviderFinder;

        /// <summary>
        /// Common setup code.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();

            // mock rule validationFactory
            this.validationFactory = this.mockery.NewMock<IValidationFactory>();
            IValidationResult aggregatedValidationResult = new ValidationResult(true);
            Stub.On(this.validationFactory).Method("CreateValidationResult").With(true).Will(
                Return.Value(aggregatedValidationResult));
            Stub.On(this.validationFactory).Method("CreateRuleSet").Will(Return.Value(new ValidationRuleSet()));

            // mock rules providers
            this.defaultRulesProvider = this.mockery.NewMock<IRulesProvider>();
            this.pluginRulesProvider = this.mockery.NewMock<IRulesProvider>();

            // mock rules provider finder
            this.rulesProviderFinder = this.mockery.NewMock<IRulesProviderFinder>();
            Stub.On(this.rulesProviderFinder).Method("FindRulesProviders").Will(
                Return.Value(new List<IRulesProvider> { this.defaultRulesProvider, this.pluginRulesProvider }));

            // create testee
            this.testee = new RuleEngine(this.rulesProviderFinder);
        }

        /// <summary>
        /// Common tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Tests that all rules found on the providers are passed to the aggregator and that the result returned by the 
        /// aggregator is passed as overall result.
        /// </summary>
        [Test]
        public void Evaluate()
        {
            IValidationRuleSetDescriptor ruleSetDescriptor = this.mockery.NewMock<IValidationRuleSetDescriptor>();

            Expect.On(ruleSetDescriptor).GetProperty("Factory").Will(Return.Value(this.validationFactory));

            IRuleSet<IValidationRule> ruleSet = new ValidationRuleSet { this.mockery.NewMock<IValidationRule>() };
            Expect.Once.On(this.defaultRulesProvider).Method("GetRules").With(ruleSetDescriptor).Will(Return.Value(ruleSet));
            Expect.Once.On(this.pluginRulesProvider).Method("GetRules").With(ruleSetDescriptor).Will(Return.Value(null));

            IValidationResult validationResult = this.mockery.NewMock<IValidationResult>();

            IValidationAggregator aggregator = this.mockery.NewMock<IValidationAggregator>();
            Stub.On(ruleSetDescriptor).GetProperty("Aggregator").Will(Return.Value(aggregator));
            Stub.On(aggregator).Method("Aggregate").With(ruleSet, Is.Out).Will(
                Return.Value(validationResult),
                Return.OutValue(1, "log"));

            IValidationResult result = this.testee.Evaluate(ruleSetDescriptor);
            Assert.AreEqual(validationResult, result, "result of aggregator is not passed correctly as result of Evaluate.");
        }
    }
}
