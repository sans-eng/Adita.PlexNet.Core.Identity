using Microsoft.EntityFrameworkCore;

namespace Adita.PlexNet.Core.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Default class for the Entity Framework database context used for identity which uses 
    /// <see cref="IdentityUser"/>, <see cref="IdentityUserClaim"/>, <see cref="IdentityUserRole"/>, <see cref="IdentityRole"/> and <see cref="IdentityRoleClaim"/>
    /// for the <see cref="DbSet{TEntity}"/>s.
    /// </summary>
    public class DefaultIdentityDbContext : IdentityDbContext<Guid, IdentityUser, IdentityUserClaim, IdentityUserRole, IdentityRole, IdentityRoleClaim>
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="DefaultIdentityDbContext"/>.
        /// </summary>
        public DefaultIdentityDbContext()
        {
        }
        /// <summary>
        /// Initialize a new instance of <see cref="DefaultIdentityDbContext"/>
        /// using specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">A <see cref="DbContextOptions"/> The options to be used by a <see cref="DbContext"/>.</param>
        public DefaultIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
        #endregion Constructors
    }
}
