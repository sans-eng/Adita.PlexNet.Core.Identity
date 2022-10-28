using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Adita.PlexNet.Core.Identity.Test.Services.PasswordValidators
{
    [TestClass]
    public class PasswordValidatorTest
    {
        [TestMethod]
        public void CanValidate()
        {
            var services = new ServiceCollection().Configure<PasswordOptions>(
                options =>
                {
                    options.RequireDigit = true;
                    options.RequiredUniqueChars = 1;
                    options.RequiredLength = 8;
                    options.RequireNonAlphanumeric = true;
                    options.RequiredLowercase = true;
                    options.RequireUppercase = true;
                });

            services.AddSingleton<IdentityErrorDescriber>();
            services.AddSingleton<PasswordValidator>();

            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfigureOptions<PasswordOptions>>();
            Assert.IsNotNull(configuration);

            var options = serviceProvider.GetRequiredService<IOptions<PasswordOptions>>();
            Assert.IsNotNull(options);

            var errorDescriber = serviceProvider.GetRequiredService<IdentityErrorDescriber>();
            Assert.IsNotNull(errorDescriber);

            var passwordValidator = serviceProvider.GetRequiredService<PasswordValidator>();
            Assert.IsNotNull(passwordValidator);

            //should success
            IdentityResult validationResult = passwordValidator.Validate("D0nt4get!");
            Assert.IsTrue(validationResult.Succeeded);

            //should not succes due to missing non alphanumeric
            IdentityResult validationResult1 = passwordValidator.Validate("Alexander32");
            Assert.IsFalse(validationResult1.Succeeded);

            //should not succes due to missing digit
            IdentityResult validationResult3 = passwordValidator.Validate("Alexander!");
            Assert.IsFalse(validationResult3.Succeeded);

            //should not succes due to missing uppercase
            IdentityResult validationResult4 = passwordValidator.Validate("alexander32!");
            Assert.IsFalse(validationResult4.Succeeded);

            //should not succes due to missing lowercase
            IdentityResult validationResult5 = passwordValidator.Validate("ALEXANDER32!");
            Assert.IsFalse(validationResult5.Succeeded);

            //should not succes due to length is not enough
            IdentityResult validationResult6 = passwordValidator.Validate("Alex2!");
            Assert.IsFalse(validationResult6.Succeeded);


        }
    }
}
