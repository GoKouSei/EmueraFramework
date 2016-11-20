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

        Variable<T> GetVariable(string name);
        bool HasVariable(string name);
        void Reset(string name, T value = default(T));
        void Resize(string name, int size);
    }
    public class DataBase : IDataBase<long>, IDataBase<string>
    {
        public DataBase(VariableInfo info) => Initialize(info);

        protected DataBase() { }

        protected void Initialize(VariableInfo info)
        {
            _strVariables = new Dictionary<string, Variable<string>>();
            _intVariables = new Dictionary<string, Variable<long>>();

            foreach (var variableInfo in info.Info)
            {
                if (variableInfo.type == typeof(string))
                    _strVariables.Add(variableInfo.name, new Variable<string>(variableInfo.name, variableInfo.size, info.NameDic[variableInfo.name]));
                else if (variableInfo.type == typeof(long))
                    _intVariables.Add(variableInfo.name, new Variable<long>(variableInfo.name, variableInfo.size, info.NameDic[variableInfo.name]));

                if (variableInfo.hasNameVariable)
                {
                    var nameVar = variableInfo.name + "NAME";
                    _strVariables.Add(nameVar, new Variable<string>(nameVar, variableInfo.size, info.NameDic.Names[variableInfo.name]));
                }
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

        public IDataBase<long> IntValues => this;
        public IDataBase<string> StrValues => this;

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

        void IDataBase<long>.Reset(string name, long value)
        {
            if (_intVariables.ContainsKey(name))
                _intVariables[name].Reset(value);
        }

        void IDataBase<long>.Resize(string name, int size)
        {
            if (_intVariables.ContainsKey(name))
                _intVariables[name].Resize(size);
        }

        Variable<long> IDataBase<long>.GetVariable(string name) => _intVariables[name];

        bool IDataBase<long>.HasVariable(string name) => _intVariables.ContainsKey(name);
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

        void IDataBase<string>.Reset(string name, string value)
        {
            if (_strVariables.ContainsKey(name))
                _strVariables[name].Reset(value);
        }

        void IDataBase<string>.Resize(string name, int size)
        {
            if (_strVariables.ContainsKey(name))
                _strVariables[name].Resize(size);
        }

        Variable<string> IDataBase<string>.GetVariable(string name) => _strVariables[name];

        bool IDataBase<string>.HasVariable(string name) => _strVariables.ContainsKey(name);
        #endregion
    }
}
