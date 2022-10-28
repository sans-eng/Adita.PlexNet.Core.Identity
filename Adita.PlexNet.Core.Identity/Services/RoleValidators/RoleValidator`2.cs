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

using Microsoft.Extensions.Options;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Provides the validation of roles.
    /// </summary>
    public class RoleValidator<TKey, TRole> : IRoleValidator<TRole>
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>
    {
        #region Private fields
        private readonly IdentityErrorDescriber _errorDescriber;
        private readonly RoleOptions _options;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="RoleValidator{TKey, TRole}"/>.
        /// </summary>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber"/> to provides the errors localization.</param>
        /// <param name="options">A <see cref="RoleOptions"/> which provides options for validations.</param>

        public RoleValidator(IdentityErrorDescriber errorDescriber,
            IOptions<RoleOptions> options)
        {
            _errorDescriber = errorDescriber;
            _options = options.Value;
        }
        #endregion Constructors

        #region Public methods
        /// <summary>
        /// Validates a role as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to validate.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the <see cref="IdentityResult" /> of the asynchronous validation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <see langword="null"/>.</exception>
        public async Task<IdentityResult> ValidateAsync(TRole role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            //validates role name length
            if (role.Name.Length < _options.RequiredRoleNameLength ||
                role.Name.Any(n => !_options.AllowedRoleNameCharacters.Contains(n)))
            {
                return new IdentityResult(false, _errorDescriber.InvalidRoleName(role.Name));
            }

            return await Task.FromResult(IdentityResult.Success);
        }
        #endregion Public methods
    }
}
