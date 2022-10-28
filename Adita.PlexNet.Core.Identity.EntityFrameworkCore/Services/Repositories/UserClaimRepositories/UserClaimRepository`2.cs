using Microsoft.EntityFrameworkCore;

namespace Adita.PlexNet.Core.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Represents a base class for a repository of claims for a user.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a user claim.</typeparam>
    /// <typeparam name="TUserClaim">A type used for the user claim.</typeparam>
    public class UserClaimRepository<TKey, TUserClaim> : UserClaimRepository<TKey, TUserClaim, DbContext>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="UserClaimRepository{TKey}" /> using specified
        /// <paramref name="context"/> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="context">A <see cref="DbContext"/> to retrieve the user claims from.</param>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="errorDescriber"/> is <c>null</c></exception>
        public UserClaimRepository(DbContext context, IdentityErrorDescriber errorDescriber) : base(context, errorDescriber)
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
