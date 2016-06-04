using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DesignPattern.Implement.ProxyPattern;

namespace CSharpNote.Data.DesignPattern.Implement
{
    public class ProxyPatternImplement : AbstractExecuteModule
    {
        /// <summary>
        ///     ProxyServer�@�}�l�N���w���V����RealServer
        /// </summary>
        [AopTarget]
        public override void Execute()
        {
            new ProxyServer().DoAction().ToConsole();
        }
    }
}