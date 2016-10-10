using SharedLibrary;
using SharedLibrary.Data;
using SharedLibrary.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class Main : IFramework
    {
        public static IFramework Framework { get; private set; }
        public dynamic Data { get; private set; }
        
        
        private Dictionary<string, object> _customVariables = new Dictionary<string, object>();
        private Dictionary<string, Method> _methods = new Dictionary<string, Method>();
        private Dictionary<SystemFunctionCode, SystemFunction> _systemFunctions = new Dictionary<SystemFunctionCode, SystemFunction>();

        private IPlatform[] _platforms;
        private IEmuera _emuera;
        private bool _initialized = false;
        private Queue<Tuple<string, PrintFlags>> _printQueue = new Queue<Tuple<string, PrintFlags>>();

        public string Name => "EmueraFramework";

        public long[] RegistedCharacters => _emuera.RegistedCharacters;

        public string Root => _emuera.Root;
        public int Encoding => _emuera.Encoding;

        public void Set(IEmuera emuera)
        {
            _emuera = emuera;
            _systemFunctions = _emuera.systemFunctions.ToDictionary(func => func.Code);
            Data = new DataBase(_emuera, _customVariables);
            Framework = this;
        }

        public void Initialize(params IPlatform[] platforms)
        {
            while (_printQueue.Count > 0)
            {
                var str = _printQueue.Dequeue();
                _emuera.Print(str.Item1, str.Item2);
            }

            string errMes = "";

            try
            {
                _platforms = platforms;

                _methods = platforms.Where(platform => platform.Methods != null).SelectMany(platform => platform.Methods)
                    .Union(_emuera.Methods).ToDictionary(method => method.Name);
                _initialized = true;
            }
            catch (Exception e)
            {
                throw errMes == null ? e : new Exception(errMes, e);
            }
        }


        public void AddCustomVariable(string name, object instance)
        {
            if (_customVariables.ContainsKey(name))
                throw new ArgumentException($"이미 정의된 변수이름 {name}입니다");
            else
                _customVariables.Add(name, instance);
        }

        public void DeleteCustomVariable(string name)
        {
            if (_customVariables.ContainsKey(name))
                _customVariables.Remove(name);
        }

        public object Call(string methodName, params object[] args)
        {
            if (!_methods.ContainsKey(methodName))
            {
                throw new ArgumentException($"정의되지 않은 메소드 {methodName}입니다", nameof(methodName));
            }
            var result = _methods[methodName].Run(args);
            return result;
        }

        public Task<object> GetInputAsync(ConsoleInputType type)=> _emuera.GetInputAsync(type);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysFunc"></param>
        /// <exception cref="ArgumentException"/>
        public void Begin(SystemFunctionCode sysFunc)
        {
            try
            {
                _systemFunctions[sysFunc].Run(this);
            }
            catch
            {
                throw new Exception($"시스템 함수 {sysFunc.ToString()}가 정의되지 않았습니다");
            }
        }

        public void Run()
        {
            if (!_initialized)
                throw new Exception("프레임워크가 초기화되지 않았습니다");
            Begin(SystemFunctionCode.TITLE);
            //_scriptTast = Task.Factory.StartNew
            //    (() =>
            //    {
            //        try
            //        {
            //            Begin(SystemFunctionCode.TITLE);
            //            return null;
            //        }
            //        catch (Exception e)
            //        {
            //            return e;
            //        }
            //    }, TaskCreationOptions.LongRunning
            //    );
        }

        public void Print(string str, PrintFlags flag)
        {
            if (!_initialized)
                _printQueue.Enqueue(Tuple.Create(str, flag));
            else
                _emuera.Print(str, flag);
        }

        public void DrawLine() => _emuera.DrawLine();

        public void RunRawLine(string rawLine) => _emuera.RunRawLine(rawLine);

        public int GetColor() => _emuera.GetColor();

        public void SetColor(int color) => _emuera.SetColor(color);

        public void AddChara(long charaNo) => _emuera.AddChara(charaNo);

        public void DelChara(long charaNo) => _emuera.DelChara(charaNo);

        public ICharacter GetChara(long charaNo) => new CharacterInfo(charaNo, _emuera);

        public bool HasMethod(string methodName) => _methods.ContainsKey(methodName);

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    foreach (var platform in _platforms)
                        platform.Dispose();
                    _emuera.Dispose();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                disposedValue = true;
                _methods = null;
                _systemFunctions = null;
                _customVariables = null;
                _emuera = null;
                Data = null;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~Main() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
