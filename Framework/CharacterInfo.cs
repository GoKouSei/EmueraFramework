using SharedLibrary;
using SharedLibrary.Data;
using System;
using System.Collections.Generic;

namespace Framework
{
    class CharacterInfo : ICharacter
    {
        IEmuera _emuera;
        internal CharacterInfo(int registrationNumber,IEmuera emuera)
        {
            _emuera = emuera;
            emuera.
        }

        public string CallName => Data.CALLNAME;
        public string Name => Data.NAME;
        public int No =>Data

        public dynamic Data { get; }

        public override string ToString()
        {
            return Name;
        }

    }
}
