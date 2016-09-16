using SharedLibrary;
using SharedLibrary.Function;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinorShift.Emuera
{

    internal class TestPlatform : IPlatform
    {
        private IFramework _framework;
        public Method[] methods
        {
            get
            {
                return new Method[]
                {
                        new Method("TEST",
                        (args)=> 
                        {
                            _framework.Data["FLAG",0] = args[0];
                            _framework.Data["FLAG",1] = _framework.GetInput(SharedLibrary.Data.ConsoleInputType.INTEGER);
                            _framework.Print("FLAG:0, FLAG:1", PrintFlags.INTEGER | PrintFlags.NEWLINE);
                        })
                };
            }
        }

        public string Name => "TestPlatform";

        public void Initialize(List<Tuple<string, Stream>> source, IFramework framework)
        {
            _framework = framework;
        }
    }
}
