﻿using SharedLibrary;
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
        private Dictionary<int, CharacterInfo> _characters = new Dictionary<int, CharacterInfo>();
        private Dictionary<int, CharacterInfo> _defaultCharacters = new Dictionary<int, CharacterInfo>();

        private Tuple<string, Type, int>[] _charaVariableInfo;
        private IFrontEnd _frontEnd;
        private NameDictionary _nameDic;
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

        public int[] RegistedCharacters => _characters.Keys.ToArray();

        public FrameworkState State { get; private set; }


        public void Initialize(
            IPlatform[] platforms, IFrontEnd frontEnd,
            Tuple<string, Type, int>[] variableInfo,
            Tuple<string, Type, int>[] charaVariableInfo,
            DefaultCharaInfo[] defaultCharas,
            NameDictionary nameDic)
        {
            State = FrameworkState.Initializing;
            
            string errMes = "";

            try
            {
                _frontEnd = frontEnd;
                _charaVariableInfo = charaVariableInfo;
                _nameDic = nameDic;

                _methods = platforms.Where(platform => platform.methods != null).SelectMany(platform => platform.methods).ToDictionary(method => method.Name);
                _systemFunctions = platforms.Where(platform => platform.systemFunctions != null).SelectMany(platform => platform.systemFunctions).ToDictionary(sysFunc => sysFunc.Code);

                Data = new DataBase(variableInfo, _customVariables, nameDic);

                if (defaultCharas != null)
                {
                    foreach (var defaultChara in defaultCharas)
                    {
                        _defaultCharacters.Add(defaultChara.CharacterNumber, new CharacterInfo(defaultChara.CharacterNumber, _charaVariableInfo, _customCharaVariables, _nameDic, defaultChara.Info));
                    }
                }

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
            _scriptTast = Task.Run
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
                }
                );
        }

        public Exception End()
        {
            return _scriptTast.Result;
        }


        public void Print(string str, int color, PrintFlags flag)
        {
            if (flag.HasFlag(PrintFlags.WAIT))
                State = FrameworkState.Waiting;
            Wait();
            throw new NotImplementedException();
        }

        public ICharacter GetChara(int num)
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

        public void AddChara(int num)
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

        public void AddVoidChara(int num)
        {
            if (_characters.ContainsKey(num))
            {
                throw new ArgumentException($"이미 등록된 캐릭터 번호 [{num}] 입니다", nameof(num));
            }
            _characters.Add(num, new CharacterInfo(num, _charaVariableInfo, _customCharaVariables, _nameDic));
        }

        public void DelChara(int num)
        {
            if (_characters.ContainsKey(num))
                _characters.Remove(num);
        }
        
    }
}
