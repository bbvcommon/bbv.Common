//-------------------------------------------------------------------------------
// <copyright file="FilesystemHelper.cs" company="bbv Software Services AG">
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

namespace bbv.Common.IO
{
    using System;

    /// <summary>
    /// Contains static methods to get informations about Filesystem.
    /// </summary>
    public static class FilesystemHelper
    {
        /// <summary>
        /// Specifies the unit for converting from i.e. Bytes to MegaBytes.
        /// </summary>
        /// <remarks>
        /// The values are tightly coupled to the <see cref="ConvertBytes"/> method!
        /// </remarks>
        public enum ByteUnit
        {
            /// <summary>
            /// Unit is byte
            /// </summary>
            Bytes = 0,

            /// <summary>
            /// Unit is Kilobytes
            /// </summary>
            Kilobytes = 1,

            /// <summary>
            /// Unit is Megabytes
            /// </summary>
            Megabytes = 2,

            /// <summary>
            /// Unit is Gigabytes
            /// </summary>
            Gigabytes = 3
        }

        /// <summary>
        /// Converts a byte value into the requested <see cref="ByteUnit"/>.
        /// </summary>
        /// <param name="bytes">The byte value to convert.</param>
        /// <param name="unit">The <see cref="ByteUnit"/> to convert into.</param>
        /// <returns>
        /// Returns a double value in the <see cref="ByteUnit"/> requested by the caller.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Exception will be thrown if the <see cref="ByteUnit"/> enumeration 
        /// was out of the defined range.
        /// </exception>
        public static double ConvertBytes(double bytes, ByteUnit unit)
        {
            if (!Enum.IsDefined(typeof(ByteUnit), unit))
            {
                throw new ArgumentOutOfRangeException("unit", unit, "Undefined value for ByteUnit.");
            }

            double result = bytes / System.Math.Pow(1024, Convert.ToDouble((int)unit));
            return result;
        }

        /// <summary>
        /// Gets the free space left on the disk given by a directory.
        /// </summary>
        /// <param name="directory">Directory where the free disk space should be counted.</param>
        /// <returns>
        /// Returns the free disk space in bytes, that can be converted by the 
        /// <see cref="ConvertBytes"/> method or by calling the overloaded 
        /// version of this method.
        /// </returns>
        public static double GetDiskFreeSpace(string directory)
        {
            // Don't use WMI ( System.Management.ManagementObject) because of
            // unexpected COM errors resulting in a COMException while logging.
            // 24.06.2003 tw

            // In case the current directory was given by a ".", expand it to the full path.
            string fullpath = System.IO.Path.GetFullPath(directory);
            string dirOnly = System.IO.Path.GetDirectoryName(fullpath);
            return InternalGetDiskFreeSpace(dirOnly);
        }

        /// <summary>
        /// Overloaded method that gets the free space left on the disk given by a directory.
        /// </summary>
        /// <param name="directory">Directory where the free disk space should be counted.</param>
        /// <param name="unit">The <see cref="ByteUnit"/> in which the result should be returned.</param>
        /// <returns>
        /// Returns the free disk space in the requested unit, i.e. MegaBytes / GigaBytes and so on.
        /// </returns>
        public static double GetDiskFreeSpace(string directory, ByteUnit unit)
        {
            double diskFreeBytes = GetDiskFreeSpace(directory);
            return ConvertBytes(diskFreeBytes, unit);
        }

        /// <summary>
        /// Adds a "\" to the directory if there is none.
        /// </summary>
        /// <param name="directory">The directory to normalize.</param>
        /// <returns>
        /// Returns the normalized directory always ending with a "\".
        /// </returns>
        public static string NormalizeDirectory(string directory)
        {
            if (!directory.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            {
                directory += System.IO.Path.DirectorySeparatorChar;
            }

            return directory;
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern int GetDiskFreeSpaceEx(string directoryName, out ulong freeBytesAvailable, out ulong totalNumberOfBytes, out ulong totalNumberOfFreeBytes);

        private static double InternalGetDiskFreeSpace(string directory)
        {
            ulong freeBytesAvailable;
            ulong totalNumberOfBytes;
            ulong totalNumberOfFreeBytes;
            GetDiskFreeSpaceEx(
                directory, out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes);

            // From the SDK Documentation:
            // Windows 2000/XP: If per-user quotas are in use, this value may 
            // be less than the total number of free bytes on the disk. 
            return Convert.ToDouble(freeBytesAvailable);
        }
    }
}
