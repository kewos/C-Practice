using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DesignPattern.Implement.AbstractFactoryPattern;

namespace CSharpNote.Data.DesignPattern.Implement
{
    public class AbstractFactoryPatternImplement : AbstractExecuteModule
    {
        /// <summary>
        ///     ���ëإ߲Ӹ`��ֽ��X
        /// </summary>
        [AopTarget]
        public override void Execute()
        {
            var windowsFactory = new WindowsFactory();
            var unixFactory = new UnixFactory();

            windowsFactory.CreateProductA().Display();
            windowsFactory.CreateProductB().Display();

            unixFactory.CreateProductA().Display();
            unixFactory.CreateProductB().Display();
        }
    }
}