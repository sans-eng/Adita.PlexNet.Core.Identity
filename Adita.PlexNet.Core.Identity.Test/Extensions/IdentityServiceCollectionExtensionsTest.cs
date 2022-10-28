using Adita.PlexNet.Core.Extensions.Identity;
using Adita.PlexNet.Core.Extensions.Logging;
using Adita.PlexNet.Core.Identity.EntityFrameworkCore;
using Adita.PlexNet.Core.Identity.Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Adita.PlexNet.Core.Identity.Test.Extensions
{
    [TestClass]
    public class IdentityServiceCollectionExtensionsTest
    {
        [TestMethod]
        public void CanAddDefaultIdentity()
        {
            IServiceCollection services = new ServiceCollection();

            string defaultDirectory = "D://";

            string? appDirectory = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            string? debugDirectory = Path.GetDirectoryName(appDirectory);
            string? projectDirectory = Path.GetDirectoryName(debugDirectory);
            string directory = Path.Combine(projectDirectory ?? defaultDirectory, "Logs");

            services.AddLogging(builder =>
            builder.AddFileLogger(configure =>
            {
                configure.Directory = directory;
                configure.FileNamePrefix = "IdentityTest";
            }));

            services.AddDbContext<IdentityDbContext>(contextOptions =>
            {
                contextOptions.UseInMemoryDatabase("IdentityTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddDefaultIdentity<IdentityDbContext>(setupAction =>
            {
                setupAction.PasswordOptions.RequireDigit = false;
                setupAction.PasswordOptions.RequireNonAlphanumeric = false;
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            IUserManager<Guid, IdentityUser, IdentityRole> userManager = serviceProvider.GetRequiredService<IUserManager<Guid, IdentityUser, IdentityRole>>();
            Assert.IsNotNull(userManager);

            IRoleManager<Guid, IdentityRole> roleManager = serviceProvider.GetRequiredService<IRoleManager<Guid, IdentityRole>>();
            Assert.IsNotNull(roleManager);

            ISignInManager<IdentityUser> signInManager = serviceProvider.GetRequiredService<ISignInManager<IdentityUser>>();
            Assert.IsNotNull(signInManager);


        }
        [TestMethod]
        public void CanAddCustomIdentity()
        {
            IServiceCollection services = new ServiceCollection();

            string defaultDirectory = "D://";

            string? appDirectory = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            string? debugDirectory = Path.GetDirectoryName(appDirectory);
            string? projectDirectory = Path.GetDirectoryName(debugDirectory);
            string directory = Path.Combine(projectDirectory ?? defaultDirectory, "Logs");

            services.AddLogging(builder =>
            builder.AddFileLogger(configure =>
            {
                configure.Directory = directory;
                configure.FileNamePrefix = "CustomIdentityTest";
            }));

            services.AddDbContext<CustomIdentityDbContext>(contextOptions =>
            {
                contextOptions.UseInMemoryDatabase("CustomIdentityTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddIdentity<string, CustomIdentityUser, CustomIdentityRole, CustomIdentityDbContext>(setupAction =>
            {
                setupAction.PasswordOptions.RequireDigit = false;
                setupAction.PasswordOptions.RequireNonAlphanumeric = false;
            });

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            IUserManager<string, CustomIdentityUser, CustomIdentityRole> userManager = serviceProvider.GetRequiredService<IUserManager<string, CustomIdentityUser, CustomIdentityRole>>();
            Assert.IsNotNull(userManager);

            IRoleManager<string, CustomIdentityRole> roleManager = serviceProvider.GetRequiredService<IRoleManager<string, CustomIdentityRole>>();
            Assert.IsNotNull(roleManager);

            ISignInManager<CustomIdentityUser> signInManager = serviceProvider.GetRequiredService<ISignInManager<CustomIdentityUser>>();
            Assert.IsNotNull(signInManager);
        }
    }
}
