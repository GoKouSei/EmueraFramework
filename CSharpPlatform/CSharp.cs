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
            var dllFiles = Directory.GetFiles(root, "*.dll", SearchOption.TopDirectoryOnly);
            var assemblys = dllFiles.Select(
                file =>
                {
                    try
                    {
                        return Assembly.LoadFrom(file);
                    }
                    catch
                    {
                        return null;
                    }
                }).Where(asm => asm != null);

            var methodGroups = (from assembly in assemblys
                                from type in assembly.GetTypes()
                                from method in type.GetMethods()
                                where ErbMethodAttribute.HasAttribute(method)
                                group method by method.DeclaringType);

            List<Method> methods = new List<Method>();
            foreach(var methodGroup in methodGroups)
            {
                var type = methodGroup.Key;
                if (type == typeof(object))
                    continue;
                var methodList = methodGroup.ToList();
                object instance = null;
                try
                {
                    instance = Activator.CreateInstance(type, null);
                }
                catch
                {
                    continue;
                }
                foreach (var method in methodList)
                {
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
                        case 1:
                            {
                                if (method.ReturnType == typeof(void))
                                    methods.Add(new Method(method.Name, (args) => { method.Invoke(instance, args); }));
                                else
                                    methods.Add(new Method(method.Name, (args) => method.Invoke(instance, args)));
                                break;
                            }
                        default:
                            {
                                continue;
                            }
                    }
                }
            }
            Methods = methods.ToArray();
        }
    }
}
