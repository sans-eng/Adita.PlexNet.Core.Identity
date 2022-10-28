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
    /// Provides the default password policy for Identity.
    /// </summary>
    public class PasswordValidator : IPasswordValidator
    {
        #region Private fields
        private readonly PasswordOptions _passwordOptions;
        private readonly IdentityErrorDescriber _identityErrorDescriber;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialze new instance of <see cref="PasswordValidator"/>.
        /// </summary>
        /// <param name="options">An <see cref="IOptions{TOptions}"/> of <see cref="PasswordOptions"/> to use.</param>
        /// <param name="identityErrorDescriber">An <see cref="IdentityErrorDescriber"/> to be used.</param>
        public PasswordValidator(IOptions<PasswordOptions> options, IdentityErrorDescriber identityErrorDescriber)
        {
            _passwordOptions = options.Value;
            _identityErrorDescriber = identityErrorDescriber;
        }
        #endregion Constructors

        #region Public methods
        /// <summary>
        /// Validates a password.
        /// </summary>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>An <see cref="IdentityResult" /> of password validation</returns>
        public IdentityResult Validate(string password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (_passwordOptions.RequireDigit && !password.Any(c => char.IsNumber(c)))
            {
                return new IdentityResult(false, _identityErrorDescriber.PasswordRequiresDigit());
            }

            if (_passwordOptions.RequiredLength > password.Length)
            {
                return new IdentityResult(false, _identityErrorDescriber.PasswordTooShort(_passwordOptions.RequiredLength));
            }

            if (_passwordOptions.RequiredUniqueChars > password.Distinct().Count())
            {
                return new IdentityResult(false, _identityErrorDescriber.PasswordRequiresUniqueChars(_passwordOptions.RequiredUniqueChars));
            }

            if (_passwordOptions.RequiredLowercase && !password.Any(c => char.IsLower(c)))
            {
                return new IdentityResult(false, _identityErrorDescriber.PasswordRequiresLower());
            }

            if (_passwordOptions.RequireNonAlphanumeric && password.All(c => char.IsDigit(c) || char.IsLetter(c)))
            {
                return new IdentityResult(false, _identityErrorDescriber.PasswordRequiresNonAlphanumeric());
            }

            if (_passwordOptions.RequireUppercase && !password.Any(c => char.IsUpper(c)))
            {
                return new IdentityResult(false, _identityErrorDescriber.PasswordRequiresUpper());
            }

            return new IdentityResult(true);

        }
        #endregion Public methods

    }
}
