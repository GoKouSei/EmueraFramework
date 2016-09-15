using SharedLibrary.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Framework
{
    class DataBase : DynamicObject
    {
        private Dictionary<string, object> _members = new Dictionary<string, object>();
        private Dictionary<string, object> _customVariables;
        private Type varType = typeof(Variable<>);

        public DataBase(
            Dictionary<string, object> customVariables,
            Tuple<string, Type, int>[] variableInfo = null,
            NameDictionary nameDic = null,
            Dictionary<string, Tuple<object, object>[]> defaultValues = null)
        {
            if (variableInfo != null)
            {
                foreach (var varInfo in variableInfo)
                {
                    Dictionary<string, int> dic = null;
                    Tuple<object, object>[] infos = null;
                    nameDic.TryGetValue(varInfo.Item1, out dic);
                    defaultValues?.TryGetValue(varInfo.Item1, out infos);
                    _members.Add(varInfo.Item1, Activator.CreateInstance(typeof(Variable<>).MakeGenericType(varInfo.Item2), varInfo.Item1, varInfo.Item3, dic, infos));
                }
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
                    result = var[indexes[1]];
                    return true;
                }
                return false;
            }
        }
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes.Length > 2) throw new ArgumentOutOfRangeException();
            if (!(indexes[0] is string)) throw new ArgumentException("string index is required", nameof(indexes));

            var varName = (string)indexes[0];

            if (value.GetType() == varType && !_members.ContainsKey(varName))
            {
                _members.Add(varName, value);
            }
            
            if (indexes.Length == 1)
            {
                return TryInput(varName, value, 0);
            }

            else
            {
                return TryInput(varName, value, indexes[1]);
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
            if (value.GetType() == varType)//Variable<T>
            {
                if (_members.ContainsKey(binder.Name)) throw new ArgumentException($"variable name {binder.Name} is alreay registed");
                _members.Add(binder.Name, value);
                return true;
            }
            else
            {
                return TryInput(binder.Name, value, 0);
            }
        }

        private bool TryInput(string name, object value, object index)
        {
            if (!_members.ContainsKey(name))
                return false;
            dynamic var = _members[name];
            MethodInfo compatibleMethod = var.GetType().GetMethod("IsCompatible")
                                                       .MakeGenericMethod(value.GetType());
            var args = new[] { value, Activator.CreateInstance(var.Type) };
            if (!compatibleMethod.Invoke(var, args))
                throw new ArgumentException($"wrong value variable name:{var.Name} {value.GetType()} is can't be {var.Type}");
            var[index] = args[1];
            return true;
        }
    }
}