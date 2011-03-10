//-------------------------------------------------------------------------------
// <copyright file="DriveUtilities.cs" company="bbv Software Services AG">
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
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Provides functionality to get drive information.
    /// </summary>
    public static class DriveUtilities
    {
        /// <summary>
        /// Gets the drive free space.
        /// </summary>
        /// <param name="driveName">Name of the drive.</param>
        /// <returns>free space in Byte</returns>
        public static double GetDriveFreeSpace(string driveName)
        {
            DriveInfo drive = new DriveInfo(driveName);
            return drive.AvailableFreeSpace;
        }

        /// <summary>
        /// Gets the total drive space.
        /// </summary>
        /// <param name="driveName">Name of the drive.</param>
        /// <returns>total drive space in Byte</returns>
        public static double GetTotalDriveSpace(string driveName)
        {
            DriveInfo drive = new DriveInfo(driveName);
            return drive.TotalSize;
        }

        /// <summary>
        /// Gets the size of the folder.
        /// </summary>
        /// <param name="path">The path of the folder</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns>folder size in Byte</returns>
        public static double GetFolderSize(string path, bool recursive)
        {
            double size = GetFolderSizeFlat(path);
            if (recursive)
            {
                foreach (string subFolder in Directory.GetDirectories(path))
                {
                    size += GetFolderSize(subFolder, true);
                }
            }

            return size;
        }

        /// <summary>
        /// Transforms the space in Bytes into more Readable form. 
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Transformed Size.</returns>
        public static string FormatByteSize(double bytes)
        {
            // Math.Pow(2, 40)
            if (bytes >= 1099511627776)
            {
                return string.Format(CultureInfo.CurrentCulture, "{0:F1} TB", bytes / 1099511627776);
            }

            // Math.Pow(2, 30)
            if (bytes >= 1073741824)
            {
                return string.Format(CultureInfo.CurrentCulture, "{0:F1} GB", bytes / 1073741824);
            }

            // Math.Pow(2, 20)
            if (bytes >= 1048576)
            {
                return string.Format(CultureInfo.CurrentCulture, "{0:f1} MB", bytes / 1048576);
            }

            // Math.Pow(2, 10)
            if (bytes >= 1024)
            {
                return string.Format(CultureInfo.CurrentCulture, "{0:f1} KB", bytes / 1024);
            }

            if (bytes > 1 && bytes < 1024)
            {
                return string.Format(CultureInfo.CurrentCulture, "{0:f0} Bytes", bytes);
            }

            if (bytes == 1)
            {
                return string.Format(CultureInfo.CurrentCulture, "{0:f0} Byte", bytes);
            }

            return string.Format(CultureInfo.CurrentCulture, "{0:f0} Bytes", bytes);
        }

        /// <summary>
        /// Gets the folder size flat.
        /// </summary>
        /// <param name="path">The path of the folder.</param>
        /// <returns>The flat size of the folder.</returns>
        private static double GetFolderSizeFlat(string path)
        {
            double size = 0;
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo f in dir.GetFiles())
            {
                size += f.Length;
            }

            return size;
        }
    }
}