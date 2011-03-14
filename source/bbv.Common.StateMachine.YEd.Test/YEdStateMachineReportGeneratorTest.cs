//-------------------------------------------------------------------------------
// <copyright file="YEdStateMachineReportGeneratorTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.StateMachine.YEd
{
    using System;
    using System.IO;

    using Xunit;

    public class YEdStateMachineReportGeneratorTest
    {
        /// <summary>
        /// Some test states for simulating an elevator.
        /// </summary>
        private enum States
        {
            /// <summary>Elevator has an Error</summary>
            Error,

            /// <summary>Elevator is healthy, i.e. no error</summary>
            Healthy,

            /// <summary>The elevator is moving (either up or down)</summary>
            Moving,

            /// <summary>The elevator is moving down.</summary>
            MovingUp,

            /// <summary>The elevator is moving down.</summary>
            MovingDown,

            /// <summary>The elevator is standing on a floor.</summary>
            OnFloor,

            /// <summary>The door is closed while standing still.</summary>
            DoorClosed,

            /// <summary>The dor is open while standing still.</summary>
            DoorOpen
        }

        /// <summary>
        /// Some test events for the elevator
        /// </summary>
        private enum Events
        {
            /// <summary>An error occurred.</summary>
            ErrorOccured,

            /// <summary>Reset after error.</summary>
            Reset,

            /// <summary>Open the door.</summary>
            OpenDoor,

            /// <summary>Close the door.</summary>
            CloseDoor,

            /// <summary>Move elevator up.</summary>
            GoUp,

            /// <summary>Move elevator down.</summary>
            GoDown,

            /// <summary>Stop the elevator.</summary>
            Stop
        }

        [Fact(Skip = "not a real unit test")]
        public void YEdGraphML()
        {
            var elevator = new PassiveStateMachine<States, Events>("Elevator");
            
            elevator.DefineHierarchyOn(States.Healthy, States.OnFloor, HistoryType.Deep, States.OnFloor, States.Moving);
            elevator.DefineHierarchyOn(States.Moving, States.MovingUp, HistoryType.Shallow, States.MovingUp, States.MovingDown);
            elevator.DefineHierarchyOn(States.OnFloor, States.DoorClosed, HistoryType.None, States.DoorClosed, States.DoorOpen);

            elevator.In(States.Healthy)
                .On(Events.ErrorOccured).Goto(States.Error);

            elevator.In(States.Error)
                .On(Events.Reset).Goto(States.Healthy);

            elevator.In(States.OnFloor)
                .On(Events.CloseDoor).Goto(States.DoorClosed)
                .On(Events.OpenDoor).Goto(States.DoorOpen)
                .On(Events.GoUp)
                    .If(CheckOverload).Goto(States.MovingUp)
                    .Otherwise()
                .On(Events.GoDown)
                    .If(CheckOverload).Goto(States.MovingDown)
                    .Otherwise();

            elevator.In(States.Moving)
                .On(Events.Stop).Goto(States.OnFloor);

            elevator.Initialize(States.OnFloor);
            
            var stream = new MemoryStream();
            var testee = new YEdStateMachineReportGenerator<States, Events>(stream);

            elevator.Report(testee);

            stream.Position = 0;
            var reader = new StreamReader(stream);
            Console.WriteLine(reader.ReadToEnd());
        }

        private static bool CheckOverload(object[] arg)
        {
            return true;
        }
    }
}