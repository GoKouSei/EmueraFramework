using SharedLibrary.Function;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MakeDescription
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
                return;
            var plugins = Directory.GetFiles(args[0], "*.plg");
            StringBuilder output = new StringBuilder();
            output.AppendLine();
            output.AppendLine();
            output.AppendLine();
            output.AppendLine("플러그인 설명파일");
            output.AppendLine();
            output.AppendLine();
            output.AppendLine();
            output.AppendLine("기본값이 있는 매개 변수는 생략이 가능합니다");
            output.AppendLine();
            output.AppendLine();
            output.AppendLine("모든 반환값은 Int64일때 RESULT:0\tString일때 RESULTS:0에 할당됩니다");
            output.AppendLine();
            output.AppendLine("Void는 반환값이 없다는 뜻 입니다");
            output.AppendLine();
            output.AppendLine();
            output.AppendLine();
            foreach (var plugin in plugins)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(plugin);
                    output.AppendLine();
                    output.AppendLine();
                    output.AppendLine();
                    output.AppendLine();
                    output.AppendLine();
                    output.AppendLine(assembly.ManifestModule.ScopeName);
                    output.AppendLine();
                    output.AppendLine();
                    foreach (var type in assembly.GetExportedTypes())
                    {
                        var methods = type.GetMethods(BindingFlags.Public|BindingFlags.Instance).Where(method => Attribute.IsDefined(method, typeof(MethodAttribute)));
                        foreach (var method in methods)
                        {
                            var attr = method.GetCustomAttribute<MethodAttribute>();
                            output.AppendLine();
                            output.AppendLine();
                            output.AppendLine(method.ReturnType.Name + " " + method.Name + $"({string.Join<string>(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))})");
                            output.AppendLine();

                            foreach (var parameter in method.GetParameters())
                            {
                                if (parameter.HasDefaultValue)
                                {
                                    output.AppendLine("["+parameter.Name + "]의 기본값 : " + parameter.DefaultValue);
                                }
                            }
                            output.AppendLine();
                            output.AppendLine();

                            if (attr.Comment != null)
                            {
                                output.AppendLine();
                                output.AppendLine("설명 : " + attr.Comment);
                            }
                            if (attr.ParameterComment != null)
                            {
                                output.AppendLine();
                                output.AppendLine("매개변수 : " + attr.ParameterComment);
                            }
                            if (attr.ReturnComment != null)
                            {
                                output.AppendLine();
                                output.AppendLine("반환값 : " + attr.ReturnComment);
                            }

                            output.AppendLine();
                            output.AppendLine();
                            output.AppendLine();
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            using (FileStream fs = new FileStream(args[1], FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(output.ToString());
                    writer.Flush();
                }
            }
        }
    }
}