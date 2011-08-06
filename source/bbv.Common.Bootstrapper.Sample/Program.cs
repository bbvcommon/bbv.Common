//-------------------------------------------------------------------------------
// <copyright file="Program.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Sample
{
    using System;

    using bbv.Common.Bootstrapper.Sample.Simple;

    /// <summary>
    /// The program whic is executed
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Console.WriteLine("================== Running simple ==================");
            Console.WriteLine();

            var bootstrapper = new DefaultBootstrapper<ISimpleExtension>();
            bootstrapper.Initialize(new SimpleStrategy());

            bootstrapper.AddExtension(new FirstSimpleExtension());
            bootstrapper.AddExtension(new SecondSimpleExtension());
            bootstrapper.AddExtension(new ThirdSimpleExtension());

            bootstrapper.Run();
            bootstrapper.Shutdown();

            Console.ReadLine();
        }
    }
}