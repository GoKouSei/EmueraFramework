using System;
using System.Collections.Generic;
using YeongHun.EmueraFramework.Data;

namespace Framework
{
    class CharacterInfo : ICharacter
    {
        internal CharacterInfo(
            long registrationNumber,
            VariableInfo variableInfo,
            Dictionary<string, object> customVariables,
            Dictionary<string, Tuple<object, object>[]> defaultInfo=null
            )
        {
            RegistrationNumber = registrationNumber;
            Data = new DataBase(customVariables, variableInfo, defaultInfo);
        }

        public string CallName => Data.CALLNAME;
        public string Name => Data.NAME;
        public long RegistrationNumber { get; private set; }

        public dynamic Data { get; }

        public object this[string name, object index]
        {
            get
            {
                return Data[name, index];
            }
            set
            {
                Data[name, index] = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
