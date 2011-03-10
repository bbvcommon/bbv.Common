//-------------------------------------------------------------------------------
// <copyright file="RulesProviderBase.cs" company="bbv Software Services AG">
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
    using System.Linq;
    using System.Reflection;
    using log4net;

    /// <summary>
    /// Abstract base class for classes implementing <see cref="IRulesProvider"/>.
    /// <para>
    /// Provides automatic calling of GetRules method based on the <see cref="IRuleSetDescriptor{TRule,TAggregationResult}"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="RuleEngine"/> will create <see cref="IRulesProvider"/>s with a
    /// <see cref="IValidationFactory"/> parameter.
    /// Therefore a rules provider has to provide a matching constructor.
    /// </para>
    /// <para>
    /// To simplify the matching of <see cref="IRuleSetDescriptor{TRule,TAggregationResult}"/>s to <see cref="IRuleSet{TRule}"/> this class
    /// implements the <see cref="GetRules{TRule,TAggregationResult}"/> method by calling the method of the derived rules provider that
    /// has the signature <code>private IRuleSet GetRules(T ruleSetDescriptor)</code> with the matching rule set
    /// descriptor type (<code>T</code>) via reflection.
    /// </para>
    /// </remarks>
    public abstract class RulesProviderBase : IRulesProvider
    {
        /// <summary>
        /// Logger of this class.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Mapping from rule descriptor types to delegates (the method that has to be called for the rule descriptor).
        /// </summary>
        private readonly Dictionary<Type, Delegate> mapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="RulesProviderBase"/> class and reflects the base class for rule provider methods.
        /// </summary>
        protected RulesProviderBase()
        {
            this.mapping = this.InitRuleSetDescriptorMapping();
        }

        /// <summary>
        /// Delegate for calling specific rules providing method.
        /// </summary>
        /// <param name="ruleSetDescriptor">The rule set descriptor for which rules are requested.</param>
        /// <returns>The rule set containing all rules relevant for the rule set descriptor.</returns>
        /// <typeparam name="TRuleSet">The type of the rule set.</typeparam>
        /// <typeparam name="T">Type of the rule set descriptor.</typeparam>
        private delegate TRuleSet RuleProviderMethod<TRuleSet, T>(T ruleSetDescriptor);

        /// <summary>
        /// Returns the rules described by the specified rule set descriptor
        /// by calling the corresponding provider method of the derived class.
        /// See remarks of class description for more details.
        /// </summary>
        /// <typeparam name="TRule">The type of the rule.</typeparam>
        /// <typeparam name="TAggregationResult">The type of the aggregation result.</typeparam>
        /// <param name="ruleSetDescriptor">The rule set descriptor.</param>
        /// <returns>A set of rules to be validated.</returns>
        public virtual IRuleSet<TRule> GetRules<TRule, TAggregationResult>(IRuleSetDescriptor<TRule, TAggregationResult> ruleSetDescriptor)
        {
            IRuleSet<TRule> result = null;

            var delegates = from item in this.mapping
                    where item.Key.IsAssignableFrom(ruleSetDescriptor.GetType())
                    select item.Value;

            if (delegates.Count() > 0)
            {
                foreach (Delegate d in delegates)
                {
                    var resultSet = (IRuleSet<TRule>)d.DynamicInvoke(ruleSetDescriptor);
                    if (result == null)
                    {
                        result = resultSet;
                    }
                    else
                    {
                        result.AddRange(resultSet);
                    }
                }
            }
            else
            {
                Log.DebugFormat(
                    "RulesProvider '{0}' does not provide rules for ruleSetDescriptor '{1}'.",
                    GetType().FullName,
                    ruleSetDescriptor.GetType().FullName);
            }

            return result;
        }

        /// <summary>
        /// Initializes the rule set descriptor mapping used to map from rule set descriptor type to rules providing method.
        /// </summary>
        /// <returns>Mapping from rule descriptor types to handler methods.</returns>
        private Dictionary<Type, Delegate> InitRuleSetDescriptorMapping()
        {
            this.CheckNoPrivateRuleProviderMethods();
            this.CheckNoStaticRuleProviderMethods();

            Dictionary<Type, Delegate> m = new Dictionary<Type, Delegate>();

            MethodInfo[] methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);

            foreach (MethodInfo methodInfo in methods)
            {
                if (methodInfo.GetCustomAttributes(typeof(RuleProviderAttribute), true).Length == 0)
                {
                    continue;
                }
                
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType.FindInterfaces(this.InterfaceFilter, typeof(IRuleSetDescriptor<,>)).Length <= 0)
                {
                    continue;
                }

                Delegate d = Delegate.CreateDelegate(
                    typeof(RuleProviderMethod<,>).MakeGenericType(methodInfo.ReturnType, parameters[0].ParameterType),
                    this,
                    methodInfo);

                m.Add(parameters[0].ParameterType, d);
            }

            return m;
        }

        /// <summary>
        /// Checks that no private methods are marked as rule provider.
        /// </summary>
        private void CheckNoPrivateRuleProviderMethods()
        {
            MethodInfo[] methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (MethodInfo methodInfo in methods)
            {
                if (methodInfo.GetCustomAttributes(typeof(RuleProviderAttribute), true).Length != 0)
                {
                    throw new RuleEngineException("Do not mark private methods as RuleProvider.");
                }
            }
        }

        /// <summary>
        /// Checks that no static methods are marked as rule provider.
        /// </summary>
        private void CheckNoStaticRuleProviderMethods()
        {
            MethodInfo[] methods = GetType().GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (MethodInfo methodInfo in methods)
            {
                if (methodInfo.GetCustomAttributes(typeof(RuleProviderAttribute), true).Length != 0)
                {
                    throw new RuleEngineException("Do not mark static methods as RuleProvider.");
                }
            }
        }

        /// <summary>
        /// Filter method to filter for interfaces. Used in <see cref="InitRuleSetDescriptorMapping"/>.
        /// </summary>
        /// <param name="type">The type to match against the criteria.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>A value indicating if <paramref name="type"/> matches the <paramref name="criteria"/>.</returns>
        private bool InterfaceFilter(Type type, object criteria)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom((Type)criteria);
        }
    }
}
