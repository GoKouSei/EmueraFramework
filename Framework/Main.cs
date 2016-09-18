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

        private IEmuera _emuera;
        private bool _initialized = false;
        private Queue<Tuple<string, PrintFlags>> _printQueue = new Queue<Tuple<string, PrintFlags>>();

        public string Name => "EmueraFramework";

        public long[] RegistedCharacters => _emuera.RegistedCharacters;


        public void Initialize(
            IEmuera emuera, params IPlatform[] platforms)
        {
            while (_printQueue.Count > 0)
            {
                var str = _printQueue.Dequeue();
                emuera.Print(str.Item1, str.Item2);
            }

            string errMes = "";

            try
            {
                _emuera = emuera;

                _methods = platforms.Union(new[] { emuera }).Where(platform => platform.Methods != null).SelectMany(platform => platform.Methods).ToDictionary(method => method.Name);
                _systemFunctions = emuera.systemFunctions.ToDictionary(func => func.Code);

                Data = new DataBase(emuera, _customVariables);

                Framework = this;
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

        public void DrawLine() => _emuera.DrawLine();

        public void RunRawLine(string rawLine) => _emuera.RunRawLine(rawLine);

        public int GetColor() => _emuera.GetColor();

        public void SetColor(int color) => _emuera.SetColor(color);

        public void Print(string str, PrintFlags flag)
        {
            if (!_initialized) _printQueue.Enqueue(Tuple.Create(str, flag));
            else _emuera.Print(str, flag);
        }

        public void AddChara(long charaNo) => _emuera.AddChara(charaNo);

        public void DelChara(long charaNo) => _emuera.DelChara(charaNo);

        public ICharacter GetChara(long charaNo) => new CharacterInfo(charaNo, _emuera);

        public bool HasMethod(string methodName) => _methods.ContainsKey(methodName);
    }
}
