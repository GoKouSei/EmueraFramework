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
        private DrawSetting _setting;

        public ConsoleLinePart[] Parts => _parts.ToArray();
        public int Width { get; private set; }
        public LineAlign Align { get; }

        public ConsoleLine(ConsoleLinePart part, DrawSetting setting, LineAlign align = LineAlign.LEFT)
        {
            _parts.Add(part);
            _setting = setting;
            Align = align;
            Width = _setting.StringCalculator.GetStringWidth(part.Str);
        }

        public ConsoleLine(ConsoleLinePart[] parts, DrawSetting setting, LineAlign align = LineAlign.LEFT)
        {
            _parts.AddRange(parts);
            Align = align;
            UpdateWidth();
        }

        private void UpdateWidth()=> Width = _parts.Select(part => _setting.StringCalculator.GetStringWidth(part.Str)).Sum();

        public static ConsoleLine operator +(ConsoleLine line, ConsoleLinePart part)
        {
            line._parts.Add(part);
            line.UpdateWidth();
            return line;
        }

        public static ConsoleLine operator -(ConsoleLine line, ConsoleLinePart part)
        {
            line._parts.Remove(part);
            line.UpdateWidth();
            return line;
        }
    }
}
