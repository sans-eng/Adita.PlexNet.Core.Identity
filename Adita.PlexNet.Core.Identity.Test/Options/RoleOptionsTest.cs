using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Adita.PlexNet.Core.Identity.Test.Options
{
    [TestClass]
    public class RoleOptionsTest
    {
        [TestMethod]
        public void VerifyDefaultTest()
        {
            RoleOptions options = new RoleOptions();

            Assert.AreEqual(options.AllowedRoleNameCharacters, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_ ");
            Assert.AreEqual(options.RequiredRoleNameLength, 6);
        }
        [TestMethod]
        public void CanCustomize()
        {
            var services = new ServiceCollection().Configure<RoleOptions>(
                options =>
                {
                    options.AllowedRoleNameCharacters = "ABCDEFG";
                    options.RequiredRoleNameLength = 10;
                });

            var serviceProvider = services.BuildServiceProvider();

            var setup = serviceProvider.GetRequiredService<IConfigureOptions<RoleOptions>>();
            Assert.IsNotNull(setup);

            var optionsGetter = serviceProvider.GetRequiredService<IOptions<RoleOptions>>();
            Assert.IsNotNull(optionsGetter);

            var options = optionsGetter.Value;
            Assert.IsFalse(options.AllowedRoleNameCharacters.Contains('Z'));
            Assert.IsTrue(options.RequiredRoleNameLength == 10);
        }
    }
}
