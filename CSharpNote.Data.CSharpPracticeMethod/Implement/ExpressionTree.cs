using System;
using System.Linq.Expressions;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.CSharpPractice.Implement
{
    public class ExpressionTree : AbstractExecuteModule
    {
        /// <summary>
        ///     1.Parameter
        ///     2.Body
        ///     3.LamdaExpression
        ///     4.Compile
        ///     5.Excute
        /// </summary>
        [AopTarget]
        public override void Execute()
        {
            //�Ѽ�
            var parameter1 = Expression.Parameter(typeof (int), "x");
            //�D��
            var multiply = Expression.Multiply(parameter1, parameter1);
            //LamdaExpression
            var square = Expression.Lambda<Func<int, int>>(
                multiply, parameter1);
            //Compile
            var lambda = square.Compile();
            //Excute
            Console.WriteLine(lambda(5));

            Expression<Func<int, int>> square1 = x => x*x;
            //�z�Lsquare1���oExpression Body�ìۥ[
            var squareplus2 = Expression.Add(square1.Body,
                Expression.Constant(3));
            //LamdaExpression
            var expr = Expression.Lambda<Func<int, int>>(squareplus2,
                square1.Parameters);
            //Compile
            var compile = expr.Compile();
            //Excute
            Console.WriteLine(compile(10));
        }
    }
}