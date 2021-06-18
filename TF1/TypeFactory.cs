using System;
using System.Linq;
using System.Diagnostics;

[assembly: TF1.Marker]

namespace TF1
{
    class TypeFactory<T, TMarker> where T : class
    {
        private ILookup<string, Type> _longNameLookup;
        private ILookup<string, Type> _shortNameLookup;
        public TypeFactory(bool allAssemblies = false)
        {
            var sw = new Stopwatch();
            sw.Start();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var markedAssemblies = assemblies.Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(TMarker)));

            var types = (allAssemblies ?  assemblies : markedAssemblies).SelectMany(x => x.GetTypes());

            var relevantTypes = types.Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface);
            _longNameLookup = relevantTypes.ToLookup(x => x.FullName);
            _shortNameLookup = relevantTypes.ToLookup(x => x.Name);

            sw.Stop();
            Console.WriteLine($"{sw.ElapsedTicks / 10.0} microseconds");
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
