using YeongHun.Data;
using YeongHun.Function;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun
{
    public interface IFramework:IDisposable
    {
        string Name { get; }

        void SetEmuera(IEmuera emuera);
        void Initialize(params IPlatform[] platforms);

        /// <summary>
        /// Start Script
        /// </summary>
        void Run();

        #region IPlatform

        string Root { get; }
        int Encoding { get; }

        bool HasMethod(string methodName);
        object Call(string methodName, params object[] args);
        void Begin(SystemFunctionCode sysFunc);
        void Print(string str, PrintFlags flag = PrintFlags.NEWLINE);
        void DrawLine();
        void RunRawLine(string rawLine);

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
