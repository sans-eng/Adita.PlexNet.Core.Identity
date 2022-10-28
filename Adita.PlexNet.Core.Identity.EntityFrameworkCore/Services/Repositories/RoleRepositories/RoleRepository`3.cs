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
    /// Represents a base class for a role persistence repository.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key of a role.</typeparam>
    /// <typeparam name="TRole">The type that encapsulate the role.</typeparam>
    /// <typeparam name="TContext">The type used for the <see cref="DbContext"/>.</typeparam>
    public class RoleRepository<TKey, TRole, TContext> :
        RoleRepositoryBase<TKey, TRole>
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>, new()
        where TContext : DbContext
    {
        #region Private fields
        private readonly TContext _context;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="RoleRepository{TKey, TRole, TContext}" /> using specified
        /// <paramref name="context"/> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="context">A <typeparamref name="TContext"/> to retrieve the roles from.</param>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="errorDescriber"/> is <c>null</c></exception>
        public RoleRepository(TContext context, IdentityErrorDescriber errorDescriber) : base(errorDescriber)
        {
            if (errorDescriber is null)
            {
                throw new ArgumentNullException(nameof(errorDescriber));
            }

            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion Constructors

        #region Public properties
        /// <inheritdoc/>
        public override IQueryable<TRole> Roles { get { return RolesSet; } }
        /// <summary>
        /// Gets or sets a flag indicating if changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
        /// </summary>
        /// <value>
        /// True if changes should be automatically persisted, otherwise false.
        /// </value>
        public bool AutoSaveChanges { get; set; } = true;
        #endregion Public properties

        #region Private properties
        private DbSet<TRole> RolesSet { get { return _context.Set<TRole>(); } }
        #endregion Private properties

        #region Public methods
        /// <summary>
        /// Creates a new role in a repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to create in the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        public override async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            await RolesSet.AddAsync(role, cancellationToken);
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
        /// Deletes a role from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to delete from the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        public override async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            RolesSet.Remove(role);
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
        /// Finds the role who has the specified <paramref name="roleId"/> as an asynchronous operation.
        /// </summary>
        /// <param name="roleId">The role ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="roleId"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        public override async Task<TRole?> FindByIdAsync(TKey roleId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (roleId == null)
            {
                throw new ArgumentNullException(nameof(roleId));
            }
            return await RolesSet.FindAsync(new object?[] { roleId }, cancellationToken: cancellationToken);
        }
        /// <summary>
        /// Finds and returns a role, if any, who has the specified normalized <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The role name to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the role matching the specified <paramref name="name"/> if it exists.</returns>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is null or only contain whitespace.</exception>
        public override Task<TRole?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return RolesSet.FirstOrDefaultAsync(r => r.NormalizedName == name, cancellationToken);
        }
        /// <summary>
        /// Updates a role in the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <c>null</c>.</exception>
        public override async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            RolesSet.Attach(role);
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            RolesSet.Update(role);
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
        #endregion Public methods

        #region Protected methods
        /// <summary>Saves the current store.</summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return AutoSaveChanges ? _context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
        }
        #endregion Protected methods
    }
}
