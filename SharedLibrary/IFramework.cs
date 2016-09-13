using SharedLibrary.Data;
using SharedLibrary.Function;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public enum FrameworkState
    {
        None,
        Initializing,
        Running,
        Waiting,
    }
    public interface IFramework:IDataBase
    {
        string Name { get; }
        FrameworkState State { get; }

        void Initialize(
            IPlatform[] platforms, IFrontEnd frontEnd,
            Tuple<string, Type, int>[] variableInfo,
            Tuple<string, Type, int>[] charaVariableInfo,
            DefaultCharaInfo[] defaultCharas,
            NameDictionary nameDic);

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
        object Call(string methodName, params object[] args);
        void Begin(SystemFunctionCode sysFunc);
        void Print(string str, int color, PrintFlags flag);
        void AddCharaCustomVariable(string name, object instance);
        void DeleteCharaCustomVariable(string name);

        ICharacter GetChara(int num);
        void AddChara(int num);
        void AddVoidChara(int num);
        void DelChara(int num);
        int[] RegistedCharacters { get; }
        #endregion
        #region IFrontEnd
        void EnterInput(ConsoleInput input);
        #endregion
    }
}
