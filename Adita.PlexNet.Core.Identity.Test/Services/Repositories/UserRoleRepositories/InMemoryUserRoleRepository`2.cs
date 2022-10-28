using Microsoft.EntityFrameworkCore;

namespace Adita.PlexNet.Core.Identity.Test.Services.Repositories.UserRoleRepositories
{
    /// <summary>
    /// Represents a base class for a repository of claims for a user.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a user role.</typeparam>
    /// <typeparam name="TUserRole">A type for a user role.</typeparam>
    public class InMemoryUserRoleRepository<TKey, TUserRole> :
        UserRoleRepositoryBase<TKey, TUserRole>
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>, new()
    {
        #region Private fields
        private readonly List<TUserRole> _context = new();
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="InMemoryUserRoleRepository{TKey, TUserRole}" /> using specified
        /// <paramref name="context"/> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="errorDescriber"/> is <c>null</c></exception>
        public InMemoryUserRoleRepository(IdentityErrorDescriber errorDescriber) : base(errorDescriber)
        {
            if (errorDescriber is null)
            {
                throw new ArgumentNullException(nameof(errorDescriber));
            }
        }
        #endregion Constructors

        #region Public properties
        /// <inheritdoc/>
        public override IQueryable<TUserRole> UserRoles { get { return _context.AsQueryable(); } }
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
        /// Creates a new user role in a repository as an asynchronous operation.
        /// </summary>
        /// <param name="userRole">The user roles to create in the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.></param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="userRole"/> is <c>null</c>.</exception>
        public override async Task<IdentityResult> CreateAsync(TUserRole userRole, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userRole == null)
            {
                throw new ArgumentNullException(nameof(userRole));
            }
            _context.Add(userRole);
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
        /// Deletes a user role from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="userRole">The user role to delete from the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="userRole"/> is <c>null</c>.</exception>
        public override async Task<IdentityResult> DeleteAsync(TUserRole userRole, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userRole == null)
            {
                throw new ArgumentNullException(nameof(userRole));
            }
            _context.Remove(userRole);
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
        /// Finds a user role that has specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An id to match.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a <typeparamref name="TUserRole"/>.</returns>
        public override Task<TUserRole?> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Task.FromResult(_context.Find(u => u.Id.Equals(id)));
        }
        /// <summary>
        /// Finds a user role that has specified <paramref name="userId"/> and <paramref name="roleId"/>.
        /// </summary>
        /// <param name="userId">The ID of the user that the role belongs to.</param>
        /// <param name="roleId">The role ID</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a <typeparamref name="TUserRole"/>.</returns>
        public override Task<TUserRole?> FindAsync(TKey userId, TKey roleId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Task.FromResult(_context.Find(u => u.Id.Equals(userId) && u.RoleId.Equals(roleId)));
        }
        /// <summary>
        /// Finda user roles that match specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A <see cref="Predicate{T}"/> of <typeparamref name="TUserRole"/> to match.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a collection of <typeparamref name="TUserRole"/>s.</returns>
        public override async Task<IList<TUserRole>> FindAsync(Func<TUserRole, bool> predicate, CancellationToken cancellationToken = default)
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
        /// Update specified <paramref name="userRole"/> in te repository.
        /// </summary>
        /// <param name="userRole">A role to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation.</returns>
        public override Task<IdentityResult> UpdateAsync(TUserRole userRole, CancellationToken cancellationToken = default)
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
