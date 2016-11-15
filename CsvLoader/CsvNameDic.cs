using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using YeongHun.EmueraFramework.Data;

namespace YeongHun.EmueraFramework.Loaders.Windows
{
    public class CsvNameDic : INameDictionary
    {
        private Dictionary<string, Dictionary<string, int>> _nameDictionarys = new Dictionary<string, Dictionary<string, int>>();
        private Dictionary<string, string[]> _nameValues = new Dictionary<string, string[]>();
        public Dictionary<string, int> this[string variableName]
        {
            get
            {
                return _nameDictionarys[variableName];
            }
        }

        public Dictionary<string, string[]> Names
        {
            get
            {
                return _nameValues;
            }
        }

        public void Initialize(string rootPath, Tuple<string, Type, int>[] varInfo)
        {
            if (!Directory.Exists(Path.Combine(rootPath, "CSV")))
                throw new DirectoryNotFoundException("Can't Find CSV Folder");
            foreach (var info in varInfo)
            {
                _nameValues.Add(info.Item1, new string[info.Item3]);
                _nameDictionarys.Add(info.Item1, new Dictionary<string, int>());
            }

            Regex csvRegex = new Regex("[^(CHARA).]+.csv", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var csvFiles = Directory.GetFiles(Path.Combine(rootPath,"CSV"), "*", SearchOption.AllDirectories).Where(file => csvRegex.IsMatch(file));

            foreach (var csvFile in csvFiles)
                LoadCSV(csvFile);

        }

        private void LoadCSV(string csvPath)
        {
            string variableName = Path.GetFileNameWithoutExtension(csvPath);
            if (!_nameDictionarys.ContainsKey(variableName))
                return;
            using(FileStream fs = new FileStream(csvPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fs, true))
                {
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            string rawLine = reader.ReadLine();
                            if (rawLine.Contains(';'))
                                rawLine = rawLine.Substring(rawLine.LastIndexOf(';'));
                            if (rawLine.Length == 0)
                                continue;

                            string[] tokens = rawLine.Split(',');

                            if (tokens.Length < 2)
                                continue;

                            int index;
                            if (!int.TryParse(tokens[0], out index))
                                continue;

                            _nameValues[variableName][index] = tokens[1];
                            _nameDictionarys[variableName].Add(tokens[1], index);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }
}
