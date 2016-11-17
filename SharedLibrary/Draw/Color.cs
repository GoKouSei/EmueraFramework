using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Draw
{
    public struct Color
    {
        enum NamedColor : int
        {
            BLACK = 0x000000,
            RED = 0xFF0000,
            BLUE = 0x00FF00,
            GREEN = 0x0000FF,
            WHITE = 0xFFFFFF,
        }
        private int _rawValue;
        private const int MASK_A = 0xFF << 24;//0xFF000000
        private const int MASK_R = 0xFF << 16;
        private const int MASK_G = 0xFF << 8;
        private const int MASK_B = 0xFF << 0;

        public Color(int argb)
        {
            _rawValue = argb;
        }

        public Color(string name)
        {
            NamedColor color;
            if (!Enum.TryParse(name.ToUpper(), out color))
                throw new ArgumentException("Unknown color name " + name);
            _rawValue = (int)color;
        }

        public int A => _rawValue & MASK_A;
        public int R => _rawValue & MASK_R;
        public int G => _rawValue & MASK_G;
        public int B => _rawValue & MASK_B;

        public int ToArgb() => _rawValue;
    }
}
