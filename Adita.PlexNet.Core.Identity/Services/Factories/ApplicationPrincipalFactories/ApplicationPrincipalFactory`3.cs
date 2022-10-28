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
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Provides methods to create an <see cref="ApplicationPrincipal"/> for a given user.
    /// </summary>
    public class ApplicationPrincipalFactory<TKey, TUser, TRole> : IApplicationPrincipalFactory<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
    {
        #region Private fields
        private readonly IUserManager<TKey, TUser, TRole> _userManager;
        private readonly IRoleManager<TKey, TRole> _roleManager;
        private readonly ApplicationIdentityOptions _options;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPrincipalFactory{TKey, TUser, TRole}"/> class.
        /// </summary>
        /// <param name="userManager">The <see cref="IUserManager{TKey, TUser, TRole}"/> to retrieve user information from.</param>
        /// <param name="roleManager">The <see cref="IRoleManager{TKey, TRole}"/> to retrieve user information from.</param>
        /// <param name="options">An options to generate <see cref="ApplicationIdentity"/>.</param>
        public ApplicationPrincipalFactory(IUserManager<TKey, TUser, TRole> userManager,
            IRoleManager<TKey, TRole> roleManager, IOptions<ApplicationIdentityOptions> options)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _options = options.Value;
        }
        #endregion Constructors

        #region Public methods
        /// <summary>
        /// Creates a <see cref="ApplicationPrincipal"/> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ApplicationPrincipal"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ApplicationPrincipal"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="user"/> is <see langword="null"/></exception>
        public async Task<ApplicationPrincipal> CreateAsync(TUser user)
        {
            TUser? foundUser = await _userManager.FindByIdAsync(user.Id);
            if (foundUser == null)
            {
                throw new ArgumentException($"{nameof(user)} not found.");
            }

            ApplicationIdentity claimsIdentity = await GenerateApplicationIdentityAsync(user);

            return new ApplicationPrincipal(claimsIdentity);
        }
        /// <summary>
        /// Generate the <see cref="ApplicationIdentity"/> for a user.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ApplicationIdentity"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ApplicationIdentity"/>.</returns>
        public async Task<ApplicationIdentity> GenerateApplicationIdentityAsync(TUser user)
        {
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            IList<TRole> roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(_options.UserIdClaimType, user.Id.ToString()!));
            claims.Add(new Claim(_options.UserNameClaimType, user.UserName));

            foreach (var role in roles)
            {
                claims.Add(new Claim(_options.RoleClaimType, role.Name));
                IEnumerable<Claim> roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }

            return new ApplicationIdentity(claims, "Password", _options.UserNameClaimType,
                _options.RoleClaimType);
        }
        #endregion Public methods
    }
}
