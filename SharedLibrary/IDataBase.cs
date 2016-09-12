using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface IDataBase
    {
        /// <summary>
        /// 사용자 정의 변수를 추가합니다 이미 있는 경우 에러가 발생합니다
        /// </summary>
        /// <param name="name">추가할 사용자 정의 변수의 이름</param>
        /// <param name="instance">사용자 변수</param>
        /// <exception cref="ArgumentException"/>
        void AddCustomVariable(string name, object instance);
        /// <summary>
        /// 사용자 정의 변수를 제거합니다 없는 경우 무시됩니다
        /// </summary>
        /// <param name="name">제거할 사용자 정의 변수의 이름</param>
        void DeleteCustomVariable(string name);
        /// <summary>
        /// 저장된 데이터 입니다 멤버 또는 인덱스로 접근합니다
        /// </summary>
        dynamic Data { get; }
    }
}
