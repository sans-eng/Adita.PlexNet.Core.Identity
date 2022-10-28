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

namespace Adita.Identity.Core.Internals
{
    internal static class IdentityErrorCodes
    {
        internal const string ConcurrencyFailure = "ConcurrencyFailure";
        internal const string DefaultError = "DefaultError";
        internal const string DuplicateEmail = "DuplicateEmail";
        internal const string DuplicateRoleName = "DuplicateRoleName";
        internal const string DuplicateUserName = "DuplicateUserName";
        internal const string InvalidEmail = "InvalidEmail";
        internal const string InvalidRoleName = "InvalidRoleName";
        internal const string InvalidToken = "InvalidToken";
        internal const string InvalidUserName = "InvalidUserName";
        internal const string LoginAlreadyAssociated = "LoginAlreadyAssociated";
        internal const string PasswordMismatch = "PasswordMismatch";
        internal const string PasswordRequiresDigit = "PasswordRequiresDigit";
        internal const string PasswordRequiresLower = "PasswordRequiresLower";
        internal const string PasswordRequiresNonAlphanumeric = "PasswordRequiresNonAlphanumeric";
        internal const string PasswordRequiresUniqueChars = "PasswordRequiresUniqueChars";
        internal const string PasswordRequiresUpper = "PasswordRequiresUpper";
        internal const string PasswordTooShort = "PasswordTooShort";
        internal const string RecoveryCodeRedemptionFailed = "RecoveryCodeRedemptionFailed";
        internal const string UserAlreadyHasPassword = "UserAlreadyHasPassword";
        internal const string UserAlreadyInRole = "UserAlreadyInRole";
        internal const string UserLockoutNotEnabled = "UserLockoutNotEnabled";
        internal const string UserNotInRole = "UserNotInRole";
        internal const string UserLockedOut = "UserLockedOut";
        internal const string UserNotFound = "UserNotFound";
        internal const string RoleNotFound = "RoleNotFound";
        internal const string ClaimAlreadyAssociated = "ClaimAlreadyAssociated";
    }
}
