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
        public dynamic Data { get; internal set; }

        private Dictionary<string, object> _customCharaVariables = new Dictionary<string, object>();
        private Dictionary<string, object> _customVariables = new Dictionary<string, object>();
        private Dictionary<string, Method> _methods = new Dictionary<string, Method>();
        private Dictionary<SystemFunctionCode, SystemFunction> _systemFunctions = new Dictionary<SystemFunctionCode, SystemFunction>();

        private IEmuera _emuera;
        private Task<Exception> _scriptTast;

        public string Name => "EmueraFramework";

        public long[] RegistedCharacters => _emuera.RegistedCharacters;

        public FrameworkState State { get; private set; }


        public void Initialize(
            IEmuera emuera, params IPlatform[] platforms)
        {
            State = FrameworkState.Initializing;

            string errMes = "";

            try
            {
                _emuera = emuera;

                _methods = platforms.Union(new[] { emuera }).Where(platform => platform.methods != null).SelectMany(platform => platform.methods).ToDictionary(method => method.Name);
                _systemFunctions = emuera.systemFunctions.ToDictionary(func => func.Code);

                Data = new DataBase(emuera, _customVariables);
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

        public void AddCharaCustomVariable(string name, object instance)
        {
            if (_customCharaVariables.ContainsKey(name))
                throw new ArgumentException($"이미 정의된 변수이름 {name}입니다");
            else
                _customCharaVariables.Add(name, instance);
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

        public object GetInput(ConsoleInputType type)=> _emuera.GetInput(type);

        public void DeleteCustomVariable(string name)
        {
            if (_customVariables.ContainsKey(name))
                _customVariables.Remove(name);
        }

        public void DeleteCharaCustomVariable(string name)
        {
            if (_customCharaVariables.ContainsKey(name))
                _customCharaVariables.Remove(name);
        }

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
            catch (ArgumentException)
            {
                throw new Exception($"시스템 함수 {sysFunc.ToString()}가 정의되지 않았습니다");
            }
        }

        public void Run()
        {
            State = FrameworkState.Running;
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

        public Exception End()
        {
            return _scriptTast.Result;
        }

        public void SetColor(int color) => _emuera.SetColor(color);

        public void Print(string str, PrintFlags flag) => _emuera.Print(str, flag);

        public void AddChara(long charaNo) => _emuera.AddChara(charaNo);

        public void DelChara(long charaNo) => _emuera.DelChara(charaNo);

        public ICharacter GetChara(long charaNo) => new CharacterInfo(charaNo, _emuera, _customCharaVariables);

        public bool HasMethod(string methodName) => _methods.ContainsKey(methodName);
    }
}
