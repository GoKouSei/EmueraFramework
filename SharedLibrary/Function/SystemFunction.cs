using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Function
{
    public enum SystemFunctionCode
    {
        TITLE, FIRST, SHOP, TRAIN, ABLUP, AFTERTRAIN, TURNEND, LOADGAME, SAVEGAME, LOADDATAEND
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SystemFunctionAttribute : Attribute
    {
        public SystemFunctionCode Code { get; }
        public SystemFunctionAttribute(SystemFunctionCode code)
        {
            Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methods"></param>
        /// <param name="systemFunctions"></param>
        /// <exception cref="ArgumentException"/>
        public static void GetSystemFunctions(IEnumerable<Tuple<string, Func<object[], object>>> methods, Dictionary<SystemFunctionCode, Func<object[], object>> systemFunctions)
        {
            foreach(var method in methods)
            {
                var attribute = method.Item2.GetMethodInfo().GetCustomAttribute<SystemFunctionAttribute>();
                if (attribute != null)
                {
                    try
                    {
                        systemFunctions.Add(attribute.Code, method.Item2);
                    }
                    catch(ArgumentException ae)
                    {
                        throw new ArgumentException($"이미 정의된 시스템 함수 {attribute.Code.ToString()}입니다", ae);
                    }
                }
            }
        }
    }
}
