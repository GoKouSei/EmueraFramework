using System;
using System.Collections.Generic;
using YeongHun.EmueraFramework.Data;

namespace Framework
{
    class CharacterInfo : DataBase
    {
        internal CharacterInfo(
            long registrationNumber,
            VariableInfo variableInfo,
            Dictionary<string, Tuple<object, object>[]> defaultInfos=null
            )
        {
            base.Initialize(variableInfo);
            ((IDataBase<long>)this)["NO"] = registrationNumber;

            foreach(var defaultInfo in defaultInfos)
            {

            }
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
