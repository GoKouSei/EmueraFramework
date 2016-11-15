using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YeongHun.EmueraFramework;
using YeongHun.EmueraFramework.Function;

namespace EmueraPlatform
{
    public class EmueraPlatform : IPlatform
    {
        private IFramework _framework;
        public Method[] Methods { get; private set; }

        public string Name => "ERA";

        public void Dispose()
        {

        }

        public void Initialize(IFramework framework)
        {
            _framework = framework;
            Assembly asm = Assembly.Load(framework.Root + "ERB.dll");
            Type erb = asm.GetType("ERB");
            object instance = Activator.CreateInstance(erb, framework);
            Methods = erb.GetMethods()
                .Select(method =>
                {
                    var parameters = method.GetParameters();
                    if (parameters.Length > 0)
                    {
                        if (method.ReturnType == typeof(void))
                            return new Method(method.Name, args => { method.Invoke(instance, args); });
                        else
                            return new Method(method.Name, args => method.Invoke(instance, args));
                    }
                    else
                    {
                        if (method.ReturnType == typeof(void))
                            return new Method(method.Name, () => { method.Invoke(instance, null); });
                        else
                            return new Method(method.Name, () => method.Invoke(instance, null));
                    }
                }).ToArray();
        }
    }
}
