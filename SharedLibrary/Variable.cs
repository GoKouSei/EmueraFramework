using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SharedLibrary
{

    public delegate void VariableChangedHandler<T>(string varName, T oldValue, ref T newValue);

    public class Variable<T>
    {
        public string Name { get; }
        public Type Type => typeof(T);

        public event VariableChangedHandler<T> VariableChanged;

        private Dictionary<string, int> _nameDic;
        private T[] _data;

        public Variable(string name, int capacity, Dictionary<string, int> dic = null, Tuple<object, object>[] defaultValue = null)
        {
            Name = name;
            _data = new T[capacity];
            _nameDic = dic ?? new Dictionary<string, int>();

            if (defaultValue != null)
            {
                foreach (var value in defaultValue)
                {
                    if (!(value.Item2 is T))
                        continue;

                    int index = -1;

                    if (value.Item1 is int)
                    {
                        index = (int)value.Item1;
                    }
                    else if (value.Item1 is string)
                    {
                        if (!_nameDic.TryGetValue((string)value.Item1, out index))
                            continue;
                    }
                    else
                    {
                        continue;
                    }

                    if (index >= 0 && index < capacity)
                    {
                        _data[index] = (T)value.Item2;
                    }
                }
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
                else if (index is string)
                    return this[(string)index];
                else
                    throw new ArgumentException("알수없는 인덱스 " + index.ToString() + " 입니다", nameof(index));
            }
            set
            {
                if (index is int)
                    this[(int)index] = value;
                else if (index is string)
                    this[(string)index] = value;
                else
                    throw new ArgumentException("알수없는 인덱스입니다", nameof(index));
            }
        }
    }
}
