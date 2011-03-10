//-------------------------------------------------------------------------------
// <copyright file="ValidationResult.cs" company="bbv Software Services AG">
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

    /// <summary>
    /// Represents the result of a validation.
    /// </summary>
    public class ValidationResult : IValidationResult
    {
        /// <summary>
        /// A list of violations associated with this validation result.
        /// </summary>
        private readonly List<IValidationViolation> violations = new List<IValidationViolation>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="valid">if set to <c>true</c> [valid].</param>
        public ValidationResult(bool valid)
        {
            this.Valid = valid;
        }

        #region IValidationResult Members

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IValidationResult"/> is valid.
        /// </summary>
        /// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets the violations.
        /// Is an empty list if <see cref="IValidationResult.Valid"/> is <c>true</c>.
        /// </summary>
        /// <value>The violations.</value>
        public IList<IValidationViolation> Violations
        {
            get { return this.violations; }
        }

        #endregion
    }
}
