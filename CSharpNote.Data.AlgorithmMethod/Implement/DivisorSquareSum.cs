using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.Algorithm.Implement
{
    /// Problem 211
    /// For a positive integer n, let �m2(n) be the sum of the squares of its divisors. For example,
    /// �m2(10) = 1 + 4 + 25 + 100 = 130.
    /// Find the sum of all n, 0
    /// < n
    /// < 64,000,000 such that �m2( n) is a perfect square.
    public class DivisorSquareSum : AbstractExecuteModule
    {
        [AopTarget]
        public override void Execute()
        {
            var obj = new object();
            var sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(Enumerable.Range(1, 64000), number =>
            {
                lock (obj)
                {
                    var factors = number.Factor();
                    var sum = GetSequenceSquareSum(factors);
                    if (CheckPerfectSquare(sum))
                        Console.WriteLine(number);
                }
            });

            sw.Stop();
            Console.WriteLine("SpentTime:{0}ms", sw.ElapsedMilliseconds);
        }

        /// <summary>
        ///     �P�_�O�_�����������
        /// </summary>
        /// <param name="numbers">�ݧP�_�Ʀr</param>
        /// <returns></returns>
        private bool CheckPerfectSquare(double number)
        {
            return (Math.Sqrt(number)%1) == 0;
        }

        /// <summary>
        ///     ���o�����ƭȪ�����[�`
        /// </summary>
        /// <param name="numbers">�ݥ���[�`�ƦC</param>
        /// <returns>����[�`</returns>
        private double GetSequenceSquareSum(List<int> numbers)
        {
            var obj = new object();
            double sum = 0;

            Parallel.ForEach(numbers, number =>
            {
                lock (obj)
                {
                    sum += Math.Pow(number, 2);
                }
            });

            return sum;
        }
    }
}