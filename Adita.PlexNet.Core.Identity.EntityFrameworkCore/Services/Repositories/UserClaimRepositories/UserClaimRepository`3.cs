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
using System.Data;

namespace Adita.PlexNet.Core.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Represents a base class for a repository of claims for a user.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a user claim.</typeparam>
    /// <typeparam name="TUserClaim">A type for a user claim.</typeparam>
    /// <typeparam name="TContext">A type for <see cref="DbContext"/>.</typeparam>
    public abstract class UserClaimRepository<TKey, TUserClaim, TContext> :
        UserClaimRepositoryBase<TKey, TUserClaim>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TContext : DbContext
    {
        #region Private fields
        private readonly TContext _context;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="UserClaimRepository{TKey, TUserClaim, TContext}" /> using specified
        /// <paramref name="context"/> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="context">A <typeparamref name="TContext"/> to retrieve the user claims from.</param>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="errorDescriber"/> is <c>null</c></exception>
        protected UserClaimRepository(TContext context, IdentityErrorDescriber errorDescriber)
            : base(errorDescriber)
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
        public override IQueryable<TUserClaim> UserClaims { get { return UserClaimsSet; } }
        /// <summary>
        /// Gets or sets a flag indicating if changes should be persisted after <see cref="CreateAsync"/>, <see cref="UpdateAsync"/> and <see cref="DeleteAsync"/> are called.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if changes should be automatically persisted, otherwise <see langword="false"/>.
        /// </value>
        public bool AutoSaveChanges { get; set; } = true;
        #endregion Public properties

        #region Private properties
        private DbSet<TUserClaim> UserClaimsSet { get { return _context.Set<TUserClaim>(); } }
        #endregion Private properties

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
            await UserClaimsSet.AddAsync(userClaim, cancellationToken);
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
            UserClaimsSet.Remove(userClaim);
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
            return await Task.FromResult(UserClaimsSet.Where(predicate).ToList());
        }
        /// <inheritdoc/>
        public override async Task<TUserClaim?> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await UserClaimsSet.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        }
        /// <summary>
        /// Update specified <paramref name="userClaim"/> in te repository.
        /// </summary>
        /// <param name="userClaim">A claim to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="userClaim"/> is <c>null</c></exception>
        public override async Task<IdentityResult> UpdateAsync(TUserClaim userClaim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userClaim == null)
            {
                throw new ArgumentNullException(nameof(userClaim));
            }
            UserClaimsSet.Attach(userClaim);
            UserClaimsSet.Update(userClaim);
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
