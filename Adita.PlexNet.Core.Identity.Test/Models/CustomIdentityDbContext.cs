using Adita.PlexNet.Core.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adita.PlexNet.Core.Identity.Test.Models
{
    public class CustomIdentityDbContext : IdentityDbContext<string>
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityDbContext{TKey}"/>.
        /// </summary>
        public CustomIdentityDbContext()
        {

        }
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityDbContext{TKey}"/>
        /// using specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">A <see cref="DbContextOptions"/> The options to be used by a <see cref="DbContext"/>.</param>
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options) : base(options)
        {

        }
        #endregion Constructors
    }
}
