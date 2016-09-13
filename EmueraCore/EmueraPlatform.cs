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
        static HashSet<string> methods;

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
                return null;
            }
        }

        Method[] IPlatform.methods
        {
            get
            {
                return methods.Select(method => new Method(method, args => Call(method, args))).ToArray();
            }
        }

        public void Initialize(List<Tuple<string, Stream>> source, IFramework framework)
        {
            throw new NotImplementedException();
        }

        object Call(string name,params object[] args)
        {
            var func = GlobalStatic.LabelDictionary.GetNonEventLabel(name);
            return null;
        }
    }
}
