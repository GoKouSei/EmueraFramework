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
            framework.Run();
            framework.End();
            long result = (long)framework.Call("TEST");
        }
    }
    
}
