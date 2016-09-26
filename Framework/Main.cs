using SharedLibrary;
using SharedLibrary.Data;
using SharedLibrary.Draw;
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

        private Config _config = null;
        private Dictionary<string, object> _customCharaVariables = new Dictionary<string, object>();
        private Dictionary<string, object> _customVariables = new Dictionary<string, object>();
        private Dictionary<string, Method> _methods = new Dictionary<string, Method>();
        private Dictionary<SystemFunctionCode, SystemFunction> _systemFunctions = new Dictionary<SystemFunctionCode, SystemFunction>();
        private Dictionary<long, CharacterInfo> _characters = new Dictionary<long, CharacterInfo>();
        private Dictionary<long, CharacterInfo> _defaultCharacters = new Dictionary<long, CharacterInfo>();

        private IFrontEnd _frontEnd;
        private Task<Exception> _scriptTast;
        private ConsoleInput _lastInput;



        private ConsoleInput LastInput
        {
            get
            {
                var temp = _lastInput;
                _lastInput = null;
                return temp;
            }
            set
            {
                _lastInput = value;
            }
        }

        public string Name => "EmueraFramework";

        public long[] RegistedCharacters => _characters.Keys.ToArray();

        public int Color { get; set; }

        public FrameworkState State { get; private set; }
        
        object IDataBase.this[string name, object index]
        {
            get
            {
                return Data[name, index];
            }
            set
            {
                Data[name, index] = value;
            }
        }

        public void Initialize(IPlatform[] platforms, IFrontEnd frontEnd,Config config)
        {
            State = FrameworkState.Initializing;
            
            string errMes = "";

            try
            {
                _frontEnd = frontEnd;

                _methods = platforms.Where(platform => platform.Methods != null).SelectMany(platform => platform.Methods).ToDictionary(method => method.Name);
                _systemFunctions = new Dictionary<SystemFunctionCode, SystemFunction>();

                Data = new DataBase(_customVariables, config.VariableInfo, config.NameDic);

                _defaultCharacters = config.DefaultCharas.Select(def => new CharacterInfo(def.CharacterNumber, config.CharaVariableInfo, _customCharaVariables, config.NameDic, def.Info)).ToDictionary(info => info.RegistrationNumber);
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
            if(!_methods.ContainsKey(methodName))
            {
                throw new ArgumentException($"정의되지 않은 메소드 {methodName}입니다", nameof(methodName));
            }
            var result = _methods[methodName].Run(args);
            Wait();
            return result;
        }

        private void Wait()
        {
            while (State == FrameworkState.Waiting)
            {
                Task.Delay(500).Wait();
            }
        }

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

        public void EnterInput(ConsoleInput input)
        {
            if (State != FrameworkState.Waiting)
            {
                return;
            }
            State = FrameworkState.Running;
            LastInput = input;
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
            _scriptTast = Task.Factory.StartNew
                (() =>
                {
                    try
                    {
                        Begin(SystemFunctionCode.TITLE);
                        return null;
                    }
                    catch (Exception e)
                    {
                        return e;
                    }
                },TaskCreationOptions.LongRunning
                );
        }

        public Exception End()
        {
            return _scriptTast.Result;
        }


        public void Print(string str, PrintFlags flag = PrintFlags.NEWLINE)
        {
            if (flag.HasFlag(PrintFlags.WAIT))
            {
                State = FrameworkState.Waiting;
                flag |= PrintFlags.NEWLINE;
            }

            if (flag.HasFlag(PrintFlags.NEWLINE))
            {
                _frontEnd.Lines.Add(new ConsoleLine(new ConsoleStringPart(str, Color)));
            }
            else
            {
                _frontEnd.LastLine += new ConsoleStringPart(str, Color);
            }
            Wait();
        }

        public void PrintButton(string str,int value,PrintFlags flag)
        {

        }

        public ICharacter GetChara(long num)
        {
            CharacterInfo chara;
            if(_characters.TryGetValue(num,out chara))
            {
                return chara;
            }
            else
            {
                throw new ArgumentException($"등록되지 않은 캐릭터 번호 [{num}] 입니다", nameof(num));
            }
        }

        public void AddChara(long num)
        {
            CharacterInfo defaultChara;
            if (_defaultCharacters.TryGetValue(num, out defaultChara))
            {
                if (_characters.ContainsKey(num))
                {
                    throw new ArgumentException($"이미 등록된 캐릭터 번호 [{num}] 입니다", nameof(num));
                }
                _characters.Add(num, _defaultCharacters[num]);
            }
            else
            {
                throw new ArgumentException($"정의되지 않은 캐릭터 번호 [{num}] 입니다", nameof(num));
            }
        }

        public void AddVoidChara(long num)
        {
            if (_characters.ContainsKey(num))
            {
                throw new ArgumentException($"이미 등록된 캐릭터 번호 [{num}] 입니다", nameof(num));
            }
            _characters.Add(num, new CharacterInfo(num, _config.CharaVariableInfo, _customCharaVariables, _config.NameDic));
        }

        public void DelChara(long num)
        {
            if (_characters.ContainsKey(num))
                _characters.Remove(num);
        }

        public void Wait(WaitType type)
        {
            throw new NotImplementedException();
        }
    }
}
