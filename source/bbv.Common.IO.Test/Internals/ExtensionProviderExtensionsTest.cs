//-------------------------------------------------------------------------------
// <copyright file="ExtensionProviderExtensionsTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.IO.Internals
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using Xunit;

    public class ExtensionProviderExtensionsTest
    {
        private const int ExpectedReturnValue = 1;

        private readonly Mock<IExtensionProvider<IExtension>> provider;

        private readonly Mock<IExtension> extension;

        private Exception exception;

        private string stringParameter;

        private bool boolParameter;

        private bool doThrowException;

        public interface IExtension
        {
            void BeginDo(bool s);

            void EndDo(bool s);

            void BeginDo(string s);

            void EndDo(string s);

            void FailDo(ref Exception exception);

            void BeginDoReturn(string s);

            void EndDoReturn(int result, string s);

            void BeginDoReturn(bool s);

            void EndDoReturn(int result, bool s);

            void FailDoReturn(ref Exception exception);
        }

        public ExtensionProviderExtensionsTest()
        {
            this.provider = new Mock<IExtensionProvider<IExtension>>();
            this.extension = new Mock<IExtension>();

            this.exception = new Exception();
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingAction_MustCallBeginWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            this.extension.Verify(e => e.BeginDo(ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingAction_MustCallEndWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            this.extension.Verify(e => e.EndDo(ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingAction_MustCallActionWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            Assert.Equal(ExpectedParameter, this.stringParameter);
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingAction_MustCallFailWithCorrectExceptionAndRethrow()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();
            this.SetupThrowsException();

            Assert.Throws<Exception>(() => this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter));

            this.extension.Verify(e => e.FailDo(ref this.exception));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingAction_MustCallFailWithCorrectExceptionAndRethrowExchangedException()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensionsWithExceptionExchangingExtension();
            this.SetupThrowsException();

            Assert.Throws<InvalidOperationException>(() => this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingActionWithOverload_MustCallBeginWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            this.extension.Verify(e => e.BeginDo(ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingActionWithOverload_MustCallEndWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            this.extension.Verify(e => e.EndDo(ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingActionWithOverload_MustCallActionWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter);

            Assert.Equal(ExpectedParameter, this.boolParameter);
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingActionWithOverload_MustCallFailWithCorrectExceptionAndRethrow()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();
            this.SetupThrowsException();

            Assert.Throws<Exception>(() => this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter));

            this.extension.Verify(e => e.FailDo(ref this.exception));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingActionWithOverload_MustCallFailWithCorrectExceptionAndRethrowExchangedException()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensionsWithExceptionExchangingExtension();
            this.SetupThrowsException();

            Assert.Throws<InvalidOperationException>(() => this.provider.Object.SurroundWithExtension(() => this.Do(ExpectedParameter), ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFunc_MustCallBeginWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            this.extension.Verify(e => e.BeginDoReturn(ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFunc_MustCallEndWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            this.extension.Verify(e => e.EndDoReturn(ExpectedReturnValue, ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFunc_MustCallActionWithCorrectParameters()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();

            int result = this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            Assert.Equal(ExpectedParameter, this.stringParameter);
            Assert.Equal(ExpectedReturnValue, result);
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFunc_MustCallFailWithCorrectExceptionAndRethrow()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensions();
            this.SetupThrowsException();

            Assert.Throws<Exception>(() => this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter));

            this.extension.Verify(e => e.FailDoReturn(ref this.exception));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFunc_MustCallFailWithCorrectExceptionAndRethrowExchangedException()
        {
            const string ExpectedParameter = "Test";

            this.SetupExtensionsWithExceptionExchangingExtension();
            this.SetupThrowsException();

            Assert.Throws<InvalidOperationException>(() => this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFuncWithOverload_MustCallBeginWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            this.extension.Verify(e => e.BeginDoReturn(ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFuncWithOverload_MustCallEndWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            this.extension.Verify(e => e.EndDoReturn(ExpectedReturnValue, ExpectedParameter));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFuncWithOverload_MustCallActionWithCorrectParameters()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();

            int result = this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter);

            Assert.Equal(ExpectedParameter, this.boolParameter);
            Assert.Equal(ExpectedReturnValue, result);
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFuncWithOverload_MustCallFailWithCorrectExceptionAndRethrow()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensions();
            this.SetupThrowsException();

            Assert.Throws<Exception>(() => this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter));

            this.extension.Verify(e => e.FailDoReturn(ref this.exception));
        }

        [Fact]
        public void SourroundWithExtensions_WhenUsingFuncWithOverload_MustCallFailWithCorrectExceptionAndRethrowExchangedException()
        {
            const bool ExpectedParameter = true;

            this.SetupExtensionsWithExceptionExchangingExtension();
            this.SetupThrowsException();

            Assert.Throws<InvalidOperationException>(() => this.provider.Object.SurroundWithExtension(() => this.DoReturn(ExpectedParameter), ExpectedParameter));
        }

        private void Do(bool parameter)
        {
            if (this.doThrowException)
            {
                throw this.exception;
            }

            this.boolParameter = parameter;
        }

        private void Do(string parameter)
        {
            if (this.doThrowException)
            {
                throw this.exception;
            }

            this.stringParameter = parameter;
        }

        private int DoReturn(string parameter)
        {
            if (this.doThrowException)
            {
                throw this.exception;
            }

            this.stringParameter = parameter;

            return ExpectedReturnValue;
        }

        private int DoReturn(bool parameter)
        {
            if (this.doThrowException)
            {
                throw this.exception;
            }

            this.boolParameter = parameter;

            return ExpectedReturnValue;
        }

        private void SetupThrowsException()
        {
            this.doThrowException = true;
        }

        private void SetupExtensions()
        {
            this.provider.Setup(p => p.Extensions).Returns(new List<IExtension> { this.extension.Object });
        }

        private void SetupExtensionsWithExceptionExchangingExtension()
        {
            this.provider.Setup(p => p.Extensions).Returns(new List<IExtension> { new ExceptionExchangingExtension(this.exception), new SecondExceptionExchangingExtension(this.exception) });
        }

        private class ExceptionExchangingExtension : IExtension 
        {
            private readonly Exception exceptionToThrow;

            public ExceptionExchangingExtension(Exception exceptionToThrow)
            {
                this.exceptionToThrow = exceptionToThrow;
            }

            public void BeginDo(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDo(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void BeginDo(string s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDo(string s)
            {
                throw this.exceptionToThrow;
            }

            public void FailDo(ref Exception exception)
            {
                exception = new InvalidOperationException();
            }

            public void BeginDoReturn(string s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(int result, string s)
            {
                throw this.exceptionToThrow; 
            }

            public void BeginDoReturn(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(int result, bool s)
            {
                throw this.exceptionToThrow;
            }

            public void FailDoReturn(ref Exception exception)
            {
                exception = new InvalidOperationException();
            }
        }

        private class SecondExceptionExchangingExtension : IExtension
        {
            private readonly Exception exceptionToThrow;

            public SecondExceptionExchangingExtension(Exception exceptionToThrow)
            {
                this.exceptionToThrow = exceptionToThrow;
            }

            public void BeginDo(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDo(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void BeginDo(string s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDo(string s)
            {
                throw this.exceptionToThrow;
            }

            public void FailDo(ref Exception exception)
            {
                exception = new ArgumentNullException();
            }

            public void BeginDoReturn(string s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(int result, string s)
            {
                throw this.exceptionToThrow;
            }

            public void BeginDoReturn(bool s)
            {
                throw this.exceptionToThrow;
            }

            public void EndDoReturn(int result, bool s)
            {
                throw this.exceptionToThrow;
            }

            public void FailDoReturn(ref Exception exception)
            {
                exception = new ArgumentNullException();
            }
        }
    }
}