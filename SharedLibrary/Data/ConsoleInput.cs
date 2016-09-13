using SharedLibrary.Draw;

namespace SharedLibrary.Data
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
