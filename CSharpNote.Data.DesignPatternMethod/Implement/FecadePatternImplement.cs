using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DesignPattern.Implement.FecadePattern;

namespace CSharpNote.Data.DesignPattern.Implement
{
    public class FecadePatternImplement : AbstractExecuteModule
    {
        /// <summary>
        ///     FecadePattern �Ω����äl�t�Ϊ��Ӹ`
        ///     ���l�t�η|��Fecade�y�����X
        /// </summary>
        [MarkedItem]
        public override void Execute()
        {
            var fecade = new Fecade();
            fecade.MethodA().ToConsole();
            fecade.MethodB().ToConsole();
            fecade.MethodC().ToConsole();
        }
    }
}