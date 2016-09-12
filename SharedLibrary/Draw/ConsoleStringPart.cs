using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Draw
{
    public class ConsoleStringPart : ConsoleLinePart
    {
        public string Str { get; }
        public int Color { get; }
        public ConsoleStringPart(string str, int color, IFrontEnd frontEnd) : base(frontEnd.CalcWidth(str))
        {
            Str = str;
            Color = color;
        }
    }
}
