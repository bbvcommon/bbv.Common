//-------------------------------------------------------------------------------
// <copyright file="DateTimeProvider.cs" company="bbv Software Services AG">
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

namespace bbv.Common
{
    using System;

    /// <summary>
    /// The date time provider implements <see cref="IDateTimeProvider"/> to return date and time information.
    /// The date time provider can be used to abstract access to the <see cref="DateTime"/> class to support
    /// better testability by mocking e.g. <see cref="DateTime.Now"/>.
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Gets a <see cref="DateTime"/> representing actual date and time. Returns <see cref="DateTime.Now"/>
        /// </summary>
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> representing today. Returns <see cref="DateTime.Today"/>
        /// </summary>
        public DateTime Today
        {
            get { return DateTime.Today; }
        }
    }
}