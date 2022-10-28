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
using System.Net.Mail;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Provides validation services for user classes.
    /// </summary>
    public class UserValidator<TKey, TUser> : IUserValidator<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        #region Private fields
        private readonly IdentityErrorDescriber _errorDescriber;
        private readonly UserOptions _options;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="UserValidator{TKey, TUser}"/>.
        /// </summary>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber"/> to provides the errors localization.</param>
        /// <param name="options">A <see cref="UserOptions"/> which provides options for validations.</param>
        public UserValidator(IdentityErrorDescriber errorDescriber,
            IOptions<UserOptions> options)
        {
            _errorDescriber = errorDescriber;
            _options = options.Value;
        }
        #endregion Constructors

        #region Public methods
        /// <summary>
        /// Validates the specified <paramref name="user" /> as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult" /> of the validation operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/>/.</exception>
        public async Task<IdentityResult> ValidateAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            //validate allowed chars
            if (user.UserName.Any(c => !_options.AllowedUserNameCharacters.Contains(c)))
            {
                return new IdentityResult(false, _errorDescriber.InvalidUserName(user.UserName));
            }
            return await Task.FromResult(IdentityResult.Success);
        }
        #endregion Public methods

        #region Private methods
        /// <summary>
        /// Reserve for future release.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        #endregion Private methods
    }
}
