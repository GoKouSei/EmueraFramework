using MinorShift.Emuera.GameProc.Function;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Function;
using System.IO;
using MinorShift.Emuera.GameData.Variable;

namespace MinorShift.Emuera
{
    class EmueraPlatform : IEmuera
    {
        internal static IFramework framework;
        IEnumerable<string> _methodNames;

        public string Name
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

        public EmueraPlatform(IEnumerable<string> methodNames)
        {
            _methodNames = methodNames;
        }

        public void Initialize(List<Tuple<string, Stream>> source, IFramework framework)
        {
            EmueraPlatform.framework = framework;
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

        public string GetStrValue(string name, params int[] indexes)
        {
            var code = (int)((VariableCode)Enum.Parse(typeof(VariableCode), name)&VariableCode.__LOWERCASE__);
            switch (indexes.Length)
            {
                case 0: return GlobalStatic.VariableData.DataString[code];
                case 1: return GlobalStatic.VariableData.DataStringArray[code][indexes[0]];
                case 2: return GlobalStatic.VariableData.DataStringArray2D[code][indexes[0],indexes[1]];
                case 3: return GlobalStatic.VariableData.DataStringArray3D[code][indexes[0], indexes[1], indexes[2]];
                default: throw new ArgumentOutOfRangeException(nameof(indexes));
            }
        }

        public void SetStrValue(string name, string value, params int[] indexes)
        {
            var code = (int)((VariableCode)Enum.Parse(typeof(VariableCode), name) & VariableCode.__LOWERCASE__);
            switch (indexes.Length)
            {
                case 0: GlobalStatic.VariableData.DataString[code] = value; break;
                case 1: GlobalStatic.VariableData.DataStringArray[code][indexes[0]] = value; break;
                case 2: GlobalStatic.VariableData.DataStringArray2D[code][indexes[0], indexes[1]] = value; break;
                case 3: GlobalStatic.VariableData.DataStringArray3D[code][indexes[0], indexes[1], indexes[2]] = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(indexes));
            }
        }

        public long GetIntValue(string name, params int[] indexes)
        {
            var code = (int)((VariableCode)Enum.Parse(typeof(VariableCode), name) & VariableCode.__LOWERCASE__);
            switch (indexes.Length)
            {
                case 0: return GlobalStatic.VariableData.DataInteger[code];
                case 1: return GlobalStatic.VariableData.DataIntegerArray[code][indexes[0]];
                case 2: return GlobalStatic.VariableData.DataIntegerArray2D[code][indexes[0], indexes[1]];
                case 3: return GlobalStatic.VariableData.DataIntegerArray3D[code][indexes[0], indexes[1], indexes[2]];
                default: throw new ArgumentOutOfRangeException(nameof(indexes));
            }
        }

        public void SetIntValue(string name, long value, params int[] indexes)
        {
            var code = (int)((VariableCode)Enum.Parse(typeof(VariableCode), name) & VariableCode.__LOWERCASE__);
            switch (indexes.Length)
            {
                case 0: GlobalStatic.VariableData.DataInteger[code] = value; break;
                case 1: GlobalStatic.VariableData.DataIntegerArray[code][indexes[0]] = value; break;
                case 2: GlobalStatic.VariableData.DataIntegerArray2D[code][indexes[0], indexes[1]] = value; break;
                case 3: GlobalStatic.VariableData.DataIntegerArray3D[code][indexes[0], indexes[1], indexes[2]] = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(indexes));
            }
        }

        public void AddChara(long charaNumber)
        {
            GlobalStatic.VEvaluator.AddCharacter(charaNumber);
        }

        public void AddCharaFromCSV(long csvNumber)
        {
            GlobalStatic.VEvaluator.AddCharacterFromCsvNo(csvNumber);
        }
    }
}
