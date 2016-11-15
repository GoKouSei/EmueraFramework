using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Function
{
    public enum SystemFunctionPriority :int
    {
        LAST,
        NORMAL,
        FIRST,
        ONLY,
    }
    public enum SystemFunctionCode:int
    {
        NULL = 0,
        SHOP = 2,
        TRAIN = 3,
        AFTERTRAIN = 4,
        ABLUP = 5,
        TURNEND = 6,
        FIRST = 7,
        TITLE = 8,
    }
    public sealed class SystemFunctionComparer : IComparer<SystemFunction>
    {
        public int Compare(SystemFunction x, SystemFunction y)
        {
            return x.Priority.CompareTo(y);
        }
    }
    public class SystemFunction
    {
        private Action<IFramework> _body;

        public SystemFunctionPriority Priority { get; }
        public SystemFunctionCode Code { get; }

        public SystemFunction(SystemFunctionCode code, Action<IFramework> body) : this(code, body, SystemFunctionPriority.NORMAL)
        {
        }

        public SystemFunction(SystemFunctionCode code, Action<IFramework> body, SystemFunctionPriority Priority)
        {
            Code = code;
            _body = body;
        }

        public void Run(IFramework framework) => _body(framework);
    }
}
