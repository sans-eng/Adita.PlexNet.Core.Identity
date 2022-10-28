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
    /// Represents the result of an identity operation.
    /// </summary>
    public class IdentityResult
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of <see cref="IdentityResult"/>.
        /// </summary>
        protected internal IdentityResult() { }
        /// <summary>
        /// Initialze a new instance of <see cref="IdentityResult"/> identified by whether is <paramref name="succeeded"/> and an
        /// optional <paramref name="errors"/> if not <paramref name="succeeded"/>.
        /// </summary>
        /// <param name="succeeded">Whether <see cref="IdentityResult"/> is succeeded or not.</param>
        /// <param name="errors">An optional <see cref="IEnumerable{T}"/> of <see cref="IdentityError"/> if not <paramref name="succeeded"/>.</param>
        public IdentityResult(bool succeeded, IEnumerable<IdentityError>? errors = null)
        {
            Errors = errors;
            Succeeded = succeeded;
        }
        /// <summary>
        /// Initialze a new instance of <see cref="IdentityResult"/>.
        /// </summary>
        /// <param name="succeeded">Whether <see cref="IdentityResult"/> is succeeded or not.</param>
        /// <param name="identityError">An <see cref="IdentityError"/> if not <paramref name="succeeded"/>.</param>
        public IdentityResult(bool succeeded, IdentityError identityError) :
            this(succeeded, new List<IdentityError>() { identityError })
        {

        }
        #endregion Constructors

        #region Public properties
        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="IdentityError"/> instances containing errors that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="IdentityError"/> instances.</value>
        public IEnumerable<IdentityError>? Errors { get; }
        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value><c>True</c> if the operation succeeded, otherwise <c>false</c>.</value>
        public bool Succeeded { get; protected set; }
        /// <summary>
        /// Returns an <see cref="IdentityResult"/> indicating a successful identity operation.
        /// </summary>
        /// <value>An <see cref="IdentityResult"/> indicating a successful operation.</value>
        public static IdentityResult Success
        {
            get { return new IdentityResult() { Succeeded = true }; }
        }
        #endregion Public properties

        #region Public methods
        /// <summary>
        /// Creates an <see cref="IdentityResult"/> indicating a failed identity operation, with a list of errors if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="IdentityError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating a failed identity operation, with a list of errors if applicable.</returns>
        public static IdentityResult Failed(params IdentityError[] errors)
        {
            if (errors?.Length > 0)
            {
                return new IdentityResult(false, errors);
            }

            return new IdentityResult(false);
        }
        /// <summary>
        /// Converts the value of the current <see cref="IdentityResult"/> object to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <returns>A <see cref="string"/> representation of the current <see cref="IdentityResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the <see cref="ToString"/> will 
        /// return "Succeeded" otherwise it returned 
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            if (Succeeded)
            {
                return "Succeeded";
            }
            else
            {
                string errorString = "Failed: ";

                if (Errors?.Any() == true)
                {
                    foreach (var error in Errors)
                    {
                        errorString += $"{error.Code};";
                    }
                }

                return errorString;
            }
        }
        #endregion Public methods
    }
}
