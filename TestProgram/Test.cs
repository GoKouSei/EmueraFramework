using YeongHun;
using YeongHun;
using YeongHun.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProgram
{
    public class Test
    {
        public void SETFLAG(long arg)
        {
            Main.Framework.Data["FLAG"] = arg;
            long flag = Main.Framework.Data["FLAG"];
            Main.Framework.Print(Main.Framework.Data["FLAG"].ToString(), PrintFlags.NEWLINE);
        }
    }
}
