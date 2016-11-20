using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YeongHun.EmueraFramework.Data
{
    public delegate void VariableChangedHandler<T>(string varName, T oldValue, ref T newValue);

    public class Variable<T>
    {
        public string Name { get; }
        public Type Type => typeof(T);

        public event VariableChangedHandler<T> VariableChanged;

        private Dictionary<string, int> _nameDic;
        private T[] _data;

        private static readonly Dictionary<string, int> _emptyNameDic = new Dictionary<string, int>();

        public Variable(string name, int capacity) : this(name, capacity, dic: null)
        {
        }

        public Variable(string name, int capacity, T[] defaultValue) : this(name, capacity, dic: null)
        {
            defaultValue.CopyTo(_data, 0);
        }

        public Variable(string name, int capacity, Dictionary<string,int> dic)
        {
            Name = name;
            _data = new T[capacity];
            _nameDic = dic ?? _emptyNameDic;
        }

        public T this[long index]
        {
            get
            {
                return _data[index];
            }
            set
            {
                VariableChanged?.Invoke(Name, _data[index], ref value);
                _data[index] = value;
            }
        }

        public T this[int index]
        {
            get
            {
                return _data[index];
            }
            set
            {
                VariableChanged?.Invoke(Name, _data[index], ref value);
                _data[index] = value;
            }
        }

        public T this[string index]
        {
            get
            {
                if (!_nameDic.ContainsKey(index))
                    throw new ArgumentException($"{Name}에 정의되지 않은 식별자 {index}입니다", nameof(index));
                return this[_nameDic[index]];
            }
            set
            {
                if (!_nameDic.ContainsKey(index))
                    throw new ArgumentException($"{Name}에 정의되지 않은 식별자 {index}입니다", nameof(index));
                this[_nameDic[index]] = value;
            }
        }

        public T this[object index]
        {
            get
            {
                if (index is int)
                    return this[(int)index];
                else if (index is long)
                    return this[(long)index];
                else if (index is string)
                    return this[(string)index];
                else
                    throw new ArgumentException("알수없는 인덱스 " + index.ToString() + " 입니다", nameof(index));
            }
            set
            {
                if (index is int)
                    this[(int)index] = value;
                else if (index is long)
                    this[(long)index] = value;
                else if (index is string)
                    this[(string)index] = value;
                else
                    throw new ArgumentException("알수없는 인덱스 " + index.ToString() + " 입니다", nameof(index));
            }
        }

        public static implicit operator T(Variable<T> var)
        {
            return var[0];
        }

        public void Save(Stream stream)
        {
            Serializers.Serialize(stream, _data);
        }

        public void Load(Stream stream)
        {
            _data = Serializers.DeSerialize<T>(stream);
        }

        public long Length => _data.Length;

        public void Resize(int size)
        {
#if DEBUG
            try
            {
#endif
                if (size <= 0)
                {
                    _data = _data.Length == 0 ? _data : new T[0];
                    return;
                }
                T[] newData = new T[size];
                if (_data.Length > 0)
                {
                    if (_data.Length < size)
                        Array.Copy(_data, newData, size);
                    else
                        _data.CopyTo(newData, 0);
                }
                _data = newData;
#if DEBUG
            }
            catch
            {

            }
#endif
        }

        internal void Reset(T defaultValue)
        {
            int length = _data.Length;
            for (int i = 0; i < length; i++)
                _data[i] = defaultValue;
        }

        internal void Reset(T[] array, int startIndex,int targetIndex, int length)
        {
            Array.Copy(array, startIndex, _data, targetIndex, length);
        }
    }

    public class ReadOnlyVariable<T> : Variable<T>
    {
        public ReadOnlyVariable(string name, int capacity, Dictionary<string, int> dic = null) : base(name, capacity, dic)
        {
            VariableChanged += ReadOnlyVariable_VariableChanged;
        }

        private void ReadOnlyVariable_VariableChanged(string varName, T oldValue, ref T newValue)
        {
            throw new InvalidOperationException("Can't edit readonly variable");
        }
    }
}
