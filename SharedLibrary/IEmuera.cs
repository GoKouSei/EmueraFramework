using YeongHun.Data;
using YeongHun.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun
{
    public interface IEmuera:IPlatform
    {
        SystemFunction[] systemFunctions { get; }

        int GetColor();

        void SetColor(int color);
        void Print(string str, PrintFlags flag);
        void DrawLine();
        void RunRawLine(string rawLine);
        object GetValue(string name, params object[] indexes);
        void SetValue(string name, object value, params object[] indexes);
        void AddChara(long charaNo);
        void AddCharaFromCSV(long csvNumber);
        void DelChara(long charaNo);
        Task<object> GetInputAsync(ConsoleInputType type);

        long[] RegistedCharacters { get; }
        int Encoding { get; }
        string Root { get; }
    }
}
