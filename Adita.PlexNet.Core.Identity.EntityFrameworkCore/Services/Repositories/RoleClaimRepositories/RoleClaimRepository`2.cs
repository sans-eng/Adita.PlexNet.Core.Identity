using Microsoft.EntityFrameworkCore;

namespace Adita.PlexNet.Core.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Represents a base class for a repository of claims for a role.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a role claim.</typeparam>
    /// <typeparam name="TRoleClaim">A type for a role claim.</typeparam>
    public class RoleClaimRepository<TKey, TRoleClaim> : RoleClaimRepository<TKey, TRoleClaim, DbContext>
        where TKey : IEquatable<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="RoleClaimRepository{TKey}" /> using specified
        /// <paramref name="context"/> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="context">A <see cref="DbContext"/> to retrieve the role claims from.</param>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="errorDescriber"/> is <c>null</c></exception>
        public RoleClaimRepository(DbContext context, IdentityErrorDescriber errorDescriber) : base(context, errorDescriber)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (errorDescriber is null)
            {
                throw new ArgumentNullException(nameof(errorDescriber));
            }
        }
        #endregion Constructors
    }
}
