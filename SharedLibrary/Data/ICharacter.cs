using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Data
{
    public interface ICharacter
    {
        long RegistrationNumber { get; }
        string Name { get; }
        string CallName { get; }
        object this[string name, object index] { get; set; }
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
