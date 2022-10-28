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
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Claims;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Provides the APIs for managing user in a persistence repository.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key of the user and role.</typeparam>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <typeparam name="TUserClaim">The type for a user claim.</typeparam>
    /// <typeparam name="TUserRole">The type for a user role.</typeparam>
    /// <typeparam name="TRole">The type of the role.</typeparam>
    public class UserManager<TKey, TUser, TUserClaim, TUserRole, TRole> : IUserManager<TKey, TUser>, IUserManager<TKey, TUser, TRole>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TRole : IdentityRole<TKey>, new()
    {
        #region private fields
        private bool _disposed;
        private readonly ILogger<UserManager<TKey, TUser, TUserClaim, TUserRole, TRole>> _logger;
        private readonly LockoutOptions _options;
        private readonly CancellationToken _cancellationToken = new CancellationTokenSource().Token;
        private readonly IdentityErrorDescriber _errorDescriber;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly IPasswordHasher<TKey, TUser> _passwordHasher;
        private readonly IPasswordValidator _passwordValidator;
        private readonly IUserValidator<TUser> _userValidator;
        #endregion private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="UserManager{TKey, TUser, TUserClaim, TUserRole, TRole}"/>.
        /// </summary>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        /// <param name="options">The accessor used to access the <see cref="LockoutOptions"/>.</param>
        /// <param name="errorDescriber">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        /// <param name="keyNormalizer">The <see cref="ILookupNormalizer"/> to use when generating index keys for users.</param>
        /// <param name="passwordHasher">The password hashing implementation to use when saving passwords.</param>
        /// <param name="passwordValidator">The password validator to validate passwords against.</param>
        /// <param name="userValidator">The user validator to validate users against.</param>
        /// <param name="userRepository">The user persistence repository the manager will operate over.</param>
        /// <param name="userClaimRepository">The user claim persistence repository the manager will operate over.</param>
        /// <param name="userRoleRepository">The user role persistence repository the manager will operate over.</param>
        /// <param name="roleManager">The role manager to be associated.</param>
        public UserManager(ILogger<UserManager<TKey, TUser, TUserClaim, TUserRole, TRole>> logger,
            IOptions<LockoutOptions> options,
            IdentityErrorDescriber errorDescriber,
            ILookupNormalizer keyNormalizer,
            IPasswordHasher<TKey, TUser> passwordHasher,
            IPasswordValidator passwordValidator,
            IUserValidator<TUser> userValidator,
            IUserRepository<TKey, TUser> userRepository,
            IUserClaimRepository<TKey, TUserClaim> userClaimRepository,
            IUserRoleRepository<TKey, TUserRole> userRoleRepository,
            IRoleManager<TKey, TRole> roleManager)
        {
            _logger = logger;
            _options = options.Value;
            _errorDescriber = errorDescriber;
            _keyNormalizer = keyNormalizer;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _userValidator = userValidator;

            UserRepository = userRepository;
            UserClaimRepository = userClaimRepository;
            UserRoleRepository = userRoleRepository;
            RoleManager = roleManager;
        }
        #endregion Constructors

        #region Public properties
        /// <summary>
        /// Returns an <see cref="IQueryable"/> of users.
        /// </summary>
        public virtual IQueryable<TUser> Users
        {
            get { return UserRepository.Users; }
        }
        #endregion Public properties

        #region Protected properties
        /// <summary>
        /// Gets the user persistence repository the manager operates over.
        /// </summary>
        protected internal IUserRepository<TKey, TUser> UserRepository { get; }
        /// <summary>
        /// Gets the user claim persistence repository the manager operates over.
        /// </summary>
        protected internal IUserClaimRepository<TKey, TUserClaim> UserClaimRepository { get; }
        /// <summary>
        /// Gets the user role persistence repository the manager operates over.
        /// </summary>
        protected internal IUserRoleRepository<TKey, TUserRole> UserRoleRepository { get; }
        /// <summary>
        /// Gets a <see cref="IRoleManager{TKey, TRole}"/> that associated with current <see cref="UserManager{TKey, TUser, TUserClaim, TUserRole, TRole}"/>.
        /// </summary>
        protected internal IRoleManager<TKey, TRole> RoleManager { get; }
        #endregion Protected properties

        #region Public methods
        /// <summary>
        /// Increments the access failed count for the user as an asynchronous operation.
        /// If the failed access account is greater than or equal to the configured maximum number of attempts,
        /// the user will be locked out for the configured lockout time span.
        /// </summary>
        /// <param name="user">The user whose failed access count to increment.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public virtual async Task<IdentityResult> AccessFailedAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} is not exist");
            }

            bool isLockOutEnabled = await UserRepository.GetLockoutEnabledAsync(user, _cancellationToken);

            if (isLockOutEnabled)
            {
                int accessFailedCount = await UserRepository.GetAccessFailedCountAsync(user, _cancellationToken);
                if (accessFailedCount >= _options.MaxFailedAccessAttempts)
                {
                    await UserRepository.SetLockoutEndDateAsync(user, DateTime.Now + _options.DefaultLockoutTimeSpan);
                    _logger.LogWarning("User {0} locked out", user.UserName);
                    return new IdentityResult(false, _errorDescriber.UserLockedOut(_options.MaxFailedAccessAttempts));
                }
            }

            _logger.LogWarning("User {0} access failed", user.UserName);
            await UserRepository.IncrementAccessFailedCountAsync(user, _cancellationToken);

            return await UpdateAsync(user);
        }
        /// <summary>
        /// Adds the specified <paramref name="claim"/> to the <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to add the claim to.</param>
        /// <param name="claim">The claim to add.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="claim"/> is
        /// <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public virtual async Task<IdentityResult> AddClaimAsync(TUser user, Claim claim)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claim is null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} is not exist.");
            }

            TUserClaim userClaim = new()
            {
                UserId = user.Id
            };
            userClaim.InitializeFromClaim(claim);

            IdentityResult result = await UserClaimRepository.CreateAsync(userClaim, _cancellationToken);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to add claim {0} to user {1}", claim, user.UserName);
                return result;
            }
            _logger.LogInformation("Claim {0} added to user {1}", claim, user.UserName);
            return result;
        }
        /// <summary>
        /// Adds the specified <paramref name="claims"/> to the <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to add the claims to.</param>
        /// <param name="claims">The claims to add.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="claims"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on reposiory.</exception>
        public virtual async Task<IdentityResult> AddClaimsAsync(TUser user, IEnumerable<Claim> claims)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claims is null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} is not exist.");
            }

            foreach (var claim in claims)
            {
                TUserClaim userClaim = new()
                {
                    UserId = user.Id
                };

                userClaim.InitializeFromClaim(claim);

                IdentityResult result = await UserClaimRepository.CreateAsync(userClaim, _cancellationToken);
                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to add claim {0} t0 user {1}", claims, user.UserName);
                    return result;
                }
            }

            _logger.LogInformation("Claims {0} added to user {1}", claims, user.UserName);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Adds the <paramref name="password"/> to the specified <paramref name="user"/> only if the user does not already have a password.
        /// </summary>
        /// <param name="user">The user whose password should be set.</param>
        /// <param name="password">The password to set.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="password"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> AddPasswordAsync(TUser user, string password)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} is not exist.");
            }

            bool isHasPasword = await UserRepository.HasPasswordAsync(user, _cancellationToken);

            if (isHasPasword)
            {
                _logger.LogError("User {0} has password already", user);
                return new IdentityResult(false, _errorDescriber.UserAlreadyHasPassword());
            }

            IdentityResult validationResult = await ValidatePasswordAsync(user, password);
            if (!validationResult.Succeeded)
            {
                _logger.LogError("Invalid password for user {0}", user);
                return validationResult;
            }

            string passwordHash = _passwordHasher.HashPassword(user, password);

            await UserRepository.SetPasswordHashAsync(user, passwordHash, _cancellationToken);

            _logger.LogInformation("User {0} password set succesfully", user);
            return await UpdateAsync(user);
        }
        /// <summary>
        /// Add the specified <paramref name="user"/> to the role.
        /// </summary>
        /// <param name="user">The user to add to the role.</param>
        /// <param name="role">The name of the role to add the user to.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="role"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> or <paramref name="role"/> is not exist
        /// on repository.</exception>
        public async Task<IdentityResult> AddToRoleAsync(TUser user, TRole role)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} is not exist.");
            }

            if (!await RoleManager.IsRoleExistsAsync(role))
            {
                _logger.LogError("Role {0} not exist", role.Name);
                throw new ArgumentException($"{nameof(role)} is not exist.");
            }

            if (await IsInRoleAsync(user, role))
            {
                _logger.LogError("User {0} in role {1} already", user.UserName, role.Name);
                return new IdentityResult(false, _errorDescriber.UserAlreadyInRole(role.Name));
            }

            TUserRole userRole = new() { UserId = user.Id, RoleId = role.Id };

            IdentityResult result = await UserRoleRepository.CreateAsync(userRole, _cancellationToken);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to add User {0} to role {1}", user.UserName, role.Name);
                return result;
            }
            _logger.LogInformation("User {0} added to role {1}", user.UserName, role.Name);
            return result;
        }
        /// <summary>
        /// Add the specified <paramref name="user"/> to the named roles.
        /// </summary>
        /// <param name="user">The user to add to the named roles.</param>
        /// <param name="roles">The name of the roles to add the user to.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="roles"/>
        /// is <see langword="null"/></exception>
        /// <remarks>
        /// Use <see cref="FindByIdAsync(TKey)"/> or <see cref="FindByNameAsync(string)"/>
        /// for specifying <paramref name="user"/>.
        /// </remarks>
        /// <exception cref="ArgumentException"><paramref name="user"/> or one or more role in
        /// <paramref name="roles"/> is not exist on repository.</exception>
        public async Task<IdentityResult> AddToRolesAsync(TUser user, IEnumerable<TRole> roles)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (roles is null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} is not exist.");
            }

            foreach (var role in roles)
            {
                if (!await RoleManager.IsRoleExistsAsync(role))
                {
                    _logger.LogError("Role {0} not exist", role.Name);
                    throw new ArgumentException($"{nameof(role)} is not exist.");
                }

                if (await IsInRoleAsync(user, role))
                {
                    _logger.LogError("User {0} in role {1} already", user.UserName, role.Name);
                    return new IdentityResult(false, _errorDescriber.UserAlreadyInRole(role.Name));
                }

                TUserRole userRole = new() { UserId = user.Id, RoleId = role.Id };

                IdentityResult result = await UserRoleRepository.CreateAsync(userRole, _cancellationToken);
                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to add user {0} to roles", user.UserName);
                    return result;
                }
            }

            _logger.LogInformation("User {0} added to roles", user);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Changes a user's password after confirming the specified <paramref name="currentPassword"/> is correct, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose password should be set.</param>
        /// <param name="currentPassword">The current password to validate before changing.</param>
        /// <param name="newPassword">The new password to set for the specified <paramref name="user"/>.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/>, <paramref name="currentPassword"/>
        /// or <paramref name="newPassword"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (currentPassword is null)
            {
                throw new ArgumentNullException(nameof(currentPassword));
            }

            if (newPassword is null)
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} is not exist.");
            }

            string passwordHash = await UserRepository.GetPasswordHashAsync(user, _cancellationToken);

            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, passwordHash, currentPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                _logger.LogError("Password verification failed for user {0}", user.UserName);
                return new IdentityResult(false, _errorDescriber.PasswordMismatch());
            }

            IdentityResult validationResult = await ValidatePasswordAsync(user, newPassword);
            if (!validationResult.Succeeded)
            {
                _logger.LogError("Invalid password for user {0}", user.UserName);
                return validationResult;
            }

            string newPasswordHash = _passwordHasher.HashPassword(user, newPassword);

            await UserRepository.SetPasswordHashAsync(user, newPasswordHash, _cancellationToken);
            _logger.LogInformation("User {0} password successfully changed", user);
            return await UpdateAsync(user);
        }
        /// <summary>
        /// Returns a flag indicating whether the given <paramref name="password"/> is valid for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password to validate</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing true if the specified <paramref name="password"/> matches the one store for the <paramref name="user"/>, otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="password"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">Specified <paramref name="user"/> not found on repository.</exception>
        /// <remarks>
        /// Use <see cref="FindByIdAsync(TKey)"/> or <see cref="FindByNameAsync(string)"/>
        /// for specifying <paramref name="user"/>, otherwise throw exception if not found on repository.
        /// </remarks>
        public async Task<bool> CheckPasswordAsync(TUser user, string password)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not found");
            }

            string passwordHash = await UserRepository.GetPasswordHashAsync(user, _cancellationToken);
            return _passwordHasher.VerifyHashedPassword(user, passwordHash, password) != PasswordVerificationResult.Failed;
        }
        /// <summary>
        /// Creates the specified <paramref name="user"/> in the backing store with given <paramref name="password"/>, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The password for the user to hash and store.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="password"/>
        /// is <see langword="null"/></exception>
        public async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            user.NormalizedUserName = _keyNormalizer.NormalizeName(user.UserName);
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            IdentityResult result = await UserRepository.CreateAsync(user, _cancellationToken);
            if (!result.Succeeded)
            {
                _logger.LogError("User {0} creation failed", user.UserName);
            }

            return result;
        }
        /// <summary>
        /// Deletes the specified <paramref name="user"/> from the repository.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        public async Task<IdentityResult> DeleteAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IdentityResult result = await UserRepository.DeleteAsync(user, _cancellationToken);
            if (!result.Succeeded)
            {
                _logger.LogError("User {0} deletion failed", user.UserName);
            }

            return result;
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ///<inheritdoc/>///
        public Task<TUser?> FindByIdAsync(TKey userId)
        {
            return UserRepository.FindByIdAsync(userId, _cancellationToken);
        }
        /// <summary>
        /// Finds and returns a user, if any, who has the specified <paramref name="userName"/>.
        /// </summary>
        /// <param name="userName">The user name to search for.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userName"/> if it exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="userName"/> is <see langword="null"/></exception>
        public Task<TUser?> FindByNameAsync(string userName)
        {
            if (userName is null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            string normalizedName = _keyNormalizer.NormalizeName(userName);

            return UserRepository.FindByNameAsync(normalizedName, _cancellationToken);
        }
        /// <summary>
        /// Retrieves the current number of failed accesses for the given <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose access failed count should be retrieved for.</param>
        /// <returns>The <see cref="Task"/> that contains the result the asynchronous operation, the current failed access count for the user.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">Specified <paramref name="user"/> not found on repository.</exception>
        public async Task<int> GetAccessFailedCountAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not found");
            }

            return await UserRepository.GetAccessFailedCountAsync(user, _cancellationToken);
        }
        /// <summary>
        /// Gets a list of claims to be belonging to the specified <paramref name="user"/> as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose claims to retrieve.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a list of claims.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">Specified <paramref name="user"/> not found on repository.</exception>
        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not found");
            }

            var userClaims = await UserClaimRepository.FindAsync(p => p.UserId.Equals(user.Id), _cancellationToken);
            return userClaims.Select(p => p.ToClaim()).ToList();
        }
        /// <summary>
        /// Retrieves a flag indicating whether user lockout can be enabled for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose ability to be locked out should be returned.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, true if a user can be locked out, otherwise false.</returns>
        /// <exception cref="ArgumentException">Specified <paramref name="user"/> not found on repository.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        public async Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not found");
            }

            return await UserRepository.GetLockoutEnabledAsync(user, _cancellationToken);
        }
        /// <summary>
        /// Gets the last <see cref="DateTimeOffset"/> a user's last lockout expired, if any. A time value in the past indicates a user is not currently locked out.
        /// </summary>
        /// <param name="user">The user whose lockout date should be retrieved.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the lookup, a <see cref="DateTimeOffset"/> containing the last time a user's lockout expired, if any.</returns>
        /// <exception cref="ArgumentException">Specified <paramref name="user"/> not found on repository.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not found");
            }

            return await UserRepository.GetLockoutEndDateAsync(user, _cancellationToken);
        }
        /// <summary>
        /// Gets a list of roles the specified <paramref name="user"/> belongs to.
        /// </summary>
        /// <param name="user">The user whose roles to retrieve.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a list of roles.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">Specified <paramref name="user"/> not found on repository.</exception>
        public async Task<IList<TRole>> GetRolesAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not found");
            }

            IList<TUserRole> identityUserRoles =
                await UserRoleRepository.FindAsync(p => p.UserId.Equals(user.Id), _cancellationToken);

            IList<TRole> roles = new List<TRole>();

            foreach (var userRole in identityUserRoles)
            {
                TRole? role = await RoleManager.FindByIdAsync(userRole.RoleId);
                if (role != null)
                {
                    roles.Add(role);
                }
            }

            return roles;
        }
        /// <summary>
        /// Gets the user identifier for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose identifier should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">Specified <paramref name="user"/> not found on repository.</exception>
        public async Task<TKey> GetUserIdAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not found");
            }

            return await UserRepository.GetUserIdAsync(user, _cancellationToken);
        }
        /// <summary>
        /// Gets the user name for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose name should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the name for the specified <paramref name="user"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">Specified <paramref name="user"/> not found on repository.</exception>
        public async Task<string> GetUserNameAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not found");
            }

            return await UserRepository.GetUserNameAsync(user, _cancellationToken);
        }
        /// <summary>
        /// Returns a list of users from the user store who have the specified <paramref name="claim"/>.
        /// </summary>
        /// <param name="claim">The claim to look for.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a list of users who have the specified claim.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="claim"/> is <see langword="null"/></exception>
        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim)
        {
            if (claim is null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            List<TUser> users = new();

            IList<TUserClaim> userClaims = await UserClaimRepository.FindAsync(p => p.ClaimType == claim.Type && p.ClaimValue == claim.Value, _cancellationToken);

            foreach (var userClaim in userClaims)
            {
                TUser? user = await UserRepository.FindByIdAsync(userClaim.UserId, _cancellationToken);
                if (user != null)
                {
                    users.Add(user);
                }
            }
            return users;
        }
        /// <summary>
        /// Returns a list of users from the user store who are members of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose users should be returned.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a list of users who are members of the specified role.</returns>
        /// <exception cref="ArgumentException">Specified <paramref name="role"/> not exists on repository.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <see langword="null"/></exception>
        public async Task<IList<TUser>> GetUsersInRoleAsync(TRole role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (!await RoleManager.IsRoleExistsAsync(role))
            {
                _logger.LogError("Role {0} not found", role.Name);
                throw new ArgumentException($"Role {role.Name} is not exists.");
            }

            List<TUser> users = new();
            IList<TUserRole> userRoles = await UserRoleRepository.FindAsync(p => p.RoleId.Equals(role.Id), _cancellationToken);

            foreach (var userRole in userRoles)
            {
                TUser? user = await UserRepository.FindByIdAsync(userRole.UserId, _cancellationToken);
                if (user != null)
                {
                    users.Add(user);
                }
            }

            return users;
        }
        /// <summary>
        /// Gets a flag indicating whether the specified <paramref name="user"/> has a password.
        /// </summary>
        /// <param name="user">The user to return a flag for, indicating whether they have a password or not.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, returning true if the specified <paramref name="user"/> has a password otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> not exist on repository.</exception>
        public async Task<bool> HasPasswordAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            return await UserRepository.HasPasswordAsync(user, _cancellationToken);
        }
        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="user"/> is a member of the given role.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="role">The role to be checked.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a flag indicating whether the specified <paramref name="user"/> is a member of the role.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="role"/> is 
        /// <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> or <paramref name="role"/>
        /// is not exist on repository.</exception>
        public async Task<bool> IsInRoleAsync(TUser user, TRole role)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            if (!await RoleManager.IsRoleExistsAsync(role))
            {
                _logger.LogError("Role {0} not found", role.Name);
                throw new ArgumentException($"{nameof(role)} not exists.");
            }

            return await UserRoleRepository.FindAsync(user.Id, role.Id, _cancellationToken) != null;
        }
        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="user"/> is locked out, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose locked out status should be retrieved.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, true if the specified <paramref name="user"/> is locked out, otherwise false.</returns>
        /// <exception cref="ArgumentException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<bool> IsLockedOutAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            bool isLockedOutEnable = await UserRepository.GetLockoutEnabledAsync(user);
            if (!isLockedOutEnable)
            {
                return false;
            }

            DateTimeOffset? lockOutEndDate = await UserRepository.GetLockoutEndDateAsync(user);
            if (lockOutEndDate == null)
            {
                return false;
            }

            if (lockOutEndDate < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Normalize user or role name for consistent comparisons.
        /// </summary>
        /// <param name="name">The name to normalize.</param>
        /// <returns>A normalized value representing the specified <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="name"/> is null or only contain whitespace.</exception>
        public string NormalizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            return _keyNormalizer.NormalizeName(name);
        }
        /// <summary>
        /// Removes the specified <paramref name="claim"/> from the given <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to remove the specified <paramref name="claim"/> from.</param>
        /// <param name="claim">The <see cref="Claim"/> to remove.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="claim"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not found on repository.</exception>
        public async Task<IdentityResult> RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claim is null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }


            TUserClaim? userClaim = (await UserClaimRepository.FindAsync(p => p.ClaimType == claim.Type &&
                p.ClaimValue == claim.Value &&
                p.UserId.Equals(user.Id),
                _cancellationToken)).FirstOrDefault();

            if (userClaim == null)
            {
                return IdentityResult.Success;
            }

            IdentityResult result = await UserClaimRepository.DeleteAsync(userClaim, _cancellationToken);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to remove claim {0} from user {1}", claim.Type, user.UserName);
            }

            _logger.LogInformation("Claim {0} removed from user {1}", claim.Type, user.UserName);
            return result;
        }
        /// <summary>
        /// Removes the specified <paramref name="claims"/> from the given <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to remove the specified <paramref name="claims"/> from.</param>
        /// <param name="claims">A collection of <see cref="Claim"/>s to remove.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="claims"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claims is null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            IList<TUserClaim> userClaims = await UserClaimRepository.FindAsync(p => claims.Any(x => x.Type == p.ClaimType &&
                x.Value == p.ClaimValue) &&
               p.UserId.Equals(user.Id),
               _cancellationToken);

            foreach (var userClaim in userClaims)
            {
                IdentityResult result = await UserClaimRepository.DeleteAsync(userClaim, _cancellationToken);
                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to remove claims from user {0}", user.UserName);
                    return result;
                }
            }

            _logger.LogInformation("Claims removed from user {0}", user.UserName);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Removes the specified <paramref name="user"/> from the named role.
        /// </summary>
        /// <param name="user">The user to remove from the named role.</param>
        /// <param name="role">The name of the role to remove the user from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="role"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> or <paramref name="role"/>
        /// is not exist on repository.</exception>
        public async Task<IdentityResult> RemoveFromRoleAsync(TUser user, TRole role)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            if (!await RoleManager.IsRoleExistsAsync(role))
            {
                _logger.LogError("Role {0} not found", role.Name);
                throw new ArgumentException($"{nameof(role)} not exists.");
            }

            TUserRole? userRole = await UserRoleRepository.FindAsync(user.Id, role.Id, _cancellationToken);

            if (userRole == null)
            {
                return IdentityResult.Success;
            }

            IdentityResult result = await UserRoleRepository.DeleteAsync(userRole, _cancellationToken);

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to remove role {0} from user {1}", role.Name, user.UserName);
                return result;
            }

            _logger.LogInformation("Role {0} removed from user {1}", role.Name, user.UserName);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Removes the specified <paramref name="user"/> from the named roles.
        /// </summary>
        /// <param name="user">The user to remove from the named <paramref name="roles"/>.</param>
        /// <param name="roles">The name of the roles to remove the <paramref name="user"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="roles"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> or one or more role(s) in <paramref name="roles"/>
        /// is not exist on repository.</exception>
        public async Task<IdentityResult> RemoveFromRolesAsync(TUser user, IEnumerable<TRole> roles)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (roles is null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            foreach (var role in roles)
            {
                if (!await RoleManager.IsRoleExistsAsync(role))
                {
                    _logger.LogError("Role {0} not exist", role.Name);
                    throw new ArgumentException($"{nameof(role)} is not exist.");
                }

                TUserRole? userRole = await UserRoleRepository.FindAsync(user.Id, role.Id, _cancellationToken);

                if (userRole == null)
                {
                    return IdentityResult.Success;
                }

                IdentityResult result = await UserRoleRepository.DeleteAsync(userRole, _cancellationToken);

                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to remove role {0} from user {1}", role.Name, user.UserName);
                    return result;
                }
            }

            _logger.LogInformation("Roles removed from user {0}", user.UserName);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Removes a user's password.
        /// </summary>
        /// <param name="user">The user whose password should be removed.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> RemovePasswordAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            await UserRepository.SetPasswordHashAsync(user, string.Empty, _cancellationToken);
            _logger.LogInformation("Password removed from user {0}", user.UserName);
            return await UpdateAsync(user);
        }
        /// <summary>
        /// Resets the access failed count for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose failed access count should be reset.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> ResetAccessFailedCountAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            await UserRepository.ResetAccessFailedCountAsync(user, _cancellationToken);
            _logger.LogInformation("User {0} access failed succesfully reset", user);
            return await UpdateAsync(user);
        }
        /// <summary>
        /// Resets the <paramref name="user"/>'s password to the specified <paramref name="newPassword"/>.
        /// </summary>
        /// <param name="user">The user whose password should be reset.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="newPassword"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> ResetPasswordAsync(TUser user, string newPassword)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (newPassword is null)
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            IdentityResult validationResult = await ValidatePasswordAsync(user, newPassword);
            if (!validationResult.Succeeded)
            {
                _logger.LogError("User {0} password validation failed", user.UserName);
                return validationResult;
            }

            string passwordHash = _passwordHasher.HashPassword(user, newPassword);

            await UserRepository.SetPasswordHashAsync(user, passwordHash, _cancellationToken);
            _logger.LogInformation("User {0} password has been reset", user.UserName);
            return await UpdateAsync(user);
        }
        /// <summary>
        /// Sets a flag indicating whether the specified <paramref name="user"/> is locked out, as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose locked out status should be set.</param>
        /// <param name="enabled">Flag indicating whether the user is locked out or not.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, the <see cref="IdentityResult"/> of the operation</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            await UserRepository.SetLockoutEnabledAsync(user, enabled, _cancellationToken);
            _logger.LogInformation("Use {0} lockout enabled has been set", user.UserName);
            return await UpdateAsync(user);
        }
        /// <summary>
        /// Locks out a user until the specified end date has passed. Setting a end date in the past immediately unlocks a user.
        /// </summary>
        /// <param name="user">The user whose lockout date should be set.</param>
        /// <param name="lockoutEnd">The <see cref="DateTimeOffset"/> after which the <paramref name="user"/>'s lockout should end.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            await UserRepository.SetLockoutEndDateAsync(user, lockoutEnd, _cancellationToken);
            _logger.LogInformation("User {0} lockout end date has been set", user.UserName);
            return await UpdateAsync(user);
        }
        /// <summary>
        /// Sets the given <paramref name="userName"/> for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose name should be set.</param>
        /// <param name="userName">The user name to set.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="userName"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> SetUserNameAsync(TUser user, string userName)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (userName is null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            IdentityResult result = await ValidateUserAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("User {0} validation failed", userName);
                return result;
            }

            await UserRepository.SetUserNameAsync(user, userName, _cancellationToken);
            _logger.LogInformation("Username {0} has been set", userName);
            return await UpdateAsync(user);
        }
        /// <summary>
        /// Updates the specified <paramref name="user"/> in the backing store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException">Specified <paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> UpdateAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            IdentityResult result = await UserRepository.UpdateAsync(user, _cancellationToken);
            if (!result.Succeeded)
            {
                _logger.LogError("User {0} update failed", user.UserName);
                return result;
            }

            _logger.LogInformation("User {0} updated", user.UserName);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Updates a user's password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="validatePassword">Whether to validate the password.</param>
        /// <returns>Whether the password has was successfully updated.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="newPassword"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> UpdatePasswordHash(TUser user, string newPassword, bool validatePassword)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (newPassword is null)
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            if (validatePassword)
            {
                IdentityResult identityResult = _passwordValidator.Validate(newPassword);
                if (!identityResult.Succeeded)
                {
                    _logger.LogError("User {0} password validation failed", user.UserName);
                    return identityResult;
                }
            }

            string passwordHash = _passwordHasher.HashPassword(user, newPassword);

            await UserRepository.SetPasswordHashAsync(user, passwordHash, _cancellationToken);
            _logger.LogInformation("User {0] password hash updated", user.UserName);
            return await UpdateAsync(user);
        }
        /// <summary>
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is called before updating the password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="password"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> ValidatePasswordAsync(TUser user, string password)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            return _passwordValidator.Validate(password);
        }
        /// <summary>
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is called before saving the user via Create or Update.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<IdentityResult> ValidateUserAsync(TUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            return await _userValidator.ValidateAsync(user);
        }
        /// <summary>
        /// Returns a <see cref="PasswordVerificationResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="password">The password to verify.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="PasswordVerificationResult"/> of the operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> or <paramref name="password"/> 
        /// is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="user"/> is not exist on repository.</exception>
        public async Task<PasswordVerificationResult> VerifyPasswordAsync(TUser user, string password)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (!await IsUserExistAsync(user))
            {
                _logger.LogError("User {0} not found", user.UserName);
                throw new ArgumentException($"{nameof(user)} not exists.");
            }

            string passwordHash = await UserRepository.GetPasswordHashAsync(user, _cancellationToken);

            return _passwordHasher.VerifyHashedPassword(user, passwordHash, password);
        }
        /// <inheritdoc/>
        public async Task<bool> IsUserExistAsync(TUser user)
        {
            return await UserRepository.FindByIdAsync(user.Id, _cancellationToken) != null;
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
                UserRepository.Dispose();
                UserClaimRepository.Dispose();
                UserRoleRepository.Dispose();
            }

            _disposed = true;
        }
        #endregion Private methods

    }
}
