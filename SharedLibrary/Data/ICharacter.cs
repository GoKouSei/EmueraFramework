using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.Data
{
    public interface ICharacter
    {
        long No { get; }
        string Name { get; }
        string CallName { get; }
        /// <summary>
        /// 저장된 데이터 입니다 멤버 또는 인덱스로 접근합니다
        /// </summary>
        dynamic Data { get; }
    }

    public class DefaultCharaInfo
    {
        public DefaultCharaInfo(int characterNumber, Dictionary<string, Tuple<object, object>[]> info)
        {
            CharacterNumber = characterNumber;
            Info = info;
        }

        public int CharacterNumber { get; }
        public Dictionary<string, Tuple<object, object>[]> Info { get; }
    }
}
