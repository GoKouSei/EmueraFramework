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

        public Method[] Methods { get; private set; }

        public string Name => "C#";

        public void Initialize(string root, IFramework framework)
        {
            if (!Directory.Exists(root + "\\Plugins"))
            {
                Methods = new Method[0];
                framework.Print("Can't find Plugins folder", PrintFlags.NEWLINE);
                return;
            }
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
                                from type in assembly.GetTypes()
                                from method in type.GetMethods()
                                where MethodAttribute.HasAttribute(method)
                                select Tuple.Create(assembly, type, method));

            List<Method> methods = new List<Method>();
            foreach (var methodGroup in methodGroups)
            {
                var type = methodGroup.Item2;
                if (type == typeof(object))
                    continue;
                object instance = null;
                try
                {
                    instance = Activator.CreateInstance(methodGroup.Item2);
                }
                catch
                {
                    continue;
                }
                framework.Print("", PrintFlags.NEWLINE);
                framework.Print($"Plugin {methodGroup.Item1.ManifestModule.ToString()}->{methodGroup.Item3.Name} installed", PrintFlags.NEWLINE);
                framework.Print("", PrintFlags.NEWLINE);
                var method = methodGroup.Item3;
                switch (method.GetParameters().Length)
                {
                    case 0:
                        {
                            if (method.ReturnType == typeof(void))
                                methods.Add(new Method(method.Name.ToUpper(), () => { method.Invoke(instance, null); }));
                            else
                                methods.Add(new Method(method.Name.ToUpper(), () => method.Invoke(instance, null)));
                            break;
                        }
                    case 1:
                        {
                            if (method.ReturnType == typeof(void))
                                methods.Add(new Method(method.Name.ToUpper(), (args) => { method.Invoke(instance, args); }));
                            else
                                methods.Add(new Method(method.Name.ToUpper(), (args) => 
                                method.Invoke(instance, args)));
                            break;
                        }
                    default:
                        {
                            continue;
                        }
                }
            }
            Methods = methods.ToArray();
        }
    }
}
