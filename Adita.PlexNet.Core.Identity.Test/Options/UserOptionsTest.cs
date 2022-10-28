using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Adita.PlexNet.Core.Identity.Test.Options
{
    [TestClass]
    public class UserOptionsTest
    {
        [TestMethod]
        public void VerifyDefaultTest()
        {
            UserOptions options = new UserOptions();

            Assert.AreEqual(options.AllowedUserNameCharacters, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+");
            Assert.IsFalse(options.RequireUniqueEmail);
        }
        [TestMethod]
        public void CanCustomize()
        {
            var services = new ServiceCollection().Configure<UserOptions>(
                options =>
                {
                    options.AllowedUserNameCharacters = "ABCDEFG";
                    options.RequireUniqueEmail = true;
                });

            var serviceProvider = services.BuildServiceProvider();

            var setup = serviceProvider.GetRequiredService<IConfigureOptions<UserOptions>>();
            Assert.IsNotNull(setup);

            var optionsGetter = serviceProvider.GetRequiredService<IOptions<UserOptions>>();
            Assert.IsNotNull(optionsGetter);

            var options = optionsGetter.Value;
            Assert.IsFalse(options.AllowedUserNameCharacters.Contains('Z'));
            Assert.IsTrue(options.RequireUniqueEmail);
        }
    }
}
