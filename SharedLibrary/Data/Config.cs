using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Data
{
    public sealed class VariableInfo
    {
        private Tuple<string, Type, int>[] _info;
        private NameDictionary _nameDic;

        public VariableInfo(NameDictionary nameDic,Tuple<string, Type, int>[] info)
        {
            _nameDic = nameDic;
            _info = info;
        }

        public Tuple<string, Type, int>[] Info => _info;
        public NameDictionary NameDic => _nameDic;
    }
    public sealed class Config
    {
        public int TextColor { get; set; } = -1;//0xFFFFFFFF
        public int BackColor { get; set; } = -16777216;//0xFF000000
        public VariableInfo VariableInfo { get; private set; }
        public VariableInfo CharaVariableInfo { get; private set; }
        public DefaultCharaInfo[] DefaultCharas { get; private set; }
        public NameDictionary NameDic { get; private set; }

        public Config(VariableInfo varInfo, VariableInfo charaVarInfo, NameDictionary nameDic, DefaultCharaInfo[] defaultCharas)
        {
            VariableInfo = varInfo;
            CharaVariableInfo = charaVarInfo;
            DefaultCharas = defaultCharas;
            NameDic = nameDic;
        }
    }
}
