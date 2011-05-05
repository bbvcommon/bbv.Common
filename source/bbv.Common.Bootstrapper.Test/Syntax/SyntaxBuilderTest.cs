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

    using Xunit;
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

        [Fact]
        public void With_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<IExtension>>());

            this.testee.With(Mock.Of<IBehavior<IExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void With_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<IExtension>>());

            this.testee.With(Mock.Of<IBehavior<IExtension>>()).With(Mock.Of<IBehavior<IExtension>>()).With(Mock.Of<IBehavior<IExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void With_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<IExtension>>();
            var behavior = Mock.Of<IBehavior<IExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee.With(behavior);

            extension.Verify(e => e.Add(behavior));
        }

        [Fact]
        public void With_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<IExtension>>();
            var firstBehavior = Mock.Of<IBehavior<IExtension>>();
            var secondBehavior = Mock.Of<IBehavior<IExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<IExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee
                .With(firstBehavior)
                .With(secondBehavior)
                .With(thirdBehavior);

            extension.Verify(e => e.Add(firstBehavior));
            extension.Verify(e => e.Add(secondBehavior));
            extension.Verify(e => e.Add(thirdBehavior));
        }

        [Fact]
        public void WithAfterExecuteWithAction_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<IExtension>>());

            this.testee.Execute(() => { }).With(Mock.Of<IBehavior<IExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithAction_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<IExtension>>());

            this.testee.Execute(() => { }).With(Mock.Of<IBehavior<IExtension>>()).With(Mock.Of<IBehavior<IExtension>>()).With(Mock.Of<IBehavior<IExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithAction_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<IExtension>>();
            var behavior = Mock.Of<IBehavior<IExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee
                .Execute(() => { })
                    .With(behavior);

            extension.Verify(e => e.Add(behavior));
        }

        [Fact]
        public void WithAfterExecuteWithAction_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<IExtension>>();
            var firstBehavior = Mock.Of<IBehavior<IExtension>>();
            var secondBehavior = Mock.Of<IBehavior<IExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<IExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee
                .Execute(() => { })
                    .With(firstBehavior)
                    .With(secondBehavior)
                    .With(thirdBehavior);

            extension.Verify(e => e.Add(firstBehavior));
            extension.Verify(e => e.Add(secondBehavior));
            extension.Verify(e => e.Add(thirdBehavior));
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtension_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<IExtension>>())).Returns(Mock.Of<IExecutable<IExtension>>());

            this.testee
                .Execute(e => e.Dispose())
                    .With(Mock.Of<IBehavior<IExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtension_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<IExtension>>())).Returns(Mock.Of<IExecutable<IExtension>>());

            this.testee.Execute(e => e.Dispose())
                .With(Mock.Of<IBehavior<IExtension>>())
                .With(Mock.Of<IBehavior<IExtension>>())
                .With(Mock.Of<IBehavior<IExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtension_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<IExtension>>();
            var behavior = Mock.Of<IBehavior<IExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<IExtension>>())).Returns(extension.Object);

            this.testee
                .Execute(e => e.Dispose())
                    .With(behavior);

            extension.Verify(e => e.Add(behavior));
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtension_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<IExtension>>();
            var firstBehavior = Mock.Of<IBehavior<IExtension>>();
            var secondBehavior = Mock.Of<IBehavior<IExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<IExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<IExtension>>())).Returns(extension.Object);

            this.testee
                .Execute(e => e.Dispose())
                    .With(firstBehavior)
                    .With(secondBehavior)
                    .With(thirdBehavior);

            extension.Verify(e => e.Add(firstBehavior));
            extension.Verify(e => e.Add(secondBehavior));
            extension.Verify(e => e.Add(thirdBehavior));
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtensionWithInitializer_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Func<IBehaviorAware<IExtension>, object>>(), It.IsAny<Action<IExtension, object>>()))
                .Returns(Mock.Of<IExecutable<IExtension>>());

            this.testee
                .Execute(() => new object(), (e, o) => e.Dispose())
                    .With(o => Mock.Of<IBehavior<IExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtensionWithInitializer_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Func<IBehaviorAware<IExtension>, object>>(), It.IsAny<Action<IExtension, object>>()))
                .Returns(Mock.Of<IExecutable<IExtension>>());

            this.testee.Execute(() => new object(), (e, o) => e.Dispose())
                .With(o => Mock.Of<IBehavior<IExtension>>())
                .With(o => Mock.Of<IBehavior<IExtension>>())
                .With(o => Mock.Of<IBehavior<IExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtensionWithInitializer_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            IBehavior<IExtension> behavior = null;
            Func<IBehaviorAware<IExtension>, object> contextInitializer = aware => null;

            var extension = new Mock<IExecutable<IExtension>>();
            extension.Setup(x => x.Add(It.IsAny<IBehavior<IExtension>>())).Callback<IBehavior<IExtension>>(
                b => behavior = b);

            this.executableFactory.Setup(
                f =>
                f.CreateExecutable(
                    It.IsAny<Func<IBehaviorAware<IExtension>, object>>(), It.IsAny<Action<IExtension, object>>())).
                Callback<Func<IBehaviorAware<IExtension>, object>, Action<IExtension, object>>(
                    (func, action) => contextInitializer = func).Returns(Mock.Of<IExecutable<IExtension>>);

            var context = new object();

            this.testee.Execute(() => context, (e, o) => e.Dispose()).With(o => new TestableBehavior(o));

            contextInitializer(extension.Object);

            behavior.Should().NotBeNull();
            behavior.As<TestableBehavior>().Context.Should().Be(context);
        }

        private class TestableBehavior : IBehavior<IExtension>
        {
            private readonly object context;

            public TestableBehavior(object context)
            {
                this.context = context;
            }

            public object Context
            {
                get
                {
                    return this.context;
                }
            }

            public void Behave(IEnumerable<IExtension> extensions)
            {
            }
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
        public void Enumeration_ShouldProvideDefinedExecutables(string execution, int expected)
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
                .Returns(Mock.Of<IExecutable<IExtension>>);
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<IExtension>>()))
                .Callback<Action<IExtension>>(action => action(Mock.Of<IExtension>()))
                .Returns(Mock.Of<IExecutable<IExtension>>);
            this.executableFactory.Setup(
                f => f.CreateExecutable(It.IsAny<Func<IBehaviorAware<IExtension>, char>>(), It.IsAny<Action<IExtension, char>>()))
                .Callback<Func<IBehaviorAware<IExtension>, char>, Action<IExtension, char>>((func, action) => action(Mock.Of<IExtension>(), func(Mock.Of<IBehaviorAware<IExtension>>())))
                .Returns(Mock.Of<IExecutable<IExtension>>);
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