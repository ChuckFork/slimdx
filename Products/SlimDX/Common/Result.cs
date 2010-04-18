﻿/* Copyright (c) 2007-2010 SlimDX Group
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;

namespace SlimDX {
    /// <summary>
    /// Contains information about the result of an operation.
    /// </summary>
    public struct Result {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> struct.
        /// </summary>
        /// <param name="code">The code.</param>
        public Result( int code )
            : this() {
            Code = code;
        }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        public int Code {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this Result instance represents success.
        /// </summary>
        public bool IsSuccess {
            get {
                // This is equivalent to the native SUCCEEDED macro.
                return Code >= 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this Result instance represents failure.
        /// </summary>
        public bool IsFailure {
            get {
                // This is equivalent to the native FAILED macro.
                return Code < 0;
            }
        }

        /// <summary>
        /// Gets the last recorded Result instance for the currently executing thread.
        /// </summary>
        public static Result Last {
            get {
                return last;
            }
        }

        public static void SetLast( Result result ) {
            last = result;
        }

        [ThreadStatic]
        static Result last;
    }
}