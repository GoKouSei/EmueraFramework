using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedLibrary.Function
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ErbFunctionAttribute : Attribute
    {

        public static void GetFunctions(IEnumerable<Tuple<string, Func<object[], object>>> source, Dictionary<string, Func<object[], object>> target)
        {
            foreach (var method in source)
            {
                if (method.Item2.GetMethodInfo().GetCustomAttribute(typeof(ErbFunctionAttribute)) != null)
                {
                    if (method.Item2.GetMethodInfo().GetCustomAttribute(typeof(SystemFunctionAttribute)) != null)
                        continue;
                    else
                        target.Add(method.Item1, method.Item2);
                }
            }
        }
    }
}
