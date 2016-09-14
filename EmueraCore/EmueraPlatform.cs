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
                return _methodNames.Select(method => new Method(method, args => Call(method,false, args))).ToArray();
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

        object Call(string name,params object[] args)
        {
            var func = GameProc.CalledFunction.CallFunction(GlobalStatic.Process, name, null);
            if (func == null)
                throw new ArgumentException($"메소드 {name}은 정의되지 않았습니다");
            GlobalStatic.Process.getCurrentState.IntoFunction(func, null,GlobalStatic.EMediator);
            return null;
        }

        void Begin(SystemFunctionCode code,IFramework framework)
        {
            GlobalStatic.Process.getCurrentState.SetBegin(code.ToString());
            GlobalStatic.Process.DoScript();
        }
    }
}
