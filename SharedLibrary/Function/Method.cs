using System;
using YeongHun.EmueraFramework.Data;

namespace YeongHun.EmueraFramework.Function
{

    public class Method
    {
        private DefaultMethod _body;
        public string Name { get; }
        public DataBase LocalVariable { get; }


        protected Method(string name, DataBase localDB, int argSize, int argsSize, int localSize, int localsSize)
        {
            Name = name;
            LocalVariable = localDB;

            IDataBase<long> intValues = LocalVariable;
            IDataBase<string> strValues = LocalVariable;
             
            intValues.Resize("ARG", argSize);
            strValues.Resize("ARGS", argsSize);
            intValues.Resize("LOCAL", localSize);
            strValues.Resize("LOCALS", localsSize);
        }

        public static DataBase DefaultLocalVariables
        {
            get
            {
                return new DataBase(new VariableInfo(new EmptyNameDictionary(),
                    new[]
                    {
                        ("ARG", typeof(long), 0, false),
                        ("ARGS", typeof(string), 0, false),
                        ("LOCAL", typeof(long), 1000, false),
                        ("LOCALS", typeof(string), 100, false),
                    }));
            }
        }

        public Method(string name, DefaultMethod body, DataBase localDB) : this(name, body, localDB, 0, 0, 1000, 100) { }

        public Method(string name, DefaultMethod body, DataBase localDB, int argSize, int argsSize, int localSize, int localsSize) : this(name, localDB, argSize, argsSize, localSize, localsSize)
        {
            _body = body;
        }

        public object Run(object[] args)
        {
            return _body(args);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExternMethodAttribute : Attribute
    {
        public int LocalSize { get; set; }
        public int LocalsSize { get; set; }
        public int ArgSize { get; set; }
        public int ArgsSize { get; set; }
        public ExternMethodAttribute() : this(0, 0, 0, 0)
        {
        }

        public ExternMethodAttribute(int argSize, int argsSize,int localSize,int localsSize)
        {
            ArgSize = Math.Max(0, argSize);
            ArgsSize = Math.Max(0, argsSize);
            LocalSize = localSize <= 0 ? 1000 : localSize;
            LocalsSize = localSize <= 0 ? 100 : localsSize;
        }
    }

    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true, Inherited = false)]
    public class CustomVariableAttribute:Attribute
    {
        public string Name { get; }
        public Type Type { get; }
        public int Size { get; }

        public CustomVariableAttribute(string name, Type type, int size)
        {
            Name = name;
            Type = type;
            Size = size;
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

    public delegate object DefaultMethod(object[] args);
    
}