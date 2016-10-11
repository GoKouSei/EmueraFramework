using System;
using YeongHun.EmueraFramework.Function;

namespace YeongHun.EmueraFramework
{
    public interface IPlatform : IDisposable
    {
        string Name { get; }
        Method[] Methods { get; }
        void Initialize(IFramework framework);
    }
}
