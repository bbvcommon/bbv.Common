//-------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="bbv Software Services AG">
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

    public static class TypeExtensions
    {
        public static string FullNameToString(this Type t)
        {
            if (!t.IsGenericType)
            {
                return t.FullName;
            }

            string value = t.FullName.Substring(0, t.FullName.IndexOf('`')) + "<";
            Type[] genericArgs = t.GetGenericArguments();
            var list = new List<string>();

            for (int i = 0; i < genericArgs.Length; i++)
            {
                value += "{" + i + "},";
                string s = FullNameToString(genericArgs[i]);
                list.Add(s);
            }

            value = value.TrimEnd(',');
            value += ">";
            value = string.Format(value, list.ToArray());
            return value;
        }
    }
}