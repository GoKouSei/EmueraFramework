using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Function
{
    [Flags]
    public enum PrintFlags:int
    {
        NONE = 0,
        NEWLINE = 0x1,
        WAIT = 0x2,
        IGNORE_COLOR = 0x4,
        LEFT_ALIGN = 0x8,
        RIGHT_ALIGN = 0x10,
    }
}
