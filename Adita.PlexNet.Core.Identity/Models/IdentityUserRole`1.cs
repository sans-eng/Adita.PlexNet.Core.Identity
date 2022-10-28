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
    /// Represents the link between a user and a role.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key used for users and roles.</typeparam>
    public class IdentityUserRole<TKey> where TKey : IEquatable<TKey>
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityUserRole{TKey}"/>.
        /// </summary>
        public IdentityUserRole()
        {

        }
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityUserRole{TKey}"/> identified by specified <paramref name="roleId"/>
        /// and <paramref name="userId"/>.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="userId">The user id.</param>
        public IdentityUserRole(TKey roleId, TKey userId)
        {
            RoleId = roleId;
            UserId = userId;
        }
        #endregion Constructors

        #region Public properties
        /// <summary>
        /// Gets or sets the primary key of current <see cref="IdentityUserRole{TKey}"/>.
        /// </summary>
        [Key]
        [Required]
        public virtual TKey Id { get; set; } = default!;
        /// <summary>
        /// Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        [Required]
        public virtual TKey RoleId { get; set; } = default!;
        /// <summary>
        /// Gets or sets the primary key of the user that is linked to a role.
        /// </summary>
        [Required]
        public virtual TKey UserId { get; set; } = default!;
        #endregion Public properties
    }
}
