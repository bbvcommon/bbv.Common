//-------------------------------------------------------------------------------
// <copyright file="IObserveFolder.cs" company="bbv Software Services AG">
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
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Allows to observe a given folder for just one file change.
    /// </summary>
    public interface IObserveFolder
    {
        /// <summary>
        /// Starts the observation on the given folder with the specified filter. When a file changed in the observed folder
        /// the observation is stopped and the file's path is returned as result of the task.
        /// </summary>
        /// <param name="folder">The folder to be observed.</param>
        /// <param name="filter">The filter to be used.</param>
        /// <param name="cancellationToken">The cancellation token which allows to stop the observation.</param>
        /// <returns>The fully qualified path to the changed file.</returns>
        Task<string> Start(string folder, string filter, CancellationToken cancellationToken);
    }
}