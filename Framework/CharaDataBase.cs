using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    class CharaDataBase:DynamicObject
    {
        private Dictionary<string, object> _customVariables;
        private IEmuera _emuera;
        private int _charaNo;

        public CharaDataBase(
            long charaNo,IEmuera emuera,
            Dictionary<string, object> customVariables)
        {
            _charaNo = (int)charaNo;
            _emuera = emuera;
            _customVariables = customVariables;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = null;
            if (indexes[0] is string)
            {
                var index = indexes[0] as string;
                var intIndexes = new int[indexes.Length];
                try
                {
                    intIndexes[0] = _charaNo;
                    for (int i = 1; i < indexes.Length; i++)
                    {
                        if (indexes[i] is int)
                            intIndexes[i] = (int)indexes[i];
                        else if (indexes[i] is long)
                            intIndexes[i] = (int)indexes[i];
                        else if (indexes[i] is string)
                            intIndexes[i] = int.Parse((string)indexes[i]);
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
                    result = _emuera.GetValue(index, intIndexes);
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
                var intIndexes = new int[indexes.Length];
                try
                {
                    intIndexes[0] = _charaNo;
                    for (int i = 1; i < indexes.Length; i++)
                    {
                        if (indexes[i] is int)
                            intIndexes[i] = (int)indexes[i];
                        else if (indexes[i] is long)
                            intIndexes[i] = (int)indexes[i];
                        else if (indexes[i] is string)
                            intIndexes[i] = int.Parse((string)indexes[i]);
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
                    _emuera.SetValue(index, value, intIndexes);
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
                return _customVariables.TryGetValue(binder.Name, out result);
            }

        }
    }
}
