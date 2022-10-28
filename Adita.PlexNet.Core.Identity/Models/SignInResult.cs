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
    /// Determines the sign-in result of a user.
    /// </summary>
    public enum SignInResult
    {
        /// <summary>
        /// Indicates that the sign-in result is succeeded.
        /// </summary>
        Succeeded,
        /// <summary>
        /// Indicates that the sign-in result is failed.
        /// </summary>
        Failed,
        /// <summary>
        /// Indicates that the sign-in result has invalid credential.
        /// </summary>
        InvalidCredential,
        /// <summary>
        /// Indicates that the user who are trying to sign-in is locked out.
        /// </summary>
        LockedOut,
        /// <summary>
        /// Indicates that the user who are trying to sign-in is not allowed to sign-in.
        /// </summary>
        NotAllowed
    }
}
