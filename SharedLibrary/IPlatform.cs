using SharedLibrary.Function;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharedLibrary
{
    public interface IPlatform:IDisposable
    {
        string Name { get; }
        Method[] Methods { get; }
        void Initialize(string root, IFramework framework);
    }
}
