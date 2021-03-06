﻿using YeongHun.EmueraFramework.Data;
using YeongHun.EmueraFramework.Function;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YeongHun.Common.Config;

namespace YeongHun.EmueraFramework
{
    public interface IFramework:IDisposable
    {
        ConfigDic Config { get; }
        string Name { get; }

        void SetEmuera(IEmuera emuera);
        void Initialize(ConfigDic config, params IPlatform[] platforms);

        /// <summary>
        /// Start Script
        /// </summary>
        void Run();

        #region IPlatform
        /// <summary>
        /// Contain DirectorySeparatorChar Last
        /// </summary>
        string Root { get; }
        int Encoding { get; }

        bool HasMethod(string methodName);
        object Call(string methodName, params object[] args);
        void Begin(SystemFunctionCode sysFunc);
        void Print(string str, PrintFlags flag = PrintFlags.NEWLINE);
        void DrawLine();
        void RunRawLine(string rawLine);
        bool CheckRawLine(string rawLine);

        void SetColor(int color);
        int GetColor();

        void AddCustomVariable(string name, object instance);
        void DeleteCustomVariable(string name);
        Task<object> GetInputAsync(ConsoleInputType type);

        dynamic Data { get; }
        
        void AddChara(long charaNo);
        void DelChara(long charaNo);
        ICharacter GetChara(long charaNo);
        long[] RegistedCharacters { get; }
        #endregion
    }
}
