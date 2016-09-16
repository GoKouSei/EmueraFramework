using SharedLibrary.Data;
using SharedLibrary.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface IEmuera:IPlatform
    {
        SystemFunction[] systemFunctions { get; }

        void SetColor(int color);
        void Print(string str, PrintFlags flag);
        object GetValue(string name, params int[] indexes);
        void SetValue(string name, object value, params int[] indexes);
        void AddChara(long charaNo);
        void AddCharaFromCSV(long csvNumber);
        void DelChara(long charaNo);
        Task<object> GetInputAsync(ConsoleInputType type);

        long[] RegistedCharacters { get; }
    }
}
