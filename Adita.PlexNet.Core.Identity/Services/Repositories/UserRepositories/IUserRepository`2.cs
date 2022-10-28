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
    /// Provides an abstraction for a repository which manages user accounts.
    /// </summary>
    /// <typeparam name="TKey">A type used for the key of a user.</typeparam>
    /// <typeparam name="TUser">A type of a user.</typeparam>
    public interface IUserRepository<TKey, TUser> :
        IRepository<TUser>,
        IDisposable,
        IUserLockoutRepository<TUser>,
        IUserPasswordRepository<TUser>
        where TUser : class
    {

        #region Properties
        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> collection of users.
        /// </summary>
        IQueryable<TUser> Users { get; }
        #endregion Properties

        /// <summary>
        /// Creates the specified <paramref name="user"/> in the user repository.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
        Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes the specified <paramref name="user"/> from the user repository.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the deletion operation.</returns>
        Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.</returns>
        Task<TUser?> FindByIdAsync(TKey userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Finds and returns a user, if any, who has the specified normalized <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The user name to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="name"/> if it exists.</returns>
        Task<TUser?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the user identifier for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose identifier should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user"/>.</returns>
        Task<TKey> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the user name for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the name for the specified <paramref name="user"/>.</returns>
        Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default);
        /// <summary>
        /// Sets the given <paramref name="userName"/> for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose name should be set.</param>
        /// <param name="userName">The user name to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the specified <paramref name="user"/> in the user repository.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default);
    }
}
