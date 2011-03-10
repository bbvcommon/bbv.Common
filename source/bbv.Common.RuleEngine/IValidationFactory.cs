//-------------------------------------------------------------------------------
// <copyright file="IValidationFactory.cs" company="bbv Software Services AG">
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

    /// <summary>
    /// Factory for all rule engine related instances used for validation rules.
    /// </summary>
    public interface IValidationFactory : IFactory<IValidationRule>
    {
        /// <summary>
        /// Creates a validation result.
        /// </summary>
        /// <param name="valid">Initial value for <see cref="ValidationResult.Valid"/></param>
        /// <returns>A newly created validation result.</returns>
        IValidationResult CreateValidationResult(bool valid);

        /// <summary>
        /// <para>Creates a validation violation.</para>
        /// <para>Use this overload if you do not have an associated message.</para>
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="reasonArguments">The arguments used in the <paramref name="reason"/> format string.</param>
        /// <returns>A newly created validation violation.</returns>
        IValidationViolation CreateValidationViolation(string reason, params object[] reasonArguments);

        /// <summary>
        /// <para>Creates a validation violation.</para>
        /// <para>Use this overload if you have an associated message.</para>
        /// </summary>
        /// <param name="reason">The reason for this validation.</param>
        /// <param name="messageId">The GUID of the message describing the reason for this violation.</param>
        /// <param name="messageArguments">The arguments used in the message referenced by <paramref name="messageId"/>.</param>
        /// <returns>A newly created validation violation.</returns>
        IValidationViolation CreateValidationViolation(string reason, Guid messageId, params object[] messageArguments);

        /// <summary>
        /// Creates a validation violation.
        /// </summary>
        /// <param name="data">Object containing detailed information about the violation.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="messageId">The GUID identifying the message describing the reason.</param>
        /// <param name="messageArguments">The message arguments.</param>
        /// <returns>A newly created validation violation.</returns>
        IValidationViolation CreateValidationViolation(object data, string reason, Guid messageId, params object[] messageArguments);
    }
}
