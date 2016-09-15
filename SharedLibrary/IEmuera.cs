using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface IEmuera:IPlatform
    {
        string GetStrValue(string name, params int[] indexes);
        void SetStrValue(string name, string value, params int[] indexes);
        long GetIntValue(string name, params int[] indexes);
        void SetIntValue(string name, long value, params int[] indexes);
    }
}
