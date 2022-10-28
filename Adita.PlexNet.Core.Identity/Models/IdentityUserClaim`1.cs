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
using System.Security.Claims;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Represents a claim that a user possesses.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key of the user and this claim.</typeparam>
    public class IdentityUserClaim<TKey> where TKey : IEquatable<TKey>
    {

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityUserClaim{TKey}"/>.
        /// </summary>
        public IdentityUserClaim()
        {

        }
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityUserClaim{TKey}"/> using specified <paramref name="userId"/> and <paramref name="claim"/>.
        /// </summary>
        /// <param name="userId">A user id of a user which claim belongs to.</param>
        /// <param name="claim">A <see cref="Claim"/> to be extract the type and value from.</param>
        public IdentityUserClaim(TKey userId, Claim claim)
        {
            UserId = userId;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
        #endregion Constructors

        #region Public properties
        /// <summary>
        /// Gets or sets the identifier for this user claim.
        /// </summary>
        [Key]
        [Required]
        public virtual TKey Id { get; set; } = default!;
        /// <summary>
        /// Gets or sets the primary key of the user associated with this claim.
        /// </summary>
        [Required]
        public virtual TKey UserId { get; set; } = default!;
        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        public virtual string ClaimType { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        public virtual string ClaimValue { get; set; } = string.Empty;

        #endregion Public properties

        #region Public methods
        /// <summary>
        /// Reads the type and value from the <see cref="Claim"/>.
        /// </summary>
        /// <param name="claim"><see cref="Claim"/>.</param>
        public virtual void InitializeFromClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
        /// <summary>
        /// Converts the entity into a <see cref="Claim"/> instance.
        /// </summary>
        /// <returns><see cref="Claim"/>.</returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
        #endregion Public methods
    }
}
