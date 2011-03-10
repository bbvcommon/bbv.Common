//-------------------------------------------------------------------------------
// <copyright file="ValidationRuleBase.cs" company="bbv Software Services AG">
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
    /// <summary>
    /// Base class for validation rules.
    /// <para>
    /// Provides a <see cref="IValidationFactory"/>.
    /// </para>
    /// </summary>
    public abstract class ValidationRuleBase : IValidationRule
    {
        /// <summary>
        /// The validationFactory that is provided to derived classes. It is passed in the constructor.
        /// </summary>
        private readonly IValidationFactory validationFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleBase"/> class.
        /// </summary>
        /// <param name="validationFactory">The factory for creating rule engine related instances.</param>
        protected ValidationRuleBase(IValidationFactory validationFactory)
        {
            this.validationFactory = validationFactory;
        }

        /// <summary>
        /// Gets the validation factory for creating rule engine related instances.
        /// </summary>
        /// <value>The validation factory.</value>
        protected IValidationFactory ValidationFactory
        {
            get { return this.validationFactory; }
        }

        /// <summary>
        /// Validates this rule.
        /// </summary>
        /// <returns>The result of the validation.</returns>
        public abstract IValidationResult Evaluate();
    }
}
