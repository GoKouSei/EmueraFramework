using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Function
{
    public enum SystemFunctionCode
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
    public class SystemFunction
    {
        private Action<IFramework> _body;

        public SystemFunctionCode Code { get; }

        public SystemFunction(SystemFunctionCode code, Action<IFramework> body)
        {
            Code = code;
            _body = body;
        }

        public void Run(IFramework framework) => _body(framework);
    }
}
