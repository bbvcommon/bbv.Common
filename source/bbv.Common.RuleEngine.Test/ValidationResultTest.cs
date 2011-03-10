//-------------------------------------------------------------------------------
// <copyright file="ValidationResultTest.cs" company="bbv Software Services AG">
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

    [TestFixture]
    public class ValidationResultTest
    {
        private ValidationResult validationResult;

        [SetUp]
        public void Creation()
        {
            this.validationResult = new ValidationResult(true);
        }

        [Test]
        public void Valid()
        {
            this.validationResult.Valid = false;

            Assert.IsFalse(this.validationResult.Valid, "Validation result was set invalid.");
        }

        [Test]
        public void Violations()
        {
            this.validationResult.Violations.Add(new ValidationViolation("test"));

            Assert.AreEqual("test", this.validationResult.Violations[0].Reason);
        }
    }
}
