//-------------------------------------------------------------------------------
// <copyright file="ReportTest.cs" company="bbv Software Services AG">
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
namespace bbv.Common.StateMachine.Internals
{
    using FluentAssertions;

    using Xunit;

    /// <summary>
    /// Tests the report functionality of the state machine.
    /// </summary>
    public class ReportTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private readonly StateMachine<States, Events> testee;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTest"/> class.
        /// </summary>
        public ReportTest()
        {
            this.testee = new StateMachine<States, Events>("Test Machine");
        }

        /// <summary>
        /// The report can be created.
        /// </summary>
        [Fact()]
        public void Report()
        {
            this.testee.DefineHierarchyOn(States.B, States.B1, HistoryType.None, States.B1, States.B2);
            this.testee.DefineHierarchyOn(States.C, States.C1, HistoryType.Shallow, States.C1, States.C2);
            this.testee.DefineHierarchyOn(States.C1, States.C1a, HistoryType.Shallow, States.C1a, States.C1b);
            this.testee.DefineHierarchyOn(States.D, States.D1, HistoryType.Deep, States.D1, States.D2);
            this.testee.DefineHierarchyOn(States.D1, States.D1a, HistoryType.Deep, States.D1a, States.D1b);

            this.testee.In(States.A)
                .ExecuteOnEntry(() => { })
                .ExecuteOnExit(() => { })
                .On(Events.A)
                .On(Events.B).Goto(States.B)
                .On(Events.C).If(eventArguments => true).Goto(States.C1)
                .On(Events.C).If(eventArguments => false).Goto(States.C2);

            this.testee.In(States.B)
                .On(Events.A).Goto(States.A);

            this.testee.In(States.B1)
                .On(Events.B2).Goto(States.B1);

            this.testee.In(States.B2)
                .On(Events.B1).Goto(States.B2);
            
            this.testee.Initialize(States.A);

            var generator = new StateMachineReport<States, Events>();
            this.testee.Report(generator);

            string report = generator.Result;

            const string ExpectedReport =
@"Test Machine: initial state = A
    B: initial state = B1 history type = None
        entry action: False
        exit action: False
        A -> A actions: 0 guard:False
        B1: initial state = None history type = None
            entry action: False
            exit action: False
            B2 -> B1 actions: 0 guard:False
        B2: initial state = None history type = None
            entry action: False
            exit action: False
            B1 -> B2 actions: 0 guard:False
    C: initial state = C1 history type = Shallow
        entry action: False
        exit action: False
        C1: initial state = C1a history type = Shallow
            entry action: False
            exit action: False
            C1a: initial state = None history type = None
                entry action: False
                exit action: False
            C1b: initial state = None history type = None
                entry action: False
                exit action: False
        C2: initial state = None history type = None
            entry action: False
            exit action: False
    D: initial state = D1 history type = Deep
        entry action: False
        exit action: False
        D1: initial state = D1a history type = Deep
            entry action: False
            exit action: False
            D1a: initial state = None history type = None
                entry action: False
                exit action: False
            D1b: initial state = None history type = None
                entry action: False
                exit action: False
        D2: initial state = None history type = None
            entry action: False
            exit action: False
    A: initial state = None history type = None
        entry action: True
        exit action: True
        A -> internal actions: 0 guard:False
        B -> B actions: 0 guard:False
        C -> C1 actions: 0 guard:True
        C -> C2 actions: 0 guard:True
";
            Assert.Equal(ExpectedReport, report);
        }

        /// <summary>
        /// The report can be created.
        /// </summary>
        [Fact()]
        public void ReportVariant()
        {
            this.testee.DefineHierarchyOn(States.B, States.B1, HistoryType.None, States.B1, States.B2);
            this.testee.DefineHierarchyOn(States.C, States.C1, HistoryType.Shallow, States.C1, States.C2);
            this.testee.DefineHierarchyOn(States.C1, States.C1a, HistoryType.Shallow, States.C1a, States.C1b);
            this.testee.DefineHierarchyOn(States.D, States.D1, HistoryType.Deep, States.D1, States.D2);
            this.testee.DefineHierarchyOn(States.D1, States.D1a, HistoryType.Deep, States.D1a, States.D1b);

            this.testee.In(States.A)
                .ExecuteOnEntry(() => { })
                .ExecuteOnExit(() => { })
                .On(Events.A)
                .On(Events.B).Goto(States.B)
                .On(Events.C).If(eventArguments => true).Goto(States.C1)
                .On(Events.C).If(eventArguments => false).Goto(States.C2);

            this.testee.In(States.B)
                .On(Events.A).Goto(States.A);

            this.testee.In(States.B1)
                .On(Events.B2).Goto(States.B1);

            this.testee.In(States.B2)
                .On(Events.B1).Goto(States.B2);

            this.testee.Initialize(States.A);

            var generator = new StateMachineReport<States, Events>();
            this.testee.Report(generator);

            string report = generator.Result;

            const string ExpectedReport =
@"Test Machine: initial state = A
    B: initial state = B1 history type = None
        entry action: False
        exit action: False
        A -> A actions: 0 guard:False
        B1: initial state = None history type = None
            entry action: False
            exit action: False
            B2 -> B1 actions: 0 guard:False
        B2: initial state = None history type = None
            entry action: False
            exit action: False
            B1 -> B2 actions: 0 guard:False
    C: initial state = C1 history type = Shallow
        entry action: False
        exit action: False
        C1: initial state = C1a history type = Shallow
            entry action: False
            exit action: False
            C1a: initial state = None history type = None
                entry action: False
                exit action: False
            C1b: initial state = None history type = None
                entry action: False
                exit action: False
        C2: initial state = None history type = None
            entry action: False
            exit action: False
    D: initial state = D1 history type = Deep
        entry action: False
        exit action: False
        D1: initial state = D1a history type = Deep
            entry action: False
            exit action: False
            D1a: initial state = None history type = None
                entry action: False
                exit action: False
            D1b: initial state = None history type = None
                entry action: False
                exit action: False
        D2: initial state = None history type = None
            entry action: False
            exit action: False
    A: initial state = None history type = None
        entry action: True
        exit action: True
        A -> internal actions: 0 guard:False
        B -> B actions: 0 guard:False
        C -> C1 actions: 0 guard:True
        C -> C2 actions: 0 guard:True
";
            report.Should().Be(ExpectedReport);
        }
    }
}