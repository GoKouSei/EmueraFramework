using SharedLibrary.Function;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharedLibrary
{
    public interface IPlatform
    {
        string Name { get; }
        Method[] methods { get; }
        SystemFunction[] systemFunctions { get; }
        void Initialize(List<Tuple<string, Stream>> source, IFramework framework);
    }
}
