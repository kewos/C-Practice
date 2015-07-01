﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNote.Data.DesignPatternMethod.SubClass.StretagyPattern
{
    public abstract class StrategyBase
    {
        private readonly Dictionary<string, Func<string>> strategies;

        public StrategyBase()
        {
            //透過反射可符合close open principle
            strategies = GetType().GetMethods
            (
                BindingFlags.NonPublic
                | BindingFlags.Instance
                | BindingFlags.DeclaredOnly
            )
            .Where(method =>
                //Arguments Null
                !method.GetGenericArguments().Any()
                //Return Int
                && method.ReturnType == typeof(string))
            .ToDictionary(
                method => method.Name,
                //轉成Delegate
                method =>
                {
                    return (Func<string>)Delegate.CreateDelegate
                    (
                        Expression.GetDelegateType
                        (
                            method.GetParameters()
                                .Select(p => p.ParameterType)
                                .Concat(new Type[] { method.ReturnType })
                                .ToArray()
                        ),
                        null,
                        method
                    );
                });
        }

        public Func<string> this[string key]
        {
            get
            {
                if (strategies == null || !strategies.Any())
                {
                    return null;
                }

                Func<string> value;
                return strategies.TryGetValue(key, out value) 
                    ? value 
                    : null;
            }
        }
    }
}
