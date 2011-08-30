//-------------------------------------------------------------------------------
// <copyright file="ActionOnExtensionWithInitializerExecutableTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Syntax.Executables
{
    using System.Collections.Generic;
    using System.Linq;

    using bbv.Common.Bootstrapper.Dummies;

    using FluentAssertions;

    using Moq;

    using Xunit;

    public class ActionOnExtensionWithInitializerExecutableTest
    {
        private readonly object context;

        private readonly ActionOnExtensionWithInitializerExecutable<object, ICustomExtension> testee;

        private int contextAccessCounter;

        private IBehaviorAware<IExtension> interceptedBehaviorAware;

        public ActionOnExtensionWithInitializerExecutableTest()
        {
            this.context = new object();

            this.testee = new ActionOnExtensionWithInitializerExecutable<object, ICustomExtension>(
                () => this.CountAccessToContext(),
                (x, i) => x.SomeMethod(i),
                (aware, ctx) => { this.interceptedBehaviorAware = aware; });
        }

        [Fact]
        public void Execute_ShouldCallInitializerOnce()
        {
            this.testee.Execute(new List<ICustomExtension> { Mock.Of<ICustomExtension>(), Mock.Of<ICustomExtension>() });

            this.contextAccessCounter.Should().Be(1);
        }

        [Fact]
        public void Execute_ShouldPassItselfToInitializer()
        {
            this.testee.Execute(new List<ICustomExtension> { Mock.Of<ICustomExtension>(), Mock.Of<ICustomExtension>() });

            this.interceptedBehaviorAware.Should().Be(this.testee);
        }

        [Fact]
        public void Execute_ShouldExecuteActionOnExtensions()
        {
            var firstExtension = new Mock<ICustomExtension>();
            var secondExtension = new Mock<ICustomExtension>();

            this.testee.Execute(new List<ICustomExtension> { firstExtension.Object, secondExtension.Object });

            firstExtension.Verify(x => x.SomeMethod(It.IsAny<object>()));
            secondExtension.Verify(x => x.SomeMethod(It.IsAny<object>()));
        }

        [Fact]
        public void Execute_ShouldPassContextToExtensions()
        {
            var firstExtension = new Mock<ICustomExtension>();
            var secondExtension = new Mock<ICustomExtension>();

            this.testee.Execute(new List<ICustomExtension> { firstExtension.Object, secondExtension.Object });

            firstExtension.Verify(x => x.SomeMethod(this.context));
            secondExtension.Verify(x => x.SomeMethod(this.context));
        }

        [Fact]
        public void Execute_ShouldExecuteBehavior()
        {
            var first = new Mock<IBehavior<ICustomExtension>>();
            var second = new Mock<IBehavior<ICustomExtension>>();
            var extensions = Enumerable.Empty<ICustomExtension>();

            this.testee.Add(first.Object);
            this.testee.Add(second.Object);

            this.testee.Execute(extensions);

            first.Verify(b => b.Behave(extensions));
            second.Verify(b => b.Behave(extensions));
        }

        private object CountAccessToContext()
        {
            this.contextAccessCounter++;
            return this.context;
        }
    }
}