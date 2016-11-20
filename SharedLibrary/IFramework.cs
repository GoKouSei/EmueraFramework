using PCLStorage;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YeongHun.EmueraFramework.Data;
using YeongHun.EmueraFramework.Draw;
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

    public interface IFramework : IDataBase<string>, IDataBase<long>
    {
        string Root { get; }
        string Name { get; }
        FrameworkState State { get; }

        IDataBase<string> StrValues { get; }
        IDataBase<long> IntValues { get; }

        void SetFrontEnd(IFrontEnd frontEnd, IFileSystem fileSystem);
        void Initialize(IAssemblyLoader assemblyLoader, IPlatform[] platforms, Config config, DrawSetting drawSetting);

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

        LineAlign LineAlign { get; set; }

        Color TextColor { get; set; }
        Color BackGroundColor { get; set; }
        void ResetColor();
        void ResetBGColor();

        void ErbCall(string methodName, params object[] args);
        object Call(string methodName, params object[] args);
        void Begin(SystemFunctionCode sysFunc);

        void Print(string str, PrintFlags flag = PrintFlags.NEWLINE);
        void PrintButton(string str, object value, PrintFlags flag = PrintFlags.NEWLINE);
        void DrawLine();
        void DrawLine(string str);

        void Wait(ConsoleInputType type);
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
        DrawSetting DrawSetting { get;}
        #endregion
    }
}
