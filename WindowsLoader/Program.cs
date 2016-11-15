using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Loaders
{
    class Program
    {
        static void Main(string[] args)
        {
            IFramework framework;
            IFrontEnd frontEnd;
            List<IPlatform> platforms = new List<IPlatform>();
            var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DLL");

            Assembly frontEndAsm = Assembly.Load(File.ReadAllBytes(Path.Combine(dllPath, "FrontEnd.dll")));
            foreach (var type in frontEndAsm.ExportedTypes)
            {
                if (type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IFrontEnd)))
                    frontEnd = Activator.CreateInstance(type, null) as IFrontEnd;
            }

            Assembly frameworkAsm = Assembly.Load(File.ReadAllBytes(Path.Combine(dllPath, "Framework.dll")));
            foreach (var type in frameworkAsm.ExportedTypes)
            {
                if (type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IFramework)))
                    framework = Activator.CreateInstance(type, null) as IFramework;
            }

            var platformPaths = Directory.GetFiles(dllPath).Where(file => Regex.IsMatch(file, "[^(Platform).]+Platform.dll"));

            foreach (var path in platformPaths)
            {
                try
                {
                    var platformAsm = Assembly.Load(File.ReadAllBytes(path));
                    foreach(var type in platformAsm.ExportedTypes)
                    {
                        if (type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IPlatform)))
                            platforms.Add(Activator.CreateInstance(type, null) as IPlatform);
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}