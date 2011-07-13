//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapper_is_shutdown.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Specification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using bbv.Common.Bootstrapper.Specification.Dummies;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern)]
    public class When_the_bootstrapper_is_shutdown : BootstrapperSpecification
    {
        Establish context = () =>
        {
            Bootstrapper.Initialize(Strategy);
            Bootstrapper.AddExtension(First);
            Bootstrapper.AddExtension(Second);
        };

        Because of = () =>
        {
            Bootstrapper.Shutdown();
        };

        It should_only_initialize_once_for_all_extensions = () =>
        {
            Strategy.ShutdownConfigurationInitializerAccessCounter.Should().Be(1);
        };

        It should_pass_the_initialized_values_to_the_extension = () =>
        {
            var expected = new KeyValuePair<string, string>("ShutdownTest", "ShutdownTestValue");

            First.ShutdownConfiguration.Should().BeEquivalentTo(expected);
            Second.ShutdownConfiguration.Should().BeEquivalentTo(expected);

            First.Unregistered.Should().Be("ShutdownTest");
            Second.Unregistered.Should().Be("ShutdownTest");
        };

        It should_execute_the_extensions_and_the_extension_point_according_to_the_strategy_defined_order = () =>
        {
            var sequence = CustomExtensionBase.Sequence;

            sequence.Should().HaveCount(7, sequence.Flatten());
            sequence.ElementAt(0).Should().StartWith("Action: CustomShutdown");

            sequence.ElementAt(1).Should().StartWith("SecondExtension: Unregister");
            sequence.ElementAt(2).Should().StartWith("FirstExtension: Unregister");

            sequence.ElementAt(3).Should().StartWith("SecondExtension: DeConfigure");
            sequence.ElementAt(4).Should().StartWith("FirstExtension: DeConfigure");

            sequence.ElementAt(5).Should().StartWith("SecondExtension: Stop");
            sequence.ElementAt(6).Should().StartWith("FirstExtension: Stop");
        };
    }
}