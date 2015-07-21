﻿using System;
using System.Collections.Generic;

namespace CSharpNote.Data.DesignPatternMethod.Implement.ServiceLocatorPattern
{
    public class ServiceLocator
    {
        private readonly Dictionary<string, object> dictionary;
        private static ServiceLocator instance;

        private ServiceLocator()
        {
            dictionary = new Dictionary<string, object>();
        }

        public static ServiceLocator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceLocator();
                }
                return instance;
            }
        }

        public void RegistService<TInterface>(TInterface obj)
        {
            var typeName = typeof(TInterface).ToString();

            if (!dictionary.ContainsKey(typeName))
            {
                dictionary.Add(typeName, obj);
            }
            else
            {
                dictionary[typeName] = obj;
            }
        }

        public TInterface GetService<TInterface>()
        {
            var typeName = typeof(TInterface).ToString();
            if (!dictionary.ContainsKey(typeName))
            {
                throw new Exception();
            }
            return (TInterface)dictionary[typeName];
        }
    }
}