using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Draw
{
    public enum LineAlign
    {
        LEFT, CENTER, RIGHT
    }
    public sealed class ConsoleLine
    {
        private List<ConsoleLinePart> _parts = new List<ConsoleLinePart>();
        public ConsoleLinePart[] Parts => _parts.ToArray();
        public LineAlign Align { get; }

        public ConsoleLine(ConsoleLinePart part, LineAlign align = LineAlign.LEFT)
        {
            _parts.Add(part);
            Align = align;
        }

        public ConsoleLine(ConsoleLinePart[] parts, LineAlign align = LineAlign.LEFT)
        {
            _parts.AddRange(parts);
            Align = align;
        }

        public static ConsoleLine operator +(ConsoleLine line,ConsoleLinePart part)
        {
            line._parts.Add(part);
            return line;
        }

        public static ConsoleLine operator -(ConsoleLine line, ConsoleLinePart part)
        {
            line._parts.Remove(part);
            return line;
        }
    }
}
