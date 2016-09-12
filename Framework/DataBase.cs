using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Framework
{
    class DataBase : DynamicObject
    {
        private Dictionary<string, object> _members = new Dictionary<string, object>();
        private Dictionary<string, object> _customVariables;
        private Type varType = typeof(Variable<>);

        public DataBase(
            Tuple<string, Type, int>[] variableInfo,
            Dictionary<string, object> customVariables,
            Dictionary<string, Dictionary<string, int>> nameDic,
            Dictionary<string, Tuple<object, object>[]> defaultValues = null)
        {
            foreach (var varInfo in variableInfo)
            {
                Dictionary<string, int> dic;
                nameDic.TryGetValue(varInfo.Item1, out dic);
                _members.Add(varInfo.Item1, Activator.CreateInstance(
                    //                                              Variable(string name, int capacity, Dictionary<string, int> dic = null, Tuple<object, object>[] defaultValue = null)
                    typeof(Variable<>).MakeGenericType(varInfo.Item2), varInfo.Item1, varInfo.Item3, dic, defaultValues == null ? null : defaultValues[varInfo.Item1]));
            }

            _customVariables = customVariables;
        }

        public override IEnumerable<string> GetDynamicMemberNames() => _members.Keys;
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = null;
            if (indexes.Length > 2)
                return false;
            else
            {
                if (indexes[0] is string)
                {
                    var index = indexes[0] as string;
                    if (!_members.ContainsKey(index))
                    {
                        return false;
                    }
                    dynamic var = _members[index];
                    if (indexes.Length == 1)
                    {
                        result = var;
                        return true;
                    }
                    try
                    {
                        result = var[indexes[1]];
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_members.TryGetValue(binder.Name, out result))
                return true;
            else if (_customVariables.TryGetValue(binder.Name, out result))
                return true;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (value.GetType() != varType)//Only Accept Variable<T>
                return false;
            if (_members.ContainsKey(binder.Name))
                return false;
            _members.Add(binder.Name, value);
            return true;
        }
    }
}