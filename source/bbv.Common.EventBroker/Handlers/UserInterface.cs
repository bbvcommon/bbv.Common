//-------------------------------------------------------------------------------
// <copyright file="UserInterface.cs" company="bbv Software Services AG">
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

namespace bbv.Common.EventBroker.Handlers
{
    using System;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    /// Handler that executes the subscription synchronously on the user interface thread (Send semantics).
    /// </summary>
    public class UserInterface : IHandler
    {
        /// <summary>
        /// The synchronization context that is used to switch to the UI thread.
        /// </summary>
        private readonly UserInterfaceSyncContextHolder syncContextHolder = new UserInterfaceSyncContextHolder();

        /// <summary>
        /// Gets the kind of the handler, whether it is a synchronous or asynchronous handler.
        /// </summary>
        /// <value>The kind of the handler (synchronous or asynchronous).</value>
        public HandlerKind Kind
        {
            get { return HandlerKind.Synchronous; }
        }

        /// <summary>
        /// Initializes the handler with the synchronization context for the user interface thread, which has to be the currently running process.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">Handler method on the subscriber.</param>
        public void Initialize(object subscriber, MethodInfo handlerMethod)
        {
            this.syncContextHolder.Initalize(subscriber, handlerMethod);
        }

        /// <summary>
        /// Executes the subscription synchronously on the user interface thread.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="subscriptionHandler">The subscription handler.</param>
        /// <returns>
        /// The exception that occurred during handling of the event. Null if no exception occurred
        /// </returns>
        public Exception Handle(object sender, EventArgs e, Delegate subscriptionHandler)
        {
            return this.RunningOnUserInterfaceThread() ? 
                this.CallWithoutThreadSwitch(subscriptionHandler, sender, e) : 
                this.CallWithThreadSwitch(subscriptionHandler, sender, e);
        }

        private bool RunningOnUserInterfaceThread()
        {
            return Thread.CurrentThread.ManagedThreadId == this.syncContextHolder.ThreadId;
        }

        private Exception CallWithoutThreadSwitch(Delegate subscriptionHandler, object sender, EventArgs e)
        {
            try
            {
                subscriptionHandler.DynamicInvoke(sender, e);

                return null;
            }
            catch (Exception exception)
            {
                return exception;
            }
        }

        private Exception CallWithThreadSwitch(Delegate subscriptionHandler, object sender, EventArgs e)
        {
            Exception exception = null;

            this.syncContextHolder.SyncContext.Send(
                delegate(object data)
                    {
                        try
                        {
                            ((Delegate)data).DynamicInvoke(sender, e);
                        }
                        catch (TargetInvocationException ex)
                        {
                            exception = ex;
                        }
                    },
                subscriptionHandler);

            return exception;
        }
    }
}