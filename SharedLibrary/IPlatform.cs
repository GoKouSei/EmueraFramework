using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharedLibrary
{
    public interface IPlatform
    {
        string Name { get; }
        Tuple<string, Func<object[], object>>[] methods { get; }
        void Initialize(List<Tuple<string, Stream>> source, IFramework framework);
    }
}
