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
            Dictionary<string, Tuple<object, object>[]> defaultInfo=null
            )
        {
            RegistrationNumber = registrationNumber;
        }

        public string CallName => Data.CALLNAME;
        public string Name => Data.NAME;
        public long RegistrationNumber { get; private set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
