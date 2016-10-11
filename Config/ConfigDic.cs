using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;

namespace YeongHun.Common.Config
{
    public class ConfigDic
    {
        private Dictionary<Type, ConfigParser<object>> _parserDic = new Dictionary<Type, ConfigParser<object>>();
        private static readonly string configPattern = @"^\s*(?<ConfigName>[^\s]+)\s*=\s*(?<ConfigValue>.+)$";

        private Dictionary<string, string> _rawValues = new Dictionary<string, string>();

        public delegate T ConfigParser<T>(string rawStr);

        public ConfigDic()
        {
            var builtInParsers = typeof(Parsers.BuiltInParser).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(method =>
                {
                    if (method.ReturnType == typeof(void))
                        return false;
                    var args = method.GetParameters();
                    if (args.Length != 1)
                        return false;
                    return args[0].ParameterType == typeof(string);
                })
                .Select(method => Tuple.Create<Type, ConfigParser<object>>(method.ReturnType, str => method.Invoke(null, new[] { str })));
            foreach (var parser in builtInParsers)
                _parserDic.Add(parser.Item1, parser.Item2);
        }

        public void AddParser<T>(ConfigParser<T> parser)
        {
            _parserDic.Add(typeof(T), str => parser(str));
        }

        public void SetValue(string name, string value)
        {
            if (!_rawValues.ContainsKey(name))
                _rawValues.Add(name, value);
            else
                _rawValues[name] = value;
        }

        public string GetValue(string name) => _rawValues[name];

        public T GetValue<T>(string name)
        {
            if (!_rawValues.ContainsKey(name))
                throw new ArgumentException("존재하지 않는 설정 이름입니다 name : " + name);
            return (T)_parserDic[typeof(T)](_rawValues[name]);
        }

        public bool TryGetValue<T>(string name, out T value, T defaultValue = default(T), string defaultString = null)
        {
            ConfigParser<object> parser;
            if (!_parserDic.TryGetValue(typeof(T), out parser))
            {
                value = defaultValue;
                return false;
            }
            else
            {
                if (!_rawValues.ContainsKey(name))
                {
                    if (defaultString != null)
                    {
                        _rawValues.Add(name, defaultString);
                    }
                    else
                    {
                        value = defaultValue;
                        return false;
                    }
                }
                value = (T)parser(_rawValues[name]);
                return true;
            }
        }

        public void Save(TextWriter output)
        {
            foreach (var value in _rawValues)
            {
                output.WriteLine(value.Key + " = " + value.Value);
            }
            output.Flush();
            output.Close();
        }

        public void Load(TextReader reader)
        {
            _rawValues.Clear();
            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();
                var match = Regex.Match(line, configPattern);
                if (!match.Success)
                    continue;
                _rawValues.Add(match.Groups["ConfigName"].Value, match.Groups["ConfigValue"].Value);
            }
        }
    }
}
