using Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinorShift.Emuera;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest
{
    [TestClass]
    public class EmueraTest
    {
        public IFramework EmueraInit()
        {
            var platform = Program.Main(@"C:\TestEra\");
            var framwork = new Main();
            framwork.Initialize(new[] { platform }, null);
            return framwork;
        }

        [TestMethod]
        [Timeout(TestTimeout.Infinite)]
        public void CallTest()
        {
            var framework = EmueraInit();
            framework.Run();
        }
    }
}
