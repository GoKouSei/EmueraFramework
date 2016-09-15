using Framework;
using MinorShift.Emuera;
using SharedLibrary;
using System;
using SharedLibrary.Function;
using System.Collections.Generic;
using System.IO;

namespace TestProgram
{
    class Program
    {
        static void Main(string[] args)
        { 
            var platform = Emuera.Init(@"C:\TestEra\");
            var framework = new Main();
            platform.Initialize(null, framework);
            framework.Initialize(platform, null);
            //framework.Run();
            //framework.End();
            framework.AddChara(0);
            var info = framework.GetChara(0);
            var num = info.No;
            var name = info.CallName;
            info.Data["ABL", 0] = 1000;
            Console.WriteLine(info.Data["ABL", 0]);
            Console.WriteLine(framework.Data["FLAG", 0]);
            Console.WriteLine(framework.Data["ABL", 0, 0]);
        }
    }
    
}
