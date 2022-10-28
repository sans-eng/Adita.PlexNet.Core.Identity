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

using System.Security.Claims;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Provides an abstraction for a repository of role specific claims.
    /// </summary>
    /// <typeparam name="TKey">The ke used for the primary key of a role claim.</typeparam>
    /// <typeparam name="TRoleClaim">The type of the role claim</typeparam>
    public interface IRoleClaimRepository<TKey, TRoleClaim> :
        IRepository<TRoleClaim>,
        IDisposable
        where TKey : IEquatable<TKey>
        where TRoleClaim : class
    {
        #region Properties
        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> collection of role claims.
        /// </summary>
        IQueryable<TRoleClaim> RoleClaims { get; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new role claim in a repository as an asynchronous operation.
        /// </summary>
        /// <param name="roleClaim">The role claim to create in the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.></param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> CreateAsync(TRoleClaim roleClaim, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes a role claim from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="roleClaim">The role claim to delete from the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> DeleteAsync(TRoleClaim roleClaim, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds a role claim using specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An ID of a role claim.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a claim.</returns>
        Task<TRoleClaim?> FindAsync(TKey id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds role claims using specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A <see cref="Predicate{T}"/> to match claims.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a <see cref="Claim"/>.</returns>
        Task<IList<TRoleClaim>> FindAsync(Func<TRoleClaim, bool> predicate, CancellationToken cancellationToken = default);
        /// <summary>
        /// Update specified <paramref name="roleClaim"/> in te repository.
        /// </summary>
        /// <param name="roleClaim">A role claim to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation.</returns>
        Task<IdentityResult> UpdateAsync(TRoleClaim roleClaim, CancellationToken cancellationToken = default);
        #endregion Methods
    }
}
