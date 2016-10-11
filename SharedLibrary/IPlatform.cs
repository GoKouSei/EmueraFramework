using System;
using YeongHun.Function;

namespace YeongHun
{
    public interface IPlatform : IDisposable
    {
        string Name { get; }
        Method[] Methods { get; }
        void Initialize(IFramework framework);
    }
}
