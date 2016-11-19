using YeongHun.EmueraFramework.Function;

namespace YeongHun.EmueraFramework.Platforms.Test
{
    [ExternType]
    public class Test
    {
        private IFramework _framework;
        public Test(IFramework framework)
        {
            _framework = framework;
        }

        [ExternSystemFunction(SystemFunctionCode.TITLE)]
        public void SystemTitle(IFramework framework)
        {
            framework.TextColor = new Draw.Color("RED");
            framework.Print("Color Test");
            framework.ResetColor();
            framework.Call("SYSTEM_TITLE");
        }

        [ExternMethod]
        public void FlagSet()
        {
            _framework.IntValues["FLAG", "AAA"] = 100;
            _framework.Print("FLAG:100 = " + _framework.IntValues["FLAG", 100]);
        }
    }
}
