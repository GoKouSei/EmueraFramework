using YeongHun;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace YeongHun.Framework
{
    class DataBase : DynamicObject
    {
        private Dictionary<string, object> _customVariables;
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
                var left = new object[indexes.Length - 1];
                Array.Copy(indexes, 1, left, 0, left.Length);
                try
                {
                    result = _emuera.GetValue(index, left);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes[0] is string)
            {
                var index = indexes[0] as string;
                var left = new object[indexes.Length - 1];
                Array.Copy(indexes, 1, left, 0, left.Length);
                try
                {
                    _emuera.SetValue(index, value, left);
                    return true;
                }
                catch
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
                result = ((dynamic)this)[binder.Name];
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