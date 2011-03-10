//-------------------------------------------------------------------------------
// <copyright file="ValidationViolation.cs" company="bbv Software Services AG">
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
    using System.Globalization;
    using Formatters;

    /// <summary>
    /// Describes the reason why a validation failed.
    /// A <see cref="IValidationRule"/> can generate multiple violations.
    /// </summary>
    public class ValidationViolation : IValidationViolation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationViolation"/> class.
        /// </summary>
        public ValidationViolation()
        {
            this.MessageId = Guid.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationViolation"/> class.
        /// </summary>
        /// <param name="reason">The reason why the validation rule failed.</param>
        public ValidationViolation(string reason)
        {
            this.MessageId = Guid.Empty;
            this.Reason = reason;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationViolation"/> class.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="messageId">The message GUID.</param>
        /// <param name="messageArguments">The message arguments.</param>
        public ValidationViolation(string reason, Guid messageId, params object[] messageArguments)
        {
            this.Reason = reason;
            this.MessageId = messageId;
            this.MessageArguments = messageArguments;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationViolation"/> class.
        /// </summary>
        /// <param name="data">Object containing detailed information about the violation.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="messageId">The message GUID.</param>
        /// <param name="messageArguments">The message arguments.</param>
        public ValidationViolation(object data, string reason, Guid messageId, params object[] messageArguments)
        {
            this.Data = data;
            this.Reason = reason;
            this.MessageId = messageId;
            this.MessageArguments = messageArguments;
        }

        #region IValidationViolation Members

        /// <summary>
        /// Gets or sets the reason for the violation.
        /// </summary>
        /// <value>The reason.</value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the GUID to identify messages used to describe the reason.
        /// </summary>
        /// <value>The message GUID.</value>
        public Guid MessageId { get; set; }

        /// <summary>
        /// Gets or sets the arguments used in the message identified by <see cref="IValidationViolation.MessageId"/>.
        /// </summary>
        /// <value>The message arguments.</value>
        public object[] MessageArguments { get; set; }

        /// <summary>
        /// Gets or sets an object describing the violation in detail.
        /// You can use this property to communicate the details about the violation to the caller
        /// of the validation.
        /// </summary>
        /// <value>The data that explains the violation in detail.</value>
        public object Data { get; set; }

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "GUID '{0}' reason '{1}' arguments '{2}'", 
                this.MessageId, 
                this.Reason,
                FormatHelper.ConvertToString(this.MessageArguments, ", "));
        }
    }
}
