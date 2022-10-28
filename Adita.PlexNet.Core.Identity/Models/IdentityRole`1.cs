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
    /// Represents a role in the identity system
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the role.</typeparam>
    public class IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityRole{TKey}"/>.
        /// </summary>
        public IdentityRole()
        {

        }
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityRole{TKey}"/> identified by specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the role.</param>
        public IdentityRole(string name)
        {
            Name = name;
        }
        #endregion Constructors

        #region Public properties
        /// <summary>
        /// Gets or sets the primary key for this role.
        /// </summary>
        [Key]
        [Required]
        public virtual TKey Id { get; set; } = default!;
        /// <summary>
        /// Gets or sets the name for this role.
        /// </summary>
        [Required]
        public virtual string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the normalized name for this role.
        /// </summary>
        [Required]
        public virtual string NormalizedName { get; set; } = string.Empty;
        /// <summary>
        /// A random value that must change whenever a role is persisted to the repository
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = string.Empty;
        #endregion Public properties
    }
}
