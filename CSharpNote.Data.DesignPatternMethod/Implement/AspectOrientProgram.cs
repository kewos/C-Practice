using System;
using System.Linq;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DesignPattern.Implement.Aop;

namespace CSharpNote.Data.DesignPattern.Implement
{
    public class AspectOrientProgram : AbstractExecuteModule
    {
        /// <summary>
        ///     �[�`�I���J���]�p�覡
        ///     �`�Ω�input validation, log���\��
        ///     �i���{����ŦX��@�d�h���]�p��h
        /// </summary>
        [MarkedItem]
        public override void Execute()
        {
            AOP.Registry.Join(
                typeof(Actor).GetConstructors().First(),
                typeof(Concern).GetConstructors().First()
                );
            var actor = (IActor)AOP.Factory.Create<Actor>("");

            Console.WriteLine(actor.Name);
            Console.WriteLine(new Actor("adabcbc").Name);
        }
    }
}