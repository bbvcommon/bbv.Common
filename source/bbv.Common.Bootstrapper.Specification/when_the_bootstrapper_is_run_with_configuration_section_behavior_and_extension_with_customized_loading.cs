//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapper_is_run_with_configuration_section_behavior_and_extension_with_customized_loading.cs" company="bbv Software Services AG">
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
    using bbv.Common.Bootstrapper.Specification.Dummies;

    using FluentAssertions;

    using Machine.Specifications;

    [Subject(Concern)]
    public class when_the_bootstrapper_is_run_with_configuration_section_behavior_and_extension_with_customized_loading : BootstrapperWithConfigurationSectionBehaviorSpecification
    {
        protected static CustomExtensionWithConfiguration Extension;

        Establish context = () =>
        {
            Extension = new CustomExtensionWithConfiguration();

            Bootstrapper.Initialize(Strategy);
            Bootstrapper.AddExtension(Extension);
        };

        Because of = () =>
        {
            Bootstrapper.Run();
        };

        It should_apply_configuration_section = () =>
            {
                Extension.AppliedSection.Should().NotBeNull();
            };

        It should_acquire_section_name = () =>
            {
                Extension.SectionNameAcquired.Should().BeTrue();
            };

        It should_acquire_section = () =>
        {
            Extension.SectionAcquired.Should().BeTrue();
        };
    }
}