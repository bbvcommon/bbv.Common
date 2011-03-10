//-------------------------------------------------------------------------------
// <copyright file="MockMapperExtensions.cs" company="bbv Software Services AG">
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

namespace bbv.Common.MappingEventBrokerExtension
{
    using System;

    using Moq;

    public static class MockMapperExtensions
    {
        public static void SetupMapping(this Mock<IMapper> mapper)
        {
            mapper.Setup(m => m.HasMapping(It.IsAny<Type>(), It.IsAny<Type>()))
                .Returns(true);

            SourceEventArgs sourceArgs = null;

            mapper.Setup(m => m.Map(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<EventArgs>()))
                .Callback<Type, Type, EventArgs>((s, d, e) => { sourceArgs = (SourceEventArgs)e; })
                .Returns(() => new DestinationEventArgs(sourceArgs.Source));
        }
    }
}