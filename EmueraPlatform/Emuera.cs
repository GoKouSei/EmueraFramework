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
        public Method[] Methods { get; private set; }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Initialize(IFramework framework)
        {
            Assembly asm = Assembly.Load(framework.Root + "ERB.dll");
            Type erb = asm.GetType("ERB");
            object instance = Activator.CreateInstance(erb, framework);
            Methods = erb.GetMethods()
                .Select(method =>
                {
                    return new Method(method.Name, args =>
                     {

                     });
                }).ToArray();
        }
    }
}
