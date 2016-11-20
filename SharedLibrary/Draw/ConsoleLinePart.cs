using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Draw
{
    public abstract class ConsoleLinePart
    {
        public Color Color { get; }
        public bool IsButton { get; }
        public int? ButtonIntValue { get; }
        public string ButtonStrValue { get; }
        public abstract string Str { get; }

        protected ConsoleLinePart(Color color)
        {
            Color = color;
            IsButton = false;
            ButtonIntValue = null;
            ButtonStrValue = null;
        }

        protected ConsoleLinePart(Color color, int intValue)
        {
            Color = color;
            IsButton = true;
            ButtonIntValue = intValue;
            ButtonStrValue = null;
        }

        protected ConsoleLinePart(Color color, string strValue)
        {
            Color = color;
            IsButton = true;
            ButtonIntValue = null;
            ButtonStrValue = strValue;
        }
    }
}
