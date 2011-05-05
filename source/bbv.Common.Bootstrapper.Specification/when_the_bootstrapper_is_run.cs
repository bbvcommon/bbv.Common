//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapper_is_run.cs" company="bbv Software Services AG">
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

    public class When_the_bootstrapper_is_run : BootstrapperSpecification
    {
        Establish context = () =>
            {
                Bootstrapper.Initialize(Strategy);
                Bootstrapper.AddExtension(First);
                Bootstrapper.AddExtension(Second);
            };

        Because of = () =>
            {
                Bootstrapper.Run();
            };

        It should_execute_the_extensions_in_the_correct_order = () =>
            {
                var sequence = CustomExtensionBase.Sequence;

                sequence.Should().HaveCount(8);
                sequence.ElementAt(0).Should().StartWith("FirstExtension");
                sequence.ElementAt(1).Should().StartWith("SecondExtension");

                sequence.ElementAt(2).Should().StartWith("FirstExtension");
                sequence.ElementAt(3).Should().StartWith("SecondExtension");

                sequence.ElementAt(4).Should().StartWith("FirstExtension");
                sequence.ElementAt(5).Should().StartWith("SecondExtension");

                sequence.ElementAt(6).Should().StartWith("FirstExtension");
                sequence.ElementAt(7).Should().StartWith("SecondExtension");
            };

        It should_only_initialize_once_for_all_extensions = () =>
            {
                Strategy.ConfigurationInitializerAccessCounter.Should().Be(1);
            };

        It should_pass_the_initialized_values_to_the_extension = () =>
            {
                var expected = new KeyValuePair<string, string>("Test", "TestValue");

                First.Configuration.Should().HaveCount(1).And.Contain(expected);
                Second.Configuration.Should().HaveCount(1).And.Contain(expected);

                First.Injected.Should().Be("Test");
                Second.Injected.Should().Be("Test");
            };

        It should_execute_the_extension_point_according_to_the_strategy_defined_order = () =>
            {
                var sequence = CustomExtensionBase.Sequence;
                var strippedSequence = sequence.Select(s => s.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Last().Trim()).Distinct();

                strippedSequence.Should().HaveCount(4);
                strippedSequence.ElementAt(0).Should().BeEquivalentTo("Start");
                strippedSequence.ElementAt(1).Should().BeEquivalentTo("Configure");
                strippedSequence.ElementAt(2).Should().BeEquivalentTo("Initialize");
                strippedSequence.ElementAt(3).Should().BeEquivalentTo("Inject");
            };
    }
}