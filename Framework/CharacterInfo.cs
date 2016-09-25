using SharedLibrary.Data;
using System;
using System.Collections.Generic;

namespace Framework
{
    class CharacterInfo : ICharacter
    {
        internal CharacterInfo(
            long registrationNumber,
            Tuple<string, Type, int>[] variableInfo,
            Dictionary<string, object> customVariables,
            NameDictionary nameDic,
            Dictionary<string, Tuple<object, object>[]> defaultInfo=null
            )
        {
            RegistrationNumber = registrationNumber;
            Data = new DataBase(customVariables, variableInfo, nameDic, defaultInfo);
        }

        public string CallName => Data.CALLNAME;
        public string Name => Data.NAME;
        public long RegistrationNumber { get; private set; }

        public dynamic Data { get; }

        object IDataBase.this[string name, object index]
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
