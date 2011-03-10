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

namespace bbv.Common.StateMachine.Sample
{
    using System;

    /// <summary>
    /// Main entry point
    /// </summary>
    public static class Program
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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // configure basic logging (all levels enabled, messages are written to the console)
            log4net.Config.BasicConfigurator.Configure();

            var elevator = new PassiveStateMachine<States, Events>("Elevator");
            elevator.AddExtension(new Extensions.Log4NetExtension<States, Events>("Elevator"));

            elevator.DefineHierarchyOn(States.Healthy, States.OnFloor, HistoryType.Deep, States.OnFloor, States.Moving);
            elevator.DefineHierarchyOn(States.Moving, States.MovingUp, HistoryType.Shallow, States.MovingUp, States.MovingDown);
            elevator.DefineHierarchyOn(States.OnFloor, States.DoorClosed, HistoryType.None, States.DoorClosed, States.DoorOpen);

            elevator.In(States.Healthy)
                .On(Events.ErrorOccured).Goto(States.Error);

            elevator.In(States.Error)
                .On(Events.Reset).Goto(States.Healthy);

            elevator.In(States.OnFloor)
                .ExecuteOnEntry(AnnounceFloor)
                .On(Events.CloseDoor).Goto(States.DoorClosed)
                .On(Events.OpenDoor).Goto(States.DoorOpen)
                .On(Events.GoUp)
                    .If(CheckOverload).Goto(States.MovingUp)
                    .Otherwise().Execute(AnnounceOverload)
                .On(Events.GoDown)
                    .If(CheckOverload).Goto(States.MovingDown)
                    .Otherwise().Execute(AnnounceOverload);

            elevator.In(States.Moving)
                .On(Events.Stop).Goto(States.OnFloor);

            elevator.Initialize(States.OnFloor);

            elevator.Fire(Events.ErrorOccured);
            elevator.Fire(Events.Reset);

            elevator.Start();

            elevator.Fire(Events.OpenDoor);
            elevator.Fire(Events.CloseDoor);
            elevator.Fire(Events.GoUp);
            elevator.Fire(Events.Stop);
            elevator.Fire(Events.OpenDoor);

            elevator.Stop();

            Console.ReadLine();
        }

        /// <summary>
        /// Announces the floor.
        /// </summary>
        private static void AnnounceFloor()
        {
        }

        /// <summary>
        /// Announces that the elevator is overloaded.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        private static void AnnounceOverload(object[] arguments)
        {
        }

        /// <summary>
        /// Checks whether the elevator is overloaded.
        /// </summary>
        /// <param name="arguments">The transition arguments.</param>
        /// <returns>Whether elevator is overloaded. Unfortunately always true here.</returns>
        private static bool CheckOverload(object[] arguments)
        {
            return true;
        }
    }
}