using System;
using System.Reflection;

namespace YeongHun.EmueraFramework.Function
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

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExternMethodAttribute : Attribute
    {
        public bool FirstRun { get; set; } = false;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExternTypeAttribute : Attribute
    {
    }
}
