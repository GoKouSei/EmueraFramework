using MinorShift.Emuera.GameData.Variable;
using SharedLibrary;
using SharedLibrary.Function;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MinorShift.Emuera
{
    class EmueraPlatform : IEmuera
    {
        internal static IFramework framework;
        IEnumerable<string> _methodNames;

        #region IEmuera
        string IPlatform.Name
        {
            get
            {
                return "EmueraCore";
            }
        }

        SystemFunction[] IPlatform.systemFunctions
        {
            get
            {
                return (from codeName in Enum.GetNames(typeof(SystemFunctionCode))
                        select (SystemFunctionCode)(Enum.Parse(typeof(SystemFunctionCode), codeName)) into code
                        select new SystemFunction(code, framework => Begin(code, framework))).ToArray();
            }
        }

        Method[] IPlatform.methods
        {
            get
            {
                return _methodNames.Select(method => new Method(method, args => Call(method, args))).ToArray();
            }
        }

        void IPlatform.Initialize(List<Tuple<string, Stream>> source, IFramework framework)
        {
            EmueraPlatform.framework = framework;
            GlobalStatic.Console.state = GameView.ConsoleState.Running;
        }

        object IEmuera.GetValue(string name, params int[] indexes)
        {
            var code = (VariableCode)Enum.Parse(typeof(VariableCode), name);

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

        void IEmuera.SetValue(string name, object value, params int[] indexes)
        {
            var code = (VariableCode)Enum.Parse(typeof(VariableCode), name);
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

        long[] IEmuera.RegistedCharacters => GlobalStatic.VariableData.CharacterList.Select(chara => chara.NO).ToArray();
        #endregion

        public EmueraPlatform(IEnumerable<string> methodNames)
        {
            _methodNames = methodNames;
        }


        object Call(string name, object[] args)
        {
            if (name == null)
                throw new ArgumentNullException();
            var func = GameProc.CalledFunction.CallFunction(GlobalStatic.Process, name, null);
            if (func == null)
                throw new ArgumentException($"Method [{name}] is undefined");
            GlobalStatic.Process.getCurrentState.IntoFunction(func, null, GlobalStatic.EMediator);
            return null;
        }

        void Begin(SystemFunctionCode code, IFramework framework)
        {
            GlobalStatic.Process.getCurrentState.SetBegin(code.ToString());
            GlobalStatic.Process.DoScript();
        }

    }
}
