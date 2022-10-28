using Adita.PlexNet.Core.Extensions.Logging;
using Adita.PlexNet.Core.Identity.Test.Services.Repositories.RoleClaimRepositories;
using Adita.PlexNet.Core.Identity.Test.Services.Repositories.RoleRepositories;
using Adita.PlexNet.Core.Identity.Test.Services.Repositories.UserClaimRepositories;
using Adita.PlexNet.Core.Identity.Test.Services.Repositories.UserRepositories;
using Adita.PlexNet.Core.Identity.Test.Services.Repositories.UserRoleRepositories;
using Adita.PlexNet.Core.Security.Principals;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Adita.PlexNet.Core.Identity.Test.Services.Factories
{
    [TestClass]
    public class ApplicationPrincipleFactoryTest
    {
        private ServiceProvider? serviceProvider;
        private IdentityUser? user;

        [TestInitialize]
        public async Task CanCreate()
        {
            var services = new ServiceCollection();

            string defaultDirectory = "D://";

            string? appDirectory = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            string? debugDirectory = Path.GetDirectoryName(appDirectory);
            string? projectDirectory = Path.GetDirectoryName(debugDirectory);
            string directory = Path.Combine(projectDirectory ?? defaultDirectory, "Logs");

            services.AddLogging(
                buildier => buildier.AddFileLogger(
                        config =>
                        {
                            config.Directory = directory;
                            config.FileNamePrefix = "Identity";
                        })
                );

            services.Configure<UserOptions>(
             options =>
             {
                 options.RequireUniqueEmail = false;
                 options.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
             });

            services.Configure<RoleOptions>(
               options =>
               {
                   options.RequiredRoleNameLength = 6;
                   options.AllowedRoleNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
               });

            services.Configure<PasswordOptions>(
              options =>
              {
                  options.RequireDigit = false;
                  options.RequireNonAlphanumeric = false;
              });

            services.Configure<LockoutOptions>(
              options => options.AllowedForNewUsers = false);

            services.Configure<ApplicationIdentityOptions>(
             options =>
             {
                 options.RoleClaimType = ClaimTypes.Role;
             });

            services.AddSingleton<IdentityErrorDescriber>();
            services.AddSingleton<IdentityErrorDescriber>();
            services.AddSingleton<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.AddSingleton<IPasswordHasher<Guid, IdentityUser>, BCryptPasswordHasher<Guid, IdentityUser>>();
            services.AddSingleton<IPasswordValidator, PasswordValidator>();
            services.AddSingleton<IUserValidator<IdentityUser>, UserValidator<Guid, IdentityUser>>();
            services.AddSingleton<IUserRepository<Guid, IdentityUser>, InMemoryUserRepository<Guid, IdentityUser>>();
            services.AddSingleton<IUserClaimRepository<Guid, IdentityUserClaim>, InMemoryUserClaimRepository<Guid, IdentityUserClaim>>();
            services.AddSingleton<IUserRoleRepository<Guid, IdentityUserRole>, InMemoryUserRoleRepository<Guid, IdentityUserRole>>();

            services.AddSingleton<IRoleRepository<Guid, IdentityRole>, InMemoryRoleRepository<Guid, IdentityRole>>();
            services.AddSingleton<IRoleClaimRepository<Guid, IdentityRoleClaim>, InMemoryRoleClaimRepository<Guid, IdentityRoleClaim>>();
            services.AddSingleton<IRoleValidator<IdentityRole>, RoleValidator<Guid, IdentityRole>>();

            services.AddSingleton<IRoleManager<Guid, IdentityRole>, RoleManager<Guid, IdentityRole, IdentityRoleClaim>>();
            services.AddSingleton<IUserManager<Guid, IdentityUser, IdentityRole>, UserManager<Guid, IdentityUser, IdentityUserClaim, IdentityUserRole, IdentityRole>>();

            services.AddSingleton<IApplicationPrincipalFactory<IdentityUser>, ApplicationPrincipalFactory<Guid, IdentityUser, IdentityRole>>();

            serviceProvider = services.BuildServiceProvider();

            IUserManager<Guid, IdentityUser, IdentityRole> userManager =
                serviceProvider.GetRequiredService<IUserManager<Guid, IdentityUser, IdentityRole>>();
            Assert.IsNotNull(userManager);

            IRoleManager<Guid, IdentityRole> roleManager =
                serviceProvider.GetRequiredService<IRoleManager<Guid, IdentityRole>>();
            Assert.IsNotNull(roleManager);

            IdentityResult roleCreateResult = await roleManager.CreateAsync(new IdentityRole("Administrator") { Id = Guid.NewGuid() });
            Assert.IsTrue(roleCreateResult.Succeeded);

            IdentityResult userCreateResult = await userManager.CreateAsync(new IdentityUser("TestUser") { Id = Guid.NewGuid() }, "Password!");
            Assert.IsTrue(userCreateResult.Succeeded);

            user = await userManager.FindByNameAsync("TestUser");
            Assert.IsNotNull(user);

            IdentityRole? role = await roleManager.FindByNameAsync("Administrator");
            Assert.IsNotNull(role);

            IdentityResult addToRoleResult = await userManager.AddToRoleAsync(user, role);
            Assert.IsTrue(addToRoleResult.Succeeded);

            IdentityResult addUserClaimResult = await userManager.AddClaimAsync(user, new Claim(ClaimTypes.MobilePhone, "1234567890"));
            Assert.IsTrue(addUserClaimResult.Succeeded);

            IdentityResult addRoleClaimResult = await roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Uri, "https://test.com"));
            Assert.IsTrue(addRoleClaimResult.Succeeded);
        }

        [TestMethod]
        public async Task VerifyClaims()
        {
            Assert.IsNotNull(serviceProvider);
            Assert.IsNotNull(user);

            IApplicationPrincipalFactory<IdentityUser> principalFactory = serviceProvider.GetRequiredService<IApplicationPrincipalFactory<IdentityUser>>();
            Assert.IsNotNull(principalFactory);

            ApplicationPrincipal principal = await principalFactory.CreateAsync(user);
            Assert.IsNotNull(principal);

            bool isInRole = principal.IsInRole("Administrator");
            Assert.IsTrue(isInRole);

            bool hasPhoneClaim = principal.HasClaim(claim => claim.Type == ClaimTypes.MobilePhone && claim.Value == "1234567890");
            Assert.IsTrue(hasPhoneClaim);

            bool hasUriClaim = principal.HasClaim(claim => claim.Type == ClaimTypes.Uri && claim.Value == "https://test.com");
            Assert.IsTrue(hasUriClaim);
        }

        public void test(UserOptions options)
        {

        }
    }
}
