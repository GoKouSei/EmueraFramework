using Framework;
using MinorShift.Emuera;
using System;

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
            framework.Run();
            Console.ReadKey(true);
        }
    }
}
