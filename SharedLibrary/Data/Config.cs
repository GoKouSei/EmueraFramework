using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Data
{
    public sealed class Config
    {
        public int TextColor { get; set; } = -1;//0xFFFFFFFF
        public int BackColor { get; set; } = -16777216;//0xFF000000
        public Tuple<string, Type, int>[] VariableInfo { get; private set; }
        public Tuple<string, Type, int>[] CharaVariableInfo { get; private set; }
        public DefaultCharaInfo[] DefaultCharas { get; private set; }
        public NameDictionary NameDic { get; private set; }

        public Config(Tuple<string, Type, int>[] varInfo, Tuple<string, Type, int>[]charaVarInfo,NameDictionary nameDic,DefaultCharaInfo[] defaultCharas)
        {
            VariableInfo = varInfo;
            CharaVariableInfo = charaVarInfo;
            DefaultCharas = defaultCharas;
            NameDic = nameDic;
        }
    }
}
