using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YeongHun.EmueraFramework.Function;
using YeongHun.EmueraFramework.Data;

namespace YeongHun.EmueraFramework.Platforms
{
    public class CSharpPlatform : IPlatform
    {

        private List<IDisposable> disposList = new List<IDisposable>();

        public Method[] Methods { get; private set; }
        public SystemFunction[] SystemFunctions { get; private set; }

        public string Name => "C# Platform";


        public void Initialize(IFramework framework)
        {
            if (!Directory.Exists(framework.Root + "CSharp"))
            {
                Methods = new Method[0];
                framework.Print("Can't find CSharp folder", PrintFlags.NEWLINE);
                return;
            }

            var assemblys = Directory.GetFiles(framework.Root + "CSharp", "*.dll", SearchOption.TopDirectoryOnly).Select(
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

            var types = assemblys.SelectMany(asm => asm.GetExportedTypes()).Where(type => type.IsDefined(typeof(ExternTypeAttribute)));
            List<Method> methods = new List<Method>();
            List<SystemFunction> systemFunctions = new List<SystemFunction>();
            foreach (var type in types)
            {
                ConstructorInfo ctor = type.GetConstructor(new[] { typeof(IFramework) });
                if (ctor == null)
                    continue;
                object instance = null;

                try
                {
                    instance = ctor.Invoke(new object[] { framework });
                }
                catch
                {
                    continue;
                }

                if (instance is IDisposable)
                {
                    disposList.Add((IDisposable)instance);
                }

                var sysFuns = type.GetMethods().Where(method => method.IsDefined(typeof(ExternSystemFunctionAttribute)));
                foreach(var sysFun in sysFuns)
                {
                    try
                    {
                        ExternSystemFunctionAttribute attribute = sysFun.GetCustomAttribute<ExternSystemFunctionAttribute>();
                        Action<IFramework> dele = sysFun.CreateDelegate(typeof(Action<IFramework>), instance) as Action<IFramework>;
                        if (dele != null)
                            systemFunctions.Add(
                                new SystemFunction(attribute.Code, attribute.ArgSize, attribute.ArgsSize, attribute.LocalSize, attribute.LocalsSize, dele, attribute.Priority));
                    }
                    catch
                    {
                        continue;
                    }
                }

                var externMethods = type.GetMethods().Where(method =>method.IsDefined(typeof(ExternMethodAttribute)) && !method.IsDefined(typeof(ExternSystemFunctionAttribute)));

                foreach (var method in externMethods)
                {
                    DefaultMethod dele = null;
                    try
                    {
                        dele = method.CreateDelegate(typeof(DefaultMethod), instance) as DefaultMethod;
                        if (dele == null)
                            continue;
                    }
                    catch
                    {
                        continue;
                    }
                    ExternMethodAttribute attribute = method.GetCustomAttribute<ExternMethodAttribute>();

                    List<(string, Type, int, bool)> varInfos = new List<(string, Type, int, bool)>()
                    {
                        ("ARG", typeof(long), 0, false),
                        ("ARGS", typeof(string), 0, false),
                        ("LOCAL", typeof(long), 0, false),
                        ("LOCALS", typeof(string), 0, false),
                    };

                    var customVariables = method.GetCustomAttributes<CustomVariableAttribute>();

                    foreach(var customVariable in customVariables)
                    {
                        varInfos.Add((customVariable.Name, customVariable.Type, customVariable.Size, false));
                    }

                    methods.Add(
                        new Method(
                            method.Name, dele,
                            new DataBase(new VariableInfo(new EmptyNameDictionary(), varInfos.ToArray())),
                            attribute.ArgSize,attribute.ArgsSize,attribute.LocalSize,attribute.LocalsSize));
                    
                }
            }
            Methods = methods.ToArray();
            SystemFunctions = systemFunctions.ToArray();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    foreach (var dispose in disposList)
                        dispose.Dispose();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                Methods = null;
                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~CSharpPlatform()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(false);
        }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        void IDisposable.Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            GC.SuppressFinalize(this);
        }
        #endregion
    }

}
