using System;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.CSharpPractice.Implement
{
    public class StringToUnicode : AbstractExecuteModule
    {
        [AopTarget]
        public override void Execute()
        {
            const int number = 100;
            Console.WriteLine("�G�i��:{0}", Convert.ToString(number, 2));
            Console.WriteLine("�K�i��:{0}", Convert.ToString(number, 8));
            Console.WriteLine("�G�i��:{0}", Convert.ToString(number, 10));
            Console.WriteLine("�Q���i��:{0}", Convert.ToString(number, 16));

            Console.WriteLine("�Q���i��:{0}", number.ToString("X"));
            Console.WriteLine("����4 �Q���i��:{0}", number.ToString("X4"));
        }
    }
}