using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Draw
{
    public class ConsoleButtonPart : ConsoleLinePart
    {
        public override string Str { get; }

        public ConsoleButtonPart(string str, Color color, int value) : base(color, value)
        {
            Str = str;
        }

        public ConsoleButtonPart(string str, Color color, string value) : base(color, value)
        {
            Str = str;
        }
    }
}
