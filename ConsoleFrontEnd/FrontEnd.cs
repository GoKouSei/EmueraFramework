using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Draw;

namespace ConsoleFrontEnd
{
    public class FrontEnd : IFrontEnd
    {
        private IFramework _framework;

        public ConsoleLine LastLine
        {
            get
            {
                return Lines.Count > 0 ? null : Lines[0];
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
            Console.Clear();
            foreach(var line in Lines)
            {
                string str = string.Concat(line.Parts.Select(part => part.Str));

                switch(line.Align)
                {
                    case LineAlign.CENTER:
                        Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, Console.CursorTop);
                        break;
                    case LineAlign.RIGHT:
                        Console.SetCursorPosition(Console.WindowWidth - str.Length, Console.CursorTop);
                        break;
                }

                Console.WriteLine(str);
            }
        }

        public List<ConsoleLine> Lines { get; private set; } = new List<ConsoleLine>();

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public void Initialize(IFramework framework)
        {
            _framework = framework;
        }
    }
}
