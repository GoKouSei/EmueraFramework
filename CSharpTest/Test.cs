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
            framework.Call("FlagSet");
        }

        [ExternMethod]
        [CustomVariable("LocalValue", typeof(long), 1)]
        public object FlagSet(object[] args)
        {
            _framework.IntValues["LocalValue"] = 10000;
            _framework.IntValues["FLAG", "영"] = 100;
            _framework.Print("FLAG:영 = " + _framework.IntValues["FLAG", "영"]);
            _framework.Print("LocalValue = " + _framework.IntValues["LocalValue"]);
            return null;
        }
    }
}
