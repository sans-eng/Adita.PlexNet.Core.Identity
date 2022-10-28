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
    /// Implements the standard Identity password hashing.
    /// </summary>
    public class BCryptPasswordHasher<TKey, TUser> : IPasswordHasher<TKey, TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        #region Public methods
        /// <summary>
        /// Returns a hashed representation of the supplied <paramref name="password" />.
        /// </summary>
        /// <param name="user">A user to hash the password.</param>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="password"/> is <c>null</c>.</exception>
        public string HashPassword(TUser user, string password)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            return BCrypt.Net.BCrypt.HashPassword(user.Id.ToString() + password);
        }

        /// <summary>
        /// Returns a <see cref="PasswordVerificationResult" /> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="user">A user to verify the password.</param>
        /// <param name="hashedPassword">The hash value for a user's stored password.</param>
        /// <param name="providedPassword">The password supplied for comparison.</param>
        /// <returns>A <see cref="PasswordVerificationResult" /> indicating the result of a password hash comparison.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/>, <paramref name="hashedPassword"/> or <paramref name="providedPassword"/>
        /// is <c>null</c>.</exception>
        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (hashedPassword is null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            if (providedPassword is null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            if (BCrypt.Net.BCrypt.Verify(user.Id.ToString() + providedPassword, hashedPassword))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }
        #endregion Public methods
    }
}
