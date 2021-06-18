﻿using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace TF1
{
    static class TypeFactoryHelper
    {
        static readonly List<Type> _types;
        static TypeFactoryHelper()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            _types = assemblies.SelectMany(x => x.GetTypes()).ToList();
        }
        public static IList<Type> Types => _types;
    }
    class TypeFactory2<T> where T : class
    {
        private ILookup<string, Type> _longNameLookup;
        private ILookup<string, Type> _shortNameLookup;
        
        public TypeFactory2()
        {
            var sw = new Stopwatch();
            sw.Start();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var relevantTypes = TypeFactoryHelper.Types.Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface);
            _longNameLookup = relevantTypes.ToLookup(x => x.FullName);
            _shortNameLookup = relevantTypes.ToLookup(x => x.Name);

            sw.Stop();
            Console.WriteLine($"TF2 ({typeof(T).Name}) {sw.ElapsedTicks / 10.0} microseconds");
        }

        public T CreateInstance(string typeName)
        {
            Type type;
            if (typeName.Contains("."))
            {
                type = _longNameLookup[typeName].FirstOrDefault();
            }
            else
            {
                type = _shortNameLookup[typeName].FirstOrDefault();
            }

            return type == null ? null : (T)Activator.CreateInstance(type);
        }
    }
}
