using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DesignPattern.Implement.ChainResponsibilityPattern;

namespace CSharpNote.Data.DesignPattern.Implement
{
    public class ChainResponsibilityPatternImplement : AbstractExecuteModule
    {
        /// <summary>
        ///     ���XTemplatePattern��hook �i�R���Ҳդ�HandlerClass
        /// </summary>
        [MarkedItem]
        public override void Execute()
        {
            var handler = new HandlerA();
            var handlerCommand = new HandlerCommand(typeof(HandlerD));
            handler.Execute(handlerCommand);
        }
    }
}