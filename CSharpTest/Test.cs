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
        public void SystemTitle()
        {
            _framework.Call("FrontEnd Test");
            _framework.Call("FlagSet");
        }

        [ExternMethod]
        public void FlagSet()
        {
            _framework.IntValues["FLAG", 100] = 100;
            _framework.Print("FLAG:100 = " + _framework.IntValues["FLAG", 100]);
        }
    }
}
