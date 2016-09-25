using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Function
{
    [Flags]
    public enum PrintFlags:int
    {
        NONE = 0,
        NEWLINE = 0x1,
        WAIT = 0x2,
        LEFT_ALIGN = 0x4,
        RIGHT_ALIGN = 0x8,
        IGNORE_COLOR = 0x10,
    }
}
