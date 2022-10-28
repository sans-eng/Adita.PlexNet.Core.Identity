using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Adita.PlexNet.Core.Identity.Test.Services.Repositories.RoleClaimRepositories
{
    /// <summary>
    /// Represents a base class for a repository of claims for a role.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a role claim.</typeparam>
    /// <typeparam name="TRoleClaim">A type for a role claim.</typeparam>
    public class InMemoryRoleClaimRepository<TKey, TRoleClaim> :
        RoleClaimRepositoryBase<TKey, TRoleClaim>
        where TKey : IEquatable<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        #region Private fields
        private readonly List<TRoleClaim> _context = new();
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="InMemoryRoleClaimRepository{TKey, TRoleClaim}" /> using specified
        /// <paramref name="context"/> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="context">A <typeparamref name="TContext"/> to retrieve the users from.</param>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="errorDescriber"/> is <c>null</c></exception>
        public InMemoryRoleClaimRepository(IdentityErrorDescriber errorDescriber)
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
        public override IQueryable<TRoleClaim> RoleClaims { get { return _context.AsQueryable(); } }
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
        /// Creates a new role claim in a repository as an asynchronous operation.
        /// </summary>
        /// <param name="roleClaim">The role claim to create in the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.></param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="roleClaim"/> is <c>null</c>.</exception>
        public override async Task<IdentityResult> CreateAsync(TRoleClaim roleClaim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }
            _context.Add(roleClaim);
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
        /// Deletes a role claim from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="roleClaim">The role claim to delete from the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="roleClaim"/> is <c>null</c>.</exception>
        public override async Task<IdentityResult> DeleteAsync(TRoleClaim roleClaim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }
            _context.Remove(roleClaim);
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
        /// Finds a role claim using specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An ID of a role claim.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a claim.</returns>
        public override Task<TRoleClaim?> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Task.FromResult(_context.Find(u => u.Id.Equals(id)));
        }
        /// <summary>
        /// Finds role claims using specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A <see cref="Predicate{T}"/> to match claims.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a <see cref="Claim"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <c>null</c></exception>
        public override async Task<IList<TRoleClaim>> FindAsync(Func<TRoleClaim, bool> predicate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            return await Task.FromResult(_context.Where(predicate).ToList());
        }
        /// <summary>
        /// Update specified <paramref name="roleClaim"/> in te repository.
        /// </summary>
        /// <param name="roleClaim">A role claim to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="roleClaim"/> is <c>null</c>.</exception>
        public override Task<IdentityResult> UpdateAsync(TRoleClaim roleClaim, CancellationToken cancellationToken = default)
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
