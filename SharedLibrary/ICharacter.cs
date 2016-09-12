using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface ICharacter
    {
        int RegistrationNumber { get; }
        string Name { get; }
        string CallName { get; }
        /// <summary>
        /// 저장된 데이터 입니다 멤버 또는 인덱스로 접근합니다
        /// </summary>
        dynamic Data { get; }
    }

    public class DefaultCharaInfo : Tuple<int, Dictionary<string, Tuple<object, object>[]>>
    {
        public DefaultCharaInfo(int item1, Dictionary<string, Tuple<object, object>[]> item2) : base(item1, item2)
        {
        }
    }
}
