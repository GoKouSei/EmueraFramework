using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Draw
{
    public interface IStringCalculator
    {
        int GetStringWidth(int fontSize, string str);
        int LineHeight { get; }
    }
}
