using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Data
{
    public class DataBase
    {

        public DataBase(VariableInfo info)
        {
            foreach(var variableInfo in info.Info)
            {
                if (variableInfo.Item2 == typeof(string))
                    _strVariables.Add(variableInfo.Item1, new Variable<string>(variableInfo.Item1, variableInfo.Item3, info.NameDic[variableInfo.Item1]));
                else if(variableInfo.Item2 == typeof(long))
                    _intVariables.Add(variableInfo.Item1, new Variable<long>(variableInfo.Item1, variableInfo.Item3, info.NameDic[variableInfo.Item1]));
            }
        }

        private Dictionary<string, Variable<string>> _strVariables;
        private Dictionary<string, Variable<long>> _intVariables;
        public long GetIntValue(string name, long index) => _intVariables[name][index];
        public long GetIntValue(string name, string index) => _intVariables[name][index];
        public string GetStrValue(string name, long index) => _strVariables[name][index];
        public string GetStrValue(string name, string index) => _strVariables[name][index];
    }
}
