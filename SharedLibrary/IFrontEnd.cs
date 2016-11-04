using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeongHun.EmueraFramework.Draw;

namespace YeongHun.EmueraFramework
{
    public interface IFrontEnd
    {
        string Root { get; }
        List<ConsoleLine> Lines { get; }
        ConsoleLine LastLine { get; set; }

        void Initialize(IFramework framework);

        void Draw();
        void Exit();
    }
}
