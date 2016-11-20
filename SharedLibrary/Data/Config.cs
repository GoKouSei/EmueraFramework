using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Data
{
    public sealed class VariableInfo
    {
        private (string name, Type type, int size, bool hasNameVariable)[] _info;
        private INameDictionary _nameDic;

        public VariableInfo(INameDictionary nameDic, (string name, Type type, int size, bool hasNameVariable)[] info)
        {
            _nameDic = nameDic;
            _info = info;
        }

        public (string name, Type type, int size, bool hasNameVariable)[] Info => _info;
        public INameDictionary NameDic => _nameDic;
    }
    public sealed class Config
    {
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
