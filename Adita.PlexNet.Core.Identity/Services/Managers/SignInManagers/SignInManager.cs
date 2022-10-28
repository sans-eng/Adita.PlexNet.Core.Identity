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

using Adita.PlexNet.Core.Security.Principals;
using Adita.PlexNet.Core.Security.Claims;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Represents a sign-in manager.
    /// </summary>
    public class SignInManager<TKey, TUser, TRole> : ISignInManager<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
    {
        #region Private fields
        private readonly IUserManager<TKey, TUser, TRole> _userManager;
        private readonly IApplicationPrincipalFactory<TUser> _principalFactory;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="SignInManager{TKey, TUser, TRole}"/> using specified <paramref name="userManager"/> and <paramref name="principalFactory"/>.
        /// </summary>
        /// <param name="userManager">A <see cref="IUserManager{TKey, TUser, TRole}"/> to get user from.</param>
        /// <param name="principalFactory">A <see cref="IApplicationPrincipalFactory{TUser}"/> to create a <see cref="ApplicationIdentity"/>.</param>
        public SignInManager(IUserManager<TKey, TUser, TRole> userManager, IApplicationPrincipalFactory<TUser> principalFactory)
        {
            _userManager = userManager;
            _principalFactory = principalFactory;
        }
        #endregion Constructors

        #region Public methods

        /// <summary>
        /// Attempts to sign in the specified <paramref name="userName" /> and <paramref name="password" /> combination asynchronously.
        /// </summary>
        /// <param name="userName">The user name to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation which contains a <see cref="SignInResult" />.</returns>
        /// <exception cref="ArgumentException"><paramref name="userName"/> is <c>null</c> or <c>empty</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentPrincipal"/> is not <see cref="ApplicationPrincipal"/>.</exception>
        public async Task<SignInResult> PasswordSignInAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException($"'{nameof(userName)}' cannot be null or empty.", nameof(userName));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            TUser? foundUser = await _userManager.FindByNameAsync(userName);
            if (foundUser == null)
            {
                return SignInResult.InvalidCredential;
            }

            PasswordVerificationResult result = await _userManager.VerifyPasswordAsync(foundUser, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return SignInResult.InvalidCredential;
            }

            bool isLockedOut = await _userManager.IsLockedOutAsync(foundUser);
            if (isLockedOut)
            {
                return SignInResult.LockedOut;
            }

            if (Thread.CurrentPrincipal is not ApplicationPrincipal currentPrincipal)
            {
                throw new InvalidOperationException($"{nameof(Thread.CurrentPrincipal)} is not {nameof(ApplicationPrincipal)}.");
            }

            ApplicationIdentity identity = await _principalFactory.GenerateApplicationIdentityAsync(foundUser);

            currentPrincipal.SetIdentity(identity);

            return SignInResult.Succeeded;
        }
        /// <summary>
        /// Signs the current user out of the application.
        /// </summary>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentPrincipal"/> is not <see cref="ApplicationPrincipal"/>.</exception>
        public Task SignOutAsync()
        {
            if (Thread.CurrentPrincipal is not ApplicationPrincipal currentPrincipal)
            {
                throw new InvalidOperationException($"{nameof(Thread.CurrentPrincipal)} is not {nameof(ApplicationPrincipal)}.");
            }

            currentPrincipal.SetIdentity(new ApplicationIdentity());

            return Task.CompletedTask;
        }
        #endregion Public methods
    }
}
