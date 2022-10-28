using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Represents an identity options.
    /// </summary>
    public class IdentityOptions
    {
        /// <summary>
        /// Gets or sets an <see cref="ApplicationIdentityOptions"/> of current <see cref="IdentityOptions"/>.
        /// </summary>
        #region Public properties
        public ApplicationIdentityOptions ApplicationIdentityOptions { get; set; } = new();
        /// <summary>
        /// Gets or sets a <see cref="LockoutOptions"/> of current <see cref="IdentityOptions"/>.
        /// </summary>
        public LockoutOptions LockoutOptions { get; set; } = new();
        /// <summary>
        /// Gets or sets a <see cref="PasswordOptions"/> of current <see cref="IdentityOptions"/>.
        /// </summary>
        public PasswordOptions PasswordOptions { get; set; } = new();
        /// <summary>
        /// Gets or sets a <see cref="RepositoryOptions"/> of current <see cref="IdentityOptions"/>.
        /// </summary>
        public RepositoryOptions RepositoryOptions { get; set; } = new();
        /// <summary>
        /// Gets or sets a <see cref="RoleOptions"/> of current <see cref="IdentityOptions"/>.
        /// </summary>
        public RoleOptions RoleOptions { get; set; } = new();
        /// <summary>
        /// Gets or sets a <see cref="UserOptions"/> of current <see cref="IdentityOptions"/>.
        /// </summary>
        public UserOptions UserOptions { get; set; } = new();
        #endregion Public properties
    }
}
