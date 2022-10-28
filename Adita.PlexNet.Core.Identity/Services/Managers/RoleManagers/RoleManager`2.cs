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

using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Provides the API's for managing roles in a persistence repository.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key of a role and role claim.</typeparam>
    /// <typeparam name="TRole">The type for a role.</typeparam>
    /// <typeparam name="TRoleClaim">The type for a role claim.</typeparam>
    public class RoleManager<TKey, TRole, TRoleClaim> : IDisposable, IRoleManager<TKey, TRole>
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        #region Private fields
        private bool _disposed;
        private readonly ILogger<RoleManager<TKey, TRole, TRoleClaim>> _logger;
        private readonly CancellationToken _cancellationToken = new CancellationTokenSource().Token;
        private readonly IdentityErrorDescriber _errorDescriber;
        private readonly IRoleValidator<TRole> _roleValidator;
        private readonly ILookupNormalizer _lookupNormalizer;
        #endregion Private fields

        #region constructors
        /// <summary>
        /// Initialize a new instance of <see cref="RoleManager{TKey, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <param name="roleRepository">The role persistence repository the manager will operate over.</param>
        /// <param name="roleClaimRepository">The role claim persistence repository the manager will operate over.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        /// <param name="errorDescriber">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        /// <param name="roleValidator">The role validator to validate roles against.</param>
        /// <param name="lookupNormalizer">An <see cref="ILookupNormalizer"/> to normalize key.</param>
        public RoleManager(IRoleRepository<TKey, TRole> roleRepository,
            IRoleClaimRepository<TKey, TRoleClaim> roleClaimRepository,
            ILogger<RoleManager<TKey, TRole, TRoleClaim>> logger,
            IdentityErrorDescriber errorDescriber,
            IRoleValidator<TRole> roleValidator,
            ILookupNormalizer lookupNormalizer)
        {
            RoleRepository = roleRepository;
            RoleClaimRepository = roleClaimRepository;

            _logger = logger;
            _errorDescriber = errorDescriber;
            _roleValidator = roleValidator;
            _lookupNormalizer = lookupNormalizer;
        }
        #endregion constructors

        #region Public properties
        /// <inheritdoc/>
        public IQueryable<TRole> Roles
        {
            get { return RoleRepository.Roles; }
        }
        #endregion Public properties

        #region Protected properties
        /// <summary>
        /// Gets a <see cref="IRoleRepository{TKey, TRole}"/> associated with current <see cref="RoleManager{TKey, TRole, TRoleClaim}"/>
        /// </summary>
        protected internal IRoleRepository<TKey, TRole> RoleRepository { get; }
        /// <summary>
        /// Gets a <see cref="IRoleClaimRepository{TKey, TRoleClaim}"/> associated with current <see cref="RoleManager{TKey, TRole, TRoleClaim}"/>
        /// </summary>
        protected internal IRoleClaimRepository<TKey, TRoleClaim> RoleClaimRepository { get; }
        #endregion Protected properties

        #region Public methods
        /// <summary>
        /// Adds a claim to a role.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> or <paramref name="claim"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="role"/> is not exist on repository.</exception>
        public async Task<IdentityResult> AddClaimAsync(TRole role, Claim claim)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (claim is null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            if (!await IsRoleExistsAsync(role))
            {
                _logger.LogError("Role {0} not exist", role.Name);
                throw new ArgumentException($"{nameof(role)} is not exist.");
            }

            TRoleClaim roleClaim = new TRoleClaim() { ClaimType = claim.Type, ClaimValue = claim.Value, RoleId = role.Id };

            IdentityResult result = await RoleClaimRepository.CreateAsync(roleClaim, _cancellationToken);

            if (result.Succeeded)
            {
                _logger.LogInformation("Claim {0} added to role {1}", claim, role.Name);
            }
            else
            {
                _logger.LogError("Failed add claim {0} to role {1}", claim, role.Name);
            }

            return result;
        }
        /// <summary>
        /// Creates the specified <paramref name="role"/> in the persistence repository.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <see langword="null"/>.</exception>
        public async Task<IdentityResult> CreateAsync(TRole role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            role.NormalizedName = _lookupNormalizer.NormalizeName(role.Name);
            IdentityResult result = await RoleRepository.CreateAsync(role, _cancellationToken);
            if (!result.Succeeded)
            {
                _logger.LogError("Role {0} creation failed", role.Name);
            }

            _logger.LogInformation("Role {0} created", role.Name);
            return result;
        }
        /// <summary>
        /// Deletes the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to delete.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the delete.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <see langword="null"/>.</exception>
        public async Task<IdentityResult> DeleteAsync(TRole role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            IdentityResult result = await RoleRepository.DeleteAsync(role, _cancellationToken);

            if (!result.Succeeded)
            {
                _logger.LogError("Role {0} deletion failed", role.Name);
            }

            _logger.LogInformation("Role {0} deleted", role.Name);

            return result;
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <inheritdoc/>
        public Task<TRole?> FindByIdAsync(TKey roleId)
        {
            return RoleRepository.FindByIdAsync(roleId, _cancellationToken);
        }
        /// <summary>
        /// Finds the role associated with the specified <paramref name="roleName"/> if any.
        /// </summary>
        /// <param name="roleName">The name of the role to be returned.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the role associated with the specified <paramref name="roleName"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="roleName"/> is <see langword="null"/></exception>
        public Task<TRole?> FindByNameAsync(string roleName)
        {
            if (roleName is null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            string normalizedName = _lookupNormalizer.NormalizeName(roleName);

            return RoleRepository.FindByNameAsync(normalizedName, _cancellationToken);
        }
        /// <summary>
        /// Gets a list of claims associated with the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose claims should be returned.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the list of <see cref="Claim"/>s associated with the specified <paramref name="role"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="role"/> is not exist on repository.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <c>null</c>.</exception>
        public async Task<IList<Claim>> GetClaimsAsync(TRole role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (!await IsRoleExistsAsync(role))
            {
                _logger.LogError("Role {0} not exist", role.Name);
                throw new ArgumentException($"{nameof(role)} is not exist.");
            }

            return (await RoleClaimRepository.FindAsync(p => p.RoleId.Equals(role.Id))).Select(p => p.ToClaim()).ToList();
        }
        /// <summary>
        /// Gets the ID of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose ID should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the ID of the specified <paramref name="role"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="role"/> is not exist on repository.</exception>
        public async Task<TKey> GetRoleIdAsync(TRole role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (!await IsRoleExistsAsync(role))
            {
                _logger.LogError("Role {0} not exist", role.Name);
                throw new ArgumentException($"{nameof(role)} is not exist.");
            }

            return await RoleRepository.GetRoleIdAsync(role, _cancellationToken);
        }
        /// <summary>
        /// Gets the name of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose name should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the name of the specified <paramref name="role"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="role"/> is not exist on repository.</exception>
        public async Task<string> GetRoleNameAsync(TRole role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (!await IsRoleExistsAsync(role))
            {
                _logger.LogError("Role {0} not exist", role.Name);
                throw new ArgumentException($"{nameof(role)} is not exist.");
            }

            return await RoleRepository.GetRoleNameAsync(role, _cancellationToken);
        }
        /// <summary>
        /// Removes a claim from a role.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> or <paramref name="claim"/> is
        /// <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="role"/> is not exist on repository.</exception>
        public async Task<IdentityResult> RemoveClaimAsync(TRole role, Claim claim)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (claim is null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            if (!await IsRoleExistsAsync(role))
            {
                _logger.LogError("Role {0} not exist", role.Name);
                throw new ArgumentException($"{nameof(role)} is not exist.");
            }

            IList<TRoleClaim> roleClaims = await RoleClaimRepository.FindAsync(p => p.ClaimType == claim.Type && p.ClaimValue == claim.Value);
            TRoleClaim? roleClaim = roleClaims.FirstOrDefault();
            if (roleClaim == null)
            {
                return IdentityResult.Success;
            }

            IdentityResult result = await RoleClaimRepository.DeleteAsync(roleClaim, _cancellationToken);
            if (result.Succeeded)
            {
                _logger.LogInformation("Claim {0} removed from role {1}", claim, role.Name);
            }
            else
            {
                _logger.LogError("Failed to remove claim {0} from role {1}", claim, role.Name);
            }
            return result;
        }
        /// <inheritdoc/>
        public async Task<bool> IsRoleExistsAsync(TRole role)
        {
            return await RoleRepository.FindByIdAsync(role.Id) != null;
        }
        /// <inheritdoc/>
        public async Task<bool> RoleNameExistsAsync(string roleName)
        {
            return await RoleRepository.FindByNameAsync(roleName) != null;
        }
        /// <inheritdoc/>
        public async Task<IdentityResult> SetRoleNameAsync(TRole role, string name)
        {
            await RoleRepository.SetRoleNameAsync(role, name, _cancellationToken);
            _logger.LogInformation("Role {0} name set successfuly", role.Name);
            return await UpdateAsync(role);
        }
        /// <summary>
        /// Updates the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to updated.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the update.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <see langword="null"/></exception>
        public async Task<IdentityResult> UpdateAsync(TRole role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            IdentityResult result = await RoleRepository.UpdateAsync(role, _cancellationToken);
            if (!result.Succeeded)
            {
                _logger.LogError("Update role {0} failed", role.Name);
                return result;
            }

            _logger.LogInformation("Role {0} updated", role.Name);
            return IdentityResult.Success;
        }
        /// <inheritdoc/>
        public async Task<IdentityResult> ValidateRoleAsync(TRole role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            IdentityResult result = await _roleValidator.ValidateAsync(role);
            if (!result.Succeeded)
            {
                _logger.LogError("Invalid role {0}", role.Name);
                return result;
            }

            return IdentityResult.Success;
        }
        #endregion Public methods

        #region Private methods
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                RoleRepository.Dispose();
                RoleClaimRepository.Dispose();
            }

            _disposed = true;
        }
        #endregion Private methods
    }
}
