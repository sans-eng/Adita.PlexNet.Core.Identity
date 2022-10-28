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

using Adita.PlexNet.Core.Security.Claims;
using Adita.PlexNet.Core.Security.Principals;
using System.Security.Claims;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Provides an abstraction for a factory to create a <see cref="ClaimsPrincipal"/> from a user.
    /// </summary>
    public interface IApplicationPrincipalFactory<TUser>
    {
        #region Methods
        /// <summary>
        /// Creates a <see cref="ApplicationPrincipal"/> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ApplicationPrincipal"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ApplicationPrincipal"/>.</returns>
        Task<ApplicationPrincipal> CreateAsync(TUser user);
        /// <summary>
        /// Generate the <see cref="ApplicationIdentity"/> for a user.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ApplicationIdentity"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ApplicationIdentity"/>.</returns>
        Task<ApplicationIdentity> GenerateApplicationIdentityAsync(TUser user);
        #endregion Methods
    }
}
