using MinorShift.Emuera.GameProc.Function;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Function;
using System.IO;

namespace MinorShift.Emuera
{
    class EmueraPlatform:IPlatform
    {
        IEnumerable<string> _methodNames;

        public string Name
        {
            get
            {
                return "EmueraCore";
            }
        }

        SystemFunction[] IPlatform.systemFunctions
        {
            get
            {
                return (from codeName in Enum.GetNames(typeof(SystemFunctionCode))
                        select (SystemFunctionCode)(Enum.Parse(typeof(SystemFunctionCode), codeName)) into code
                        select new SystemFunction(code, framework => Begin(code, framework))).ToArray();
            }
        }

        Method[] IPlatform.methods
        {
            get
            {
                return _methodNames.Select(method => new Method(method, args => Call(method, args))).ToArray();
            }
        }

        public EmueraPlatform(IEnumerable<string> methodNames)
        {
            _methodNames = methodNames;
        }

        public void Initialize(List<Tuple<string, Stream>> source, IFramework framework)
        {
            throw new NotImplementedException();
        }

        object Call(string name, object[] args)
        {
            if (name == null)
                throw new ArgumentNullException();
            var func = GameProc.CalledFunction.CallFunction(GlobalStatic.Process, name, null);
            if (func == null)
                throw new ArgumentException($"Method [{name}] is undefined");
            GlobalStatic.Process.getCurrentState.IntoFunction(func, null,GlobalStatic.EMediator);
            return null;
        }

        void Begin(SystemFunctionCode code, IFramework framework)
        {
            GlobalStatic.Process.getCurrentState.SetBegin(code.ToString());
            GlobalStatic.Process.DoScript();
        }
    }
}
