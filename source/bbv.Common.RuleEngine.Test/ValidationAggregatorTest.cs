//-------------------------------------------------------------------------------
// <copyright file="ValidationAggregatorTest.cs" company="bbv Software Services AG">
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
    using NMock2;

    using NUnit.Framework;

    /// <summary>
    /// Tests the <see cref="ValidationAggregator"/>.
    /// </summary>
    [TestFixture]
    public class ValidationAggregatorTest
    {
        /// <summary>Mock factory</summary>
        private Mockery mockery;

        /// <summary>Mock for validation factory</summary>
        private IValidationFactory validationFactory;

        /// <summary>
        /// Common setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();

            this.validationFactory = this.mockery.NewMock<IValidationFactory>();
            Stub.On(this.validationFactory).Method("CreateValidationResult").With(true).Will(Return.Value(new ValidationResult(true)));
            Stub.On(this.validationFactory).Method("CreateValidationResult").With(false).Will(Return.Value(new ValidationResult(false)));
        }

        /// <summary>
        /// Tests that aggregation of a rule set with valid rules results in a valid result.
        /// </summary>
        [Test]
        public void AggregateARuleSetWithValidRulesResultsInAValidResult()
        {
            ValidationAggregator testee = new ValidationAggregator(this.validationFactory, false);

            IRuleSet<IValidationRule> ruleSet = new ValidationRuleSet
                                                    {
                                                        this.mockery.NewMock<IValidationRule>(),
                                                        this.mockery.NewMock<IValidationRule>()
                                                    };
            
            Expect.On(ruleSet[0]).Method("Evaluate").Will(Return.Value(new ValidationResult(true)));
            Expect.On(ruleSet[1]).Method("Evaluate").Will(Return.Value(new ValidationResult(true)));
            
            string logInfo;
            IValidationResult result = testee.Aggregate(ruleSet, out logInfo);

            Assert.IsTrue(result.Valid, "aggregated result has to be valid if all rules are valid.");
        }

        /// <summary>
        /// Tests that aggregation of a rule set with invalid rules results in an invalid result and 
        /// that violations are contained in the result.
        /// </summary>
        [Test]
        public void AggregateARuleSetWithInvalidRulesResultsInAnInvalidResult()
        {
            ValidationAggregator testee = new ValidationAggregator(this.validationFactory, false);

            IRuleSet<IValidationRule> ruleSet = new ValidationRuleSet
                                                    {
                                                        this.mockery.NewMock<IValidationRule>(),
                                                        this.mockery.NewMock<IValidationRule>()
                                                    };

            Expect.On(ruleSet[0]).Method("Evaluate").Will(Return.Value(new ValidationResult(true)));

            ValidationResult invalidResult = new ValidationResult(false);
            invalidResult.Violations.Add(new ValidationViolation("test"));
            Expect.On(ruleSet[1]).Method("Evaluate").Will(Return.Value(invalidResult));

            string logInfo;
            IValidationResult result = testee.Aggregate(ruleSet, out logInfo);

            Assert.IsFalse(result.Valid, "aggregated result has to be valid if all rules are valid.");
            Assert.AreEqual(1, result.Violations.Count, "violation was not passed to result.");
        }

        /// <summary>
        /// Stop aggregation after first invalid rule if BreakOnFirstViolation is set to true.
        /// </summary>
        [Test]
        public void StopOnFirstViolation()
        {
            ValidationAggregator testee = new ValidationAggregator(this.validationFactory, true);

            IRuleSet<IValidationRule> ruleSet = new ValidationRuleSet
                                                    {
                                                        this.mockery.NewMock<IValidationRule>(),
                                                        this.mockery.NewMock<IValidationRule>()
                                                    };

            Expect.On(ruleSet[0]).Method("Evaluate").Will(Return.Value(new ValidationResult(false)));

            ValidationResult invalidResult = new ValidationResult(false);
            invalidResult.Violations.Add(new ValidationViolation("test"));
            Expect.On(ruleSet[1]).Method("Evaluate").Will(Return.Value(invalidResult));
            
            string logInfo;
            IValidationResult result = testee.Aggregate(ruleSet, out logInfo);

            Assert.IsFalse(result.Valid, "aggregated result has to be valid if all rules are valid.");
            Assert.AreEqual(0, result.Violations.Count, "the violation of the second rule should not be in the result because aggregation should have stopped on first invalid rule.");
        }
    }
}