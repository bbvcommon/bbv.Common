//-------------------------------------------------------------------------------
// <copyright file="ExtensibilityAcceptanceTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.RuleEngine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using NUnit.Framework;

    /// <summary>
    /// Tests that the rule engine can be extended with custom rules and aggregators.
    /// </summary>
    [TestFixture]
    public class ExtensibilityAcceptanceTest : IRulesProviderFinder
    {
        /// <summary>Rule engine instance</summary>
        private RuleEngine ruleEngine;

        /// <summary>we need two rules provider for demonstration</summary>
        private IRulesProvider globalRulesProvider;

        /// <summary>
        /// Defines which maintenance is required. This is the result of the evaluation call to the rule engine.
        /// </summary>
        private enum MaintenanceRequired
        {
            /// <summary>
            /// No maintenance required.
            /// </summary>
            None,

            /// <summary>
            /// The maintenance that every day has to be done is required.
            /// </summary>
            Daily,

            /// <summary>
            /// Weekly maintenance is required.
            /// </summary>
            Weekly,
        }

        /// <summary>
        /// Defines the result of a single rule evaluation.
        /// </summary>
        private enum MaintenenaceRequest
        {
            /// <summary>
            /// Daily maintenance is required.
            /// </summary>
            Daily,

            /// <summary>
            /// Weekly maintenance is required.
            /// </summary>
            Weekly
        }

        /// <summary>
        /// Initializes the whole rule engine component
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            IRulesProviderFinder rulesProviderFinder = this;
            this.ruleEngine = new RuleEngine(rulesProviderFinder);

            this.globalRulesProvider = new GlobalRulesProvider();
        }

        /// <summary>
        /// No rule requests maintenance.
        /// </summary>
        [Test]
        public void NoMaintenanceRequired()
        {
            MaintenanceRequired result = this.ruleEngine.Evaluate(new MaintenanceRuleSetDescriptor(
                new DateTime(2000, 1, 1), 
                new DateTime(2000, 1, 1, 3, 0, 0)));
            
            Assert.AreEqual(MaintenanceRequired.None, result);
        }

        /// <summary>
        /// A rule requests daily maintenance.
        /// </summary>
        [Test]
        public void DailyMaintenanceRequired()
        {
            MaintenanceRequired result = this.ruleEngine.Evaluate(new MaintenanceRuleSetDescriptor(
                new DateTime(2000, 1, 1),
                new DateTime(2000, 1, 3)));

            Assert.AreEqual(MaintenanceRequired.Daily, result);
        }

        /// <summary>
        /// A rule requests weekly maintenance.
        /// </summary>
        [Test]
        public void WeeklyMaintenanceRequired()
        {
            MaintenanceRequired result = this.ruleEngine.Evaluate(new MaintenanceRuleSetDescriptor(
                new DateTime(2000, 1, 1),
                new DateTime(2000, 1, 20)));

            Assert.AreEqual(MaintenanceRequired.Weekly, result);
        }

        /// <summary>
        /// Finds the rules providers.
        /// </summary>
        /// <typeparam name="TRule">The type of the rule.</typeparam>
        /// <typeparam name="TAggregationResult">The type of the aggregation result.</typeparam>
        /// <param name="ruleSetDescriptor">The rule set descriptor.</param>
        /// <returns>Rules providers</returns>
        public ICollection<IRulesProvider> FindRulesProviders<TRule, TAggregationResult>(IRuleSetDescriptor<TRule, TAggregationResult> ruleSetDescriptor)
        {
            return new List<IRulesProvider>
                       {
                           this.globalRulesProvider,
                       };
        }

        /// <summary>
        /// Descriptor for rules to check.
        /// </summary>
        private class MaintenanceRuleSetDescriptor : 
            IRuleSetDescriptor<IRule<MaintenenaceRequest?>, MaintenanceRequired>,
            IFactory<IRule<MaintenenaceRequest?>>,
            IAggregator<IRule<MaintenenaceRequest?>, MaintenanceRequired>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ExtensibilityAcceptanceTest.MaintenanceRuleSetDescriptor"/> class.
            /// </summary>
            /// <param name="lastMainteneance">The last maintenance.</param>
            /// <param name="timeOfRequest">The time of request.</param>
            public MaintenanceRuleSetDescriptor(DateTime lastMainteneance, DateTime timeOfRequest)
            {
                this.LastMaintenance = lastMainteneance;
                this.TimeOfRequest = timeOfRequest;
            }

            /// <summary>
            /// Gets the last maintenance.
            /// </summary>
            /// <value>The last maintenance.</value>
            public DateTime LastMaintenance { get; private set; }

            /// <summary>
            /// Gets the time of request.
            /// </summary>
            /// <value>The time of request.</value>
            public DateTime TimeOfRequest { get; private set; }

            /// <summary>
            /// Gets the factory used to create needed instances of rule engine related classes.
            /// </summary>
            /// <value>The factory.</value>
            public IFactory<IRule<MaintenenaceRequest?>> Factory
            {
                get { return this; }
            }

            /// <summary>
            /// Gets the aggregator that is used to combine all rules taking part in the evaluation.
            /// </summary>
            /// <value>The aggregator.</value>
            public IAggregator<IRule<MaintenenaceRequest?>, MaintenanceRequired> Aggregator
            {
                get { return this; }
            }

            /// <summary>
            /// Creates a rule set.
            /// </summary>
            /// <returns>A newly created rule set.</returns>
            public IRuleSet<IRule<MaintenenaceRequest?>> CreateRuleSet()
            {
                return new RuleSet<IRule<MaintenenaceRequest?>>();
            }

            /// <summary>
            /// Aggregates the specified rule set by prioritizing the individual requests of rules.
            /// </summary>
            /// <param name="ruleSet">The rule set.</param>
            /// <param name="logInfo">The log info. The aggregator should provide information about the results of the different rules and how they
            /// influenced the overall result. This info is written to the log by the rule engine.</param>
            /// <returns>
            /// The aggregated result of all rules taking part in the evaluation.
            /// </returns>
            public MaintenanceRequired Aggregate(IRuleSet<IRule<MaintenenaceRequest?>> ruleSet, out string logInfo)
            {
                MaintenanceRequired? required = null;

                StringBuilder sb = new StringBuilder();
                foreach (IRule<MaintenenaceRequest?> rule in ruleSet)
                {
                    MaintenenaceRequest? request = rule.Evaluate();

                    sb.AppendFormat("Rule {0} returned {1}, ", rule, request);

                    if (request.HasValue)
                    {
                        switch (request)
                        {
                            case MaintenenaceRequest.Daily:

                                required = MaintenanceRequired.Daily;
                                break;

                            case MaintenenaceRequest.Weekly:
                                sb.AppendFormat("Weekly maintenance required, no more rules are evaluated.");
                                logInfo = sb.ToString();
                                return MaintenanceRequired.Weekly;
                        }
                    }
                }

                if (required == MaintenanceRequired.Daily)
                {
                    sb.AppendFormat("Daily maintenance required.");
                    logInfo = sb.ToString();
                    return MaintenanceRequired.Daily;   
                }

                sb.AppendFormat("No maintenance required.");
                logInfo = sb.ToString();
                return MaintenanceRequired.None;
            }
        }

        /// <summary>
        /// The global rules provider for this test.
        /// </summary>
        private class GlobalRulesProvider : RulesProviderBase
        {
            /// <summary>
            /// Gets the rules.
            /// </summary>
            /// <param name="descriptor">The descriptor.</param>
            /// <returns><see cref="RuleSet{TRule}"/> with all rules to evaluate in this test.</returns>
            [RuleProvider]
            public IRuleSet<IRule<MaintenenaceRequest?>> GetRules(MaintenanceRuleSetDescriptor descriptor)
            {
                return new RuleSet<IRule<MaintenenaceRequest?>>
                           {
                               new RuleCheckDailyMaintenance(descriptor.LastMaintenance, descriptor.TimeOfRequest),
                               new RuleCheckWeeklyMaintenance(descriptor.LastMaintenance, descriptor.TimeOfRequest)
                           };
            }
        }

        /// <summary>
        /// Checks whether the difference of request time and last maintenance is smaller that 1 day.
        /// </summary>
        private class RuleCheckDailyMaintenance : IRule<MaintenenaceRequest?>
        {
            /// <summary>time of last maintenance</summary>
            private readonly DateTime lastMaintenance;

            /// <summary>time of request</summary>
            private readonly DateTime timeOfRequest;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExtensibilityAcceptanceTest.RuleCheckDailyMaintenance"/> class.
            /// </summary>
            /// <param name="lastMaintenance">The last maintenance.</param>
            /// <param name="timeOfRequest">The time of request.</param>
            public RuleCheckDailyMaintenance(DateTime lastMaintenance, DateTime timeOfRequest)
            {
                this.lastMaintenance = lastMaintenance;
                this.timeOfRequest = timeOfRequest;
            }

            /// <summary>
            /// Evaluates this instance.
            /// </summary>
            /// <returns><see cref="MaintenenaceRequest.Daily"/> if time since last maintenance > 1 day; otherwise null.</returns>
            public MaintenenaceRequest? Evaluate()
            {
                return (this.timeOfRequest.Subtract(this.lastMaintenance).Days > 1) ? MaintenenaceRequest.Daily : (MaintenenaceRequest?)null;
            }
        }

        /// <summary>
        /// Checks whether the difference of request time and last maintenance is smaller that 7 day.
        /// </summary>
        private class RuleCheckWeeklyMaintenance : IRule<MaintenenaceRequest?>
        {
            /// <summary>time of last maintenance</summary>
            private readonly DateTime lastMaintenance;

            /// <summary>time of request</summary>
            private readonly DateTime timeOfRequest;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExtensibilityAcceptanceTest.RuleCheckWeeklyMaintenance"/> class.
            /// </summary>
            /// <param name="lastMaintenance">The last maintenance.</param>
            /// <param name="timeOfRequest">The time of request.</param>
            public RuleCheckWeeklyMaintenance(DateTime lastMaintenance, DateTime timeOfRequest)
            {
                this.lastMaintenance = lastMaintenance;
                this.timeOfRequest = timeOfRequest;
            }

            /// <summary>
            /// Evaluates this instance.
            /// </summary>
            /// <returns><see cref="MaintenenaceRequest.Weekly"/> if time since last maintenance > 7 day; otherwise null.</returns>
            public MaintenenaceRequest? Evaluate()
            {
                return (this.timeOfRequest.Subtract(this.lastMaintenance).Days > 7) ? MaintenenaceRequest.Weekly : (MaintenenaceRequest?)null;
            }
        }
    }
}