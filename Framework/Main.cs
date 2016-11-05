using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YeongHun.EmueraFramework;
using YeongHun.EmueraFramework.Data;
using YeongHun.EmueraFramework.Draw;
using YeongHun.EmueraFramework.Function;

namespace Framework
{
    public class Main : IFramework
    {
        public string Root => _frontEnd.Root;

        private Config _config = null;
        private Dictionary<string, Method> _methods = new Dictionary<string, Method>();
        private Dictionary<SystemFunctionCode, SystemFunction> _systemFunctions = new Dictionary<SystemFunctionCode, SystemFunction>();
        private List<CharacterInfo> _characters = new List<CharacterInfo>();
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

        public int Color { get; set; }

        public FrameworkState State { get; private set; }

        public int BackGroundColor { get; set; }

        public object Result { get; private set; }

        public Alignment Align { get; set; }

        public object this[string name, object index]
        {
            get
            {
                if (_intVariables.ContainsKey(name))
                    return _intVariables[name][index];
                else
                    return _strVariables[name][index];
            }
            set
            {
                if (_intVariables.ContainsKey(name))
                    _intVariables[name][index] = (long)value;
                else
                    _strVariables[name][index] = (string)value;
            }
        }

        private Dictionary<string, Variable<long>> _intVariables;
        private Dictionary<string, Variable<string>> _strVariables;

        public long GetIntValue(string name, int index)
        {
            return _intVariables[name][index];
        }
        public long GetIntValue(string name, string index)
        {
            return _intVariables[name][index];
        }

        public void SetIntValue(string name, int index, long value)
        {
            _intVariables[name][index] = value;
        }
        public void SetIntValue(string name, string index, long value)
        {
            _intVariables[name][index] = value;
        }

        public string GetStrValue(string name, int index)
        {
            return _strVariables[name][index];
        }
        public string GetStrValue(string name, string index)
        {
            return _strVariables[name][index];
        }

        public void SetstrValue(string name, int index, string value)
        {
            _strVariables[name][index] = value;
        }
        public void SetstrValue(string name, string index, string value)
        {
            _strVariables[name][index] = value;
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

                _intVariables = new Dictionary<string, Variable<long>>();
                _strVariables = new Dictionary<string, Variable<string>>();

                foreach(var info in config.VariableInfo.Info)
                {
                    if (info.Item2 == typeof(string))
                    {
                        _strVariables.Add(info.Item1, new Variable<string>(info.Item1, info.Item3, config.VariableInfo.NameDic[info.Item1]));
                    }
                    else if (info.Item2 == typeof(long))
                    {
                        _intVariables.Add(info.Item1, new Variable<long>(info.Item1, info.Item3, config.VariableInfo.NameDic[info.Item1]));
                    }
                }

                _defaultCharacters = config.DefaultCharas.Select(def => new CharacterInfo(def.CharacterNumber, config.CharaVariableInfo, def.Info)).ToDictionary(info => info.RegistrationNumber);
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

        public void ErbCall(string methodName, params object[] args)
        {
            var result = Call(methodName, args);
            if (result is int)
                Data["RESULT"] = (int)result;
            else if (result is long)
                Data["RESULT"] = (long)result;
            else if (result is string)
                Data["RESULTS"] = (string)result;
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
            if (_frontEnd == null)
                return;
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

        public long GetChara(long num)
        {
            if(num<_characters.Count)
            {
                return _characters[(int)num].RegistrationNumber;
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
                _characters.Add(_defaultCharacters[num]);
            }
            else
            {
                throw new ArgumentException($"정의되지 않은 캐릭터 등록 번호 [{num}] 입니다", nameof(num));
            }
        }

        public void AddVoidChara(long num)
        {
            if (num < _characters.Count)
            {
                throw new ArgumentException($"이미 등록된 캐릭터 번호 [{num}] 입니다", nameof(num));
            }
            _characters.Add(new CharacterInfo(num, _config.CharaVariableInfo, _customCharaVariables));
        }

        public void DelChara(long num)
        {
            if (num < _characters.Count)
            {
                _characters.RemoveAt((int)num);
            }
        }

        public void Wait(WaitType type)
        {
            throw new NotImplementedException();
        }

        public void ResetColor()
        {
            throw new NotImplementedException();
        }

        public void ResetBGColor()
        {
            throw new NotImplementedException();
        }

        public void TWait(long time, long flag)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void PrintButton(string str, object value, PrintFlags flag = PrintFlags.NEWLINE)
        {
            throw new NotImplementedException();
        }

        public void DrawLine()
        {
            throw new NotImplementedException();
        }

        public void DrawLine(string str)
        {
            throw new NotImplementedException();
        }
    }
}
