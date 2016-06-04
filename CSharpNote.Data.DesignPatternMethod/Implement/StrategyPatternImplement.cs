using System;
using System.Linq;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DesignPattern.Implement.StretagyPattern;

namespace CSharpNote.Data.DesignPattern.Implement
{
    public class StrategyPatternImplement : AbstractExecuteModule
    {
        /// <summary>
        ///     �ھڤ��P���A���ܰ��浦��
        /// </summary>
        [MarkedItem]
        public override void Execute()
        {
            var strategyA = StrategyFactory.Create<StrategyA>();
            var strategyB = StrategyFactory.Create<StrategyB>();

            foreach (var status in Enumerable.Range(1, 3).Select(index => string.Format("Strategy00{0}", index)))
            {
                Console.WriteLine(strategyA[status].Invoke());
                Console.WriteLine(strategyB[status].Invoke());
            }
        }
    }
}