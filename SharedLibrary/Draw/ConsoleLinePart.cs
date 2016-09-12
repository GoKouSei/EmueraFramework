using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Draw
{
    public abstract class ConsoleLinePart
    {
        public int Width { get; }


        public ConsoleLinePart(int width)
        {
            Width = width;
        }

        public bool Contains(Point location,Point p)
        {
            return
                p.X >= location.X && p.X <= location.X + Width;
        }
    }
}
