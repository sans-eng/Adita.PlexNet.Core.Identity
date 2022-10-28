using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Adita.PlexNet.Core.Identity.Test.Services.Repositories.UserClaimRepositories
{
    /// <summary>
    /// Represents a base class for a repository of claims for a user.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a user claim.</typeparam>
    /// <typeparam name="TUserClaim">A type for a user claim.</typeparam>
    public class InMemoryUserClaimRepository<TKey, TUserClaim> :
        UserClaimRepositoryBase<TKey, TUserClaim>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
    {
        #region Private fields
        private readonly List<TUserClaim> _context = new();
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="InMemoryUserClaimRepository{TKey, TUserClaim}" /> using specified
        /// <paramref name="context"/> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="errorDescriber"/> is <c>null</c></exception>
        public InMemoryUserClaimRepository(IdentityErrorDescriber errorDescriber)
            : base(errorDescriber)
        {
            if (errorDescriber is null)
            {
                throw new ArgumentNullException(nameof(errorDescriber));
            }
        }
        #endregion Constructors

        #region Public properties
        /// <inheritdoc/>
        public override IQueryable<TUserClaim> UserClaims { get { return _context.AsQueryable(); } }
        /// <summary>
        /// Gets or sets a flag indicating if changes should be persisted after <see cref="CreateAsync"/>, <see cref="UpdateAsync"/> and <see cref="DeleteAsync"/> are called.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if changes should be automatically persisted, otherwise <see langword="false"/>.
        /// </value>
        public bool AutoSaveChanges { get; set; } = true;
        #endregion Public properties

        #region Public methods
        /// <summary>
        /// Creates a new user claim in a repository as an asynchronous operation.
        /// </summary>
        /// <param name="userClaim">The user claim to create in the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.></param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="userClaim"/> is <c>null</c>.</exception>
        public override async Task<IdentityResult> CreateAsync(TUserClaim userClaim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userClaim == null)
            {
                throw new ArgumentNullException(nameof(userClaim));
            }
            _context.Add(userClaim);
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }
        /// <summary>
        /// Deletes a user claim from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="userClaim">The user claim to delete from the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="userClaim"/> is <c>null</c>.</exception>
        public override async Task<IdentityResult> DeleteAsync(TUserClaim userClaim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userClaim == null)
            {
                throw new ArgumentNullException(nameof(userClaim));
            }
            _context.Remove(userClaim);
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }
        /// <summary>
        /// Finds user claims on repository that match specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A <see cref="Func{T, TResult}"/> to match claims.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a collection of user claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <c>null</c></exception>
        public override async Task<IList<TUserClaim>> FindAsync(Func<TUserClaim, bool> predicate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            return await Task.FromResult(_context.Where(predicate).ToList());
        }
        /// <inheritdoc/>
        public override Task<TUserClaim?> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Task.FromResult(_context.Find(u => u.Id.Equals(id)));
        }
        /// <summary>
        /// Update specified <paramref name="userClaim"/> in te repository.
        /// </summary>
        /// <param name="userClaim">A claim to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="userClaim"/> is <c>null</c></exception>
        public override Task<IdentityResult> UpdateAsync(TUserClaim userClaim, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(IdentityResult.Success);
        }
        #endregion Public methods

        #region Protected methods
        /// <summary>Saves the current store.</summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        #endregion Protected methods
    }
}
