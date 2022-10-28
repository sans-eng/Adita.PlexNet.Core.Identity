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
    /// Provides an abstraction for a repository which maps users to roles.
    /// </summary>
    /// <typeparam name="TKey">The type used for primary key of a user role.</typeparam>
    /// <typeparam name="TUserRole">The type for a user role.</typeparam>
    public interface IUserRoleRepository<TKey, TUserRole> :
        IRepository<TUserRole>,
        IDisposable
        where TKey : IEquatable<TKey>
        where TUserRole : class
    {
        #region Properties
        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> collection of user roles.
        /// </summary>
        IQueryable<TUserRole> UserRoles { get; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new user role in a repository as an asynchronous operation.
        /// </summary>
        /// <param name="userRole">The user roles to create in the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.></param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> CreateAsync(TUserRole userRole, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes a user role from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="userRole">The user role to delete from the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> DeleteAsync(TUserRole userRole, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds a user role that has specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">An id to match.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a <typeparamref name="TUserRole"/>.</returns>
        Task<TUserRole?> FindAsync(TKey id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds a user role that has specified <paramref name="userId"/> and <paramref name="roleId"/>.
        /// </summary>
        /// <param name="userId">The ID of the user that the role belongs to.</param>
        /// <param name="roleId">The role ID</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a <typeparamref name="TUserRole"/>.</returns>
        Task<TUserRole?> FindAsync(TKey userId, TKey roleId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finda user roles that match specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A <see cref="Predicate{T}"/> of <typeparamref name="TUserRole"/> to match.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation which contains a collection of <typeparamref name="TUserRole"/>s.</returns>
        Task<IList<TUserRole>> FindAsync(Func<TUserRole, bool> predicate, CancellationToken cancellationToken = default);
        /// <summary>
        /// Update specified <paramref name="userRole"/> in te repository.
        /// </summary>
        /// <param name="userRole">A role to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents an asynchronous operation.</returns>
        Task<IdentityResult> UpdateAsync(TUserRole userRole, CancellationToken cancellationToken = default);
        #endregion Methods
    }
}
