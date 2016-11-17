using System.Collections.Generic;
using YeongHun.EmueraFramework.Draw;

namespace YeongHun.EmueraFramework
{
    public interface IFrontEnd
    {
        List<ConsoleLine> Lines { get; }
        ConsoleLine LastLine { get; set; }

        void Initialize(IFramework framework, DrawSetting setting);

        void Draw();
        void Exit();
    }
}
