using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface IEmuera:IPlatform
    {
        object GetValue(string name, params int[] indexes);
        void SetValue(string name, object value, params int[] indexes);
        void AddChara(long charaNo);
        void AddCharaFromCSV(long csvNumber);
        void DelChara(long charaNo);

        long[] RegistedCharacters { get; }
    }
}
