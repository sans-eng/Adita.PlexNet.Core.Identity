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
    /// Represents a base class for a user persistence repository.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key of a user.</typeparam>
    /// <typeparam name="TUser">The type that encapsulate the user.</typeparam>
    /// <typeparam name="TContext">The type used for the <see cref="DbContext"/>.</typeparam>
    public class UserRepository<TKey, TUser, TContext> :
         UserRepositoryBase<TKey, TUser>
         where TKey : IEquatable<TKey>
         where TUser : IdentityUser<TKey>, new()
         where TContext : DbContext
    {
        #region Private fields
        private readonly TContext _context;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="UserRepository{TKey, TUser, TContext}" /> using specified
        /// <paramref name="context"/> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="context">A <typeparamref name="TContext"/> to retrieve the users from.</param>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="errorDescriber"/> is <c>null</c></exception>
        public UserRepository(TContext context, IdentityErrorDescriber errorDescriber) : base(errorDescriber)
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
        public override IQueryable<TUser> Users { get { return UsersSet; } }
        /// <summary>
        /// Gets or sets a flag indicating if changes should be persisted after <see cref="CreateAsync"/>, <see cref="UpdateAsync"/> and <see cref="DeleteAsync"/> are called.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if changes should be automatically persisted, otherwise <see langword="false"/>.
        /// </value>
        public bool AutoSaveChanges { get; set; } = true;
        #endregion Public properties

        #region Private properties
        private DbSet<TUser> UsersSet { get { return _context.Set<TUser>(); } }
        #endregion Private properties

        #region Public methods
        /// <summary>
        /// Creates the specified <paramref name="user"/> in the user repository.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.SecurityStamp = Guid.NewGuid().ToString();
            await UsersSet.AddAsync(user, cancellationToken);
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
        /// Deletes the specified <paramref name="user"/> from the user repository.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the deletion operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UsersSet.Remove(user);
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
        /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.</returns>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        public override async Task<TUser?> FindByIdAsync(TKey userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await UsersSet.FirstOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
        }
        /// <summary>
        /// Finds and returns a user, if any, who has the specified normalized <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The user name to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="name"/> if it exists.</returns>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        /// <exception cref="ArgumentException"><paramref name="name"/> is null or only contain whitespace.</exception>
        public override Task<TUser?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return UsersSet.FirstOrDefaultAsync(u => u.NormalizedUserName == name, cancellationToken);
        }
        /// <summary>
        /// Updates the specified <paramref name="user"/> in the user repository.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <c>null</c>.</exception>
        public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UsersSet.Attach(user);
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            UsersSet.Update(user);
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
