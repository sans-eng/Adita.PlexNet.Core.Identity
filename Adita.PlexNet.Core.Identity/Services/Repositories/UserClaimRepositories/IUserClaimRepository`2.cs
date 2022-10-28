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

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Provides an abstraction for a repository of claims for a user.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of user claim.</typeparam>
    /// <typeparam name="TUserClaim">A type for a user claim.</typeparam>
    public interface IUserClaimRepository<TKey, TUserClaim> :
        IRepository<TUserClaim>,
        IDisposable
        where TKey : IEquatable<TKey>
        where TUserClaim : class
    {
        #region Properties
        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> collection of user claim.
        /// </summary>
        IQueryable<TUserClaim> UserClaims { get; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new user claim in a repository as an asynchronous operation.
        /// </summary>
        /// <param name="userClaim">The user claim to create in the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.></param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> CreateAsync(TUserClaim userClaim, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes a user claim from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="userClaim">The user claim to delete from the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> DeleteAsync(TUserClaim userClaim, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds user claims on repository that match specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A <see cref="Func{T, TResult}"/> to match claims.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a collection of user claims.</returns>
        Task<IList<TUserClaim>> FindAsync(Func<TUserClaim, bool> predicate, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds a user claim on repository that has specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">A ID of a claim.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a user claim.</returns>
        Task<TUserClaim?> FindAsync(TKey id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Update specified <paramref name="userClaim"/> in te repository.
        /// </summary>
        /// <param name="userClaim">A claim to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> UpdateAsync(TUserClaim userClaim, CancellationToken cancellationToken = default);
        #endregion Methods
    }
}
