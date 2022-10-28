using Adita.PlexNet.Core.Identity.Test.Services.Repositories.UserRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Adita.PlexNet.Core.Identity.Test.Services.UserValidators
{
    [TestClass]
    public class UserValidatorTest
    {
        [TestMethod]
        public async Task CanValidate()
        {
            var services = new ServiceCollection().Configure<UserOptions>(
             options =>
             {
                 options.RequireUniqueEmail = false;
                 options.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
             });

            services.AddSingleton<IdentityErrorDescriber>();
            services.AddSingleton<IUserRepository<Guid, IdentityUser>, InMemoryUserRepository<Guid, IdentityUser>>();
            services.AddSingleton<UserValidator<Guid, IdentityUser>>();

            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfigureOptions<UserOptions>>();
            Assert.IsNotNull(configuration);

            var options = serviceProvider.GetRequiredService<IOptions<UserOptions>>();
            Assert.IsNotNull(options);

            var errorDescriber = serviceProvider.GetRequiredService<IdentityErrorDescriber>();
            Assert.IsNotNull(errorDescriber);

            var repository = serviceProvider.GetRequiredService<IUserRepository<Guid, IdentityUser>>();
            Assert.IsNotNull(repository);

            var userValidator = serviceProvider.GetRequiredService<UserValidator<Guid, IdentityUser>>();
            Assert.IsNotNull(userValidator);

            IdentityResult result1 = await repository.CreateAsync(new IdentityUser("Administrator") { Id = Guid.NewGuid(), NormalizedUserName = "Administrator" });
            Assert.IsTrue(result1.Succeeded);

            IdentityResult result2 = await repository.CreateAsync(new IdentityUser("admin!") { Id = Guid.NewGuid(), NormalizedUserName = "admin!" });
            Assert.IsTrue(result2.Succeeded);

            IdentityResult result3 = await repository.CreateAsync(new IdentityUser("admin1") { Id = Guid.NewGuid(), NormalizedUserName = "admin1" });
            Assert.IsTrue(result3.Succeeded);

            IdentityUser? user1 = await repository.FindByNameAsync("Administrator");
            Assert.IsNotNull(user1);

            IdentityUser? user2 = await repository.FindByNameAsync("admin!");
            Assert.IsNotNull(user2);

            IdentityUser? user3 = await repository.FindByNameAsync("admin1");
            Assert.IsNotNull(user3);

            //should success
            IdentityResult validationResult1 = await userValidator.ValidateAsync(user1);
            Assert.IsTrue(validationResult1.Succeeded);

            //should invalid due to not allowed char non alphanumeric
            IdentityResult validationResult2 = await userValidator.ValidateAsync(user2);
            Assert.IsFalse(validationResult2.Succeeded);

            //should invalid due to not allowed char digit
            IdentityResult validationResult3 = await userValidator.ValidateAsync(user3);
            Assert.IsFalse(validationResult3.Succeeded);
        }
    }
}
