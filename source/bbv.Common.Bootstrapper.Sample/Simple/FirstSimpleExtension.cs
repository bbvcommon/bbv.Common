//-------------------------------------------------------------------------------
// <copyright file="FirstSimpleExtension.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Sample.Simple
{
    using System;

    /// <summary>
    /// First simple extension.
    /// </summary>
    public class FirstSimpleExtension : SimpleExtensionBase
    {
        /// <inheritdoc />
        public override void Start()
        {
            base.Start();

            Console.WriteLine("First Simple Extension is starting.");
        }

        /// <inheritdoc />
        public override void Shutdown()
        {
            base.Shutdown();

            Console.WriteLine("First Simple Extension is shutting down.");
        }

        /// <inheritdoc />
        public override string Describe()
        {
            return "First simple extension";
        }
    }
}