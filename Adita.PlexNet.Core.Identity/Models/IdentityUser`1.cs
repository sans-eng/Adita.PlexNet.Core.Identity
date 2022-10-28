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

using System.ComponentModel.DataAnnotations;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Represents a user in the identity system
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
    public class IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/>.
        /// </summary>
        public IdentityUser() { }
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/> using specified <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName">The user name.</param>
        public IdentityUser(string userName)
        {
            UserName = userName;
        }
        #endregion Constructors

        #region Public properties
        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        [Key]
        [Required]
        public virtual TKey Id { get; set; } = default!;
        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        [Required]
        public virtual string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        public virtual string NormalizedUserName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        [Required]
        public virtual string PasswordHash { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the number of failed login attempts for the current user.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }
        /// <summary>
        /// A random value that must change whenever a user is persisted to the repository
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        [EmailAddress]
        public virtual string Email { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        public virtual string NormalizedEmail { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }
        /// <summary>
        /// Gets or sets a flag indicating if the user could be locked out.
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }
        /// <summary>
        /// Gets or sets the date and time, in UTC, when any user lockout ends.
        /// </summary>
        public virtual DateTimeOffset? LockoutEnd { get; set; }
        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        [Phone]
        public virtual string PhoneNumber { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }
        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public virtual string SecurityStamp { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }
        #endregion Public properties

        #region Public methods
        /// <summary>
        /// Returns the username for this user.
        /// </summary>
        /// <returns><see cref="string"/>.</returns>
        public override string ToString()
        {
            return UserName;
        }
        #endregion Public methods
    }
}