using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Data
{
    public interface INameDictionary
    {
        void Initialize(string rootPath, Tuple<string, Type, int>[] varInfo);
        Dictionary<string, int> this[string variableName] { get; }
        Dictionary<string, string[]> Names { get; }
    }
}
