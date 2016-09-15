using SharedLibrary;
using SharedLibrary.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Linq;

namespace Framework
{
    class DataBase : DynamicObject
    {
        private Dictionary<string, object> _customVariables;
        private Type varType = typeof(Variable<>);
        private IEmuera _emuera;

        public DataBase(
            IEmuera emuera,
            Dictionary<string, object> customVariables)
        {
            _emuera = emuera;
            _customVariables = customVariables;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = null;
            if (indexes[0] is string)
            {
                var index = indexes[0] as string;
                var intIndexes = new int[indexes.Length - 1];
                try
                {
                    for (int i = 1; i < indexes.Length; i++)
                    {
                        if (indexes[i] is int)
                            intIndexes[i - 1] = (int)indexes[i];
                        else if (indexes[i] is long)
                            intIndexes[i - 1] = (int)indexes[i];
                        else if (indexes[i - 1] is string)
                            intIndexes[i - 1] = int.Parse((string)indexes[i]);
                        else
                            return false;
                    }
                }
                catch
                {
                    throw new ArgumentException("인덱스가 잘못되었습니다");
                }
                try
                {
                    result = _emuera.GetIntValue(index, intIndexes);
                    return true;
                }
                catch
                {
                    try
                    {
                        result = _emuera.GetStrValue(index, intIndexes);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes[0] is string)
            {
                var index = indexes[0] as string;
                var intIndexes = new int[indexes.Length - 1];
                try
                {
                    for (int i = 1; i < indexes.Length; i++)
                    {
                        if (indexes[i] is int)
                            intIndexes[i - 1] = (int)indexes[i];
                        else if (indexes[i] is long)
                            intIndexes[i - 1] = (int)indexes[i];
                        else if (indexes[i - 1] is string)
                            intIndexes[i - 1] = int.Parse((string)indexes[i]);
                        else
                            return false;
                    }
                }
                catch
                {
                    throw new ArgumentException("인덱스가 잘못되었습니다");
                }
                if (value is string)
                {
                    try
                    {
                        _emuera.SetStrValue(index, (string)value, intIndexes);
                        return true;
                    }
                    catch
                    {
                        long temp;
                        if (!long.TryParse((string)value, out temp)) return false;
                        value = temp;
                    }
                }
                if (value is int || value is long)
                {
                    try
                    {
                        _emuera.SetIntValue(index, (long)value, intIndexes);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = ((dynamic)this)[binder.Name, 0];
                return true;
            }
            catch
            {
                if (_customVariables.TryGetValue(binder.Name, out result)) return true;
                else return false;
            }

        }
    }
}