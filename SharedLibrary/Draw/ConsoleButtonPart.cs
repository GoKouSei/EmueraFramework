using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Draw
{
    public class ConsoleButtonPart : ConsoleStringPart
    {
        public object Value { get; }
        public bool IsString => Value is string;
        public bool IsInteger => Value is long;
        public ConsoleButtonPart(string str, int color, object value, IFrontEnd frontEnd) : base(str, color, frontEnd)
        {
            Value = value;
        }
    }
}
