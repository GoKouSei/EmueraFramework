using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Data
{
    public interface IDataBase
    {
        /// <summary>
        /// 저장된 데이터 입니다 멤버 또는 인덱스로 접근합니다
        /// </summary>
        dynamic Data { get; }
        
        object this[string name, object index] { get; set; }
    }
}
