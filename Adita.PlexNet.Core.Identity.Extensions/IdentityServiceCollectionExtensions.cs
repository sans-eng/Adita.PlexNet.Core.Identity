using Adita.PlexNet.Core.Identity;
using Adita.PlexNet.Core.Identity.EntityFrameworkCore;
using Adita.PlexNet.Core.Security.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Adita.PlexNet.Core.Extensions.Identity
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class IdentityServiceCollectionExtensions
    {
        #region Public methods
        /// <summary>
        /// Adds and configures the default identity system.
        /// </summary>
        /// <typeparam name="TContext">The type for the <see cref="DbContext"/>.</typeparam>
        /// <param name="serviceDescriptors">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
        /// <returns>An <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> for creating and configuring the identity system.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serviceDescriptors"/> or <paramref name="setupAction"/> is <c>null</c>.</exception>
        public static IdentityBuilder<Guid, IdentityUser, IdentityUserClaim, IdentityUserRole, IdentityRole, IdentityRoleClaim>
            AddDefaultIdentity<TContext>(this IServiceCollection serviceDescriptors, Action<IdentityOptions> setupAction)
            where TContext : DbContext
        {
            if (serviceDescriptors is null)
            {
                throw new ArgumentNullException(nameof(serviceDescriptors));
            }

            if (setupAction is null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            return AddIdentity<Guid, IdentityUser, IdentityUserClaim, IdentityUserRole, IdentityRole, IdentityRoleClaim, TContext>(serviceDescriptors, setupAction);
        }

        /// <summary>
        /// Adds and configures the identity system.
        /// </summary>
        /// <typeparam name="TKey">The type used for the primary key of the users and roles.</typeparam>
        /// <typeparam name="TUser">The type for the users.</typeparam>
        /// <typeparam name="TRole">The type for the roles.</typeparam>
        /// <typeparam name="TContext">The type for the <see cref="DbContext"/>.</typeparam>
        /// <param name="serviceDescriptors">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
        /// <returns>An <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> for creating and configuring the identity system.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serviceDescriptors"/> or <paramref name="setupAction"/> is <c>null</c>.</exception>
        public static IdentityBuilder<TKey, TUser, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, TRole, IdentityRoleClaim<TKey>>
            AddIdentity<TKey, TUser, TRole, TContext>(this IServiceCollection serviceDescriptors, Action<IdentityOptions> setupAction)
            where TKey : IEquatable<TKey>
            where TUser : IdentityUser<TKey>, new()
            where TRole : IdentityRole<TKey>, new()
            where TContext : DbContext
        {
            if (serviceDescriptors is null)
            {
                throw new ArgumentNullException(nameof(serviceDescriptors));
            }

            if (setupAction is null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            return AddIdentity<TKey, TUser, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, TRole, IdentityRoleClaim<TKey>, TContext>(serviceDescriptors, setupAction);
        }

        /// <summary>
        /// Adds and configures the identity system.
        /// </summary>
        /// <typeparam name="TKey">The type used for the primary key of the users, user claims, user roles, roles and role claims.</typeparam>
        /// <typeparam name="TUser">The type for the users.</typeparam>
        /// <typeparam name="TUserClaim">The type for the user claims.</typeparam>
        /// <typeparam name="TUserRole">The type for the user roles.</typeparam>
        /// <typeparam name="TRole">The type for the roles.</typeparam>
        /// <typeparam name="TRoleClaim">The type for the role claims.</typeparam>
        /// <typeparam name="TContext">The type for the <see cref="DbContext"/>.</typeparam>
        /// <param name="serviceDescriptors">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
        /// <returns>An <see cref="IdentityBuilder{TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim}"/> for creating and configuring the identity system.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serviceDescriptors"/> or <paramref name="setupAction"/> is <c>null</c>.</exception>
        public static IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim>
            AddIdentity<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim, TContext>
            (this IServiceCollection serviceDescriptors, Action<IdentityOptions> setupAction)
            where TKey : IEquatable<TKey>
            where TUser : IdentityUser<TKey>, new()
            where TUserClaim : IdentityUserClaim<TKey>, new()
            where TUserRole : IdentityUserRole<TKey>, new()
            where TRole : IdentityRole<TKey>, new()
            where TRoleClaim : IdentityRoleClaim<TKey>, new()
            where TContext : DbContext
        {
            if (serviceDescriptors is null)
            {
                throw new ArgumentNullException(nameof(serviceDescriptors));
            }

            if (setupAction is null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            IdentityBuilder<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim> builder =
                new(serviceDescriptors);

            builder.AddUserValidator<UserValidator<TKey, TUser>>()
                            .AddApplicationPrincipalFactory<ApplicationPrincipalFactory<TKey, TUser, TRole>>()
                            .AddErrorDescriber<IdentityErrorDescriber>()
                            .AddPasswordHasher<BCryptPasswordHasher<TKey, TUser>>()
                            .AddPasswordValidator<PasswordValidator>()
                            .AddUserManager<UserManager<TKey, TUser, TUserClaim, TUserRole, TRole>>()
                            .AddRoleValidator<RoleValidator<TKey, TRole>>()
                            .AddRoleManager<RoleManager<TKey, TRole, TRoleClaim>>()
                            .AddSignInManager<SignInManager<TKey, TUser, TRole>>()
                            .AddLookupNormalizer<UpperInvariantLookupNormalizer>()
                            .AddUserRepository<UserRepository<TKey, TUser, TContext>>()
                            .AddUserClaimRepository<UserClaimRepository<TKey, TUserClaim, TContext>>()
                            .AddUserRoleRepository<UserRoleRepository<TKey, TUserRole, TContext>>()
                            .AddRoleRepository<RoleRepository<TKey, TRole, TContext>>()
                            .AddRoleClaimRepository<RoleClaimRepository<TKey, TRoleClaim, TContext>>()
                            .ConfigureIdentityOptions(setupAction);

            builder.Services.AddScoped<IAuthorizationManager, AuthorizationManager>();

            return builder;
        }
        #endregion Public methods
    }
}
