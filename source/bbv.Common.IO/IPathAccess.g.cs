//-------------------------------------------------------------------------------
// <copyright file="IPathAccess.cs" company="bbv Software Services AG">
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
    using System.Collections.Generic;

    /// <summary>
    /// Abstraction layer which simplifies access to paths.
    /// </summary>
    public interface IPathAccess
    {
        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetDirectoryName(System.String)"]/*' />
        string GetDirectoryName(string path);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetFileName(System.String)"]/*' />
        string GetFileName(string path);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetFileNameWithoutExtension(System.String)"]/*' />
        string GetFileNameWithoutExtension(string path);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.Combine(System.String,System.String)"]/*' />
        string Combine(string path1, string path2);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetRandomFileName"]/*' />
        string GetRandomFileName();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.ChangeExtension(System.String,System.String)"]/*' />
        string ChangeExtension(string path, string extension);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetExtension(System.String)"]/*' />
        string GetExtension(string path);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetFullPath(System.String)"]/*' />
        string GetFullPath(string path);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetInvalidFileNameChars"]/*' />
        IEnumerable<char> GetInvalidFileNameChars();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetInvalidPathChars"]/*' />
        IEnumerable<char> GetInvalidPathChars();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetPathRoot(System.String)"]/*' />
        string GetPathRoot(string path);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetTempFileName"]/*' />
        string GetTempFileName();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.GetTempPath"]/*' />
        string GetTempPath();

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.HasExtension(System.String)"]/*' />
        bool HasExtension(string path);

        /// <include file='mscorlib.xml' path='/doc/members/member[@name="M:System.IO.Path.IsPathRooted(System.String)"]/*' />
        bool IsPathRooted(string path);
    }
}