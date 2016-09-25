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

        void Initialize(IPlatform[] platforms, IFrontEnd frontEnd, Config config);

        /// <summary>
        /// Start Script
        /// </summary>
        void Run();

        /// <summary>
        /// End Script
        /// </summary>
        /// <returns>일어났던 예외(발생하지 않으면 null)</returns>
        Exception End();


        /// <summary>
        /// 사용자 정의 변수를 추가합니다 이미 있는 경우 에러가 발생합니다
        /// </summary>
        /// <param name="name">추가할 사용자 정의 변수의 이름</param>
        /// <param name="instance">사용자 변수</param>
        /// <exception cref="ArgumentException"/>
        void AddCustomVariable(string name, object instance);
        /// <summary>
        /// 사용자 정의 변수를 제거합니다 없는 경우 무시됩니다
        /// </summary>
        /// <param name="name">제거할 사용자 정의 변수의 이름</param>
        void DeleteCustomVariable(string name);

        #region IPlatform
        int Color { get; set; }

        object Call(string methodName, params object[] args);
        void Begin(SystemFunctionCode sysFunc);
        void Print(string str, PrintFlags flag = PrintFlags.NEWLINE);
        void AddCharaCustomVariable(string name, object instance);
        void DeleteCharaCustomVariable(string name);

        ICharacter GetChara(long num);
        void AddChara(long num);
        void AddVoidChara(long num);
        void DelChara(long num);
        long[] RegistedCharacters { get; }
        #endregion
        #region IFrontEnd
        void EnterInput(ConsoleInput input);
        #endregion
    }
}
