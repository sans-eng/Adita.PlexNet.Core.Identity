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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Represents a builder that build identity system.
    /// </summary>
    public class IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TRole : IdentityRole<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> using specified <paramref name="serviceDescriptors"/>.
        /// </summary>
        /// <param name="serviceDescriptors">The <see cref="IServiceCollection"/> to attach to.</param>
        public IdentityBuilder(IServiceCollection serviceDescriptors)
        {
            Services = serviceDescriptors;
        }
        #endregion Constructors

        #region Public properties
        /// <summary>
        /// Gets an <see cref="IServiceCollection"/> that currently managed by current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        public IServiceCollection Services { get; }
        /// <summary>
        /// Gets the key <see cref="Type"/> that stores on current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        public Type KeyType { get { return typeof(TKey); } }
        /// <summary>
        /// Gets the user <see cref="Type"/> that stores on current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        public Type UserType { get { return typeof(TUser); } }
        /// <summary>
        /// Gets the user claim <see cref="Type"/> that stores on current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        public Type UserClaimType { get { return typeof(TUserClaim); } }
        /// <summary>
        /// Gets the user role <see cref="Type"/> that stores on current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        public Type UserRoleType { get { return typeof(TUserRole); } }
        /// <summary>
        /// Gets the role <see cref="Type"/> that stores on current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        public Type RoleType { get { return typeof(TRole); } }
        /// <summary>
        /// Gets the role claim <see cref="Type"/> that stores on current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        public Type RoleClaimType { get { return typeof(TRoleClaim); } }
        #endregion Public properties

        #region Public methods
        /// <summary>
        /// Adds an <see cref="ILookupNormalizer"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TNormalizer">The <see cref="ILookupNormalizer"/> type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddLookupNormalizer<TNormalizer>() where TNormalizer : ILookupNormalizer
        => AddScoped(typeof(ILookupNormalizer), typeof(TNormalizer));
        /// <summary>
        /// Adds an <see cref="IUserValidator{TUser}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TValidator">The user validator type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddUserValidator<TValidator>() where TValidator : IUserValidator<TUser>
            => AddScoped(typeof(IUserValidator<TUser>), typeof(TValidator));
        /// <summary>
        /// Adds an <see cref="IApplicationPrincipalFactory{TUser}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TFactory">The type of the application principal factory.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddApplicationPrincipalFactory<TFactory>() where TFactory : IApplicationPrincipalFactory<TUser>
            => AddScoped(typeof(IApplicationPrincipalFactory<TUser>), typeof(TFactory));
        /// <summary>
        /// Adds an <see cref="IdentityErrorDescriber"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TDescriber">The type of the error describer.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddErrorDescriber<TDescriber>() where TDescriber : IdentityErrorDescriber
        {
            Services.AddScoped<IdentityErrorDescriber, TDescriber>();
            return this;
        }
        /// <summary>
        /// Adds an <see cref="IPasswordValidator"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TValidator">The validator type used to validate passwords.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddPasswordValidator<TValidator>() where TValidator : IPasswordValidator
            => AddScoped(typeof(IPasswordValidator), typeof(TValidator));
        /// <summary>
        /// Adds an <see cref="IPasswordHasher{TKey, TUser}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TPasswordHasher">The hasher type used to hashing passwords.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddPasswordHasher<TPasswordHasher>() where TPasswordHasher : IPasswordHasher<TKey, TUser>
            => AddScoped(typeof(IPasswordHasher<TKey, TUser>), typeof(TPasswordHasher));
        /// <summary>
        /// Adds an <see cref="IUserRepository{TKey, TUser}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TRepository">The user repository type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddUserRepository<TRepository>() where TRepository : IUserRepository<TKey, TUser>
            => TryAddScoped(typeof(IUserRepository<TKey, TUser>), typeof(TRepository));
        /// <summary>
        /// Adds an <see cref="IUserClaimRepository{TKey, TUserClaim}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TRepository">The user claim repository type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddUserClaimRepository<TRepository>() where TRepository : IUserClaimRepository<TKey, TUserClaim>
            => TryAddScoped(typeof(IUserClaimRepository<TKey, TUserClaim>), typeof(TRepository));
        /// <summary>
        /// Adds an <see cref="IUserClaimRepository{TKey, TUserClaim}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TRepository">The user role repository type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddUserRoleRepository<TRepository>() where TRepository : IUserRoleRepository<TKey, TUserRole>
            => TryAddScoped(typeof(IUserRoleRepository<TKey, TUserRole>), typeof(TRepository));
        /// <summary>
        /// Adds a <see cref="IUserManager{TKey, TUser, TRole}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TUserManager">The type of the user manager to add.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddUserManager<TUserManager>() where TUserManager : IUserManager<TKey, TUser, TRole>
            => AddScoped(typeof(IUserManager<TKey, TUser, TRole>), typeof(TUserManager));
        /// <summary>
        /// Adds an <see cref="IRoleValidator{TRole}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TRoleValidator">The role validator type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddRoleValidator<TRoleValidator>() where TRoleValidator : IRoleValidator<TRole> =>
            AddScoped(typeof(IRoleValidator<TRole>), typeof(TRoleValidator));
        /// <summary>
        /// Adds a <see cref="IRoleRepository{TKey, TRole}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TRepository">The role repository type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddRoleRepository<TRepository>() where TRepository : IRoleRepository<TKey, TRole>
            => TryAddScoped(typeof(IRoleRepository<TKey, TRole>), typeof(TRepository));
        /// <summary>
        /// Adds a <see cref="IRoleClaimRepository{TKey, TRoleClaim}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TRepository">The role claim repository type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddRoleClaimRepository<TRepository>() where TRepository : IRoleClaimRepository<TKey, TRoleClaim>
            => TryAddScoped(typeof(IRoleClaimRepository<TKey, TRoleClaim>), typeof(TRepository));
        /// <summary>
        /// Adds a <see cref="IRoleManager{TKey, TRole}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TRoleManager">The type of the role manager to add.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddRoleManager<TRoleManager>() where TRoleManager : IRoleManager<TKey, TRole>
        {
            return AddScoped(typeof(IRoleManager<TKey, TRole>), typeof(TRoleManager));
        }
        /// <summary>
        /// Adds a <see cref="ISignInManager{TUser}"/> to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <typeparam name="TSignInManager">The type of the sign-in manager to add.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddSignInManager<TSignInManager>()
            where TSignInManager : ISignInManager<TUser>
        {
            return AddScoped(typeof(ISignInManager<TUser>), typeof(TSignInManager));
        }
        /// <summary>
        /// Registers specified <paramref name="configureAction"/> to configure an <see cref="IdentityOptions"/>
        /// to current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/>.
        /// </summary>
        /// <param name="configureAction">The <see cref="Action{T}"/> of <see cref="IdentityOptions"/> to configure an <see cref="IdentityOptions"/>.</param>
        /// <returns>The current <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> instance.</returns>
        public virtual IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> ConfigureIdentityOptions(Action<IdentityOptions> configureAction)
        {
            Services.Configure(configureAction);
            Services.AddOptions<ApplicationIdentityOptions>().Configure<IdentityOptions>(ConfigureApplicationIdentityOptions);
            Services.AddOptions<LockoutOptions>().Configure<IdentityOptions>(ConfigureLockoutOptions);
            Services.AddOptions<PasswordOptions>().Configure<IdentityOptions>(ConfigurePasswordOptions);
            Services.AddOptions<RepositoryOptions>().Configure<IdentityOptions>(ConfigureRepositoryOptions);
            Services.AddOptions<RoleOptions>().Configure<IdentityOptions>(ConfigureRoleOptions);
            Services.AddOptions<UserOptions>().Configure<IdentityOptions>(ConfigureUserOptions);


            return this;
        }
        #endregion Public methods

        #region Private methods
        private void ConfigureApplicationIdentityOptions(ApplicationIdentityOptions options, IdentityOptions identityOptions)
        {
            options.EmailClaimType = identityOptions.ApplicationIdentityOptions.EmailClaimType;
            options.RoleClaimType = identityOptions.ApplicationIdentityOptions.RoleClaimType;
            options.UserIdClaimType = identityOptions.ApplicationIdentityOptions.UserIdClaimType;
            options.UserNameClaimType = identityOptions.ApplicationIdentityOptions.UserNameClaimType;
        }
        private void ConfigureLockoutOptions(LockoutOptions options, IdentityOptions identityOptions)
        {
            options.AllowedForNewUsers = identityOptions.LockoutOptions.AllowedForNewUsers;
            options.MaxFailedAccessAttempts = identityOptions.LockoutOptions.MaxFailedAccessAttempts;
            options.DefaultLockoutTimeSpan = identityOptions.LockoutOptions.DefaultLockoutTimeSpan;
        }
        private void ConfigurePasswordOptions(PasswordOptions options, IdentityOptions identityOptions)
        {
            options.RequireDigit = identityOptions.PasswordOptions.RequireDigit;
            options.RequireNonAlphanumeric = identityOptions.PasswordOptions.RequireNonAlphanumeric;
            options.RequiredLength = identityOptions.PasswordOptions.RequiredLength;
            options.RequiredLowercase = identityOptions.PasswordOptions.RequiredLowercase;
            options.RequiredUniqueChars = identityOptions.PasswordOptions.RequiredUniqueChars;
            options.RequireUppercase = identityOptions.PasswordOptions.RequireUppercase;
        }
        private void ConfigureRepositoryOptions(RepositoryOptions options, IdentityOptions identityOptions)
        {
            options.MaxLengthForKeys = identityOptions.RepositoryOptions.MaxLengthForKeys;
        }
        private void ConfigureRoleOptions(RoleOptions options, IdentityOptions identityOptions)
        {
            options.AllowedRoleNameCharacters = identityOptions.RoleOptions.AllowedRoleNameCharacters;
            options.RequiredRoleNameLength = identityOptions.RoleOptions.RequiredRoleNameLength;
        }
        private void ConfigureUserOptions(UserOptions options, IdentityOptions identityOptions)
        {
            options.AllowedUserNameCharacters = identityOptions.UserOptions.AllowedUserNameCharacters;
            options.RequireUniqueEmail = identityOptions.UserOptions.RequireUniqueEmail;
        }

        private IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }
        private IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> TryAddScoped(Type serviceType, Type concreteType)
        {
            Services.TryAddScoped(serviceType, concreteType);
            return this;
        }
        #endregion Private methods
    }
}
