using System.Linq;
using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DesignPattern.Implement.DependecyContainer;

namespace CSharpNote.Data.DesignPattern.Implement
{
    public class DependencyInjectionPatternImplement : AbstractExecuteModule
    {
        /// <summary>
        ///     �j�j��Pattern ��{����l�ɫإ�BootStraper
        ///     �����󳣨̿�DependencyInjectorContainer
        ///     ���᪫�󪺳]�p���Ψ̿�`�J���覡��ֽ��X��
        ///     �åB�̿�`�J�����󳣥i�H�ɥ��~��Interface���覡�Ӳ��Ͱ�����
        ///     �F��i�������{���X���ؼ�
        /// </summary>
        [AopTarget]
        public override void Execute()
        {
            var container = DependecyConainer.Instance;

            //���U����
            container.RegistType<IInstanceA, InstanceA>();
            container.RegistSingleton<IInstanceB, InstanceB>();
            container.RegistType<IDependencyInjectorA, DependencyInjectorA>();
            container.RegistType<IDependencyInjectorB, DependencyInjectorB>();

            "==============================================>InstanceA".ToConsole();
            Enumerable.Range(0, 5).ForEach(n => container.Resolve<IInstanceA>().Do());

            "==============================================>InstanceB".ToConsole();
            Enumerable.Range(0, 5).ForEach(n => container.Resolve<IInstanceB>().Do());
        }
    }
}