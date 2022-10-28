using Microsoft.EntityFrameworkCore;

namespace Adita.PlexNet.Core.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Represents a base class for a role persistence repository.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key of a role.</typeparam>
    /// <typeparam name="TRole">The type that encapsulate the role.</typeparam>
    public class RoleRepository<TKey, TRole> : RoleRepository<TKey, TRole, DbContext>
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>, new()
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="RoleRepository{TKey}" /> using specified
        /// <paramref name="context" /> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="context">A <see cref="DbContext"/> to retrieve the roles from.</param>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context" /> or <paramref name="errorDescriber" /> is <c>null</c></exception>
        public RoleRepository(DbContext context, IdentityErrorDescriber errorDescriber) : base(context, errorDescriber)
        {
        }
        #endregion Constructors
    }
}
