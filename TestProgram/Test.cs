using YeongHun.EmueraFramework;
using YeongHun.EmueraFramework.Function;

namespace TestProgram
{
    [ExternType]
    public class Test
    {
        private IFramework _framework;

        public Test(IFramework framework)
        {
            _framework = framework;
        }

        [ExternMethod]
        public async void InputTest()
        {
            long input = (long)await _framework.GetInputAsync(YeongHun.EmueraFramework.Data.ConsoleInputType.IntValue);
            _framework.Print("GetInput:" + input.ToString());
        }

        [ExternMethod]
        public void SYSTEM_TITLE()
        {
            InputTest();
        }
    }
}
