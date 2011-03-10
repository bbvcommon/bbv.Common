//-------------------------------------------------------------------------------
// <copyright file="Background.cs" company="bbv Software Services AG">
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
    /// Handler that executes the subscription on a thread pool worker process (asynchronous).
    /// </summary>
    public class Background : IHandler
    {
        /// <summary>
        /// Gets the kind of the handler, whether it is a synchronous or asynchronous handler.
        /// </summary>
        /// <value>The kind of the handler (synchronous or asynchronous).</value>
        public HandlerKind Kind
        {
            get
            {
                return HandlerKind.Asynchronous;
            }
        }

        /// <summary>
        /// Initializes the handler.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">Name of the handler method on the subscriber.</param>
        public void Initialize(object subscriber, MethodInfo handlerMethod)
        {
            // there is nothing to initialize
        }

        /// <summary>
        /// Executes the subscription on a thread pool worker thread.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="subscriptionHandler">The subscription handler.</param>
        /// <returns>Returns null. Asynchronous operation cannot return exception here.</returns>
        public Exception Handle(object sender, EventArgs e, Delegate subscriptionHandler)
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object state)
                    {
                        CallInBackgroundArguments args = (CallInBackgroundArguments)state;
                        args.Handler.DynamicInvoke(args.Sender, args.EventArgs);
                    },
                new CallInBackgroundArguments(sender, e, subscriptionHandler));

            return null;
        }

        /// <summary>
        /// Struct that is passed to the background worker thread.
        /// </summary>
        private struct CallInBackgroundArguments
        {
            /// <summary>
            /// The event topic handler method on the subscriber.
            /// </summary>
            public readonly Delegate Handler;

            /// <summary>
            /// The publisher sending the event.
            /// </summary>
            public readonly object Sender;

            /// <summary>
            /// The event args of the event.
            /// </summary>
            public readonly EventArgs EventArgs;

            /// <summary>
            /// Initializes a new instance of the <see cref="Background.CallInBackgroundArguments"/> struct.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            /// <param name="handler">The handler.</param>
            public CallInBackgroundArguments(object sender, EventArgs eventArgs, Delegate handler)
            {
                this.Sender = sender;
                this.EventArgs = eventArgs;
                this.Handler = handler;
            }
        }
    }
}