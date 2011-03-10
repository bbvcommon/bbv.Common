//-------------------------------------------------------------------------------
// <copyright file="UnitTestHelper.cs" company="bbv Software Services AG">
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

namespace bbv.Common.TestUtilities
{
    using System;

    /// <summary>
    /// Provides helper functionality to write unit tests.
    /// </summary>
    public class UnitTestHelper
    {
        /// <summary>
        /// Executes the required unit test and if that unit test fails then the error message points to that unit test.
        /// Use this method to chain unit tests. Helps to find the root of a problem faster.
        /// </summary>
        /// <param name="unitTestMethod">The unit test method.</param>
        public static void ExecuteRequiredUnitTest(Action unitTestMethod)
        {
            const string ErrorMessage = "Required unit test failed: ";
            try
            {
                unitTestMethod();
            }
            catch (Exception e)
            {
                if (e.Message.StartsWith(ErrorMessage))
                {
                    throw;
                }

                throw new RequiredUnitTestException(ErrorMessage + unitTestMethod.Method.Name);
            }
        }
    }
}