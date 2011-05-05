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

    public class CustomExtensionBase : ICustomExtension
    {
        private static readonly Queue<string> sequenceQueue = new Queue<string>();

        public static IEnumerable<string> Sequence
        {
            get
            {
                return sequenceQueue;
            }
        }

        public IDictionary<string, string> Configuration
        {
            get; private set;
        }

        public string Injected
        {
            get; private set;
        }

        public void Start()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        public void Configure(IDictionary<string, string> configuration)
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);

            this.Configuration = configuration;
        }

        public void Initialize()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        public void Inject(string magic)
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);

            this.Injected = magic;
        }

        public void Stop()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        public void Dispose()
        {
            this.Dump(MethodBase.GetCurrentMethod().Name);
        }

        private void Dump(string methodName)
        {
            sequenceQueue.Enqueue(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", this.GetType().Name, methodName));
        }
    }
}