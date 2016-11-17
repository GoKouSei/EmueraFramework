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
    public class MainFramework : DataBase, IFramework
    {
        public string Root => _frontEnd.Root;

        private Config _config = null;
        private Dictionary<string, Method> _methods = new Dictionary<string, Method>();
        private Dictionary<SystemFunctionCode, SystemFunction[]> _systemFunctions = new Dictionary<SystemFunctionCode, SystemFunction[]>();
        private List<CharacterInfo> _characters = new List<CharacterInfo>();
        private Dictionary<long, CharacterInfo> _defaultCharacters = new Dictionary<long, CharacterInfo>();

        private IFrontEnd _frontEnd;
        private Task<Exception> _scriptTast;
        private ConsoleInput _lastInput;

        public Method CurrentMethod { get; private set; }


        #region IDataBase<long>
        long IDataBase<long>.GetValue(string name, long index)
        {
            switch (name)
            {
                case "LOCAL":
                    return CurrentMethod.LOCAL[index];
                case "ARG":
                    return CurrentMethod.ARG[index];
                default:
                    return _intVariables[name];
            }
        }
        long IDataBase<long>.this[string name, long index]
        {
            get
            {
                switch (name)
                {
                    case "LOCAL":
                        return CurrentMethod.LOCAL[index];
                    case "ARG":
                        return CurrentMethod.ARG[index];
                    default:
                        return _intVariables[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "LOCAL":
                        CurrentMethod.LOCAL[index] = value;
                        break;
                    case "ARG":
                        CurrentMethod.ARG[index] = value;
                        break;
                    default:
                        _intVariables[name][index] = value;
                        break;
                }
            }
        }
        #endregion

        #region IDataBase<string>
        string IDataBase<string>.GetValue(string name, long index)
        {
            switch (name)
            {
                case "LOCALS":
                    return CurrentMethod.LOCALS[index];
                case "ARGS":
                    return CurrentMethod.ARGS[index];
                default:
                    return _strVariables[name];
            }
        }
        string IDataBase<string>.this[string name, long index]
        {
            get
            {
                switch (name)
                {
                    case "LOCALS":
                        return CurrentMethod.LOCALS[index];
                    case "ARGS":
                        return CurrentMethod.ARGS[index];
                    default:
                        return _strVariables[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "LOCALS":
                        CurrentMethod.LOCALS[index] = value;
                        break;
                    case "ARGS":
                        CurrentMethod.ARGS[index] = value;
                        break;
                    default:
                        _strVariables[name][index] = value;
                        break;
                }
            }
        }
        #endregion

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

        public LineAlign LineAlign { get; set; }

        public IAssemblyLoader AssemblyLoader { get; private set; }

        public IDataBase<string> StrValues => this;

        public IDataBase<long> IntValues => this;

        public void SetFrontEnd(IFrontEnd frontEnd)
        {
            _frontEnd = frontEnd;
        }

        public void Initialize(IAssemblyLoader assemblyLoader, IPlatform[] platforms, Config config)
        {
            State = FrameworkState.Initializing;
            AssemblyLoader = assemblyLoader;

            string errMes = "";

            try
            {

                base.Initialize(config.VariableInfo);

                _methods = platforms.Where(platform => platform.Methods != null).SelectMany(platform => platform.Methods).ToDictionary(method => method.Name);

                var systemFunctions = platforms.SelectMany(platform => platform.SystemFunctions).OrderBy(sysFunc => sysFunc.Priority);
                var sysFuncTemp = new Dictionary<SystemFunctionCode, List<SystemFunction>>();
                foreach (SystemFunctionCode code in Enum.GetValues(typeof(SystemFunctionCode)))
                    sysFuncTemp.Add(code, new List<SystemFunction>());
                foreach (var systemFunction in systemFunctions)
                    sysFuncTemp[systemFunction.Code].Add(systemFunction);
                foreach (var temp in sysFuncTemp)
                    _systemFunctions.Add(temp.Key, temp.Value.ToArray());
                systemFunctions = null;
                sysFuncTemp = null;


                _defaultCharacters = config.DefaultCharas.Select(def => new CharacterInfo(def.CharacterNumber, config.CharaVariableInfo, def.Info)).ToDictionary(info => info.RegistrationNumber);
            }
            catch (Exception e)
            {
                throw errMes == null ? e : new Exception(errMes, e);
            }
        }

        public void ErbCall(string methodName, params object[] args)
        {
            var result = Call(methodName, args);
            if (result is int)
                ((IDataBase<long>)this)["RESULT"] = (int)result;
            else if (result is long)
                ((IDataBase<long>)this)["RESULT"] = (long)result;
            else if (result is string)
                ((IDataBase<string>)this)["RESULT"] = (string)result;
        }

        public object Call(string methodName, params object[] args)
        {
            if (!_methods.ContainsKey(methodName))
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
                var functions = _systemFunctions[sysFunc];
                foreach (var func in functions)
                {
                    func.Run(this);
                }
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
                        Print(e.Message);
                        return e;
                    }
                }, TaskCreationOptions.LongRunning
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
                _frontEnd.Lines.Add(new ConsoleLine(new ConsoleStringPart(str, flag.HasFlag(PrintFlags.IGNORE_COLOR) ? _config.TextColor : Color), LineAlign));
            }
            else
            {
                _frontEnd.LastLine += new ConsoleStringPart(str, flag.HasFlag(PrintFlags.IGNORE_COLOR) ? _config.TextColor : Color);
            }
            _frontEnd.Draw();
            Wait();
        }

        public void PrintButton(string str, int value, PrintFlags flag)
        {

        }

        public long GetChara(long num)
        {
            if (num < _characters.Count)
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
            _characters.Add(new CharacterInfo(num, _config.CharaVariableInfo));
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
