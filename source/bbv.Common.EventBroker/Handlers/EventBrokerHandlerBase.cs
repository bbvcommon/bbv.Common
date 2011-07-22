//-------------------------------------------------------------------------------
// <copyright file="EventBrokerHandlerBase.cs" company="bbv Software Services AG">
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

    using bbv.Common.EventBroker.Internals;

    public abstract class EventBrokerHandlerBase : IHandler
    {
        protected IExtensionHost ExtensionHost { get; private set; }

        public abstract HandlerKind Kind { get; }

        public virtual void Initialize(object subscriber, MethodInfo handlerMethod, IExtensionHost extensionHost)
        {
            this.ExtensionHost = extensionHost;
        }

        public abstract void Handle(IEventTopic eventTopic, object sender, EventArgs e, Delegate subscriptionHandler);

        protected void HandleSubscriberMethodException(TargetInvocationException ex, IEventTopic eventTopic)
        {
            ex.PreserveStackTrace();

            var context = new ExceptionHandlingContext();

            this.ExtensionHost.ForEach(extension => extension.SubscriberExceptionOccurred(eventTopic, ex.InnerException, context));
                
            if (!context.Handled)
            {
                throw ex.InnerException;
            }
        }
    }
}