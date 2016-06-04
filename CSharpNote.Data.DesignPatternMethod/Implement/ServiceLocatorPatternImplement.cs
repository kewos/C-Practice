using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extensions;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DesignPattern.Implement.ServiceLocatorPattern;

namespace CSharpNote.Data.DesignPattern.Implement
{
    public class ServiceLocatorPatternImplement : AbstractExecuteModule
    {
        /// <summary>
        ///     �����U�t����ɪ�Pattern
        ///     ���I:static���l�� ��unit test
        /// </summary>
        [MarkedItem]
        public override void Execute()
        {
            var serviceLocator = ServiceLocator.Instance;
            serviceLocator.RegistService<IService1>(new Service1());
            serviceLocator.RegistService<IService2>(new Service2());

            var service1 = ServiceLocator.Instance.GetService<IService1>();
            var service2 = ServiceLocator.Instance.GetService<IService2>();

            service1.GetName().ToConsole();
            service2.GetName().ToConsole();
        }
    }
}