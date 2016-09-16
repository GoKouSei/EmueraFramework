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
            if (!Directory.Exists(root + "DLL\\"))
                Directory.CreateDirectory(root + "DLL\\");
            var dllFiles = Directory.GetFiles(root + "DLL\\", "*.dll", SearchOption.TopDirectoryOnly);

            var methodGroups = (from file in dllFiles
                                select Assembly.LoadFrom(file) into assembly
                                from type in assembly.GetTypes()
                                from method in type.GetMethods()
                                group method by method.DeclaringType);

            List<Method> methods = new List<Method>();
            foreach(var methodGroup in methodGroups)
            {
                var type = methodGroup.Key;
                if (type == typeof(object))
                    continue;
                var methodList = methodGroup.ToList();
                var instance = Activator.CreateInstance(type);
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
