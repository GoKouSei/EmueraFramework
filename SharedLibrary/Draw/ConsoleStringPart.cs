﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Draw
{
    public class ConsoleStringPart : ConsoleLinePart
    {
        public override string Str { get; }
        public ConsoleStringPart(string str, int color) : base(color)
        {
            Str = str;
        }
    }
}
