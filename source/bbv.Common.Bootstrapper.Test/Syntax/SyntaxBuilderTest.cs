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
        private const int NumberOfExecutablesForBegin = 1;

        private const int NumberOfExecutablesForEnd = 2;

        private readonly StringBuilder executionChainingBuilder;

        private readonly Mock<IExecutableFactory<ICustomExtension>> executableFactory;

        private readonly ISyntaxBuilder<ICustomExtension> testee;

        public SyntaxBuilderTest()
        {
            this.executionChainingBuilder = new StringBuilder();
            this.executableFactory = new Mock<IExecutableFactory<ICustomExtension>>();

            this.testee = new SyntaxBuilder<ICustomExtension>(this.executableFactory.Object);
        }

        [Fact]
        public void BeginWith_Behavior_ShouldAddExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Begin.With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(NumberOfExecutablesForBegin);
        }

        [Fact]
        public void BeginWith_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Begin.With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(NumberOfExecutablesForBegin);
        }

        [Fact]
        public void BeginWith_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);

            this.testee.Begin.With(behavior);

            extension.Verify(e => e.Add(behavior));
        }

        [Fact]
        public void BeginWith_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);

            this.testee
                .Begin
                    .With(firstBehavior)
                    .With(secondBehavior)
                    .With(thirdBehavior);

            extension.Verify(e => e.Add(firstBehavior));
            extension.Verify(e => e.Add(secondBehavior));
            extension.Verify(e => e.Add(thirdBehavior));
        }

        [Fact]
        public void BeginWithLateBound_Behavior_ShouldAddExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Begin.With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(NumberOfExecutablesForBegin);
        }

        [Fact]
        public void BeginWithLateBound_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Begin.With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(NumberOfExecutablesForBegin);
        }

        [Fact]
        public void BeginWithLateBound_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);

            this.testee.Begin.With(() => behavior);

            extension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()));
        }

        [Fact]
        public void BeginWithLateBound_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);

            this.testee
                .Begin
                    .With(() => firstBehavior)
                    .With(() => secondBehavior)
                    .With(() => thirdBehavior);

            extension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()), Times.Exactly(3));
        }

        [Fact]
        public void BeginWithLateBound_Behavior_ShouldBehaveLateBound()
        {
            IBehavior<ICustomExtension> interceptedBehavior = null;
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = new Mock<IBehavior<ICustomExtension>>();

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);
            extension.Setup(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>())).Callback<IBehavior<ICustomExtension>>(b => interceptedBehavior = b);

            this.testee.Begin.With(() => behavior.Object);

            interceptedBehavior.Behave(Enumerable.Empty<ICustomExtension>());

            behavior.Verify(b => b.Behave(Enumerable.Empty<ICustomExtension>()));
        }

        [Fact]
        public void EndWith_Behavior_ShouldAddExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Begin.End.With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(NumberOfExecutablesForEnd);
        }

        [Fact]
        public void EndWith_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Begin.End.With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(NumberOfExecutablesForEnd);
        }

        [Fact]
        public void EndWith_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var firstExtension = new Mock<IExecutable<ICustomExtension>>();
            var secondExtension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.SetupSequence(f => f.CreateExecutable(It.IsAny<Action>()))
                .Returns(firstExtension.Object)
                .Returns(secondExtension.Object);

            this.testee.Begin.End.With(behavior);

            firstExtension.Verify(e => e.Add(behavior), Times.Never());
            secondExtension.Verify(e => e.Add(behavior));
        }

        [Fact]
        public void EndWith_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var firstExtension = new Mock<IExecutable<ICustomExtension>>();
            var secondExtension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.SetupSequence(f => f.CreateExecutable(It.IsAny<Action>()))
                .Returns(firstExtension.Object)
                .Returns(secondExtension.Object);

            this.testee
                .Begin.End
                    .With(firstBehavior)
                    .With(secondBehavior)
                    .With(thirdBehavior);

            firstExtension.Verify(e => e.Add(firstBehavior), Times.Never());
            firstExtension.Verify(e => e.Add(secondBehavior), Times.Never());
            firstExtension.Verify(e => e.Add(thirdBehavior), Times.Never());

            secondExtension.Verify(e => e.Add(firstBehavior));
            secondExtension.Verify(e => e.Add(secondBehavior));
            secondExtension.Verify(e => e.Add(thirdBehavior));
        }

        [Fact]
        public void EndWithLateBound_Behavior_ShouldAddExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Begin.End.With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(NumberOfExecutablesForEnd);
        }

        [Fact]
        public void EndWithLateBound_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Begin.End.With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(NumberOfExecutablesForEnd);
        }

        [Fact]
        public void EndWithLateBound_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var firstExtension = new Mock<IExecutable<ICustomExtension>>();
            var secondExtension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.SetupSequence(f => f.CreateExecutable(It.IsAny<Action>()))
                .Returns(firstExtension.Object)
                .Returns(secondExtension.Object);

            this.testee.Begin.End.With(() => behavior);

            firstExtension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()), Times.Never());
            secondExtension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()));
        }

        [Fact]
        public void EndWithLateBound_BehaviorMultipleTimes_ShouldAddBehaviorToLastExecutable()
        {
            var firstExtension = new Mock<IExecutable<ICustomExtension>>();
            var secondExtension = new Mock<IExecutable<ICustomExtension>>();
            var firstBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var secondBehavior = Mock.Of<IBehavior<ICustomExtension>>();
            var thirdBehavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.executableFactory.SetupSequence(f => f.CreateExecutable(It.IsAny<Action>()))
                .Returns(firstExtension.Object)
                .Returns(secondExtension.Object);

            this.testee
                .Begin.End
                    .With(() => firstBehavior)
                    .With(() => secondBehavior)
                    .With(() => thirdBehavior);

            firstExtension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()), Times.Never());
            secondExtension.Verify(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>()), Times.Exactly(3));
        }

        [Fact]
        public void EndWithLateBound_Behavior_ShouldBehaveLateBound()
        {
            IBehavior<ICustomExtension> interceptedBehavior = null;
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = new Mock<IBehavior<ICustomExtension>>();

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);
            extension.Setup(e => e.Add(It.IsAny<IBehavior<ICustomExtension>>())).Callback<IBehavior<ICustomExtension>>(b => interceptedBehavior = b);

            this.testee.Begin.End.With(() => behavior.Object);

            interceptedBehavior.Behave(Enumerable.Empty<ICustomExtension>());

            behavior.Verify(b => b.Behave(Enumerable.Empty<ICustomExtension>()));
        }

        [Fact]
        public void WithAfterExecuteWithAction_Behavior_ShouldAddExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Execute(() => { }).With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithAction_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Execute(() => { }).With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>()).With(Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithAction_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);

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

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);

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
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Execute(() => { }).With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithAction_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.SetupCreateActionExecutableReturnsAnyExecutable();

            this.testee.Execute(() => { }).With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>()).With(() => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithLateBoundAfterExecuteWithAction_Behavior_ShouldAddBehaviorToLastExecutable()
        {
            var extension = new Mock<IExecutable<ICustomExtension>>();
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);

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

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);

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

            this.SetupCreateActionExecutableReturnsExecutable(extension.Object);
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
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Func<object>>(), It.IsAny<Action<ICustomExtension, object>>(), It.IsAny<Action<IBehaviorAware<ICustomExtension>, object>>()))
                .Returns(Mock.Of<IExecutable<ICustomExtension>>());

            this.testee
                .Execute(() => new object(), (e, o) => e.Dispose())
                    .With(o => Mock.Of<IBehavior<ICustomExtension>>());

            this.testee.Should().HaveCount(1);
        }

        [Fact]
        public void WithAfterExecuteWithActionOnExtensionWithInitializer_BehaviorMultipleTimes_ShouldOnlyAddOneExecutable()
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Func<object>>(), It.IsAny<Action<ICustomExtension, object>>(), It.IsAny<Action<IBehaviorAware<ICustomExtension>, object>>()))
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
            Action<IBehaviorAware<ICustomExtension>, object> contextInitializer = null;

            var extension = new Mock<IExecutable<ICustomExtension>>();
            extension.Setup(x => x.Add(It.IsAny<IBehavior<ICustomExtension>>())).Callback<IBehavior<ICustomExtension>>(
                b => behavior = b);

            this.executableFactory.Setup(
                f =>
                f.CreateExecutable(It.IsAny<Func<object>>(), It.IsAny<Action<ICustomExtension, object>>(), It.IsAny<Action<IBehaviorAware<ICustomExtension>, object>>()))
                .Callback<Func<object>, Action<ICustomExtension, object>, Action<IBehaviorAware<ICustomExtension>, object>>(
                    (func, action, ctx) => contextInitializer = ctx).Returns(Mock.Of<IExecutable<ICustomExtension>>);

            var context = new object();

            this.testee.Execute(() => context, (e, o) => e.Dispose()).With(o => new TestableBehavior(o));

            contextInitializer(extension.Object, context);

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
                f => f.CreateExecutable(It.IsAny<Func<char>>(), It.IsAny<Action<ICustomExtension, char>>(), It.IsAny<Action<IBehaviorAware<ICustomExtension>, char>>()))
                .Callback<Func<char>, Action<ICustomExtension, char>, Action<IBehaviorAware<ICustomExtension>, char>>((func, action, context) =>
                    {
                        var ctx = func();
                        context(Mock.Of<IBehaviorAware<ICustomExtension>>(), ctx);
                        action(Mock.Of<ICustomExtension>(), ctx);
                    })
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

        private void SetupCreateActionExecutableReturnsExecutable(IExecutable<ICustomExtension> executable)
        {
            this.executableFactory.Setup(f => f.CreateExecutable(It.IsAny<Action>())).Returns(executable);
        }

        private void SetupCreateActionExecutableReturnsAnyExecutable()
        {
            this.SetupCreateActionExecutableReturnsExecutable(Mock.Of<IExecutable<ICustomExtension>>());
        }
    }
}