using System;
using System.Text;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.CSharpPractice.Implement
{
    public class ExplicitImplicitMethod : AbstractExecuteModule
    {
        [AopTarget]
        public override void Execute()
        {
            //���t�ഫ
            TypeConvert obj1 = "test1";
            Console.WriteLine(obj1.ToString());

            //���T�ഫ
            var sb = new StringBuilder("test2");
            var obj2 = (TypeConvert) sb;

            //���t�ഫ
            if (obj2)
            {
                Console.WriteLine(obj2.ToString());
            }

            Console.Read();
        }

        public class TypeConvert
        {
            private readonly string name;
            //�N�غc�l�]���p���A�N��L�k��new����rnew�XA���O
            private TypeConvert(string name)
            {
                this.name = "ExampleObj:" + name;
            }

            //���t�ഫ
            public static implicit operator TypeConvert(string expandedName)
            {
                return new TypeConvert(expandedName);
            }

            public static implicit operator bool(TypeConvert obj)
            {
                return true;
            }

            //���T�ഫ
            public static explicit operator TypeConvert(StringBuilder expandedName)
            {
                return new TypeConvert(expandedName.ToString());
            }

            public override string ToString()
            {
                return name;
            }
        }
    }
}