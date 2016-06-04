using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.CSharpPractice.Implement
{
    public class OperateDynamicObject : AbstractExecuteModule
    {
        /// <summary>
        ///     �ާ@�ʺA����
        /// </summary>
        [AopTarget]
        public override void Execute()
        {
            dynamic dObject = new ExpandoObject();
            //add Property Changed Event
            ((INotifyPropertyChanged) dObject).PropertyChanged +=
                (s, e) =>
                    Console.WriteLine("Property Changed{0}:", e.PropertyName);
            //�W�[Property
            dObject.Name = "DynamicObject";
            Console.WriteLine(dObject.Name);

            //�z�Ldictionary �W�[Property
            var dicObject = dObject as IDictionary<string, object>;
            dicObject["hello"] = (Action<string>) (msg => Console.WriteLine(msg));
            dObject.hello("hello world");

            //����Property
            foreach (var property in dObject as IDictionary<string, object>)
            {
                Console.WriteLine("key:{0} value:{1}", property.Key, property.Value);
            }
        }
    }
}