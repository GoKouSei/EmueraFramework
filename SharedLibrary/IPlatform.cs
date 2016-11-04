using System;
using System.Collections.Generic;
using System.IO;
using YeongHun.EmueraFramework.Function;

namespace YeongHun.EmueraFramework
{
    public interface IPlatform:IDisposable
    {
        string Name { get; }
        Method[] Methods { get; }
        void Initialize(IFramework framework);
    }
}
