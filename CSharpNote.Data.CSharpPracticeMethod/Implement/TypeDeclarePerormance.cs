using System;
using System.Linq;
using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.CSharpPractice.Implement
{
    public class TypeDeclarePerormance : AbstractExecuteModule
    {
        /// <summary>
        ///     �����w���O�֤@�I�I
        /// </summary>
        [AopTarget]
        public override void Execute()
        {
            var times = 10000000;
            var executeTimes = Enumerable.Range(0, times);

            Action typeVar = () => { executeTimes.ForEach(n => { var a = n; }); };

            Action typeDeclare = () => { executeTimes.ForEach(n => { var a = n; }); };

            typeVar.CaculateExcuteTime().ToConsole("typeVar:");
            typeDeclare.CaculateExcuteTime().ToConsole("typeDeclare:");
        }
    }
}