using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    class CharacterInfo : ICharacter
    {
        internal CharacterInfo(
            int registrationNumber,
            Tuple<string, Type, int>[] variableInfo,
            Dictionary<string, object> customVariables,
            Dictionary<string, Dictionary<string, int>> nameDic,
            Dictionary<string, Tuple<object, object>[]> defaultInfo=null
            )
        {
            RegistrationNumber = registrationNumber;
            Data = new DataBase(variableInfo, customVariables, nameDic, defaultInfo);
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
