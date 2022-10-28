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

using Microsoft.EntityFrameworkCore;

namespace Adita.PlexNet.Core.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Base class for the Entity Framework database context used for identity.
    /// </summary>
    /// <typeparam name="TKey">The type for the primary key of the users and roles.</typeparam>
    /// <typeparam name="TUser">The type that encapsulating the user.</typeparam>
    /// <typeparam name="TUserClaim">The type that encapsulating the user claim.</typeparam>
    /// <typeparam name="TUserRole">The type that encapsulating the user role.</typeparam>
    /// <typeparam name="TRole">The type that encapsulating the role.</typeparam>
    /// <typeparam name="TRoleClaim">The type that encapsulating the role claim.</typeparam>
    public class IdentityDbContext<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> : DbContext
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TRole : IdentityRole<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityDbContext{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        public IdentityDbContext()
        {

        }
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityDbContext{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>
        /// using specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">A <see cref="DbContextOptions"/> The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityDbContext(DbContextOptions options) : base(options)
        {

        }
        #endregion Constructors

        #region Public properties
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public virtual DbSet<TUser> Users { get; set; } = default!;
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of user claims.
        /// </summary>
        public virtual DbSet<TUserClaim> UserClaims { get; set; } = default!;
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of user roles.
        /// </summary>
        public virtual DbSet<TUserRole> UserRoles { get; set; } = default!;
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
        /// </summary>
        public virtual DbSet<TRole> Roles { get; set; } = default!;
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of role claims.
        /// </summary>
        public virtual DbSet<TRoleClaim> RoleClaims { get; set; } = default!;
        #endregion Public properties
    }
}
