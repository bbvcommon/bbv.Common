//-------------------------------------------------------------------------------
// <copyright file="Log4NetExtension.cs" company="bbv Software Services AG">
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

namespace bbv.Common.EvaluationEngine.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using bbv.Common.EvaluationEngine.Internals;

    using log4net;

    /// <summary>
    /// Logger using log4net.
    /// </summary>
    public class Log4NetExtension : ILogExtension
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetExtension"/> class.
        /// </summary>
        public Log4NetExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Log4NetExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Log4NetExtension(ILog logger)
        {
            this.log = logger;
        }

        /// <summary>
        /// Logs the found answer together with information how it was found.
        /// </summary>
        /// <param name="context">The context.</param>
        public void FoundAnswer(Context context)
        {
            var expressions = from expression in context.Expressions
                                    select string.Format(CultureInfo.InvariantCulture, "{0} => {1}", expression.Expression.Describe(), expression.ExpressionResult);

            string format = 
                context.Parameter == Missing.Value ?
                "Question = {1}{0}Answer = {3}{0}Used strategy = {4}{0}Used Aggregator = {5}{0}Expressions = {6}" :
                "Question = {1}{0}Parameter = {2}{0}Answer = {3}{0}Used strategy = {4}{0}Used Aggregator = {5}{0}Expressions = {6}";

            string message = string.Format(
                CultureInfo.InvariantCulture,
                format,
                Environment.NewLine,
                context.Question.Describe(),
                context.Parameter,
                context.Answer,
                context.Strategy.Describe(),
                context.Aggregator.Describe(),
                expressions.Aggregate(string.Empty, (aggregate, value) => aggregate + Environment.NewLine + "    " + value)); 

            this.log.Info(message);
        }
    }
}