﻿using SharedLibrary;
using SharedLibrary.Data;
using System;
using System.Collections.Generic;

namespace Framework
{
    class CharacterInfo : ICharacter
    {
        internal CharacterInfo(long registrationNumber, IEmuera emuera)
        {
            Data = new CharaDataBase(registrationNumber, emuera);
        }

        public string CallName => Data.CALLNAME;
        public string Name => Data.NAME;
        public long No => Data.NO;

        public dynamic Data { get; }

        public override string ToString()
        {
            return Name;
        }

    }
}
