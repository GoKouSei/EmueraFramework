using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Function
{
    [Flags]
    public enum PrintFlags
    {
        NONE = 0,
        NEWLINE = 0x1,
        WAIT = 0x2,
        LEFT_ALIGN = 0x4,
        RIGHT_ALIGN = 0x8,
        IGNORE_COLOR = 0x10,
        BUTTON = 0x20,
        FORM = 0x40,
        FORMS = 0x80,
        INTEGER = 0x100,
        STRING = 0x200,
    }

    public static class PrintTool
    {
        public static string ToPrintString(this PrintFlags flag)
        {
            string str = "PRINT";
            if (flag.HasFlag(PrintFlags.INTEGER))
                str += "V";
            else if (flag.HasFlag(PrintFlags.STRING))
                str += "S";
            else if (flag.HasFlag(PrintFlags.FORM))
                str += "FORM";
            else if (flag.HasFlag(PrintFlags.FORMS))
                str += "FORMS";
            if (flag.HasFlag(PrintFlags.RIGHT_ALIGN))
                str += "C";
            else if (flag.HasFlag(PrintFlags.LEFT_ALIGN))
                str += "LC";
            if (flag.HasFlag(PrintFlags.IGNORE_COLOR))
                str += "D";
            if (flag.HasFlag(PrintFlags.WAIT))
                str += "W";
            else if (flag.HasFlag(PrintFlags.NEWLINE))
                str += "L";
            return str;
        }
    }
}
