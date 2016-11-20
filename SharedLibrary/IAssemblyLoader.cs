using System.Reflection;

namespace YeongHun.EmueraFramework
{
    public interface IAssemblyLoader
    {
        Assembly Load(string path);
    }
}
