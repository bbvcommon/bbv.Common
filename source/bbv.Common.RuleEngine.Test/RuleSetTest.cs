//-------------------------------------------------------------------------------
// <copyright file="RuleSetTest.cs" company="bbv Software Services AG">
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
    /// Tests for the <see cref="ValidationRuleSet"/> class.
    /// </summary>
    [TestFixture]
    public class RuleSetPropertyTest
    {
        /// <summary>Mock factory.</summary>
        private Mockery mockery;

        /// <summary>Instance of the class under test.</summary>
        private ValidationRuleSet testee;

        /// <summary>
        /// Initializes the <see cref="mockery"/> and <see cref="testee"/>.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();
            this.testee = new ValidationRuleSet();
        }

        /// <summary>
        /// Checks that all expectations on the mockery are fulfilled.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            this.mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Rules can be added to rule sets.
        /// </summary>
        [Test]
        public void Rules()
        {
            IValidationRule rule = this.mockery.NewMock<IValidationRule>();
            this.testee.Add(rule);

            Assert.AreEqual(1, this.testee.Count);
        }
    }
}
