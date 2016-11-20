using PCLStorage;
using System;
using System.Collections.Generic;

namespace YeongHun.EmueraFramework.Data
{
    public interface INameDictionary
    {
        void Initialize(IFileSystem fileSystem, (string name, Type type, int size, bool hasNameVariable)[] varInfo);
        Dictionary<string, int> this[string variableName] { get; }
        Dictionary<string, string[]> Names { get; }
    }
    public class EmptyNameDictionary : INameDictionary
    {
        public Dictionary<string, int> this[string variableName] => null;

        public Dictionary<string, string[]> Names => null;

        public void Initialize(IFileSystem fileSystem, (string name, Type type, int size, bool hasNameVariable)[] varInfo)
        {
            throw new NotImplementedException();
        }
    }
}
