using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using YeongHun.Common.Config;
using YeongHun.EmueraFramework.Data;
using YeongHun.EmueraFramework.Draw;

namespace YeongHun.EmueraFramework.Loaders.Windows
{
    class Program
    {
        static void Main(string[] args)
        {
            IFramework framework = new Framework.MainFramework();
            IFrontEnd frontEnd = null;
            List<IPlatform> platforms = new List<IPlatform>();
            var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DLL");

            Assembly frontEndAsm = Assembly.Load(File.ReadAllBytes(Path.Combine(dllPath, "FrontEnd.dll")));
            foreach (var type in frontEndAsm.ExportedTypes)
            {
                if (typeof(IFrontEnd).IsAssignableFrom(type))
                    frontEnd = Activator.CreateInstance(type, null) as IFrontEnd;
            }

            if (frontEnd == null)
                throw new DllNotFoundException("Can't find FrontEnd");

            frontEnd.Initialize(framework);
            framework.SetFrontEnd(frontEnd, AppDomain.CurrentDomain.BaseDirectory);

            var platformPaths = Directory.GetFiles(dllPath).Where(file => Regex.IsMatch(file, "[^(Platform).]+Platform.dll"));

            foreach (var path in platformPaths)
            {
                try
                {
                    var platformAsm = Assembly.Load(File.ReadAllBytes(path));
                    foreach (var type in platformAsm.ExportedTypes)
                    {
                        if (typeof(IPlatform).IsAssignableFrom(type))
                        {
                            IPlatform platform = Activator.CreateInstance(type, null) as IPlatform;
                            platform.Initialize(framework);
                            platforms.Add(platform);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            var csvDic = new CsvNameDic();
            Tuple<string, Type, int>[] varInfo = new Tuple<string, Type, int>[]
            {
                Tuple.Create("FLAG",typeof(long),10000),
                Tuple.Create("COUNT",typeof(long),100),
                Tuple.Create("RESULT",typeof(long),100),
            };
            var charaCsvDic = new CsvNameDic();
            Tuple<string, Type, int>[] charaVarInfo = new Tuple<string, Type, int>[]
            {
                Tuple.Create("ABL",typeof(long),100),
            };
            csvDic.Initialize(framework.Root, varInfo);
            charaCsvDic.Initialize(framework.Root, charaVarInfo);

            var drawSetting = new DrawSetting(new StringCalculator());
            var config = new ConfigDic();
            drawSetting.Load(config);

            framework.Initialize(new AssemblyLoader(), platforms.ToArray(), new Config(new VariableInfo(csvDic, varInfo), new VariableInfo(charaCsvDic, charaVarInfo), new CharaCsvLoader().GetDefaultCharaInfos(framework.Root)), drawSetting);
            framework.Run();
            framework.End();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press Any Key to continue...");
            Console.ReadKey(true);
            frontEnd.Exit();
        }
        private class AssemblyLoader : IAssemblyLoader
        {
            public Assembly Load(string path)
            {
                return Assembly.Load(File.ReadAllBytes(path));
            }
        }
        private class StringCalculator : IStringCalculator
        {
            public int LineHeight
            {
                get
                {
                    return 1;
                }
            }

            public int GetStringWidth(int fontSize, string str)
            {
                return str.Length;
            }
        }
    }
}