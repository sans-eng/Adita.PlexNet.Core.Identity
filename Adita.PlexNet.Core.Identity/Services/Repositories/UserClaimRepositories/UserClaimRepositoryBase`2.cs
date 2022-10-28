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
    /// Provides an abstraction for a repository of claims for a user.
    /// </summary>
    /// <typeparam name="TKey">A type used for the primary key of a user claim.</typeparam>
    /// <typeparam name="TUserClaim">A type for a user claim.</typeparam>
    public abstract class UserClaimRepositoryBase<TKey, TUserClaim> :
        IUserClaimRepository<TKey, TUserClaim>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
    {
        #region Private fields
        private bool _disposed;
        #endregion Private fields

        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="UserClaimRepositoryBase{TKey, TUserClaim}"/> using specified
        /// <paramref name="errorDescriber"/>.
        /// </summary>
        /// <param name="errorDescriber">An <see cref="IdentityErrorDescriber"/>
        /// to get localized error strings from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="errorDescriber"/> is <c>null</c></exception>
        protected UserClaimRepositoryBase(IdentityErrorDescriber errorDescriber)
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
        /// <summary>
        /// Returns an <see cref="IQueryable{T}" /> collection of user claim.
        /// </summary>
        public abstract IQueryable<TUserClaim> UserClaims { get; }
        #endregion Public properties

        #region Protected properties
        /// <summary>
        /// Gets an <see cref="IdentityErrorDescriber"/> of current <see cref="UserClaimRepositoryBase{TKey, TUser}"/>.
        /// </summary>
        protected IdentityErrorDescriber ErrorDescriber { get; }
        #endregion Protected properties

        #region Public methods
        /// <inheritdoc/>
        public abstract Task<IdentityResult> CreateAsync(TUserClaim userClaim, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<IdentityResult> DeleteAsync(TUserClaim userClaim, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<IList<TUserClaim>> FindAsync(Func<TUserClaim, bool> predicate, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<TUserClaim?> FindAsync(TKey id, CancellationToken cancellationToken = default);
        /// <inheritdoc/>
        public abstract Task<IdentityResult> UpdateAsync(TUserClaim userClaim, CancellationToken cancellationToken = default);
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
        /// Dispose current <see cref="UserClaimRepositoryBase{TKey, TUserClaim}"/> also dispose
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
