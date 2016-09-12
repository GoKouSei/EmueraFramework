using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorShift.Emuera
{
    class EmueraPlatform:IPlatform
    {
        public string Name => "Emuera";
        public object Call(string name,params object[] args)
        {
            return null;
        }
    }
}
