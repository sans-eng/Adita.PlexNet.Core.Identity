using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Adita.PlexNet.Core.Identity.Test.Options
{
    [TestClass]
    public class LockoutOptionsTest
    {
        [TestMethod]
        public void VerifyDefaultTest()
        {
            LockoutOptions options = new LockoutOptions();

            Assert.IsTrue(options.AllowedForNewUsers);
            Assert.AreEqual(options.DefaultLockoutTimeSpan, TimeSpan.FromSeconds(30));
            Assert.AreEqual(options.MaxFailedAccessAttempts, 5);
        }
        [TestMethod]
        public void CanCustomize()
        {
            var services = new ServiceCollection().Configure<LockoutOptions>(
                options =>
                {
                    options.AllowedForNewUsers = false;
                    options.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
                    options.MaxFailedAccessAttempts = 10;
                });

            var serviceProvider = services.BuildServiceProvider();

            var setup = serviceProvider.GetRequiredService<IConfigureOptions<LockoutOptions>>();
            Assert.IsNotNull(setup);

            var optionsGetter = serviceProvider.GetRequiredService<IOptions<LockoutOptions>>();
            Assert.IsNotNull(optionsGetter);

            var options = optionsGetter.Value;
            Assert.IsFalse(options.AllowedForNewUsers);
            Assert.IsTrue(options.DefaultLockoutTimeSpan == TimeSpan.FromSeconds(10));
            Assert.IsTrue(options.MaxFailedAccessAttempts == 10);
        }
    }
}
