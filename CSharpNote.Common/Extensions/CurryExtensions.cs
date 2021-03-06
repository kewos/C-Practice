﻿using System;

namespace CSharpNote.Common.Extensions
{
    public static class CurryExtensions
    {
        /// <summary>
        ///     f()
        /// </summary>
        public static Func<TInput1, TOutput>
            Curry<TInput1, TOutput>
            (this Func<TInput1, TOutput> f)
        {
            return x => f(x);
        }

        /// <summary>
        ///     f()()
        /// </summary>
        public static Func<TInput1, Func<TInput2, TOutput>>
            Curry<TInput1, TInput2, TOutput>
            (this Func<TInput1, TInput2, TOutput> f)
        {
            return w => x => f(w, x);
        }

        /// <summary>
        ///     f()()()
        /// </summary>
        public static Func<TInput1, Func<TInput2, Func<TInput3, TOutput>>>
            Curry<TInput1, TInput2, TInput3, TOutput>
            (this Func<TInput1, TInput2, TInput3, TOutput> f)
        {
            return w => x => y => f(w, x, y);
        }

        /// <summary>
        ///     f()()()()
        /// </summary>
        public static Func<TInput1, Func<TInput2, Func<TInput3, Func<TInput4, TOutput>>>>
            Curry<TInput1, TInput2, TInput3, TInput4, TOutput>
            (this Func<TInput1, TInput2, TInput3, TInput4, TOutput> f)
        {
            return w => x => y => z => f(w, x, y, z);
        }
    }
}