//-------------------------------------------------------------------------------
// <copyright file="when_behavior_is_attached.cs" company="bbv Software Services AG">
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
    using System.Linq;

    using bbv.Common.Bootstrapper.Specification.Dummies;

    using FluentAssertions;

    using Machine.Specifications;

    public class when_behavior_is_attached : BootstrapperWithBehaviorSpecification
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

        It should_execute_the_behaviors_in_the_correct_order = () =>
        {
            var sequence = CustomExtensionBase.Sequence;

            sequence.Should().HaveCount(4);
            sequence.ElementAt(0).Should().StartWith("FirstExtension");
            sequence.ElementAt(1).Should().StartWith("SecondExtension");
            sequence.ElementAt(2).Should().StartWith("FirstExtension");
            sequence.ElementAt(3).Should().StartWith("SecondExtension");
        };
    }
}