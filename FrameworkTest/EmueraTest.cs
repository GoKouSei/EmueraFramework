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
            var platform = Emuera.Init(@"C:\TestEra\");
            var framework = new Main();
            platform.Initialize(null, framework);
            framework.Initialize(new[] { platform }, null);
            return framework;
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
