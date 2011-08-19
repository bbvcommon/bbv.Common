//-------------------------------------------------------------------------------
// <copyright file="SyntaxBuilderWithContextTest.cs" company="bbv Software Services AG">
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

    using bbv.Common.Bootstrapper.Dummies;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class SyntaxBuilderWithContextTest
    {
        private readonly Mock<ISyntaxBuilderWithoutContext<ICustomExtension>> syntaxBuilder;

        private readonly Mock<IEndWithBehavior<ICustomExtension>> endWithBehavior;

        private readonly Queue<Func<object, IBehavior<ICustomExtension>>> behaviorProviders;

        private readonly SyntaxBuilderWithContext<ICustomExtension, object> testee;

        public SyntaxBuilderWithContextTest()
        {
            this.behaviorProviders = new Queue<Func<object, IBehavior<ICustomExtension>>>();

            this.syntaxBuilder = new Mock<ISyntaxBuilderWithoutContext<ICustomExtension>>();
            this.endWithBehavior = this.syntaxBuilder.As<IEndWithBehavior<ICustomExtension>>();

            this.testee = new SyntaxBuilderWithContext<ICustomExtension, object>(this.syntaxBuilder.Object, this.behaviorProviders);
        }

        [Fact]
        public void End_ShouldDelegateToInternal()
        {
            IEndWithBehavior<ICustomExtension> result = this.testee.End;

            this.syntaxBuilder.Verify(b => b.End);
        }

        [Fact]
        public void Execute_WithAction_ShouldDelegateToInternal()
        {
            Action action = () => { };

            this.testee.Execute(action);

            this.syntaxBuilder.Verify(b => b.Execute(action));
        }

        [Fact]
        public void Execute_WithActionOnExtension_ShouldDelegateToInternal()
        {
            Action<ICustomExtension> action = e => { };

            this.testee.Execute(action);

            this.syntaxBuilder.Verify(b => b.Execute(action));
        }

        [Fact]
        public void Execute_WithInitializerAndActionOnExtension_ShouldDelegateToInternal()
        {
            Func<object> initializer = () => new object();
            Action<ICustomExtension, object> action = (e, ctx) => { };

            this.testee.Execute(initializer, action);

            this.syntaxBuilder.Verify(b => b.Execute(initializer, action));
        }

        [Fact]
        public void With_WithConstantBehaviorWhichReturnsEndWithBehavior_ShouldDelegateToInternal()
        {
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();
            
            this.testee.With(behavior);

            this.endWithBehavior.Verify(b => b.With(behavior));
        }

        [Fact]
        public void With_WithLazyBehaviorWhichReturnsEndWithBehavior_ShouldDelegateToInternal()
        {
            Func<IBehavior<ICustomExtension>> behaviorProvider = () => Mock.Of<IBehavior<ICustomExtension>>();

            this.testee.With(behaviorProvider);

            this.endWithBehavior.Verify(b => b.With(behaviorProvider));
        }

        [Fact]
        public void With_WithBehaviorOnContext_ShouldTrackBehaviorProviders()
        {
            var behavior = Mock.Of<IBehavior<ICustomExtension>>();
            var anyObject = new object();

            this.testee.With(ctx => behavior);

            this.behaviorProviders.Should().HaveCount(1);
            this.behaviorProviders.Single()(anyObject).Should().Be(behavior);
        }

        [Fact]
        public void GetEnumerator_ShouldDelegateToInternal()
        {
            var enumerator = this.testee.GetEnumerator();

            this.syntaxBuilder.Verify(b => b.GetEnumerator());
        }
    }
}