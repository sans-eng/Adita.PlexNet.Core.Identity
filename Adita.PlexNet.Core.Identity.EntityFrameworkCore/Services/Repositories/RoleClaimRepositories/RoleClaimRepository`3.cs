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
using System.Security.Claims;

namespace Adita.PlexNet.Core.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Represents a base class for a repository of claims for a role.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a role claim.</typeparam>
    /// <typeparam name="TRoleClaim">A type for a role claim.</typeparam>
    /// <typeparam name="TContext">A type for <see cref="DbContext"/>.</typeparam>
    public class RoleClaimRepository<TKey, TRoleClaim, TContext> :
        RoleClaimRepositoryBase<TKey, TRoleClaim>
        where TKey : IEquatable<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
        where TContext : DbContext
    {
        #region Private fields
        private readonly TContext _context;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="RoleClaimRepository{TKey, TRoleClaim, TContext}" /> using specified
        /// <paramref name="context"/> and <paramref name="errorDescriber" />.
        /// </summary>
        /// <param name="context">A <typeparamref name="TContext"/> to retrieve the role claims from.</param>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber" />
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="errorDescriber"/> is <c>null</c></exception>
        public RoleClaimRepository(TContext context, IdentityErrorDescriber errorDescriber)
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
        public override IQueryable<TRoleClaim> RoleClaims { get { return RoleClaimsSet; } }
        /// <summary>
        /// Gets or sets a flag indicating if changes should be persisted after <see cref="CreateAsync"/>, <see cref="UpdateAsync"/> and <see cref="DeleteAsync"/> are called.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if changes should be automatically persisted, otherwise <see langword="false"/>.
        /// </value>
        public bool AutoSaveChanges { get; set; } = true;
        #endregion Public properties

        #region Private properties
        private DbSet<TRoleClaim> RoleClaimsSet { get { return _context.Set<TRoleClaim>(); } }
        #endregion Private properties

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
            await RoleClaimsSet.AddAsync(roleClaim, cancellationToken);
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
            RoleClaimsSet.Remove(roleClaim);
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
        public override async Task<TRoleClaim?> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await RoleClaimsSet.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
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
            return await Task.FromResult(RoleClaimsSet.Where(predicate).ToList());
        }
        /// <summary>
        /// Update specified <paramref name="roleClaim"/> in te repository.
        /// </summary>
        /// <param name="roleClaim">A role claim to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="roleClaim"/> is <c>null</c>.</exception>
        public override async Task<IdentityResult> UpdateAsync(TRoleClaim roleClaim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }
            RoleClaimsSet.Attach(roleClaim);
            RoleClaimsSet.Update(roleClaim);
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
