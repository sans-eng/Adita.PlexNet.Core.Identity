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

using Adita.Identity.Core.Internals;
using Adita.PlexNet.Core.Identity.Internals.Resources;
using System.Globalization;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Service to enable localization for application facing identity errors.
    /// </summary>
    public class IdentityErrorDescriber
    {
        #region Private fields
        private readonly CultureInfo _currentCulture = CultureInfo.CurrentUICulture;
        #endregion Private fields

        #region Public methods
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a concurrency failure.</returns>
        public virtual IdentityError ConcurrencyFailure()
        {
            return new IdentityError(IdentityErrorCodes.ConcurrencyFailure,
                GetLocalizedString(ErrorStrings.ConcurrencyFailure, _currentCulture));
        }
        /// <summary>
        /// Returns the default <see cref="IdentityError"/>.
        /// </summary>
        /// <returns>The default <see cref="IdentityError"/>.</returns>
        public virtual IdentityError DefaultError()
        {
            return new IdentityError(IdentityErrorCodes.DefaultError,
                GetLocalizedString(ErrorStrings.DefaultError, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified <paramref name="email"/> is already associated with an account.
        /// </summary>
        /// <param name="email">The email that is already associated with an account.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specified <paramref name="email"/> is already associated with an account.</returns>
        public virtual IdentityError DuplicateEmail(string email)
        {
            return new IdentityError(IdentityErrorCodes.DuplicateEmail,
                FormatString(GetLocalizedString(ErrorStrings.DuplicateEmail, _currentCulture), email));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified <paramref name="role"/> name already exists.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specific <paramref name="role"/> name already exists.</returns>
        public virtual IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError(IdentityErrorCodes.DuplicateRoleName,
                FormatString(GetLocalizedString(ErrorStrings.DuplicateRoleName, _currentCulture), role));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified <paramref name="userName"/> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specified <paramref name="userName"/> already exists.</returns>
        public virtual IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError(IdentityErrorCodes.DuplicateUserName,
                FormatString(GetLocalizedString(ErrorStrings.DuplicateUserName, _currentCulture), userName));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified <paramref name="email"/> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specified email is invalid.</returns>
        public virtual IdentityError InvalidEmail(string email)
        {
            return new IdentityError(IdentityErrorCodes.InvalidEmail,
                FormatString(GetLocalizedString(ErrorStrings.InvalidEmail, _currentCulture), email));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified <paramref name="role"/> name is invalid.
        /// </summary>
        /// <param name="role">The invalid role.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specific <paramref name="role"/> name is invalid.</returns>
        public virtual IdentityError InvalidRoleName(string role)
        {
            return new IdentityError(IdentityErrorCodes.InvalidRoleName,
                FormatString(GetLocalizedString(ErrorStrings.InvalidRoleName, _currentCulture), role));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating an invalid token.</returns>
        public virtual IdentityError InvalidToken()
        {
            return new IdentityError(IdentityErrorCodes.InvalidToken,
                GetLocalizedString(ErrorStrings.InvalidToken, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified <paramref name="userName"/> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specified <paramref name="userName"/> is invalid.</returns>
        public virtual IdentityError InvalidUserName(string userName)
        {
            return new IdentityError(IdentityErrorCodes.InvalidUserName,
                FormatString(GetLocalizedString(ErrorStrings.InvalidUserName, _currentCulture), userName));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating an external login is already associated with an account.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating an external login is already associated with an account.</returns>
        public virtual IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError(IdentityErrorCodes.LoginAlreadyAssociated,
                GetLocalizedString(ErrorStrings.LoginAlreadyAssociated, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password mismatch.</returns>
        public virtual IdentityError PasswordMismatch()
        {
            return new IdentityError(IdentityErrorCodes.PasswordMismatch,
                GetLocalizedString(ErrorStrings.PasswordMismatch, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password entered does not contain a numeric character.</returns>
        public virtual IdentityError PasswordRequiresDigit()
        {
            return new IdentityError(IdentityErrorCodes.PasswordRequiresDigit,
                GetLocalizedString(ErrorStrings.PasswordRequiresDigit, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password entered does not contain a lower case letter.</returns>
        public virtual IdentityError PasswordRequiresLower()
        {
            return new IdentityError(IdentityErrorCodes.PasswordRequiresLower,
                GetLocalizedString(ErrorStrings.PasswordRequiresLower, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public virtual IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError(IdentityErrorCodes.PasswordRequiresNonAlphanumeric,
                GetLocalizedString(ErrorStrings.PasswordRequiresNonAlphanumeric, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.
        /// </summary>
        /// <param name="uniqueChars">The number of different chars that must be used.</param>
        /// <returns>An <see cref="IdentityError"/> indicating a password does not meet the minimum number <paramref name="uniqueChars"/> of unique chars.</returns>
        public virtual IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError(IdentityErrorCodes.PasswordRequiresUniqueChars,
                FormatString(GetLocalizedString(ErrorStrings.PasswordRequiresUniqueChars, _currentCulture), uniqueChars));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a password entered does not contain an upper case letter.</returns>
        public virtual IdentityError PasswordRequiresUpper()
        {
            return new IdentityError(IdentityErrorCodes.PasswordRequiresUpper,
                GetLocalizedString(ErrorStrings.PasswordRequiresUpper, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a password of the specified <paramref name="length"/> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="IdentityError"/> indicating a password of the specified length does not meet the minimum length requirements.</returns>
        public virtual IdentityError PasswordTooShort(int length)
        {
            return new IdentityError(IdentityErrorCodes.PasswordTooShort,
                FormatString(GetLocalizedString(ErrorStrings.PasswordTooShort, _currentCulture), length));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a recovery code was not redeemed.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a recovery code was not redeemed.</returns>
        public virtual IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError(IdentityErrorCodes.RecoveryCodeRedemptionFailed,
                GetLocalizedString(ErrorStrings.RecoveryCodeRedemptionFailed, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a user already has a password.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a user already has a password.</returns>
        public virtual IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError(IdentityErrorCodes.UserAlreadyHasPassword,
                GetLocalizedString(ErrorStrings.UserAlreadyHasPassword, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a user is already in the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="IdentityError"/> indicating a user is already in the specified <paramref name="role"/>.</returns>
        public virtual IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError(IdentityErrorCodes.UserAlreadyInRole,
                FormatString(GetLocalizedString(ErrorStrings.UserAlreadyInRole, _currentCulture), role));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating user lockout is not enabled.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating user lockout is not enabled.</returns>
        public virtual IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError(IdentityErrorCodes.UserLockoutNotEnabled,
                GetLocalizedString(ErrorStrings.UserLockoutNotEnabled, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a user is not in the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="IdentityError"/> indicating a user is not in the specified <paramref name="role"/>.</returns>
        public virtual IdentityError UserNotInRole(string role)
        {
            return new IdentityError(IdentityErrorCodes.UserNotInRole,
                FormatString(GetLocalizedString(ErrorStrings.UserNotInRole, _currentCulture), role));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a user is locked out.
        /// </summary>
        /// <param name="maxAttempt">The max failed attempt.</param>
        /// <returns>An <see cref="IdentityError"/> indicating a user is locked out.</returns>
        public virtual IdentityError UserLockedOut(int maxAttempt)
        {
            return new IdentityError(IdentityErrorCodes.UserLockedOut,
                FormatString(GetLocalizedString(ErrorStrings.UserNotInRole, _currentCulture), maxAttempt));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a user is not found.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a user is not found.</returns>
        public virtual IdentityError UserNotFound()
        {
            return new IdentityError(IdentityErrorCodes.UserNotFound, GetLocalizedString(ErrorStrings.UserNotInRole, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a role is not found.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a role is not found.</returns>
        public virtual IdentityError RoleNotFound()
        {
            return new IdentityError(IdentityErrorCodes.RoleNotFound, GetLocalizedString(ErrorStrings.RoleNotFound, _currentCulture));
        }
        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating a claim is already associated.
        /// </summary>
        /// <returns>An <see cref="IdentityError"/> indicating a claim is already associated.</returns>
        public virtual IdentityError ClaimAlreadyAssociated(string claimName)
        {
            return new IdentityError(IdentityErrorCodes.ClaimAlreadyAssociated,
                FormatString(GetLocalizedString(ErrorStrings.ClaimAlreadyAssociated, _currentCulture), claimName));
        }
        #endregion Public methods

        #region Private methods
        private static string GetLocalizedString(string key, CultureInfo cultureInfo)
        {
            return ErrorStrings.ResourceManager.GetString(key, cultureInfo) ?? string.Empty;
        }
        private static string FormatString(string localizedString, params object[] parameters)
        {
            return string.Format(localizedString, parameters);
        }
        #endregion Private methods
    }
}
