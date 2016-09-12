using SharedLibrary.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class ConsoleInput
    {
        public Point Point { get; }
        public ConsoleLine SelectedLine { get; }
        public ConsoleLinePart SelectedPart => SelectedLine.GetPart(Point);
        public ConsoleInput(Point point, ConsoleLine selectedLine)
        {
            Point = point;
            SelectedLine = selectedLine;
        }
    }
}
