//-------------------------------------------------------------------------------
// <copyright file="ValidationRuleBaseTest.cs" company="bbv Software Services AG">
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
    using System.Diagnostics.CodeAnalysis;

    using NMock2;

    using NUnit.Framework;

    /// <summary>
    /// Tests the <see cref="ValidationRuleBase"/> class.
    /// </summary>
    [TestFixture]
    public class ValidationRuleBaseTest
    {
        /// <summary>Mock factory</summary>
        private Mockery mockery;

        /// <summary>Mock for the validation factory</summary>
        private IValidationFactory validationFactory;

        /// <summary>object under test (derived from <see cref="ValidationRuleBase"/>)</summary>
        private TestValidationRule testee;
        
        /// <summary>
        /// Common setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.mockery = new Mockery();
            this.validationFactory = this.mockery.NewMock<IValidationFactory>();

            IValidationResult validationResult = this.mockery.NewMock<IValidationResult>();
            Stub.On(validationResult).GetProperty("Valid").Will(Return.Value(true));
            Stub.On(this.validationFactory).Method("CreateValidationResult").With(true).Will(Return.Value(validationResult));

            this.testee = new TestValidationRule(this.validationFactory);
        }

        /// <summary>
        /// The factory passed in the constructor is used.
        /// </summary>
        [Test]
        public void Factory()
        {
            IValidationResult result = this.testee.Evaluate();

            Assert.IsTrue(result.Valid, "Result should be valid because it was initialized valid.");
        }

        /// <summary>
        /// Test class
        /// </summary>
        private class TestValidationRule : ValidationRuleBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ValidationRuleBaseTest.TestValidationRule"/> class.
            /// </summary>
            /// <param name="validationFactory">The factory for creating rule engine related instances.</param>
            public TestValidationRule(IValidationFactory validationFactory)
                : base(validationFactory)
            {
            }

            /// <summary>
            /// Validates this rule.
            /// </summary>
            /// <returns>The result of the validation.</returns>
            public override IValidationResult Evaluate()
            {
                return this.ValidationFactory.CreateValidationResult(true);
            }
        }
    }
}
