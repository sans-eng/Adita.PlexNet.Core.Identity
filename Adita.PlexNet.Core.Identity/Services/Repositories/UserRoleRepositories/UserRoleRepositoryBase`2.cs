//MIT License

//Copyright (c) 2022 Adita

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

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Provides an abstraction for a repository of roles for a user.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a user role.</typeparam>
    /// <typeparam name="TUserRole">A type for a user role.</typeparam>
    public abstract class UserRoleRepositoryBase<TKey, TUserRole> :
        IUserRoleRepository<TKey, TUserRole>
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>, new()
    {
        #region Private fields
        private bool _disposed;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="UserRoleRepositoryBase{TKey, TUserRole}"/> using
        /// specified <paramref name="errorDescriber"/>.
        /// </summary>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber"/>
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="errorDescriber"/> is <c>null</c></exception>
        protected UserRoleRepositoryBase(IdentityErrorDescriber errorDescriber)
        {
            ErrorDescriber = errorDescriber ?? throw new ArgumentNullException(nameof(errorDescriber));
        }
        #endregion Constructors

        #region Destructors
        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UserRoleRepositoryBase()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
        #endregion Destructors

        #region Public properties
        /// <inheritdoc/>
        public abstract IQueryable<TUserRole> UserRoles { get; }
        #endregion Public properties

        #region Protected properties
        /// <summary>
        /// Gets an <see cref="IdentityErrorDescriber"/> of current <see cref="UserRoleRepositoryBase{TKey, TUserRole}"/>.
        /// </summary>
        protected IdentityErrorDescriber ErrorDescriber { get; }
        #endregion Protected properties

        #region Public methods
        /// <inheritdoc/>
        public abstract Task<IdentityResult> CreateAsync(TUserRole userRole, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<IdentityResult> DeleteAsync(TUserRole userRole, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<TUserRole?> FindAsync(TKey id, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<TUserRole?> FindAsync(TKey userId, TKey roleId, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<IList<TUserRole>> FindAsync(Func<TUserRole, bool> predicate, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<IdentityResult> UpdateAsync(TUserRole userRole, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Public methods

        #region Protected methods
        /// <summary>
        /// Dispose current <see cref="UserRoleRepositoryBase{TKey, TUserRole}"/> also dispose
        /// any managed objects identified by <paramref name="disposing"/>.
        /// </summary>
        /// <param name="disposing"><c>true</c> if need to dispose any managed objects.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposed = true;
            }
        }

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        #endregion Protected methods
    }
}
