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

    using bbv.Common.Bootstrapper.Sample.Complex;
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

            var simpleBootstrapper = new DefaultBootstrapper<ISimpleExtension>();
            simpleBootstrapper.Initialize(new SimpleStrategy());

            simpleBootstrapper.AddExtension(new FirstSimpleExtension());
            simpleBootstrapper.AddExtension(new SecondSimpleExtension());
            simpleBootstrapper.AddExtension(new ThirdSimpleExtension());

            simpleBootstrapper.Run();
            simpleBootstrapper.Shutdown();

            Console.WriteLine("================== End simple ==================");
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();

            Console.WriteLine("================== Running complex ==================");
            Console.WriteLine();

            var complexBootstrapper = new DefaultBootstrapper<IComplexExtension>();
            complexBootstrapper.Initialize(new ComplexStrategy());

            complexBootstrapper.AddExtension(new Log4netExtension());
            complexBootstrapper.AddExtension(new ExtensionWhichRegistersSomething());
            complexBootstrapper.AddExtension(new ExtensionWhichNeedsDependency());
            complexBootstrapper.AddExtension(new ExtensionWhichIsFunqlet());
            complexBootstrapper.AddExtension(new ExtensionWithExtensionConfigurationSection());
            complexBootstrapper.AddExtension(new ExtensionWithExtensionConfigurationSectionWithConversionAndCustomizedLoading());
            complexBootstrapper.AddExtension(new ExtensionWithExtensionConfigurationSectionWithDictionary());

            complexBootstrapper.Run();
            complexBootstrapper.Shutdown();

            Console.WriteLine("================== End complex ==================");
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}