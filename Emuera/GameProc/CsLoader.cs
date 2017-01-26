using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using YeongHun.EmueraFramework;

namespace MinorShift.Emuera.GameProc
{
    class CsLoader
    {
        private CSharpParseOptions _parseOptions;
        private CSharpCompilationOptions _compilationOptions;
        private MetadataReference[] _references;

        public CsLoader()
        {
            _parseOptions = new CSharpParseOptions(LanguageVersion.CSharp7, DocumentationMode.Parse, SourceCodeKind.Regular);
            _compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, false);

            _references = new[] 
            {
                MetadataReference.CreateFromFile(Assembly.Load("mscorlib").Location),
                MetadataReference.CreateFromFile(Assembly.Load("SharedLibrary").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a").Location),
            };
        }
        
        public List<Assembly> LoadCS(string csFolder)
        {
            if (!Directory.Exists(csFolder))
                return new List<Assembly>();

            var assemblys = new List<Assembly>();


            var sourceFiles = new DirectoryInfo(csFolder).GetFiles("*.cs", 
                Config.SearchSubdirectory 
                ? SearchOption.AllDirectories 
                : SearchOption.TopDirectoryOnly);

            Parallel.ForEach(sourceFiles, source =>
            {
                try
                {
                using (var reader = source.OpenText())
                    using (var peStream = new MemoryStream())
                    using(var pdbStream = new MemoryStream())
                    {
                        var syntaxTree = CSharpSyntaxTree.ParseText(reader.ReadToEnd(), _parseOptions, source.FullName, Encoding.UTF8);
                        var result = CSharpCompilation.Create("ERA", options: _compilationOptions)
                        .AddSyntaxTrees(syntaxTree)
                        .AddReferences(_references)
                        .Emit(peStream, pdbStream);
                        if (!result.Success)
                        {
#if DEBUG

                            lock (this)
                                foreach (var diagnostic in result.Diagnostics)
                                {
                                    Debug.WriteLine(diagnostic.GetMessage());
                                }

#endif
                            return;
                        }
                        var asm = Assembly.Load(peStream.ToArray(), pdbStream.ToArray());
                        lock (assemblys)
                            assemblys.Add(asm);
                    }


                }
                catch
                {
                    return;
                }
            });

            return assemblys;
        }
    }
}
