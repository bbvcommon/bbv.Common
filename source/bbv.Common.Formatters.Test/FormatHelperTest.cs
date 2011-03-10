//-------------------------------------------------------------------------------
// <copyright file="FormatHelperTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Formatters
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class FormatHelperTest
    {
        [Test]
        public void SecureFormat()
        {
            const string Format = "{0} {1} {2}";
            const string Expected = "!!! FORMAT ERROR !!!! " + Format + ": ";
            const string Correct = "1 1.25 string";
            const int I = 1;
            const decimal D = 1.25m;
            const string S = "string";

            string result;
            result = FormatHelper.SecureFormat(Format, I);
            Assert.AreEqual(Expected + I + ", ", result, "Too few parameters.");

            result = FormatHelper.SecureFormat(Format, I, D, S);
            Assert.AreEqual(Correct, result, "Correct number of parameters.");

            result = FormatHelper.SecureFormat(Format, I, D, S, I);
            Assert.AreEqual(Correct, result, "Too much parameters.");
        }

        [Test]
        public void SecureFormatSpecialValues()
        {
            Assert.AreEqual(string.Empty, FormatHelper.SecureFormat(null), "null should result in empty string.");
            Assert.AreEqual("format", FormatHelper.SecureFormat("format", null), "no args should result in format string.");
        }

        [Test]
        public void ConvertCollectionToString()
        {
            string s = FormatHelper.ConvertToString(new object[] { 3, "hello", new Exception("exception") }, ", ");
            Assert.AreEqual("3, hello, System.Exception: exception", s);
        }

        [Test]
        public void ConvertDictionaryToString()
        {
            IDictionary<object, object> dictionary = new Dictionary<object, object>();
            dictionary.Add("bla", "haha");
            dictionary.Add(1, 25);
            dictionary.Add("exception", new Exception("exception"));

            string s = FormatHelper.ConvertToString(dictionary, "; ");

            Assert.AreEqual("bla=haha; 1=25; exception=System.Exception: exception", s);
        }
    }
}
