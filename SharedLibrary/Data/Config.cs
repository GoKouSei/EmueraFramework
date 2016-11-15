using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Data
{
    public sealed class VariableInfo
    {
        private Tuple<string, Type, int>[] _info;
        private INameDictionary _nameDic;

        public VariableInfo(INameDictionary nameDic,Tuple<string, Type, int>[] info)
        {
            _nameDic = nameDic;
            _info = info;
        }

        public Tuple<string, Type, int>[] Info => _info;
        public INameDictionary NameDic => _nameDic;
    }
    public sealed class Config
    {
        public int TextColor { get; set; } = -1;//0xFFFFFFFF
        public int BackColor { get; set; } = -16777216;//0xFF000000
        public VariableInfo VariableInfo { get; private set; }
        public VariableInfo CharaVariableInfo { get; private set; }
        public DefaultCharaInfo[] DefaultCharas { get; private set; }

        public Config(VariableInfo varInfo, VariableInfo charaVarInfo, DefaultCharaInfo[] defaultCharas)
        {
            VariableInfo = varInfo;
            CharaVariableInfo = charaVarInfo;
            DefaultCharas = defaultCharas;
        }
    }
}
