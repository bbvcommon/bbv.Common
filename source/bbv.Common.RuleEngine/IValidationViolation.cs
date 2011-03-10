//-------------------------------------------------------------------------------
// <copyright file="IValidationViolation.cs" company="bbv Software Services AG">
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
    /// Describes the reason why an <see cref="IValidationResult"/> is failed.
    /// <para>
    /// You can use either the <see cref="Reason"/> to describe why the validation failed
    /// or the <see cref="MessageId"/> to identify a message or a combination of both.
    /// </para>
    /// </summary>
    public interface IValidationViolation
    {
        /// <summary>
        /// Gets or sets the reason for the violation.
        /// </summary>
        /// <value>The reason.</value>
        string Reason { get; set; }

        /// <summary>
        /// Gets or sets the GUID to identify messages used to describe the reason.
        /// </summary>
        /// <value>The message GUID.</value>
        Guid MessageId { get; set; }

        /// <summary>
        /// Gets or sets the arguments used in the message identified by <see cref="MessageId"/>.
        /// </summary>
        /// <value>The message arguments.</value>
        object[] MessageArguments { get; set; }

        /// <summary>
        /// Gets or sets an object describing the violation in detail.
        /// You can use this property to communicate the details about the violation to the caller
        /// of the validation.
        /// </summary>
        /// <value>Object describing the violation in detail.</value>
        object Data { get; set; }
    }
}
