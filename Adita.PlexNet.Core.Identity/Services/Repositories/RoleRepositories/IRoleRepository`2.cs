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
    /// Provides an abstraction for a storage and management of roles.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a role.</typeparam>
    /// <typeparam name="TRole">A type of a role.</typeparam>
    public interface IRoleRepository<TKey, TRole> : IRepository<TRole>, IDisposable
        where TKey : IEquatable<TKey>
        where TRole : class
    {
        #region Properties
        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> collection of roles.
        /// </summary>
        IQueryable<TRole> Roles { get; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new role in a repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to create in the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.></param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes a role from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to delete from the repository.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds the role who has the specified <paramref name="roleId"/> as an asynchronous operation.
        /// </summary>
        /// <param name="roleId">The role ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        Task<TRole?> FindByIdAsync(TKey roleId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds the role who has the specified <paramref name="name"/> as an asynchronous operation.
        /// </summary>
        /// <param name="name">The role name to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        Task<TRole?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the ID for a role from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
        Task<TKey> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the name for a role from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
        Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default);
        /// <summary>
        /// Sets the name of a role in the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates a role in the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default);
        #endregion Methods
    }
}
