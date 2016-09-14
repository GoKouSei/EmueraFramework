using SharedLibrary.Data;
using System;
using System.Collections.Generic;

namespace Framework
{
    class CharacterInfo : ICharacter
    {
        internal CharacterInfo(
            int registrationNumber,
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
        public int RegistrationNumber { get; private set; }

        public dynamic Data { get; }

        public override string ToString()
        {
            return Name;
        }

    }
}
