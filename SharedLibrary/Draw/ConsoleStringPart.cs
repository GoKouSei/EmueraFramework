using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Draw
{
    public class ConsoleStringPart : ConsoleLinePart
    {
        public override string Str { get; }
        public ConsoleStringPart(string str, Color color) : base(color)
        {
            Str = str;
        }
    }
}
