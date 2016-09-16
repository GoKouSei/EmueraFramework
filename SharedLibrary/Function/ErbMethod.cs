using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Function
{
    public class ErbMethodAttribute:Attribute
    {
        public static bool HasAttribute(MethodInfo method) => method.GetCustomAttribute(typeof(ErbMethodAttribute)) != null;
    }
}
