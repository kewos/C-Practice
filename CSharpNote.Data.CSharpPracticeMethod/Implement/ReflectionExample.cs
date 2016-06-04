using System;
using System.Diagnostics;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.CSharpPractice.Implement
{
    public class ReflectionExample : AbstractExecuteModule
    {
        /// <summary>
        ///     type ���o�覡
        ///     1.��instance �z�Linstance GetType()
        ///     2.�w��type �z�Ltypeof() ���otype
        ///     3.�w��namespace �z�Ldll���otype
        /// </summary>
        [AopTarget]
        public override void Execute()
        {
            Action<Caculator> action = caculator =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                caculator.Add(10, 10);
                stopwatch.Stop();
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            };

            var type = typeof (Caculator);

            var caculator1 = Activator.CreateInstance(type) as Caculator;
            action.Invoke(caculator1);

            dynamic caculator2 = Activator.CreateInstance(type);
            action.Invoke(caculator2);
        }

        private class Caculator
        {
            public int Add(int i, int j)
            {
                return i + j;
            }
        }
    }
}