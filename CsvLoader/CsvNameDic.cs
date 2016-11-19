using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using YeongHun.EmueraFramework.Data;
using PCLStorage;
using PCLStorage.Exceptions;

namespace YeongHun.EmueraFramework.Loaders.Windows
{
    public class CsvNameDic : INameDictionary
    {
        private Dictionary<string, Dictionary<string, int>> _nameDictionarys = new Dictionary<string, Dictionary<string, int>>();
        private Dictionary<string, string[]> _nameValues = new Dictionary<string, string[]>();
        private IFileSystem _fileSystem;


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

        public void Initialize(IFileSystem fileSystem, Tuple<string, Type, int>[] varInfo)
        {
            if (fileSystem.LocalStorage.CheckExistsAsync("CSV").Result != ExistenceCheckResult.FolderExists)
                throw new DirectoryNotFoundException("Can't Find CSV Folder");

            _fileSystem = fileSystem;

            foreach (var info in varInfo)
            {
                _nameValues.Add(info.Item1, new string[info.Item3]);
                _nameDictionarys.Add(info.Item1, new Dictionary<string, int>());
            }

            Regex csvRegex = new Regex("[^(CHARA).]+.csv", RegexOptions.IgnoreCase);
            var csvFiles = fileSystem.LocalStorage.GetFolderAsync("CSV").Result.GetFilesAsync().Result.Where(file => csvRegex.IsMatch(file.Name));

            foreach (var csvFile in csvFiles)
                LoadCSV(csvFile);

        }

        private void LoadCSV(IFile csvFile)
        {
            string variableName = Path.GetFileNameWithoutExtension(csvFile.Name);
            if (!_nameDictionarys.ContainsKey(variableName))
                return;
            using (var fs = csvFile.OpenAsync(FileAccess.Read).Result)
            {
                using (StreamReader reader = new StreamReader(fs, true))
                {
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            string rawLine = reader.ReadLine();
                            if (rawLine.Contains(";"))
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
