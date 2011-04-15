//-------------------------------------------------------------------------------
// <copyright file="SyntaxBuilderTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Syntax
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FluentAssertions;

    using Moq;

    using Xunit.Extensions;

    public class SyntaxBuilderTest
    {
        private readonly StringBuilder executionChainingBuilder;

        private readonly Mock<IExecutableFactory<IExtension>> executableFactory;

        private readonly SyntaxBuilder<IExtension> testee;

        public SyntaxBuilderTest()
        {
            this.executionChainingBuilder = new StringBuilder();
            this.executableFactory = new Mock<IExecutableFactory<IExtension>>();

            this.testee = new SyntaxBuilder<IExtension>(this.executableFactory.Object);
        }

        [Theory,
         InlineData("ABC", "ABCI"),
         InlineData("CBA", "CIBA"),
         InlineData("AAA", "AAA"),
         InlineData("BBB", "BBB"),
         InlineData("CCC", "CICICI")]
        public void Execute_Chaining_ShouldBePossible(string execution, string expected)
        {
            this.ExecuteChaining(execution);

            this.executionChainingBuilder.ToString().Should().Be(expected);
        }

        [Theory,
        InlineData("ABC", 3),
         InlineData("CBA", 3),
         InlineData("AAA", 3),
         InlineData("BBB", 3),
         InlineData("CCC", 3),
         InlineData("AAAA", 4),
         InlineData("AAAAA", 5)]
        public void Enumeration_ShouldProvidedDefinedExecutables(string execution, int expected)
        {
            this.ExecuteChaining(execution);

            this.testee.Count().Should().Be(expected);
        }

        private void ExecuteChaining(string syntax)
        {
            this.SetupAutoExecutionOfExecutables();

            Dictionary<char, Action> actions = this.DefineCharToActionMapping();

            foreach (char c in syntax.ToUpperInvariant())
            {
                actions[c].Invoke();
            }
        }

        private void SetupAutoExecutionOfExecutables()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>()))
                .Callback<Action>(action => action())
                .Returns(Mock.Of<IExecutable>);
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<IExtension>>()))
                .Callback<Action<IExtension>>(action => action(Mock.Of<IExtension>()))
                .Returns(Mock.Of<IExecutable>);
            this.executableFactory.Setup(
                f => f.CreateExecutable(It.IsAny<Func<char>>(), It.IsAny<Action<IExtension, char>>()))
                .Callback<Func<char>, Action<IExtension, char>>((func, action) => action(Mock.Of<IExtension>(), func()))
                .Returns(Mock.Of<IExecutable>);
        }

        private Dictionary<char, Action> DefineCharToActionMapping()
        {
            return new Dictionary<char, Action>
                {
                    { 'A', () => this.testee.Execute(() => this.executionChainingBuilder.Append('A')) },
                    { 'B', () => this.testee.Execute(extension => this.executionChainingBuilder.Append('B')) },
                    {
                        'C', () => this.testee.Execute(
                            () => 'I',
                            (extension, context) =>
                                {
                                    this.executionChainingBuilder.Append('C');
                                    this.executionChainingBuilder.Append(context);
                                })
                        },
                };
        }
    }
}