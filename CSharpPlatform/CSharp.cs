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

        public void Initialize(IFramework framework)
        {
            if (!Directory.Exists(framework.Root + "\\Plugins"))
            {
                Methods = new Method[0];
                framework.Print("Can't find Plugins folder", PrintFlags.NEWLINE);
                return;
            }

            var assemblys = Directory.GetFiles(framework.Root + "\\Plugins", "*.plg", SearchOption.TopDirectoryOnly).Select(
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
                    framework.Print("");
                    framework.Print($"Plugin {methodGroup.Item1.ManifestModule.ToString()}->{method.Name} installed");
                    framework.Print("");
                    var paramaters = method.GetParameters();
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
                                    methods.Add(new Method(method.Name, (args) =>
                                    {
                                        if (args.Length < paramaters.Length)
                                        {
                                            object[] newPara = new object[paramaters.Length];
                                            args.CopyTo(newPara, 0);
                                            for (int i = paramaters.Length - args.Length - 1; i >= 0; i--)
                                                newPara[args.Length + i] = Type.Missing;
                                            method.Invoke(instance, newPara);
                                        }
                                        else method.Invoke(instance, args);
                                    }));
                                else
                                    methods.Add(new Method(method.Name, (args) =>
                                    {
                                        if (args.Length < paramaters.Length && paramaters[paramaters.Length - 1].IsOptional)
                                        {
                                            object[] newPara = new object[paramaters.Length];
                                            args.CopyTo(newPara, 0);
                                            for (int i = paramaters.Length - args.Length; i >= 0; i--)
                                                newPara[args.Length + i] = Type.Missing;
                                            return method.Invoke(instance, newPara);
                                        }
                                        else return method.Invoke(instance, args);
                                    }));
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
