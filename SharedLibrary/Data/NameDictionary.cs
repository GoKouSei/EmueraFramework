using PCLStorage;
using System;
using System.Collections.Generic;

namespace YeongHun.EmueraFramework.Data
{
    public interface INameDictionary
    {
        void Initialize(IFileSystem fileSystem, Tuple<string, Type, int>[] varInfo);
        Dictionary<string, int> this[string variableName] { get; }
        Dictionary<string, string[]> Names { get; }
    }
}
