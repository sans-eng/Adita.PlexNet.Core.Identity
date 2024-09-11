# Adita.PlexNet.Core.Identity

This repo contains:
- `Adita.PlexNet.Core.Identity`
  <br>A core Identity library for PlexNet framework that targeting .NET 6
- `Adita.PlexNet.Core.Identity.EntityFrameworkCore`
  <br>An implementation of `Adita.PlexNet.Core.Identity` for `Microsoft.EntityFrameworkCore`
- `Adita.PlexNet.Core.Identity.Extensions`
  <br>Extensions for add identity to `Microsoft.Extensions.DependencyInjection.IServiceCollection`

![Passing](https://github.com/sans-eng/Adita.PlexNet.Core.Identity/actions/workflows/main.yml/badge.svg?branch=master)

## How to use

### Add the dependencies
You need to add the dependencies to your project through the *nuget package manager*.
- `Adita.PlexNet.Core.Identity`
- `Adita.PlexNet.Core.Identity.EntityFrameworkCore`
- `Adita.PlexNet.Core.Identity.Extensions`
- `Adita.PlexNet.Core.Security`

### Create the Database Context
You can create the *Database Context* in several ways.
- **Default Identity model**<br>
  If you want just using the default Identity that served by the library you can create the *Database Context* by deriving it from `Adita.PlexNet.Core.Identity.EntityFrameworkCore.DefaultIdentityDbContext` type.
  A default identity is using `Adita.PlexNet.Core.Identity.IdentityUser`, `Adita.PlexNet.Core.Identity.IdentityUserClaim`, `Adita.PlexNet.Core.Identity.IdentityUserRole`, `Adita.PlexNet.Core.Identity.IdentityRole`
  and `Adita.PlexNet.Core.Identity.IdentityRoleClaim`.<br>
  e.g.:
  ```
    public class AppIdentityDbContext : Adita.PlexNet.Core.Identity.EntityFrameworkCore.DefaultIdentityDbContext
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }
    }
  ```
- **Custom Identity model**<br>
  To use custom identity model you need to implements your own `Adita.PlexNet.Core.Identity.IdentityUser<T>`, `Adita.PlexNet.Core.Identity.IdentityUserClaim<T>`, `Adita.PlexNet.Core.Identity.IdentityUserRole<T>`, `Adita.PlexNet.Core.Identity.IdentityRole<T>`
  and `Adita.PlexNet.Core.Identity.IdentityRoleClaim<T>`, than you can use each of the models in your *Database Context* that deriving from `Adita.PlexNet.Core.Identity.EntityFrameworkCore.IdentityDbContext<TKey, TUser, TUserClaim, TUserRole, TRole, TRoleClaim>` where the The `TKey` is your own key type.<br>
  e.g.:
  ```
    public class AppIdentityDbContext : Adita.PlexNet.Core.Identity.EntityFrameworkCore.IdentityDbContext<string, AppIdentityUser, AppIdentityUserClaim, AppIdentityUserRole, AppIdentityRole, AppIdentityRoleClaim>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }
    }
  ```

### Add the Database Context to your service container
The examples is using Custom `AppIdentityDbContext` that derived from `Adita.PlexNet.Core.Identity.EntityFrameworkCore.DefaultIdentityDbContext`.<br>
e.g. using in memory:
```
services.AddDbContext<AppIdentityDbContext>(contextOptions =>
{
    contextOptions.UseInMemoryDatabase("IdentityTest")
    .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
});
```

e.g. using in sqlite:
```
services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlite(infrastructureOptions.IdentityDbConnectionString));
```

  > [!IMPORTANT]
  > Don't forget to migrate your database or just use `EnsureCreated()` based on your requirements,<br>
  > you can find more information about managing database schemas [in here](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli) and [in here](https://learn.microsoft.com/en-us/ef/core/managing-schemas/ensure-created)

### Add the identity APIs
Add the Identity using the extensions that contains on `Adita.PlexNet.Core.Extensions.Identity.IdentityServiceCollectionExtensions`<br>
The options is depends on your requirment,<br>
e.g. using default identity:
```
services.AddDefaultIdentity<DefaultIdentityDbContext>(options =>
{
    options.PasswordOptions.RequireNonAlphanumeric = false;
    options.PasswordOptions.RequiredLength = 4;
    options.PasswordOptions.RequireDigit = false;
    options.PasswordOptions.RequireUppercase = false;
    options.PasswordOptions.RequiredLowercase = false;

    options.LockoutOptions.AllowedForNewUsers = false;
    options.LockoutOptions.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(5);
});
```

e.g. using custom Identity models:
```
services.AddIdentity<string, AppIdentityUser, AppIdentityUserClaim, AppIdentityUserRole, AppIdentityRole, AppIdentityRoleClaim, AppIdentityDbContext>(options =>
{
    options.PasswordOptions.RequireNonAlphanumeric = false;
    options.PasswordOptions.RequiredLength = 4;
    options.PasswordOptions.RequireDigit = false;
    options.PasswordOptions.RequireUppercase = false;
    options.PasswordOptions.RequiredLowercase = false;
 
    options.LockoutOptions.AllowedForNewUsers = false;
    options.LockoutOptions.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(5);
});
```

### Consuming the APIs
#### Primary APIs
- **Sign in manager**<br>
  The sign in manager is registered to the dependency container as `Adita.PlexNet.Core.Identity.ISignInManager<TUser>`, it means you can only get the instance by specifying your `TUser`.<br>
  Sign in the user:
  ```
  private readonly ISignInManager<IdentityUser> _signInManager;
  ```
  ```
  var result = await _signInManager.PasswordSignInAsync("UserName", "Pasword");
  ```
  Logout the user:
  ```
  await _signInManager.SignOutAsync();
  ```

> [!NOTE]
> The signed in user will be registered as `Adita.PlexNet.Core.Security.Claims.ApplicationIdentity` in `System.Threading.Thread.CurrentPrincipal` which the actual `System.Threading.Thread.CurrentPrincipal` will be a `Adita.PlexNet.Core.Security.Principals.ApplicationPrincipal`, then you can easily access the current user, claims, roles etc. there.<br>
> You can learn more about role-based security [here](https://learn.microsoft.com/en-us/dotnet/standard/security/role-based-security).

- **User manager**<br>
  The user manager is registered to the dependency container as `Adita.PlexNet.Core.Identity.IUserManager<TKey, TUser, TRole>`, it means you can only get the instance by specifying your `TKey`, `TUser` and `TUser`.<br>
  You can manage the users such as create user, delete user, reset password, add to roles, add claims etc. here.

- **Role manager**<br>
  The role manager is registered to the dependency container as `Adita.PlexNet.Core.Identity.IRoleManager<TKey, TRole>`, it means you can only get the instance by specifying your `TKey` and `TRole`.<br>
  You can manage the roles such as create, delete, update, checking specific role exist etc. here.