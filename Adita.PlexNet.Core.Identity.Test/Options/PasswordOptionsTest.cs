using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Adita.PlexNet.Core.Identity.Test.Options
{
    [TestClass]
    public class PasswordOptionsTest
    {
        [TestMethod]
        public void VerifyDefaultTest()
        {
            PasswordOptions options = new PasswordOptions();

            Assert.IsTrue(options.RequireDigit);
            Assert.AreEqual(options.RequiredLength, 6);
            Assert.AreEqual(options.RequiredUniqueChars, 1);
            Assert.IsTrue(options.RequiredLowercase);
            Assert.IsTrue(options.RequireNonAlphanumeric);
            Assert.IsTrue(options.RequireUppercase);
        }
        [TestMethod]
        public void CanCustomize()
        {
            var services = new ServiceCollection().Configure<PasswordOptions>(
                options =>
                {
                    options.RequireDigit = false;
                    options.RequiredLength = 8;
                    options.RequiredUniqueChars = 2;
                    options.RequiredLowercase = false;
                    options.RequireNonAlphanumeric = false;
                    options.RequireUppercase = false;
                });

            var serviceProvider = services.BuildServiceProvider();

            var setup = serviceProvider.GetRequiredService<IConfigureOptions<PasswordOptions>>();
            Assert.IsNotNull(setup);

            var optionsGetter = serviceProvider.GetRequiredService<IOptions<PasswordOptions>>();
            Assert.IsNotNull(optionsGetter);

            var options = optionsGetter.Value;
            Assert.IsFalse(options.RequireDigit);
            Assert.IsTrue(options.RequiredLength == 8);
            Assert.IsTrue(options.RequiredUniqueChars == 2);
            Assert.IsFalse(options.RequiredLowercase);
            Assert.IsFalse(options.RequireNonAlphanumeric);
            Assert.IsFalse(options.RequireUppercase);
        }
    }
}
