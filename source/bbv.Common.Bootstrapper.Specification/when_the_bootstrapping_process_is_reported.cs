//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapping_process_is_reported.cs" company="bbv Software Services AG">
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
    using FluentAssertions;
    using Machine.Specifications;

    [Subject(Concern)]
    public class when_the_bootstrapping_process_is_reported : BootstrapperReportingSpecification
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
            Bootstrapper.Shutdown();
            Bootstrapper.Dispose();
        };

        It should_report_complete_bootstrapping_process = () =>
            {
                ReportingContext.Should().NotBeNull();
            };
    }
}