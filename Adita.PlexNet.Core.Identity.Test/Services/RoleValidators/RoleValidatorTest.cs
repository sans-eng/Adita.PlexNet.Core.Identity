using Adita.PlexNet.Core.Identity.Test.Services.Repositories.RoleRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Adita.PlexNet.Core.Identity.Test.Services.RoleValidators
{
    [TestClass]
    public class RoleValidatorTest
    {
        [TestMethod]
        public async Task CanValidate()
        {
            var services = new ServiceCollection().Configure<RoleOptions>(
               options =>
               {
                   options.RequiredRoleNameLength = 6;
                   options.AllowedRoleNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
               });

            services.AddSingleton<IdentityErrorDescriber>();
            services.AddSingleton<IRoleRepository<Guid, IdentityRole>, InMemoryRoleRepository<Guid, IdentityRole>>();
            services.AddSingleton<RoleValidator<Guid, IdentityRole>>();

            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfigureOptions<RoleOptions>>();
            Assert.IsNotNull(configuration);

            var options = serviceProvider.GetRequiredService<IOptions<RoleOptions>>();
            Assert.IsNotNull(options);

            var errorDescriber = serviceProvider.GetRequiredService<IdentityErrorDescriber>();
            Assert.IsNotNull(errorDescriber);

            var repository = serviceProvider.GetRequiredService<IRoleRepository<Guid, IdentityRole>>();
            Assert.IsNotNull(repository);

            var roleValidator = serviceProvider.GetRequiredService<RoleValidator<Guid, IdentityRole>>();
            Assert.IsNotNull(roleValidator);

            IdentityResult result1 = await repository.CreateAsync(new IdentityRole("Administrator") { Id = Guid.NewGuid(), NormalizedName = "Administrator" });
            Assert.IsTrue(result1.Succeeded);

            IdentityResult result2 = await repository.CreateAsync(new IdentityRole("admin") { Id = Guid.NewGuid(), NormalizedName = "admin" });
            Assert.IsTrue(result2.Succeeded);

            IdentityResult result3 = await repository.CreateAsync(new IdentityRole("admin!") { Id = Guid.NewGuid(), NormalizedName = "admin!" });
            Assert.IsTrue(result3.Succeeded);

            IdentityResult result4 = await repository.CreateAsync(new IdentityRole("admin1") { Id = Guid.NewGuid(), NormalizedName = "admin1" });
            Assert.IsTrue(result4.Succeeded);

            IdentityRole? role1 = await repository.FindByNameAsync("Administrator");
            Assert.IsNotNull(role1);

            IdentityRole? role2 = await repository.FindByNameAsync("admin");
            Assert.IsNotNull(role2);

            IdentityRole? role3 = await repository.FindByNameAsync("admin!");
            Assert.IsNotNull(role3);

            IdentityRole? role4 = await repository.FindByNameAsync("admin1");
            Assert.IsNotNull(role4);

            //should success
            IdentityResult validationResult1 = await roleValidator.ValidateAsync(role1);
            Assert.IsTrue(validationResult1.Succeeded);

            //should invalid due to length not enough
            IdentityResult validationResult2 = await roleValidator.ValidateAsync(role2);
            Assert.IsFalse(validationResult2.Succeeded);

            //should invalid due to not allowed char non alphanumeric
            IdentityResult validationResult3 = await roleValidator.ValidateAsync(role3);
            Assert.IsFalse(validationResult3.Succeeded);

            //should invalid due to not allowed char digit
            IdentityResult validationResult4 = await roleValidator.ValidateAsync(role4);
            Assert.IsFalse(validationResult4.Succeeded);
        }
    }
}
