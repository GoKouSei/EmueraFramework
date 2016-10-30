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
        List<ConsoleLine> Lines { get; }
        ConsoleLine LastLine { get; set; }

        void Initialize(IFramework framework);

        void Draw();
        void Exit();
    }
}
