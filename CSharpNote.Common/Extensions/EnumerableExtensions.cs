﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSharpNote.Common.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Foreach迴圈
        /// </summary>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> elements, Func<T, T> func)
        {
            func.ValidationNotNull();

            return elements.Select(func);
        }

        /// <summary>
        ///     Foreach迴圈
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> elements, Action<T> action)
        {
            action.ValidationNotNull();

            foreach (var element in elements)
            {
                action(element);
            }
        }

        /// <summary>
        ///     有引數的Foreach迴圈
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> elements, Action<int, T> action)
        {
            action.ValidationNotNull();

            var index = 0;
            foreach (var element in elements)
            {
                action(index++, element);
            }
        }

        /// <summary>
        ///     全部為真則真 反之為假
        /// </summary>
        public static bool All<T>(this IEnumerable<T> elements, Func<int, T, bool> func)
        {
            func.ValidationNotNull();

            var index = 0;
            return elements.All(element => func(index++, element));
        }

        /// <summary>
        ///     顯示容量內容於畫面上
        /// </summary>
        public static void Dump<T>(this IEnumerable<T> elements, int index = 0)
        {
            elements.ForEach(element => Console.WriteLine("{0}.{1}", index++, element));
        }

        /// <summary>
        ///     顯示容量內容於畫面上
        /// </summary>
        public static IEnumerable DumpMany(this IEnumerable enumerable, int dumpLevel = 0)
        {
            var index = 0;
            var dumpMany = enumerable as IList<object> ?? enumerable.Cast<object>().ToList();
            foreach (var element in dumpMany)
            {
                Console.WriteLine("{0}{1}.{2}", new string('-', dumpLevel*3), index++, element);
                if (element is IEnumerable)
                {
                    (element as IEnumerable).DumpMany(dumpLevel + 1);
                }
            }
            return dumpMany;
        }

        /// <summary>
        ///     隨機排列
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> elements)
        {
            var random = new Random();

            return elements.OrderBy(element => random.Next()%100);
        }

        /// <summary>
        ///     排列組合
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            // Ensure that the source IEnumerable is evaluated only once
            return permutations(source.ToArray());
        }

        private static IEnumerable<IEnumerable<T>> permutations<T>(IEnumerable<T> source)
        {
            var c = source.Count();
            if (c == 1)
            {
                yield return source;
            }
            for (var i = 0; i < c; i++)
            {
                foreach (var p in permutations(source.Take(i).Concat(source.Skip(i + 1))))
                {
                    yield return source.Skip(i).Take(1).Concat(p);
                }
            }
        }

        /// <summary>
        ///     裝飾者集合會逐一裝飾目標
        /// </summary>
        public static T Decorate<T>(this IEnumerable<Func<T, T>> decorators, T target)
        {
            decorators.ValidationNotEmptyAndNull();
            target.ValidationNotNull();

            return decorators.Aggregate(target, (current, decorator) => decorator(current));
        }

        /// <summary>
        ///     轉成HashSet
        /// </summary>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            source.ValidationNotEmptyAndNull();

            return new HashSet<T>(source);
        }

        /// <summary>
        ///     交叉運算
        /// </summary>
        public static IEnumerable<T> CrossCaculate<T>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, T, T> func)
        {
            left.ValidationNotEmptyAndNull();
            right.ValidationNotEmptyAndNull();
            func.ValidationNotNull();

            return left.SelectMany(x => right.Select(y => func(x, y)));
        }
    }
}