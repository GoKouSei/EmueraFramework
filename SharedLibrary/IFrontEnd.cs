using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Draw;

namespace SharedLibrary
{
    public interface IFrontEnd
    {
        void Initialize(IFramework framework);
        void UpdateLine(ConsoleLine[] lines);
        void Exit();
        int CalcWidth(string str);
    }
}
