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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adita.PlexNet.Core.Identity
{
    /// <summary>
    /// Options for configuring user lockout.
    /// </summary>
    public class LockoutOptions
    {
        /// <summary>
        /// Gets or sets a flag indicating whether a new user can be locked out. Defaults to <see langword="true"/>.
        /// </summary>
        public bool AllowedForNewUsers { get; set; } = true;
        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> a user is locked out for when a lockout occurs. Defaults to 30 seconds.
        /// </summary>
        public TimeSpan DefaultLockoutTimeSpan { get; set; } = TimeSpan.FromSeconds(30);
        /// <summary>
        /// Gets or sets the number of failed access attempts allowed before a user is locked out, assuming lock out is enabled. Defaults to 5.
        /// </summary>
        public int MaxFailedAccessAttempts { get; set; } = 5;
    }
}
