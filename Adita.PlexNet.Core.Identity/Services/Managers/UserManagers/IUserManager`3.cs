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
    /// Provides an abstraction for managing user in a persistence repository.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key of the user and role.</typeparam>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <typeparam name="TRole">The type of the role.</typeparam>
    public interface IUserManager<TKey, TUser, TRole> :
        IUserManager<TKey, TUser>,
        IDisposable
        where TKey : IEquatable<TKey>
        where TUser : class
        where TRole : class
    {
        #region Methods
        /// <summary>
        /// Add the specified <paramref name="user"/> to the named role.
        /// </summary>
        /// <param name="user">The user to add to the named role.</param>
        /// <param name="role">The name of the role to add the user to.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        Task<IdentityResult> AddToRoleAsync(TUser user, TRole role);
        /// <summary>
        /// Add the specified <paramref name="user"/> to the named roles.
        /// </summary>
        /// <param name="user">The user to add to the named roles.</param>
        /// <param name="roles">The name of the roles to add the user to.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        Task<IdentityResult> AddToRolesAsync(TUser user, IEnumerable<TRole> roles);
        /// <summary>
        /// Gets a list of roles the specified <paramref name="user"/> belongs to.
        /// </summary>
        /// <param name="user">The user whose roles to retrieve.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a list of roles.</returns>
        Task<IList<TRole>> GetRolesAsync(TUser user);
        /// <summary>
        /// Returns a list of users from the user store who are members of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose users should be returned.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a list of users who are members of the specified role.</returns>
        Task<IList<TUser>> GetUsersInRoleAsync(TRole role);
        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="user"/> is a member of the given role.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="role">The role to be checked.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a flag indicating whether the specified <paramref name="user"/> is a member of the role.</returns>
        Task<bool> IsInRoleAsync(TUser user, TRole role);
        /// <summary>
        /// Removes the specified <paramref name="user"/> from the named role.
        /// </summary>
        /// <param name="user">The user to remove from the named role.</param>
        /// <param name="role">The name of the role to remove the user from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        Task<IdentityResult> RemoveFromRoleAsync(TUser user, TRole role);
        /// <summary>
        /// Removes the specified <paramref name="user"/> from the named roles.
        /// </summary>
        /// <param name="user">The user to remove from the named <paramref name="roles"/>.</param>
        /// <param name="roles">The name of the roles to remove the <paramref name="user"/> from.</param>
        /// <returns></returns>
        Task<IdentityResult> RemoveFromRolesAsync(TUser user, IEnumerable<TRole> roles);
        #endregion Methods
    }
}
