using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YeongHun.EmueraFramework.Data;
using YeongHun.EmueraFramework.Function;

namespace YeongHun.EmueraFramework
{
    public enum FrameworkState
    {
        None,
        Initializing,
        Running,
        Waiting,
    }
    
    public enum WaitType
    {
        ANYKEY = 0,
        ENTERKEY,
        INTEGER,
        STRING,
    }

    public enum Alignment
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public interface IFramework : IDataBase<string>, IDataBase<long>
    {
        string Root { get; }
        string Name { get; }
        FrameworkState State { get; }

        void Initialize(IAssemblyLoader assemblyLoader, IPlatform[] platforms, IFrontEnd frontEnd, Config config);

        /// <summary>
        /// Start Script
        /// </summary>
        void Run();

        /// <summary>
        /// End Script
        /// </summary>
        /// <returns>일어났던 예외(발생하지 않으면 null)</returns>
        Exception End();

        #region IPlatform
        int Color { get; set; }
        int BackGroundColor { get; set; }

        Alignment Align { get; set; }


        void ResetColor();
        void ResetBGColor();

        void ErbCall(string methodName, params object[] args);
        object Call(string methodName, params object[] args);
        void Begin(SystemFunctionCode sysFunc);

        void Print(string str, PrintFlags flag = PrintFlags.NEWLINE);
        void PrintButton(string str, object value, PrintFlags flag = PrintFlags.NEWLINE);
        void DrawLine();
        void DrawLine(string str);

        void Wait(WaitType type);
        void TWait(long time, long flag);

        void Save();
        void Load();

        object Result { get; }

        long GetChara(long num);
        void AddChara(long num);
        void AddVoidChara(long num);
        void DelChara(long num);
        #endregion
        #region IFrontEnd
        void EnterInput(ConsoleInput input);
        #endregion
    }
}
