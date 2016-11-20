using System.Collections.Generic;
using System.Linq;
using Colorful;
using YeongHun.EmueraFramework.Draw;

namespace YeongHun.EmueraFramework.FrontEnds.Windows
{
    public class ConsoleFrontEnd : IFrontEnd
    {
        private IFramework _framework;
        private DrawSetting _drawSetting;

        public ConsoleLine LastLine
        {
            get
            {
                return Lines.Count > 0 ? new ConsoleLine(new ConsoleStringPart("", new Color(0)), _framework.DrawSetting) : Lines[0];
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
            Console.BackgroundColor = System.Drawing.Color.FromArgb(_framework.BackGroundColor.ToArgb());
            Console.Clear();
            foreach (var line in Lines)
            {
                switch (line.Align)
                {
                    case LineAlign.CENTER:
                        Console.SetCursorPosition((Console.WindowWidth - line.Width) / 2, Console.CursorTop);
                        break;
                    case LineAlign.RIGHT:
                        Console.SetCursorPosition(Console.WindowWidth - line.Width, Console.CursorTop);
                        break;
                }

                foreach (var part in line.Parts)
                {
                    Console.ForegroundColor = System.Drawing.Color.FromArgb(part.Color.ToArgb());
                    Console.WriteLine(part.Str);
                }
            }
        }

        public List<ConsoleLine> Lines { get; private set; } = new List<ConsoleLine>();

        public void Exit()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public void Initialize(IFramework framework)
        {
            _framework = framework;
            _drawSetting = framework.DrawSetting;
        }
    }
}
