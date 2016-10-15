using MinorShift.Emuera;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameProc.Function;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeongHun;
using YeongHun.Common.Config;
using YeongHun.EmueraFramework.Data;
using YeongHun.EmueraFramework.Function;

namespace YeongHun.EmueraFramework.Platforms
{
    class EmueraPlatform : IEmuera
    {
        static EmueraPlatform()
        {
            if (File.Exists(Program.ExeDir + Program.ConfigFileName))
            {
                using (FileStream fs = new FileStream(Program.ExeDir + Program.ConfigFileName, FileMode.Open))
                {
                    ConfigDic.Load(new StreamReader(fs));
                }
            }
            ConfigDic.AddParser(
                str =>
            {
                int codepage;
                if (int.TryParse(str, out codepage))
                    return Encoding.GetEncoding(codepage);
                else
                    return Encoding.GetEncoding(str);
            });
        }

        internal static IFramework framework;
        internal static Type returnType = typeof(void);
        internal static object input;
        internal static bool EzEmueraState { get; set; } = false;
        internal static ConfigDic ConfigDic { get; private set; } = new ConfigDic();
        IEnumerable<string> _methodNames;

        #region IEmuera

        string IPlatform.Name => "Emuera";

        SystemFunction[] IEmuera.systemFunctions
        {
            get
            {
                return (from codeName in Enum.GetNames(typeof(SystemFunctionCode))
                        select (SystemFunctionCode)(Enum.Parse(typeof(SystemFunctionCode), codeName)) into code
                        select new SystemFunction(code, framework => Begin(code, framework))).ToArray();
            }
        }

        Method[] IPlatform.Methods
        {
            get
            {
                return _methodNames.Select(method => new Method(method, args => Call(method, args)))
                    .Concat(
                    new[]
                    {
                        new Method("AutoTransOFF", () =>
                        {
                            EzEmueraState = false;
                        }),
                        new Method("AutoTransON", ()=>
                        {
                           EzEmueraState = true;
                        }),
                        new Method("AddDictionary", args =>
                        {
                            string key = (string)args[0];
                            string value = (string)args[1];
                            bool isPre = args.Length == 2 || (long)args[2] == 0L;
                            EZTrans.TranslateXP.UserDic.AddOrUpdate(key, value, isPre);
                        }),
                        new Method("DeleteDictionary", args =>
                        {
                            string key = (string)args[0];
                            bool isPre = args.Length == 1 || (long)args[1] == 0L;
                            EZTrans.TranslateXP.UserDic.Delete(key, (long)args[1] == 0L);
                        }),
                        new Method("PrintCurrentScriptPosition", () =>
                        {
                            framework.Print(GlobalStatic.Process.getCurrentLine.Position.ToString());
                        }),
                    }).ToArray();
            }
        }

        void IPlatform.Initialize(IFramework framework)
        {
            EmueraPlatform.framework = framework;
            GlobalStatic.Console.state = MinorShift.Emuera.GameView.ConsoleState.Running;
        }

        object IEmuera.GetValue(string name, params object[] indexes)
        {
            int[] target = new int[indexes.Length];
            var code = (VariableCode)Enum.Parse(typeof(VariableCode), name);
            for (int i = 0; i < indexes.Length; i++)
            {
                try
                {
                    if (indexes[i] is long)
                        target[i] = (int)(long)indexes[i];
                    else if (indexes[i] is int)
                        target[i] = (int)indexes[i];
                    else if (indexes[i] is string)
                    {
                        string temp;
                        int num;
                        if (!int.TryParse((string)(indexes[i]), out num))
                            target[i] = GlobalStatic.ConstantData.GetKeywordDictionary(out temp, code, i)[(string)indexes[i]];
                        else
                            target[i] = num;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    throw new ArgumentException($"알수없는 인덱스 {indexes[i]} 입니다");
                }
            }
            return GetValue(code, target);
        }

        object GetValue(VariableCode code, int[] indexes)
        {
            int[] index = new int[(
                code.HasFlag(VariableCode.__ARRAY_3D__) ? 3 :
                code.HasFlag(VariableCode.__ARRAY_2D__) ? 2 :
                code.HasFlag(VariableCode.__ARRAY_1D__) ? 1 : 0) +
               (code.HasFlag(VariableCode.__CHARACTER_DATA__) ? 1 : 0)];

            if (indexes.Length > index.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(indexes));
            }

            indexes.CopyTo(index, 0);


            var target = (int)(code & VariableCode.__LOWERCASE__);
            if (code.HasFlag(VariableCode.__CHARACTER_DATA__))
            {
                if (code.HasFlag(VariableCode.__INTEGER__))
                {
                    switch (index.Length)
                    {
                        case 1: return GlobalStatic.VariableData.CharacterList[index[0]].DataInteger[target];
                        case 2: return GlobalStatic.VariableData.CharacterList[index[0]].DataIntegerArray[target][index[1]];
                        case 3: return GlobalStatic.VariableData.CharacterList[index[0]].DataIntegerArray2D[target][index[1], index[2]];
                        default: throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }
                else
                {
                    switch (index.Length)
                    {
                        case 1: return GlobalStatic.VariableData.CharacterList[index[0]].DataString[target];
                        case 2: return GlobalStatic.VariableData.CharacterList[index[0]].DataStringArray[target][index[1]];
                        case 3: return GlobalStatic.VariableData.CharacterList[index[0]].DataStringArray2D[target][index[1], index[2]];
                        default: throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }
            }
            else
            {
                if (code.HasFlag(VariableCode.__INTEGER__))
                {
                    switch (index.Length)
                    {
                        case 0: return GlobalStatic.VariableData.DataInteger[target];
                        case 1: return GlobalStatic.VariableData.DataIntegerArray[target][index[0]];
                        case 2: return GlobalStatic.VariableData.DataIntegerArray2D[target][index[0], index[1]];
                        case 3: return GlobalStatic.VariableData.DataIntegerArray3D[target][index[0], index[1], index[2]];
                        default: throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }
                else
                {
                    switch (index.Length)
                    {
                        case 0: return GlobalStatic.VariableData.DataString[target];
                        case 1: return GlobalStatic.VariableData.DataStringArray[target][index[0]];
                        case 2: return GlobalStatic.VariableData.DataStringArray2D[target][index[0], index[1]];
                        case 3: return GlobalStatic.VariableData.DataStringArray3D[target][index[0], index[1], index[2]];
                        default: throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }
            }
        }

        void IEmuera.SetValue(string name, object value, params object[] indexes)
        {
            int[] target = new int[indexes.Length];
            var code = (VariableCode)Enum.Parse(typeof(VariableCode), name);
            for (int i = 0; i < indexes.Length; i++)
            {
                try
                {
                    if (indexes[i] is long)
                        target[i] = (int)(long)indexes[i];
                    else if (indexes[i] is int)
                        target[i] = (int)indexes[i];
                    else if (indexes[i] is string)
                    {
                        string temp;
                        int num;
                        if (int.TryParse((string)(indexes[i]), out num))
                            target[i] = num;
                        else
                            target[i] = GlobalStatic.ConstantData.GetKeywordDictionary(out temp, code, i)[(string)indexes[i]];
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    throw new ArgumentException($"알수없는 인덱스 {indexes[i]} 입니다");
                }
            }
            SetValue(code, value, target);
        }

        void SetValue(VariableCode code, object value, int[] indexes)
        {
            var target = (int)(code & VariableCode.__LOWERCASE__);

            int[] index = new int[(
                code.HasFlag(VariableCode.__ARRAY_3D__) ? 3 :
                code.HasFlag(VariableCode.__ARRAY_2D__) ? 2 :
                code.HasFlag(VariableCode.__ARRAY_1D__) ? 1 : 0) +
               (code.HasFlag(VariableCode.__CHARACTER_DATA__) ? 1 : 0)];

            if (indexes.Length > index.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            indexes.CopyTo(index, 0);

            if (code.HasFlag(VariableCode.__CHARACTER_DATA__))
            {
                if (code.HasFlag(VariableCode.__INTEGER__))
                {
                    long num = TryGetLong(value);
                    switch (index.Length)
                    {
                        case 1: GlobalStatic.VariableData.CharacterList[index[0]].DataInteger[target] = num; break;
                        case 2: GlobalStatic.VariableData.CharacterList[index[0]].DataIntegerArray[target][index[1]] = num; break;
                        case 3: GlobalStatic.VariableData.CharacterList[index[0]].DataIntegerArray2D[target][index[1], index[2]] = num; break;
                        default: throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }
                else
                {
                    string str = TryGetString(value);
                    switch (index.Length)
                    {
                        case 1: GlobalStatic.VariableData.CharacterList[index[0]].DataString[target] = str; break;
                        case 2: GlobalStatic.VariableData.CharacterList[index[0]].DataStringArray[target][index[1]] = str; break;
                        case 3: GlobalStatic.VariableData.CharacterList[index[0]].DataStringArray2D[target][index[1], index[2]] = str; break;
                        default: throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }
            }
            else
            {
                if (code.HasFlag(VariableCode.__INTEGER__))
                {
                    long num = TryGetLong(value);
                    switch (index.Length)
                    {
                        case 0: GlobalStatic.VariableData.DataInteger[target] = num; break;
                        case 1: GlobalStatic.VariableData.DataIntegerArray[target][index[0]] = num; break;
                        case 2: GlobalStatic.VariableData.DataIntegerArray2D[target][index[0], index[1]] = num; break;
                        case 3: GlobalStatic.VariableData.DataIntegerArray3D[target][index[0], index[1], index[2]] = num; break;
                        default: throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }
                else
                {
                    string str = TryGetString(value);
                    switch (index.Length)
                    {
                        case 0: GlobalStatic.VariableData.DataString[target] = str; break;
                        case 1: GlobalStatic.VariableData.DataStringArray[target][index[0]] = str; break;
                        case 2: GlobalStatic.VariableData.DataStringArray2D[target][index[0], index[1]] = str; break;
                        case 3: GlobalStatic.VariableData.DataStringArray3D[target][index[0], index[1], index[2]] = str; break;
                        default: throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }
            }
        }

        private static string TryGetString(object value)
        {
            if (!(value is string))
                throw new ArgumentException($"{value} is not string");
            string str = (string)value;
            return str;
        }

        private static long TryGetLong(object value)
        {
            long num;
            if (value is int)
                num = (int)value;
            else if (value is long)
                num = (long)value;
            else if (value is string)
            {
                if (!long.TryParse((string)value, out num))
                    throw new ArgumentException($"{value} is not integer");
            }
            else
            {
                throw new ArgumentException($"{value} is not integer");
            }

            return num;
        }

        int IEmuera.GetColor()
        {
            return GlobalStatic.Console.StringStyle.Color.ToArgb();
        }

        void IEmuera.SetColor(int color)
        {
            GlobalStatic.Console.SetStringStyle(System.Drawing.Color.FromArgb(color));
        }

        void IEmuera.Print(string str, PrintFlags flag)
        {
            var func = ParseLine(flag.ToPrintString() + " " + str);
            if (func == null || !ArgumentParser.SetArgumentTo(func))
                throw new Exception("변환에 실패했습니다 " + flag.ToPrintString() + " " + str);

            func.Function.Instruction.DoInstruction(GlobalStatic.EMediator, func, GlobalStatic.Process.getCurrentState);
            GlobalStatic.Console.RefreshStrings(true);
        }

        private static InstructionLine ParseLine(string rawLine)
        {
            try
            {
                var func = LogicalLineParser.ParseLine(rawLine, GlobalStatic.Console) as InstructionLine;
                if (func != null)
                {
                    if (func.Argument == null)
                    {
                        ArgumentParser.SetArgumentTo(func);
                        if (func.IsError)
                            return null;
                    }
                }
                return func;
            }
            catch
            {
                return null;
            }
        }

        Task<object> IEmuera.GetInputAsync(ConsoleInputType type)
        {
            var req = new InputRequest();
            req.InputType = (InputType)(Enum.Parse(typeof(InputType), ((int)type).ToString()));
            GlobalStatic.Console.WaitInput(req);
            return Task.Run(() =>
            {
                while (!GlobalStatic.Console.IsRunning)
                {
                    Task.Delay(100).Wait();
                }
                return input;
            });
        }

        void IEmuera.AddChara(long charaNo)
        {
            GlobalStatic.VEvaluator.AddCharacter(charaNo);
        }

        void IEmuera.AddCharaFromCSV(long csvNumber)
        {
            GlobalStatic.VEvaluator.AddCharacterFromCsvNo(csvNumber);
        }

        void IEmuera.DelChara(long charaNo)
        {
            GlobalStatic.VEvaluator.DelCharacter(charaNo);
        }

        void IEmuera.DrawLine()
        {
            GlobalStatic.Console.PrintBar();
            GlobalStatic.Console.NewLine();
        }

        void IEmuera.RunRawLine(string rawLine)
        {
            var func = ParseLine(rawLine);
            if (func == null)
                return;
            GlobalStatic.Process.DoDebugNormalFunction(func, false);
            GlobalStatic.Console.RefreshStrings(true);
        }

        bool IEmuera.CheckRawLine(string rawLine)
        {
            return ParseLine(rawLine) != null;
        }

        long[] IEmuera.RegistedCharacters => GlobalStatic.VariableData.CharacterList.Select(chara => chara.NO).ToArray();

        int IEmuera.Encoding => Config.Encode.CodePage;

        string IEmuera.Root => Program.ExeDir;
        #endregion

        public EmueraPlatform(IEnumerable<string> methodNames)
        {
            _methodNames = methodNames;
        }

        internal static void EmueraCall(string labelName, IOperandTerm[] args)
        {
            var result = framework.Call(labelName, args.Select<IOperandTerm, object>(arg =>
              {
                  if (arg.IsInteger)
                      return arg.GetIntValue(GlobalStatic.EMediator);
                  else
                      return arg.GetStrValue(GlobalStatic.EMediator);
              }).ToArray());

            if (result is int)
                GlobalStatic.VEvaluator.RESULT = (int)result;
            else if (result is long)
                GlobalStatic.VEvaluator.RESULT = (long)result;
            else if (result is string)
                GlobalStatic.VEvaluator.RESULTS = (string)result;
        }

        object Call(string name, object[] args)
        {
            if (name == null)
                throw new ArgumentNullException();
            int intIndex = 0, strIndex = 0;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is long || args[i] is int)
                    ((IEmuera)this).SetValue("ARG", args[i], intIndex++);
                else if (args[i] is string)
                    ((IEmuera)this).SetValue("ARGS", args[i], strIndex++);
                else
                    throw new ArgumentException("매개변수는 string과 int, long 형식만 가능합니다", nameof(args));
            }

            var func = CalledFunction.CallFunction(GlobalStatic.Process, name, null);
            if (func == null)
                throw new ArgumentException($"Method [{name}] is undefined");

            GlobalStatic.Process.getCurrentState.IntoFunction(func, null, GlobalStatic.EMediator);
            GlobalStatic.Console.callEmueraProgram("");
            GlobalStatic.Console.RefreshStrings(true);

            if (returnType == typeof(long))
                return ((IEmuera)this).GetValue("RESULT", 0);
            else
                return null;
        }

        void Begin(SystemFunctionCode code, IFramework framework)
        {
            GlobalStatic.Process.getCurrentState.SetBegin(code.ToString());
            GlobalStatic.Console.callEmueraProgram("");
            GlobalStatic.Console.RefreshStrings(true);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.

                using (FileStream fs = new FileStream(Program.ExeDir + Program.ConfigFileName, FileMode.Create))
                {
                    ConfigDic.Save(new StreamWriter(fs));
                }
                EZTrans.TranslateXP.SaveDictionary(Program.ExeDir + "UserDic.xml");
                EZTrans.TranslateXP.Terminate();
                EZTrans.TranslateXP.SaveCache();
                _methodNames = null;
                disposedValue = true;
            }
        }

        //TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~EmueraPlatform()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(false);
        }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        void IDisposable.Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
