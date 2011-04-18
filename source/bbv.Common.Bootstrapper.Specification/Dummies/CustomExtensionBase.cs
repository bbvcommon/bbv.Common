//-------------------------------------------------------------------------------
// <copyright file="CustomExtensionBase.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Specification.Dummies
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Text;

    public class CustomExtensionBase : ICustomExtension
    {
        private readonly StringBuilder stringBuilder;

        public CustomExtensionBase()
        {
            this.stringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        public void Start()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        public void Configure(IDictionary<string, string> configuration)
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);

            foreach (KeyValuePair<string, string> keyValuePair in configuration)
            {
                this.stringBuilder.AppendFormat(
                    CultureInfo.InvariantCulture, "{0}, {1}{2}", keyValuePair.Key, keyValuePair.Value, Environment.NewLine);
            }
        }

        public void Stop()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        private void Dump(string methodName)
        {
            this.stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", this.GetType().Name, methodName));
        }
    }
}