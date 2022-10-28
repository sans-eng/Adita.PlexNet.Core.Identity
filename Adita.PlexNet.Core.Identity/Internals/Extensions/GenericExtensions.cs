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

namespace Adita.Identity.Core.Internals.Extensions
{
    /// <summary>
    /// Provides generic <see cref="Type"/> extensions.
    /// </summary>
    internal static class GenericExtensions
    {
        /// <summary>
        /// Returns whether specified <paramref name="genericType"/> is a base class and a generic <see cref="Type"/> of <paramref name="type"/>.
        /// </summary>
        /// <param name="genericType">The generic <see cref="Type"/> to compare with <paramref name="type"/>.</param>
        /// <param name="type">The <see cref="Type"/> to check. </param>
        /// <returns>Whether specified <paramref name="genericType"/> is a base class and a generic <see cref="Type"/> of <paramref name="type"/>.</returns>
        public static bool IsGenericTypeOf(this Type genericType, Type type)
        {
            if (type.IsGenericType
                    && genericType == type.GetGenericTypeDefinition())
            {
                return true;
            }

            return type.BaseType != null
                    && IsGenericTypeOf(genericType, type.BaseType);
        }
    }
}