﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNote.Data.DesignPatternMethod.SubClass.Aop
{
    public sealed class Interceptor : RealProxy, System.Runtime.Remoting.IRemotingTypeInfo
    {
        object Target { get; set; }
        object[] Concerns { get; set; }
        JoinPoint[] Joinpoints { get; set; }

        internal Interceptor(object target, object[] concerns, JoinPoint[] joinpoints)
            : base(typeof(MarshalByRefObject))
        {
            Target = target;
            Concerns = concerns;
            Joinpoints = joinpoints;
        }

        public string TypeName { get; set; }

        public bool CanCastTo(Type fromType, object o) { return true; }

        //有使用任何方法都會呼叫invoke
        public override System.Runtime.Remoting.Messaging.IMessage Invoke(System.Runtime.Remoting.Messaging.IMessage msg)
        {
            object returnValue = null;
            IMethodCallMessage methodMessage = (IMethodCallMessage)msg;
            MethodBase method = methodMessage.MethodBase;

            var concernMethod = Joinpoints
                .Where(
                    x =>
                        x.pointcutMethod.Name == method.Name
                        && Utils.TypeArrayMatch(x.pointcutMethod.GetParameters().Select(p => p.ParameterType).ToArray(), method.GetParameters().Select(p => p.ParameterType).ToArray())
                    )
                    .Select(x => x.concernMethod).FirstOrDefault();

            if (concernMethod != null)
            {
                var param = concernMethod.IsStatic ? null : Concerns.First(x => x.GetType() == concernMethod.DeclaringType);
                returnValue = concernMethod.Invoke(param, methodMessage.Args);
            }

            return new ReturnMessage(returnValue, methodMessage.Args, methodMessage.ArgCount, methodMessage.LogicalCallContext, methodMessage);
        }
    }
}
