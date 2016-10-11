using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.Common.Config.Parsers
{
    static class BuiltInParser
    {
        public static string StringParser(string rawStr) => rawStr;
        public static Int64 Int64Parser(string rawStr) => Int64.Parse(rawStr);
        public static Int32 Int32Parser(string rawStr) => Int32.Parse(rawStr);
        public static Int16 Int16Parser(string rawStr) => Int16.Parse(rawStr);
        public static Byte ByteParser(string rawStr) => Byte.Parse(rawStr);
    }
}
