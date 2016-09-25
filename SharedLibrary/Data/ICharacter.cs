using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Data
{
    public interface ICharacter:IDataBase
    {
        long RegistrationNumber { get; }
        string Name { get; }
        string CallName { get; }
    }

    public class DefaultCharaInfo
    {
        public DefaultCharaInfo(long characterNumber, Dictionary<string, Tuple<object, object>[]> info)
        {
            CharacterNumber = characterNumber;
            Info = info;
        }

        public long CharacterNumber { get; }
        public Dictionary<string, Tuple<object, object>[]> Info { get; }
    }
}
