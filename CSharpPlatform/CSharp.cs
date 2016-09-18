using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Function;
using System.IO;
using System.Reflection;

namespace CSharpPlatform
{
    public class CSharp : IPlatform
    {

        private List<IDisposable> disposList = new List<IDisposable>();

        public Method[] Methods { get; private set; }

        public string Name => "C#";

        ~CSharp()
        {
            ((IDisposable)this).Dispose();
        }

        public void Initialize(string root, IFramework framework)
        {
            var dllFiles = Directory.GetFiles(root + "\\Plugins", "*.plg", SearchOption.TopDirectoryOnly);
            //Array.ForEach(dllFiles, assembly => 
            //{
            //    try
            //    {
            //        Assembly.Load(File.ReadAllBytes(assembly));
            //    }
            //    catch
            //    {
            //        return;
            //    }
                
            //    });
            var assemblys = dllFiles.Select(
                file =>
                {
                    try
                    {
                        return Assembly.Load(File.ReadAllBytes(file));
                    }
                    catch
                    {
                        return null;
                    }
                }).Where(asm => asm != null);


            var methodGroups = (from assembly in assemblys
                                from Type type in assembly.GetExportedTypes()
                                select Tuple.Create(assembly, type, type.GetMethods().Where(method => MethodAttribute.IsMethod(method))));
            List<Method> methods = new List<Method>();
            foreach (var methodGroup in methodGroups)
            {
                var type = methodGroup.Item2;
                if (type == typeof(object))
                    continue;
                object instance = null;
                try
                {
                    instance = Activator.CreateInstance(type, framework);
                }
                catch
                {
                    continue;
                }
                if(instance is IDisposable)
                {
                    disposList.Add((IDisposable)instance);
                }
                foreach (var method in methodGroup.Item3)
                {
                    framework.Print($"Plugin {methodGroup.Item2.Name} -> {method.Name} installed");
                    switch (method.GetParameters().Length)
                    {
                        case 0:
                            {
                                if (method.ReturnType == typeof(void))
                                    methods.Add(new Method(method.Name, () => { method.Invoke(instance, null); }));
                                else
                                    methods.Add(new Method(method.Name, () => method.Invoke(instance, null)));
                                break;
                            }
                        default:
                            {
                                if (method.ReturnType == typeof(void))
                                    methods.Add(new Method(method.Name, (args) => { method.Invoke(instance, args); }));
                                else
                                    methods.Add(new Method(method.Name, (args) => method.Invoke(instance, args)));
                                break;
                            }
                    }
                }
            }
            Methods = methods.ToArray();
        }

        void IDisposable.Dispose()
        {
            foreach (var dispose in disposList)
                dispose.Dispose();
        }
    }
}
