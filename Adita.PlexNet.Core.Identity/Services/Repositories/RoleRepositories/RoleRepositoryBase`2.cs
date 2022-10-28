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
    /// Represents a base class for role persistence repository.
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key of a role.</typeparam>
    /// <typeparam name="TRole">The type for the role.</typeparam>
    public abstract class RoleRepositoryBase<TKey, TRole> :
        IRoleRepository<TKey, TRole>
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>, new()
    {
        #region Private fields
        private bool _disposed;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="RoleRepositoryBase{TKey, TRole}"/> using
        /// specified <paramref name="errorDescriber"/>.
        /// </summary>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber"/>
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="errorDescriber"/> is <c>null</c></exception>
        protected RoleRepositoryBase(IdentityErrorDescriber errorDescriber)
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
        public abstract IQueryable<TRole> Roles { get; }
        #endregion Public properties

        #region Protected properties
        /// <summary>
        /// Gets an <see cref="IdentityErrorDescriber"/> of current <see cref="RoleRepositoryBase{TKey, TRole}"/>.
        /// </summary>
        protected IdentityErrorDescriber ErrorDescriber { get; }
        #endregion Protected properties

        #region Public methods
        /// <inheritdoc/>
        public abstract Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<TRole?> FindByIdAsync(TKey roleId, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<TRole?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the ID for a role from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}" /> that contains the ID of the role.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        public Task<TKey> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Id);
        }
        /// <summary>
        /// Gets the name for a role from the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}" /> that contains the name of the role.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name);
        }
        /// <summary>
        /// Sets the name of a role in the repository as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="role"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="roleName"/> is <c>null</c> or only contains white spaces.</exception>
        /// <exception cref="ObjectDisposedException">Repository is disposed.</exception>
        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException($"'{nameof(roleName)}' cannot be null or whitespace.", nameof(roleName));
            }

            role.Name = roleName;

            return Task.CompletedTask;
        }
        /// <inheritdoc/>
        public abstract Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default);
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
        /// Dispose current <see cref="RoleRepositoryBase{TKey, TRole}"/> also dispose
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
