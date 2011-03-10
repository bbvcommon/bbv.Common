//-------------------------------------------------------------------------------
// <copyright file="ValidationFactoryTest.cs" company="bbv Software Services AG">
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
    using NUnit.Framework;

    /// <summary>
    /// Tests for the <see cref="ValidationFactory"/> class.
    /// </summary>
    [TestFixture]
    public class ValidationFactoryTest
    {
        /// <summary>The object under test.</summary>
        private IValidationFactory testee;

        /// <summary>
        /// Initializes the <see cref="testee"/>.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.testee = new ValidationFactory();
        }

        /// <summary>
        /// Factory can create rule sets.
        /// </summary>
        [Test]
        public void CreateRuleSet()
        {
            IRuleSet<IValidationRule> ruleSet = this.testee.CreateRuleSet();

            Assert.IsNotNull(ruleSet, "newly created RuleSet should not be null.");
            Assert.AreEqual(0, ruleSet.Count, "newly created RuleSet should contain no rules.");
        }

        /// <summary>
        /// Factory can create validation results initialized to valid.
        /// </summary>
        [Test]
        public void CreateValidationResultInitializedInvalid()
        {
            IValidationResult validationResult = this.testee.CreateValidationResult(false);

            Assert.IsFalse(validationResult.Valid, "was initializes invalid.");
            Assert.AreEqual(0, validationResult.Violations.Count, "newly created validation result should not contain violations.");
        }

        /// <summary>
        /// Factory can create validation results initialized to invalid.
        /// </summary>
        [Test]
        public void CreateValidationResultInitializedValid()
        {
            IValidationResult validationResult = this.testee.CreateValidationResult(true);

            Assert.IsTrue(validationResult.Valid, "was initialized valid.");
            Assert.AreEqual(0, validationResult.Violations.Count, "newly created validation result should not contain violations.");
        }

        /// <summary>
        /// Factory can create validation violations.
        /// </summary>
        [Test]
        public void CreateValidationViolation()
        {
            const string Reason = "reason {0}";
            IValidationViolation violation = this.testee.CreateValidationViolation(Reason, "some argument");

            Assert.AreEqual("reason some argument", violation.Reason, "Reason is not initialized correctly.");
        }

        /// <summary>
        /// Factory can create validations violations with GUID.
        /// </summary>
        [Test]
        public void CreateValidationViolationWithMessageId()
        {
            const string Reason = "reason";
            Guid guid = Guid.NewGuid();
            IValidationViolation violation = this.testee.CreateValidationViolation(Reason, guid, "some argument");

            Assert.AreEqual(Reason, violation.Reason, "Reason is not initialized correctly.");
            Assert.AreEqual(guid, violation.MessageId);
            Assert.AreEqual("some argument", violation.MessageArguments[0]);
        }
    }
}
