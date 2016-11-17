using System;
using System.Reflection;

namespace YeongHun.EmueraFramework.Function
{

    public class Method
    {
        private Func<object[], object> _body;
        public string Name { get; }
        public long[] ARG { get; }
        public string[] ARGS { get; }
        public long[] LOCAL { get; }
        public string[] LOCALS { get; }


        protected Method(string name, int argSize, int argsSize, int localSize, int localsSize)
        {
            Name = name;
            ARG = new long[argSize];
            ARGS = new string[argsSize];
            LOCAL = new long[localSize];
            LOCALS = new string[localsSize];
        }

        public Method(string name, int argSize, int argsSize, int localSize, int localsSize, Action body) : this(name, argSize, argsSize, localSize, localsSize)
        {
            _body = args => { body(); return null; };
        }

        public Method(string name, int argSize, int argsSize, int localSize, int localsSize, Action<object[]> body) : this(name, argSize, argsSize, localSize, localsSize)
        {
            _body = args => { body(args); return null; };
        }

        public Method(string name, int argSize, int argsSize, int localSize, int localsSize, Func<object> body) : this(name, argSize, argsSize, localSize, localsSize)
        {
            _body = args => body();
        }

        public Method(string name, int argSize, int argsSize, int localSize, int localsSize, Func<object[], object> body) : this(name, argSize, argsSize, localSize, localsSize)
        {
            _body = body;
        }

        public object Run(params object[] args) => _body(args);
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExternMethodAttribute : Attribute
    {
        public int LocalSize { get; set; }
        public int LocalsSize { get; set; }
        public int ArgSize { get; set; }
        public int ArgsSize { get; set; }
        public ExternMethodAttribute()
        {
            LocalSize = 1000;
            LocalsSize = 100;
            ArgSize = 1000;
            ArgsSize = 100;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ExternTypeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExternSystemFunctionAttribute : ExternMethodAttribute
    {
        public SystemFunctionCode Code { get; }
        public SystemFunctionPriority Priority { get; set; } = SystemFunctionPriority.NORMAL;

        public ExternSystemFunctionAttribute(SystemFunctionCode code) : base()
        {
            Code = code;
        }
    }
}