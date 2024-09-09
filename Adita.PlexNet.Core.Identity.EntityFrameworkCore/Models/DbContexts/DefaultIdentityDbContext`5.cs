//MIT License

//Copyright (c) 2024 Adita

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using Microsoft.EntityFrameworkCore;

namespace Adita.PlexNet.Core.Identity.EntityFrameworkCore
{
    /// <summary>
    /// Default class for the Entity Framework database context used for identity which uses 
    /// <see cref="IdentityUser"/>, <see cref="IdentityUserClaim"/>, <see cref="IdentityUserRole"/>, <see cref="IdentityRole"/> and <see cref="IdentityRoleClaim"/>
    /// for the <see cref="DbSet{TEntity}"/>s.
    /// </summary>
    public class DefaultIdentityDbContext : IdentityDbContext<Guid, IdentityUser, IdentityUserClaim, IdentityUserRole, IdentityRole, IdentityRoleClaim>
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="DefaultIdentityDbContext"/>.
        /// </summary>
        public DefaultIdentityDbContext()
        {
        }
        /// <summary>
        /// Initialize a new instance of <see cref="DefaultIdentityDbContext"/>
        /// using specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">A <see cref="DbContextOptions"/> The options to be used by a <see cref="DbContext"/>.</param>
        public DefaultIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
        #endregion Constructors
    }
}
