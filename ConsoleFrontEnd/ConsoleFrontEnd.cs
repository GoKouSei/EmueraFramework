using System.Collections.Generic;
using System.Linq;
using Colorful;
using YeongHun.EmueraFramework.Draw;

namespace YeongHun.EmueraFramework.FrontEnds.Windows
{
    public class ConsoleFrontEnd : IFrontEnd
    {
        private IFramework _framework;

        public ConsoleLine LastLine
        {
            get
            {
                return Lines.Count > 0 ? new ConsoleLine(0) : Lines[0];
            }

            set
            {
                if (Lines.Count > 0)
                    Lines[0] = value;
                else
                    Lines.Add(value);
            }
        }

        public void Draw()
        {
            Console.BackgroundColor = System.Drawing.Color.FromArgb(_framework.BackGroundColor);
            Console.Clear();
            foreach (var line in Lines)
            {
                string str = line.AllPartString;

                switch(line.Align)
                {
                    case LineAlign.CENTER:
                        Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, Console.CursorTop);
                        break;
                    case LineAlign.RIGHT:
                        Console.SetCursorPosition(Console.WindowWidth - str.Length, Console.CursorTop);
                        break;
                }

                Console.ForegroundColor = System.Drawing.Color.FromArgb(line.Color);
                Console.WriteLine(str);
            }
        }

        public List<ConsoleLine> Lines { get; private set; } = new List<ConsoleLine>();

        public void Exit()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public void Initialize(IFramework framework,DrawSetting drawSetting)
        {
            _framework = framework;
        }
    }
}
