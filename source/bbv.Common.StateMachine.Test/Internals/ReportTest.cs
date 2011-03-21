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
        [Fact]
        public void Report()
        {
            this.testee.DefineHierarchyOn(States.B, States.B1, HistoryType.None, States.B1, States.B2);
            this.testee.DefineHierarchyOn(States.C, States.C1, HistoryType.Shallow, States.C1, States.C2);
            this.testee.DefineHierarchyOn(States.C1, States.C1a, HistoryType.Shallow, States.C1a, States.C1b);
            this.testee.DefineHierarchyOn(States.D, States.D1, HistoryType.Deep, States.D1, States.D2);
            this.testee.DefineHierarchyOn(States.D1, States.D1a, HistoryType.Deep, States.D1a, States.D1b);

            this.testee.In(States.A)
                .ExecuteOnEntry(EnterA)
                .ExecuteOnExit(ExitA)
                .On(Events.A)
                .On(Events.B).Goto(States.B)
                .On(Events.C).If(eventArguments => true).Goto(States.C1)
                .On(Events.C).If(eventArguments => false).Goto(States.C2);

            this.testee.In(States.B)
                .On(Events.A).Goto(States.A).Execute(Action);

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
        entry action: 
        exit action: 
        A -> A actions: Action guard:
        B1: initial state = None history type = None
            entry action: 
            exit action: 
            B2 -> B1 actions:  guard:
        B2: initial state = None history type = None
            entry action: 
            exit action: 
            B1 -> B2 actions:  guard:
    C: initial state = C1 history type = Shallow
        entry action: 
        exit action: 
        C1: initial state = C1a history type = Shallow
            entry action: 
            exit action: 
            C1a: initial state = None history type = None
                entry action: 
                exit action: 
            C1b: initial state = None history type = None
                entry action: 
                exit action: 
        C2: initial state = None history type = None
            entry action: 
            exit action: 
    D: initial state = D1 history type = Deep
        entry action: 
        exit action: 
        D1: initial state = D1a history type = Deep
            entry action: 
            exit action: 
            D1a: initial state = None history type = None
                entry action: 
                exit action: 
            D1b: initial state = None history type = None
                entry action: 
                exit action: 
        D2: initial state = None history type = None
            entry action: 
            exit action: 
    A: initial state = None history type = None
        entry action: EnterA
        exit action: ExitA
        A -> internal actions:  guard:
        B -> B actions:  guard:
        C -> C1 actions:  guard:anonymous
        C -> C2 actions:  guard:anonymous
";
            Assert.Equal(ExpectedReport, report);
        }

        private static void EnterA()
        {
        }

        private static void ExitA()
        {
        }

        private static void Action()
        {
        }
    }
}