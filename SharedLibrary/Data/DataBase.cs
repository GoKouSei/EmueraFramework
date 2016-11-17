using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Data
{
    public interface IDataBase<T>
    {
        T GetValue(string name, long index = 0);
        T GetValue(string name, string index);
        void SetValue(string name, long index, T value);
        void SetValue(string name, string index, T value);
        T this[string name, long index = 0] { get; set; }
        T this[string name, string index] { get; set; }
    }
    public class DataBase : IDataBase<long>, IDataBase<string>
    {

        protected void Initialize(VariableInfo info)
        {
            _strVariables = new Dictionary<string, Variable<string>>();
            _intVariables = new Dictionary<string, Variable<long>>();

            foreach (var variableInfo in info.Info)
            {
                if (variableInfo.Item2 == typeof(string))
                    _strVariables.Add(variableInfo.Item1, new Variable<string>(variableInfo.Item1, variableInfo.Item3, info.NameDic[variableInfo.Item1]));
                else if (variableInfo.Item2 == typeof(long))
                    _intVariables.Add(variableInfo.Item1, new Variable<long>(variableInfo.Item1, variableInfo.Item3, info.NameDic[variableInfo.Item1]));
            }
        }

        protected Dictionary<string, Variable<string>> _strVariables;
        protected Dictionary<string, Variable<long>> _intVariables;

        public bool HasVariable(Type type, string name)
        {
            if (type == typeof(string))
                return _strVariables.ContainsKey(name);
            else if (type == typeof(long))
                return _intVariables.ContainsKey(name);
            else
                return false;
        }

        #region IDataBase<long>
        long IDataBase<long>.this[string name, long index]
        {
            get
            {
                return _intVariables[name][index];
            }

            set
            {
                _intVariables[name][index] = value;
            }
        }

        long IDataBase<long>.this[string name, string index]
        {
            get
            {
                return _intVariables[name][index];
            }

            set
            {
                _intVariables[name][index] = value;
            }
        }

        long IDataBase<long>.GetValue(string name, long index)
        {
            return _intVariables[name][index];
        }

        long IDataBase<long>.GetValue(string name, string index)
        {
            return _intVariables[name][index];
        }

        void IDataBase<long>.SetValue(string name, long index, long value)
        {
            _intVariables[name][index] = value;
        }

        void IDataBase<long>.SetValue(string name, string index, long value)
        {
            _intVariables[name][index] = value;
        }
        #endregion
        #region IDataBase<string>
        string IDataBase<string>.GetValue(string name, long index)
        {
            return _strVariables[name][index];
        }

        string IDataBase<string>.GetValue(string name, string index)
        {
            return _strVariables[name][index];
        }

        void IDataBase<string>.SetValue(string name, long index, string value)
        {
            _strVariables[name][index] = value;
        }

        void IDataBase<string>.SetValue(string name, string index, string value)
        {
            _strVariables[name][index] = value;
        }

        string IDataBase<string>.this[string name, long index]
        {
            get
            {
                return _strVariables[name][index];
            }

            set
            {
                _strVariables[name][index] = value;
            }
        }

        string IDataBase<string>.this[string name, string index]
        {
            get
            {
                return _strVariables[name][index];
            }

            set
            {
                _strVariables[name][index] = value;
            }
        }
        #endregion
    }
}
