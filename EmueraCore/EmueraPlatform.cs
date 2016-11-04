using MinorShift.Emuera.GameData.Variable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameProc.Function;
using MinorShift.Emuera.GameProc;
using YeongHun.EmueraFramework;
using YeongHun.EmueraFramework.Function;

namespace MinorShift.Emuera
{
    class EmueraPlatform : IPlatform
    {
        internal static IFramework framework;
        internal static Type returnType = typeof(void);
        internal static object input;
        IEnumerable<string> _methodNames;

        #region IPlatform

        string IPlatform.Name => "Emuera";

        Method[] IPlatform.Methods
        {
            get
            {
                return _methodNames.Select(method => new Method(method, args => Call(method, args))).ToArray();
            }
        }

        void IPlatform.Initialize(IFramework framework)
        {
            EmueraPlatform.framework = framework;
            GlobalStatic.Console.state = GameView.ConsoleState.Running;
        }

        private static InstructionLine ParseLine(string rawLine)
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
        #endregion

        public EmueraPlatform(IEnumerable<string> methodNames)
        {
            _methodNames = methodNames;
        }

        internal static void EmueraCall(string labelName,IOperandTerm[] args)
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
                    framework.Data["ARG",intIndex++] = args[i];
                else if (args[i] is string)
                    framework.Data["ARGS", strIndex++] = args[i];
                else
                    throw new ArgumentException("매개변수는 string과 int, long 형식만 가능합니다", nameof(args));
            }

            var func = CalledFunction.CallFunction(GlobalStatic.Process, name, null);
            if (func == null)
                throw new ArgumentException($"Method [{name}] is undefined");

            GlobalStatic.Process.getCurrentState.IntoFunction(func, null, GlobalStatic.EMediator);
            GlobalStatic.Process.DoScript();

            if (returnType == typeof(long))
                return framework.Data.RESULT;
            else
                return null;
        }

        void IDisposable.Dispose()
        {
            return;
        }
    }
}
