//MIT License

//Copyright (c) 2022 Adita

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Encapsulates an error from the identity subsystem.
    /// </summary>
    public class IdentityError
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityError"/> identified by specified <paramref name="code"/> and <paramref name="description"/>.
        /// </summary>
        /// <param name="code">The code of error.</param>
        /// <param name="description">The error description.</param>
        public IdentityError(string code, string description)
        {
            Code = code;
            Description = description;
        }
        #endregion Constructors

        #region Public properties
        /// <summary>
        /// Gets the code for this error.
        /// </summary>
        public string Code { get; }
        /// <summary>
        /// Gets the description for this error.
        /// </summary>
        public string Description { get; }
        #endregion Public properties
    }
}
