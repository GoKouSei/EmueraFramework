using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Data
{
    public interface ICharacter : IDataBase<string>, IDataBase<long>
    {
        long RegistrationNumber { get; }
        string Name { get; }
        string CallName { get; }
    }

    public class DefaultCharaInfo
    {
        public DefaultCharaInfo(long characterNumber, Dictionary<string, (int index, object value)[]> info)
        {
            CharacterNumber = characterNumber;
            Info = info;
        }

        public long CharacterNumber { get; }
        public Dictionary<string, (int index, object value)[]> Info { get; }
    }
}
