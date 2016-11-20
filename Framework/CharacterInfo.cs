using System;
using System.Collections.Generic;
using YeongHun.EmueraFramework.Data;

namespace Framework
{
    internal class CharacterInfo : DataBase
    {
        internal CharacterInfo(
            long registrationNumber,
            VariableInfo variableInfo,
            Dictionary<string, (int index, object value)[]> defaultInfos=null
            )
        {
            base.Initialize(variableInfo);

            var intValues = this as IDataBase<long>;
            var strValues = this as IDataBase<string>;

            foreach(var defaultInfo in defaultInfos)
            {
                if (intValues.HasVariable(defaultInfo.Key))
                {
                    foreach(var defaultValue in defaultInfo.Value)
                    {
                        if (defaultValue.value is long num)
                            intValues[defaultInfo.Key, defaultValue.index] = num;
                    }
                }
                else if (strValues.HasVariable(defaultInfo.Key))
                {
                    foreach(var defaultValue in defaultInfo.Value)
                    {
                        if (defaultValue.value is string str)
                            strValues[defaultInfo.Key, defaultValue.index] = str;
                    }
                }
            }

            ((IDataBase<long>)this)["NO"] = registrationNumber;
        }

        public string CallName => ((IDataBase<string>)this)["CALLNAME"];
        public string Name => ((IDataBase<string>)this)["NAME"];
        public long RegistrationNumber => ((IDataBase<long>)this)["NO"];

        public override string ToString()
        {
            return Name;
        }

    }
}
