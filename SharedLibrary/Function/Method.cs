using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Function
{
    public class Method
    {
        private Func<object[], object> _body;
        public string Name { get; }

        public Method(string name, Action body)
        {
            Name = name;
            _body = args => { body(); return null; };
        }

        public Method(string name, Action<object[]> body)
        {
            Name = name;
            _body = args => { body(args); return null; };
        }

        public Method(string name, Func<object> body)
        {
            Name = name;
            _body = args => body();
        }

        public Method(string name, Func<object[], object> body)
        {
            Name = name;
            _body = body;
        }

        public object Run(params object[] args) => _body(args);
    }


    public class MethodAttribute : Attribute
    {
        public static bool IsMethod(MethodInfo method) => method.GetCustomAttribute(typeof(MethodAttribute)) != null;
    }
}
