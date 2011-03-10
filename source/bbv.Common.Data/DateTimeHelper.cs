//-------------------------------------------------------------------------------
// <copyright file="DateTimeHelper.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Data
{
    using System;

    /// <summary>
    /// Provides functionality for <see cref="DateTime"/>s.
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Returns the international Date Time Format for selecting 
        /// DateTime values from DataTables.
        /// </summary>
        public const string InternationalDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Same as InternationalDateTimeFormat but with the fraction of 
        /// a second.
        /// </summary>
        public const string InternationalDateTimeFormatwithFraction = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// Compares two dates with the resolution of one day.
        /// if both dates are in the same day the method returns 0, 
        /// if date 1 is older than date 2 the method returns a value 
        /// smaller 0 and vice versa
        /// </summary>
        /// <param name="day1">
        /// day of date 1
        /// </param>
        /// <param name="month1">
        /// month of date 1
        /// </param>
        /// <param name="year1">
        /// year of date 1
        /// </param>
        /// <param name="date2">
        /// date 2 to be compared
        /// </param>
        /// <returns>
        /// returns 0 if both dates are in the same day, smaller 0 if date1 is at least 
        /// one day older than date2 and vice versa  
        /// </returns>
        public static int CompareDay(int day1, int month1, int year1, DateTime date2)
        {
            return CompareDay(new DateTime(year1, month1, day1), date2);
        }

        /// <summary>
        /// Compares two dates with the resolution of one day.
        /// if both dates are in the same day the method returns 0, 
        /// if date 1 is older than date 2 the method returns a value 
        /// smaller 0 and vice versa
        /// </summary>
        /// <param name="date1">
        /// date 1 to be compared
        /// </param>
        /// <param name="date2">
        /// date 2 to be compared
        /// </param>
        /// <returns>
        /// returns 0 if both dates are in the same day, smaller 0 if date1 is at least 
        /// one day older than date2 and vice versa  
        /// </returns>
        public static int CompareDay(DateTime date1, DateTime date2)
        {
            return date1.Date == date2.Date ? 0 : date1.CompareTo(date2);
        }
    }
}
