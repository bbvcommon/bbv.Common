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

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "bbv.Common.Threading.UserInterfaceThreadSynchronizer.Log(System.String,System.Int32,System.String,System.Delegate)", Scope = "member", Target = "bbv.Common.Threading.UserInterfaceThreadSynchronizer.#LogAsynchronous(System.Delegate,System.Int32,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "bbv.Common.Threading.UserInterfaceThreadSynchronizer.Log(System.String,System.Int32,System.String,System.Delegate)", Scope = "member", Target = "bbv.Common.Threading.UserInterfaceThreadSynchronizer.#LogSynchronous(System.Delegate,System.Int32,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "log4net.ILog.DebugFormat(System.String,System.Object[])", Scope = "member", Target = "bbv.Common.Threading.UserInterfaceThreadSynchronizer.#Log(System.String,System.Int32,System.String,System.Delegate)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "log4net.ILog.DebugFormat(System.String,System.Object[])", Scope = "member", Target = "bbv.Common.Threading.UserInterfaceThreadSynchronizer.#LogSynchronous`1(System.Delegate,System.Int32,System.String,!!0)")]
