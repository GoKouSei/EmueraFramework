using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Draw
{
    public enum LineAlign
    {
        LEFT, CENTER, RIGHT
    }
    public abstract class ConsoleLine
    {
        protected ConsoleLinePart[] parts;
        public Point Location { get; }
        public int Height { get; }
        public int Width => parts.Select(part => part.Width).Sum();
        public LineAlign Align { get; }

        public ConsoleLine(ConsoleLinePart[] parts,Point location,int height,LineAlign align=LineAlign.LEFT)
        {
            this.parts = parts;
            Location = location;
            Height = height;
            Align = align;
        }

        public ConsoleLinePart GetPart(Point p)
        {
            if (p.Y >= Location.Y && p.Y <= Location.Y + Height)
            {
                var match = parts.Where(part => part.Contains(Location, p));

                if (match.Any()) return match.First();
                else return null;
            }
            else
                return null;
        }


    }
}
