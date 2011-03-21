//-------------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="bbv Software Services AG">
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

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IState`2.#Fire(bbv.Common.StateMachine.Internals.ITransitionContext`2<!0,!1>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IStateContext`2.#GetRecords()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IStateDictionary`2.#GetStates()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "bbv.Common.StateMachine.Internals.TransitionDictionary`2.#GetTransitions()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Scope = "member", Target = "bbv.Common.StateMachine.IStateMachine`2.#Fire(!1,System.Object[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Scope = "member", Target = "bbv.Common.StateMachine.IStateMachine`2.#FirePriority(!1,System.Object[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Scope = "member", Target = "bbv.Common.StateMachine.Internals.ITransition`2.#Fire(bbv.Common.StateMachine.Internals.ITransitionContext`2<!0,!1>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Scope = "member", Target = "bbv.Common.StateMachine.Internals.StateMachine`2.#Fire(!1)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Scope = "member", Target = "bbv.Common.StateMachine.Internals.StateMachine`2.#Fire(!1,System.Object[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "bbv.Common.StateMachine.Internals.State`2.#ExecuteEntryAction(bbv.Common.StateMachine.Internals.IStateContext`2<!0,!1>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "bbv.Common.StateMachine.Internals.State`2.#ExecuteExitAction(bbv.Common.StateMachine.Internals.IStateContext`2<!0,!1>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "bbv.Common.StateMachine.Internals.Transition`2.#PerformActions(System.Object[],bbv.Common.StateMachine.Internals.ITransitionContext`2<!0,!1>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "bbv.Common.StateMachine.Internals.Transition`2.#ShouldFire(System.Object[],bbv.Common.StateMachine.Internals.ITransitionContext`2<!0,!1>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "2#", Scope = "member", Target = "bbv.Common.StateMachine.IExtension`2.#FiringEvent(bbv.Common.StateMachine.IStateMachineInformation`2<!0,!1>,!1&,System.Object[]&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#", Scope = "member", Target = "bbv.Common.StateMachine.IExtension`2.#FiringEvent(bbv.Common.StateMachine.IStateMachineInformation`2<!0,!1>,!1&,System.Object[]&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#", Scope = "member", Target = "bbv.Common.StateMachine.IExtension`2.#HandlingEntryActionException(bbv.Common.StateMachine.IStateMachineInformation`2<!0,!1>,bbv.Common.StateMachine.Internals.IState`2<!0,!1>,bbv.Common.StateMachine.Internals.IStateContext`2<!0,!1>,System.Exception&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#", Scope = "member", Target = "bbv.Common.StateMachine.IExtension`2.#HandlingExitActionException(bbv.Common.StateMachine.IStateMachineInformation`2<!0,!1>,bbv.Common.StateMachine.Internals.IState`2<!0,!1>,bbv.Common.StateMachine.Internals.IStateContext`2<!0,!1>,System.Exception&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#", Scope = "member", Target = "bbv.Common.StateMachine.IExtension`2.#HandlingGuardException(bbv.Common.StateMachine.IStateMachineInformation`2<!0,!1>,bbv.Common.StateMachine.Internals.ITransition`2<!0,!1>,bbv.Common.StateMachine.Internals.ITransitionContext`2<!0,!1>,System.Exception&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "3#", Scope = "member", Target = "bbv.Common.StateMachine.IExtension`2.#HandlingTransitionException(bbv.Common.StateMachine.IStateMachineInformation`2<!0,!1>,bbv.Common.StateMachine.Internals.ITransition`2<!0,!1>,bbv.Common.StateMachine.Internals.ITransitionContext`2<!0,!1>,System.Exception&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#", Scope = "member", Target = "bbv.Common.StateMachine.IExtension`2.#InitializingStateMachine(bbv.Common.StateMachine.IStateMachineInformation`2<!0,!1>,!0&)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "bbv.Common.StateMachine.TransitionEventArgs`2.#EventArguments")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "bbv.Common.StateMachine.Internals.ITransitionContext`2.#EventArguments")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "bbv.Common.StateMachine.Internals.EventInformation`1.#EventArguments")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Stop", Scope = "member", Target = "bbv.Common.StateMachine.IStateMachine`2.#Stop()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "In", Scope = "member", Target = "bbv.Common.StateMachine.IStateMachine`2.#In(!0)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Exit", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IState`2.#Exit(bbv.Common.StateMachine.Internals.IStateContext`2<!0,!1>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "GoTo", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IOtherwiseSyntax`2.#Goto(!0)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "If", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IOnSyntax`2.#If(System.Func`2<System.Object[],System.Boolean>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "GoTo", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IOnSyntax`2.#Goto(!0)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "GoTo", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IIfSyntax`2.#Goto(!0)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "If", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IIfOrOtherwiseSyntax`2.#If(System.Func`2<System.Object[],System.Boolean>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "On", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IEventSyntax`2.#On(!1)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "bbv.Common.StateMachine.Internals.TransitionDictionary`2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "bbv.Common.StateMachine.Internals.StateDictionary`2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "bbv.Common.StateMachine.Internals.IStateDictionary`2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "bbv.Common.StateMachine.Internals.State`2.#ExecuteEntryAction(bbv.Common.StateMachine.Internals.IActionHolder,bbv.Common.StateMachine.Internals.IStateContext`2<!0,!1>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "bbv.Common.StateMachine.Internals.State`2.#ExecuteExitAction(bbv.Common.StateMachine.Internals.IActionHolder,bbv.Common.StateMachine.Internals.IStateContext`2<!0,!1>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "bbv.Common.StateMachine.Internals.TransitionDictionary`2+TransitionInfo")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "If", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IIfOrOtherwiseSyntax`2.#If(System.Func`1<System.Boolean>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "If", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IIfOrOtherwiseSyntax`2.#If`1(System.Func`2<!!0,System.Boolean>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "If", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IOnSyntax`2.#If(System.Func`1<System.Boolean>)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "If", Scope = "member", Target = "bbv.Common.StateMachine.Internals.IOnSyntax`2.#If`1(System.Func`2<!!0,System.Boolean>)")]
