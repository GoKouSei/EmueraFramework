using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Data
{
    public class NameDictionary : Dictionary<string, Dictionary<string, int>>
    {
        public new Dictionary<string,int> this[string index]
        {
            get
            {
                if (ContainsKey(index))
                    return base[index];
                else
                    return null;
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
