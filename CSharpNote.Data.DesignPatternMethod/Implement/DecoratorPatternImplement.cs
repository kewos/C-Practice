using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DesignPattern.Implement.DecoratorPattern;

namespace CSharpNote.Data.DesignPattern.Implement
{
    public class DecoratorPatternImplement : AbstractExecuteModule
    {
        /// <summary>
        ///     �ۭ���ʺA�X�R��k�\��
        /// </summary>
        [MarkedItem]
        public override void Execute()
        {
            new DecoratorB(new DecoratorA(new ConcreteComponentA())).Operation().ToConsole();
        }
    }
}