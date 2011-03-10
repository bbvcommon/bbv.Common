//-------------------------------------------------------------------------------
// <copyright file="Log4NetHelper.cs" company="bbv Software Services AG">
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

namespace bbv.Common.TestUtilities
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Filter;

    /// <summary>
    /// Helper class for unit testing log entries.
    /// </summary>
    public class Log4netHelper : IDisposable
    {
        /// <summary>
        /// Log4net Appender for testing log.
        /// </summary>
        private readonly MemoryAppender logAppender;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4netHelper"/> class.
        /// </summary>
        public Log4netHelper()
            : this(new IFilter[] { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4netHelper"/> class.
        /// </summary>
        /// <param name="loggerName">Name of the logger. Only messages of this logger will be collected.</param>
        public Log4netHelper(string loggerName)
            : this(new IFilter[] { new LoggerMatchFilter { LoggerToMatch = loggerName } })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4netHelper"/> class.
        /// </summary>
        /// <param name="logFilter">The log filter to filter for certain messages only.</param>
        public Log4netHelper(params IFilter[] logFilter)
        {
            this.logAppender = new MemoryAppender();

            foreach (IFilter filter in logFilter)
            {
                this.logAppender.AddFilter(filter);
            }

            if (logFilter.Count() > 0)
            {
                this.logAppender.AddFilter(new DenyAllFilter());
            }

            log4net.Config.BasicConfigurator.Configure(this.logAppender);
        }

        /// <summary>
        /// Fails if the specified message was not contained in a log entry.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogContains(string message)
        {
            this.LogContains(null, message);
        }

        /// <summary>
        /// Fails if the specified message was not contained in a log entry.
        /// </summary>
        /// <param name="level">The level of the messages to look through.</param>
        /// <param name="message">The message.</param>
        public void LogContains(Level level, string message)
        {
            bool found = (from e in this.logAppender.GetEvents()
                          where (level == null || e.Level == level) && e.MessageObject.ToString().Contains(message)
                          select e).Count() > 0;

            if (!found)
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendFormat("Missing log message: level {0} searched message\n\r    {1}", level, message);
                errorMessage.AppendLine();
                this.DumpMessages(errorMessage);

                throw new Log4NetHelperException(errorMessage.ToString());
            }
        }

        /// <summary>
        /// Fails if the specified pattern (regex) is not found in a log message.
        /// </summary>
        /// <param name="pattern">The regex pattern.</param>
        public void LogMatch(string pattern)
        {
            this.LogMatch(null, pattern);
        }

        /// <summary>
        /// Fails if the specified pattern (regex) is not found in a log message.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="pattern">The regex pattern.</param>
        public void LogMatch(Level level, string pattern)
        {
            Regex regex = new Regex(pattern);

            bool found = (from e in this.logAppender.GetEvents()
                          where (level == null || e.Level == level) && regex.Match(e.MessageObject.ToString()).Success
                          select e).Count() > 0;

            if (!found)
            {
                StringBuilder message = new StringBuilder();
                message.AppendFormat("Missing log message: level {0} searched pattern\n\r    {1}", level, pattern);
                message.AppendLine();
                this.DumpMessages(message);

                throw new Log4NetHelperException(message.ToString());
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            LogManager.ResetConfiguration();
        }

        /// <summary>
        /// Dumps all messages to the console.
        /// </summary>
        public void DumpAllMessagesToConsole()
        {
            StringBuilder messages = new StringBuilder();
            this.DumpMessages(messages);

            Console.Write(messages);
        }

        /// <summary>
        /// Dumps the messages to the string builder.
        /// </summary>
        /// <param name="message">The message.</param>
        private void DumpMessages(StringBuilder message)
        {
            message.AppendLine("Existing messages:");
            foreach (LoggingEvent loggingEvent in this.logAppender.GetEvents())
            {
                message.Append("    ");
                message.Append(loggingEvent.LoggerName);
                message.Append("    ");
                message.Append(loggingEvent.Level);
                message.Append("    ");
                message.AppendLine(loggingEvent.MessageObject.ToString());
            }
        }
    }
}