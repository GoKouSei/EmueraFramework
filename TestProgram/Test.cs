using YeongHun.EmueraFramework.Framework;
using YeongHun.EmueraFramework.Function;

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
