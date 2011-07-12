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

    using bbv.Common.Bootstrapper.Dummies;

    using FluentAssertions;

    using Moq;

    using Xunit;
    using Xunit.Extensions;

    public class SyntaxBuilderTest
    {
        private readonly StringBuilder executionChainingBuilder;

        private readonly Mock<IExecutableFactory<ICustomExtension>> executableFactory;

        private readonly SyntaxBuilder<ICustomExtension> testee;

        public SyntaxBuilderTest()
        {
            this.executionChainingBuilder = new StringBuilder();
            this.executableFactory = new Mock<IExecutableFactory<ICustomExtension>>();

            this.testee = new SyntaxBuilder<ICustomExtension>(this.executableFactory.Object);
        }

        [Fact]
        public void With_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void With_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void With_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee.With(behavior);

            extension.Verify(e => e.Add(behavior));
        }

        [Fact]
        public void With_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

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
        public void WithLateBound_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithLateBound_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithLateBound_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee.With(() => behavior);

            extension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()));
        }

        [Fact]
        public void WithLateBound_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee
                .With(() => firstBehavior)
                .With(() => secondBehavior)
                .With(() => thirdBehavior);

            extension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()), Times.Exactly(3));
        }

        [Fact]
        public void WithLateBound_Behavior_ShouldBehaveLateBound()
        {
            IBehavior<ICustomExtension> interceptedBehavior = null;
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = new Mock<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);
            extension.Setup(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>())).Callback<IBehavior<ICustomExtension>>(b => interceptedBehavior = b);

            this.testee.With(() => behavior.Object);

            interceptedBehavior.Behave(Enumerable.Empty<ICustomExtension>());

            behavior.Verify(b => b.Behave(Enumerable.Empty<ICustomExtension>()));
        }

        [Fact]
        public void WithAfterExecuteWithAction_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.Execute(() => { }).With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithAction_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.Execute(() => { }).With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithAction_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee
                .Execute(() => { })
                    .With(behavior);

            extension.Verify(e => e.Add(behavior));
        }

        [Fact]
        public void WithAfterExecuteWithAction_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

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
        public void WithLateBoundAfterExecuteWithAction_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.Execute(() => { }).With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithAction_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.Execute(() => { }).With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithAction_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee
                .Execute(() => { })
                    .With(() => behavior);

            extension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()));
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithAction_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);

            this.testee
                .Execute(() => { })
                    .With(() => firstBehavior)
                    .With(() => secondBehavior)
                    .With(() => thirdBehavior);

            extension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()), Times.Exactly(3));
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithAction_Behavior_ShouldBehaveLateBound()
        {
            IBehavior<ICustomExtension> interceptedBehavior = null;
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = new Mock<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(extension.Object);
            extension.Setup(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>())).Callback<IBehavior<ICustomExtension>>(b => interceptedBehavior = b);

            this.testee
                .Execute(() => { })
                    .With(() => behavior.Object);

            interceptedBehavior.Behave(Enumerable.Empty<ICustomExtension>());

            behavior.Verify(b => b.Behave(Enumerable.Empty<ICustomExtension>()));
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtension_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee
                .Execute(e => e.Dispose())
                    .With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtension_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.Execute(e => e.Dispose())
                .With(Mock.Of<IBehavior<ICustomExtension>>())
                .With(Mock.Of<IBehavior<ICustomExtension>>())
                .With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtension_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>())).Returns(extension.Object);

            this.testee
                .Execute(e => e.Dispose())
                    .With(behavior);

            extension.Verify(e => e.Add(behavior));
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtension_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>())).Returns(extension.Object);

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
        public void WithBoundLateAfterExecuteWithActionOnExtension_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee
                .Execute(e => e.Dispose())
                    .With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithActionOnExtension_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>())).Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.Execute(e => e.Dispose())
                .With(() => Mock.Of<IBehavior<ICustomExtension>>())
                .With(() => Mock.Of<IBehavior<ICustomExtension>>())
                .With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithActionOnExtension_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>())).Returns(extension.Object);

            this.testee
                .Execute(e => e.Dispose())
                    .With(() => behavior);

            extension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()));
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithActionOnExtension_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>())).Returns(extension.Object);

            this.testee
                .Execute(e => e.Dispose())
                    .With(() => firstBehavior)
                    .With(() => secondBehavior)
                    .With(() => thirdBehavior);

            extension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()), Times.Exactly(3));
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithActionOnExtension_Behavior_ShouldBehaveLateBound()
        {
            IBehavior<ICustomExtension> interceptedBehavior = null;
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = new Mock<IBehavior<ICustomExtension>>();

            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>())).Returns(extension.Object);
            extension.Setup(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>())).Callback<IBehavior<ICustomExtension>>(b => interceptedBehavior = b);

            this.testee
                .Execute(e => e.Dispose())
                    .With(() => behavior.Object);

            interceptedBehavior.Behave(Enumerable.Empty<ICustomExtension>());

            behavior.Verify(b => b.Behave(Enumerable.Empty<ICustomExtension>()));
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtensionWithInitializer_Behavior_ShouldAddExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Func<IBehaviorAware<ICustomExtension>, object>>(), It.IsAny<Action<ICustomExtension, object>>()))
                .Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee
                .Execute(() => new object(), (e, o) => e.Dispose())
                    .With(o => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtensionWithInitializer_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Func<IBehaviorAware<ICustomExtension>, object>>(), It.IsAny<Action<ICustomExtension, object>>()))
                .Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee.Execute(() => new object(), (e, o) => e.Dispose())
                .With(o => Mock.Of<IBehavior<ICustomExtension>>())
                .With(o => Mock.Of<IBehavior<ICustomExtension>>())
                .With(o => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtensionWithInitializer_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            IBehavior<ICustomExtension> behavior = null;
            Func<IBehaviorAware<ICustomExtension>, object> contextInitializer = aware => null;

            var extension = new Mock<IExecutable<ICustomExtension>>();
            extension.Setup(x => x.Add(It.IsAny<IBehavior<ICustomExtension>>())).Callback<IBehavior<ICustomExtension>>(
                b => behavior = b);

            this.executableFactory.Setup(
                f =>
                f.CreateExecutable(
                    It.IsAny<Func<IBehaviorAware<ICustomExtension>, object>>(), It.IsAny<Action<ICustomExtension, object>>())).
                Callback<Func<IBehaviorAware<ICustomExtension>, object>, Action<ICustomExtension, object>>(
                    (func, action) => contextInitializer = func).Returns(Mock.Of<IExecutable<ICustomExtension>>);

            var context = new object();

            this.testee.Execute(() => context, (e, o) => e.Dispose()).With(o => new TestableBehavior(o));

            contextInitializer(extension.Object);

            behavior.Should().NotBeNull();
            behavior.As<TestableBehavior>().Context.Should().Be(context);
        }

        private class TestableBehavior : IBehavior<ICustomExtension>
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

            public void Behave(IEnumerable<ICustomExtension> extensions)
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
                .Returns(Mock.Of<IExecutable<ICustomExtension>>);
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action<ICustomExtension>>()))
                .Callback<Action<ICustomExtension>>(action => action(Mock.Of<ICustomExtension>()))
                .Returns(Mock.Of<IExecutable<ICustomExtension>>);
            this.executableFactory.Setup(
                f => f.CreateExecutable(It.IsAny<Func<IBehaviorAware<ICustomExtension>, char>>(), It.IsAny<Action<ICustomExtension, char>>()))
                .Callback<Func<IBehaviorAware<ICustomExtension>, char>, Action<ICustomExtension, char>>((func, action) => action(Mock.Of<ICustomExtension>(), func(Mock.Of<IBehaviorAware<ICustomExtension>>())))
                .Returns(Mock.Of<IExecutable<ICustomExtension>>);
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