//-------------------------------------------------------------------------------
// <copyright file="XmlEventArgsSerializerTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.DistributedEventBroker.Serializer
{
    using System.ComponentModel;
    using Xunit;

    public class XmlEventArgsSerializerTest
    {
        private const string InputAndOutput = @"<?xml version=""1.0"" encoding=""utf-16""?>
<CancelEventArgs xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Cancel>true</Cancel>
</CancelEventArgs>";

        private readonly XmlEventArgsSerializer testee;

        public XmlEventArgsSerializerTest()
        {
            this.testee = new XmlEventArgsSerializer();
        }

        [Fact]
        public void Serialize_MustSerializeEventArgsToXmlString()
        {
            var eventArgs = new CancelEventArgs(true);

            var result = this.testee.Serialize(eventArgs);

            Assert.Equal(InputAndOutput, result);
        }

        [Fact]
        public void Deserialize_MustDeserializeEventArgsFromXmlString()
        {
            var result = this.testee.Deserialize(typeof(CancelEventArgs), InputAndOutput);

            Assert.IsType<CancelEventArgs>(result);
            Assert.True(((CancelEventArgs)result).Cancel);
        }
    }
}