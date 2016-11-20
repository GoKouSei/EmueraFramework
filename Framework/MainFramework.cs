using PCLStorage;
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
        public string Root => FileSystem.LocalStorage.Path;

        private Config _config = null;
        private Dictionary<string, Method> _methods = new Dictionary<string, Method>();
        private Dictionary<SystemFunctionCode, SystemFunction[]> _systemFunctions = new Dictionary<SystemFunctionCode, SystemFunction[]>();
        private List<CharacterInfo> _characters = new List<CharacterInfo>();
        private Dictionary<long, CharacterInfo> _defaultCharacters = new Dictionary<long, CharacterInfo>();

        private IFrontEnd _frontEnd;
        private Task<Exception> _scriptTask;
        private Task<Exception> _displayTask;
        private ConsoleInput _lastInput;
        private ConsoleInputType _requiredInputType;
        
        public Method CurrentMethod => _methodStack.Count > 0 ? _methodStack.Peek() : null;
        private Stack<Method> _methodStack = new Stack<Method>();
        public DrawSetting DrawSetting { get; private set; }


        #region IDataBase<long>
        long IDataBase<long>.GetValue(string name, long index)
        {
            if (CurrentMethod.LocalVariable.HasVariable(typeof(long), name))
                return CurrentMethod.LocalVariable.IntValues[name, index];
            else
                return _intVariables[name][index];
        }
        void IDataBase<long>.SetValue(string name, long index, long value)
        {
            if (CurrentMethod.LocalVariable.HasVariable(typeof(long), name))
                CurrentMethod.LocalVariable.IntValues[name, index] = value;
            else
                _intVariables[name][index] = value;
        }
        long IDataBase<long>.this[string name, long index]
        {
            get
            {
                if (CurrentMethod.LocalVariable.HasVariable(typeof(long), name))
                    return CurrentMethod.LocalVariable.IntValues[name, index];
                else
                    return _intVariables[name][index];
            }
            set
            {
                if (CurrentMethod.LocalVariable.HasVariable(typeof(long), name))
                    CurrentMethod.LocalVariable.IntValues[name, index] = value;
                else
                    _intVariables[name][index] = value;
            }
        }
        #endregion

        #region IDataBase<string>
        string IDataBase<string>.GetValue(string name, long index)
        {
            if (CurrentMethod.LocalVariable.HasVariable(typeof(long), name))
                return CurrentMethod.LocalVariable.StrValues[name, index];
            else
                return _strVariables[name][index];
        }

        void IDataBase<string>.SetValue(string name, long index, string value)
        {
            if (CurrentMethod.LocalVariable.HasVariable(typeof(long), name))
                CurrentMethod.LocalVariable.StrValues[name, index] = value;
            else
                _strVariables[name][index] = value;
        }

        string IDataBase<string>.this[string name, long index]
        {
            get
            {
                if (CurrentMethod.LocalVariable.HasVariable(typeof(long), name))
                    return CurrentMethod.LocalVariable.StrValues[name, index];
                else
                    return _strVariables[name][index];
            }
            set
            {
                if (CurrentMethod.LocalVariable.HasVariable(typeof(long), name))
                    CurrentMethod.LocalVariable.StrValues[name, index] = value;
                else
                    _strVariables[name][index] = value;
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

        public Color TextColor { get; set; }

        public FrameworkState State { get; private set; }

        public Color BackGroundColor { get; set; }

        public object Result { get; private set; }

        public LineAlign LineAlign { get; set; }

        public IAssemblyLoader AssemblyLoader { get; private set; }

        private int _targetFPS;
        public int TargetFPS
        {
            get
            {
                return _targetFPS;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                _targetFPS = value;
                _refreshInterval = TimeSpan.FromTicks((long)((1.0 / value) * 10000000));
            }
        }

        public IFileSystem FileSystem { get; private set; }

        private TimeSpan _refreshInterval;

        public void SetFrontEnd(IFrontEnd frontEnd, IFileSystem fileSystem)
        {
            TargetFPS = 10;
            _frontEnd = frontEnd;
            FileSystem = fileSystem;
        }

        public void Initialize(IAssemblyLoader assemblyLoader, IPlatform[] platforms, Config config, DrawSetting drawSetting)
        {
            State = FrameworkState.Initializing;
            AssemblyLoader = assemblyLoader;
            DrawSetting = drawSetting;
            TextColor = drawSetting.TextColor;
            BackGroundColor = drawSetting.BackGroundColor;

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
                ((IDataBase<long>)this)["RESULT", 0] = (int)result;
            else if (result is long)
                ((IDataBase<long>)this)["RESULT", 0] = (long)result;
            else if (result is string)
                ((IDataBase<string>)this)["RESULT", 0] = (string)result;
        }

        public object Call(string methodName, params object[] args)
        {
            if (!_methods.ContainsKey(methodName))
            {
                throw new ArgumentException($"정의되지 않은 메소드 {methodName}입니다", nameof(methodName));
            }
            _methodStack.Push(_methods[methodName]);
            var result = CurrentMethod.Run(args);
            Wait();
            _methodStack.Pop();
            return result;
        }

        private void Wait()
        {
            while (State == FrameworkState.Waiting)
            {
                Task.Delay(500).Wait();
            }
        }

        //This Method call by another Thread(IFrontEnd)
        public void EnterInput(ConsoleInput input)
        {
            if (State != FrameworkState.Waiting)
            {
                return;
            }
            if (input.Type == _requiredInputType)
            {
                lock (this)
                {
                    State = FrameworkState.Running;
                    LastInput = input;
                }
            }
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
                    _methodStack.Push(func);
                    func.Run(this);
                    _methodStack.Pop();
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
            _scriptTask = Task.Factory.StartNew
                (() =>
                {
                    try
                    {
                        Begin(SystemFunctionCode.TITLE);
                        return null;
                    }
                    catch (Exception e)
                    {
                        Print("Exception occured in Script Task");
                        Print(e.Message);
                        return e;
                    }
                }, TaskCreationOptions.LongRunning
                );

            _displayTask = Task.Factory.StartNew
                (() =>
                {
                    DateTime previousDrawTime = DateTime.Now;
                    try
                    {
                        while (!_scriptTask.IsCompleted)
                        {
                            if (DateTime.Now - previousDrawTime >= _refreshInterval)
                            {
                                _frontEnd.Draw();
                                previousDrawTime = DateTime.Now;
                            }
                        }
                        _frontEnd.Draw();
                        return null;
                    }
                    catch (Exception e)
                    {
                        Print("Exception occured in Draw Task");
                        Print(e.Message);
                        return e;
                    }
                }, TaskCreationOptions.LongRunning);
        }

        public Exception End()
        {
            var result = _scriptTask.Result;
            _displayTask.Wait();
            return result;
        }


        public void Print(string str, PrintFlags flag = PrintFlags.NEWLINE)
        {
            if (_frontEnd == null)
                return;

            if (flag.HasFlag(PrintFlags.WAIT))
            {
                lock (this)
                {
                    State = FrameworkState.Waiting;
                }
                _requiredInputType = ConsoleInputType.ANYKEY;
                flag |= PrintFlags.NEWLINE;
            }

            if (flag.HasFlag(PrintFlags.NEWLINE))
            {
                _frontEnd.Lines.Add(new ConsoleLine(new ConsoleStringPart(str, flag.HasFlag(PrintFlags.IGNORE_COLOR) ? DrawSetting.TextColor : TextColor), DrawSetting, LineAlign));
            }
            else
            {
                _frontEnd.LastLine += new ConsoleStringPart(str, flag.HasFlag(PrintFlags.IGNORE_COLOR) ? DrawSetting.TextColor : TextColor);
            }

            Wait();
        }

        public void PrintButton(string str, int value)
        {
            _frontEnd.LastLine += new ConsoleButtonPart(str, TextColor, value);
        }

        public void PrintButton(string str, string value)
        {
            _frontEnd.LastLine += new ConsoleButtonPart(str, TextColor, value);
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
            if (_defaultCharacters.TryGetValue(num, out var defaultChara))
            {
                _characters.Add(defaultChara);
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

        public void Wait(ConsoleInputType type)
        {
            if (State == FrameworkState.Waiting)
                return;
            State = FrameworkState.Waiting;
            _requiredInputType = type;
        }

        public void ResetColor()
        {
            TextColor = DrawSetting.TextColor;
        }

        public void ResetBGColor()
        {
            BackGroundColor = DrawSetting.BackGroundColor;
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
