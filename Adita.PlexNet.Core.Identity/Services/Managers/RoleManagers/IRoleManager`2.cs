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
    /// Provides the abstraction for managing roles in a persistence repository.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key of a role.</typeparam>
    /// <typeparam name="TRole">The type for the role.</typeparam>
    public interface IRoleManager<TKey, TRole> : IDisposable
        where TKey : IEquatable<TKey>
        where TRole : class
    {
        #region Properties
        /// <summary>
        /// Gets an <see cref="IQueryable{T}"/> of <typeparamref name="TRole"/> that associated.
        /// </summary>
        IQueryable<TRole> Roles { get; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Adds a claim to a role.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        Task<IdentityResult> AddClaimAsync(TRole role, Claim claim);
        /// <summary>
        /// Creates the specified <paramref name="role"/> in the persistence repository.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task<IdentityResult> CreateAsync(TRole role);
        /// <summary>
        /// Deletes the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to delete.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the delete.</returns>
        Task<IdentityResult> DeleteAsync(TRole role);
        /// <summary>
        /// Finds the role associated with the specified <paramref name="roleId"/> if any.
        /// </summary>
        /// <param name="roleId">The role ID whose role should be returned.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the role associated with the specified <paramref name="roleId"/></returns>
        Task<TRole?> FindByIdAsync(TKey roleId);
        /// <summary>
        /// Finds the role associated with the specified <paramref name="roleName"/> if any.
        /// </summary>
        /// <param name="roleName">The name of the role to be returned.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the role associated with the specified <paramref name="roleName"/></returns>
        Task<TRole?> FindByNameAsync(string roleName);
        /// <summary>
        /// Gets a list of claims associated with the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose claims should be returned.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of <see cref="Claim"/>s associated with the specified <paramref name="role"/>.</returns>
        Task<IList<Claim>> GetClaimsAsync(TRole role);
        /// <summary>
        /// Gets the ID of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose ID should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the ID of the specified <paramref name="role"/>.</returns>
        Task<TKey> GetRoleIdAsync(TRole role);
        /// <summary>
        /// Gets the name of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose name should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the name of the specified <paramref name="role"/></returns>
        Task<string> GetRoleNameAsync(TRole role);
        /// <summary>
        /// Removes a claim from a role.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        Task<IdentityResult> RemoveClaimAsync(TRole role, Claim claim);
        /// <summary>
        /// Gets a flag indicating whether the role is exists.
        /// </summary>
        /// <param name="role">The role whose existence of the role should be checked.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing true if the role exists, otherwise false.</returns>
        Task<bool> IsRoleExistsAsync(TRole role);
        /// <summary>
        /// Gets a flag indicating whether the specified <paramref name="roleName"/> exists.
        /// </summary>
        /// <param name="roleName">The role name whose existence should be checked.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing true if the role name exists, otherwise false.</returns>
        Task<bool> RoleNameExistsAsync(string roleName);
        /// <summary>
        /// Sets the name of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="name">The name to set.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        Task<IdentityResult> SetRoleNameAsync(TRole role, string name);
        /// <summary>
        /// Updates the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to updated.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the update.</returns>
        Task<IdentityResult> UpdateAsync(TRole role);
        /// <summary>
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is called before saving the role via Create or Update.
        /// </summary>
        /// <param name="role">The role</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        Task<IdentityResult> ValidateRoleAsync(TRole role);
        #endregion Methods
    }
}
