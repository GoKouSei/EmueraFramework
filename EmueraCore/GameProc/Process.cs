using System;
using System.Collections.Generic;
using System.IO;
using MinorShift.Emuera.GameData;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.GameProc.Function;
using MinorShift.Emuera.GameData.Function;
using MinorShift.Emuera.GameView;

namespace MinorShift.Emuera.GameProc
{

	internal sealed partial class Process
	{
		public Process(EmueraConsole view)
		{
			console = view;
		}

        public LogicalLine getCurrentLine { get { return state.CurrentLine; } }

		/// <summary>
		/// @~~と$~~を集めたもの。CALL命令などで使う
		/// 実行順序はLogicalLine自身が保持する。
		/// </summary>
		LabelDictionary labelDic;
		public LabelDictionary LabelDictionary { get { return labelDic; } }

		/// <summary>
		/// 変数全部。スクリプト中で必要になる変数は（ユーザーが直接触れないものも含め）この中にいれる
		/// </summary>
		private VariableEvaluator vEvaluator;
		public VariableEvaluator VEvaluator { get { return vEvaluator; } }
		private ExpressionMediator exm;
		private GameBase gamebase;
		readonly EmueraConsole console;
		private IdentifierDictionary idDic;
		ProcessState state;
		ProcessState originalState;//リセットする時のために
        bool noError = false;
        //色々あって復活させてみる
        bool initialiing;
        public bool inInitializeing { get { return initialiing;  } }

        public bool Initialize()
		{
			LexicalAnalyzer.UseMacro = false;
            state = new ProcessState(console);
            originalState = state;
            initialiing = true;
            try
            {
				ParserMediator.Initialize(console);
				if (ParserMediator.HasWarning)
				{
					ParserMediator.FlushWarningList();
				}
				
                if (Config.UseKeyMacro && !Program.AnalysisMode)
                {
                    if (File.Exists(Program.ExeDir + "macro.txt"))
                    {
                        KeyMacro.LoadMacroFile(Program.ExeDir + "macro.txt");
                    }
                }
                if (Config.UseReplaceFile && !Program.AnalysisMode)
                {
					if (File.Exists(Program.CsvDir + "_Replace.csv"))
					{
						ConfigData.Instance.LoadReplaceFile(Program.CsvDir + "_Replace.csv");
						if (ParserMediator.HasWarning)
						{
							ParserMediator.FlushWarningList();
						}
					}
                }
                Config.SetReplace(ConfigData.Instance);
                //ここでBARを設定すれば、いいことに気づいた予感

				if (Config.UseRenameFile)
                {
                    if (File.Exists(Program.CsvDir + "_Rename.csv"))
                    {
                        ParserMediator.LoadEraExRenameFile(Program.CsvDir + "_Rename.csv");
                    }
                }
				gamebase = new GameBase();
                if (!gamebase.LoadGameBaseCsv(Program.CsvDir + "GAMEBASE.CSV"))
                {
                    return false;
                }
				GlobalStatic.GameBaseData = gamebase;

				ConstantData constant = new ConstantData(gamebase);
				constant.LoadData(Program.CsvDir, console, Config.DisplayReport);
				GlobalStatic.ConstantData = constant;
				TrainName = constant.GetCsvNameList(VariableCode.TRAINNAME);

                vEvaluator = new VariableEvaluator(gamebase, constant);
				GlobalStatic.VEvaluator = vEvaluator;

				idDic = new IdentifierDictionary(vEvaluator.VariableData);
				GlobalStatic.IdentifierDictionary = idDic;

				StrForm.Initialize();
				VariableParser.Initialize();

				exm = new ExpressionMediator(this, vEvaluator, console);
				GlobalStatic.EMediator = exm;

				labelDic = new LabelDictionary();
				GlobalStatic.LabelDictionary = labelDic;
				HeaderFileLoader hLoader = new HeaderFileLoader(console, idDic, this);

				LexicalAnalyzer.UseMacro = false;
				if (!hLoader.LoadHeaderFiles(Program.ErbDir, Config.DisplayReport))
				{
					return false;
				}
				LexicalAnalyzer.UseMacro = idDic.UseMacro();

				ErbLoader loader = new ErbLoader(console, exm, this);
                if (Program.AnalysisMode)
                    noError = loader.loadErbs(Program.AnalysisFiles, labelDic);
                else
                    noError = loader.LoadErbFiles(Program.ErbDir, Config.DisplayReport, labelDic);
                initSystemProcess();
                initialiing = false;
            }
			catch (Exception e)
			{
				return false;
			}
			if (labelDic == null)
			{
				return false;
			}
			state.Begin(BeginType.TITLE);
			GC.Collect();
            return true;
		}

		public void ReloadErb()
		{
			saveCurrentState(false);
			state.SystemState = SystemStateCode.System_Reloaderb;
			ErbLoader loader = new ErbLoader(console, exm, this);
            loader.LoadErbFiles(Program.ErbDir, false, labelDic);
		}

		public void ReloadPartialErb(List<string> path)
		{
			saveCurrentState(false);
			state.SystemState = SystemStateCode.System_Reloaderb;
			ErbLoader loader = new ErbLoader(console, exm, this);
			loader.loadErbs(path, labelDic);
		}

		public void SetCommnds(Int64 count)
		{
			coms = new List<long>((int)count);
			isCTrain = true;
			Int64[] selectcom = vEvaluator.SELECTCOM_ARRAY;
			if (count >= selectcom.Length)
			{
				throw new CodeEE("CALLTRAIN命令の引数の値がSELECTCOMの要素数を超えています");
			}
			for (int i = 0; i < (int)count; i++)
			{
				coms.Add(selectcom[i + 1]);
			}
		}

        public bool ClearCommands()
        {
            coms.Clear();
            count = 0;
            isCTrain = false;
            skipPrint = true;
            return (callFunction("CALLTRAINEND", false, false));
        }

		public void InputInteger(Int64 i)
		{
			vEvaluator.RESULT = i;
		}
		public void InputSystemInteger(Int64 i)
		{
			systemResult = i;
		}
		public void InputString(string s)
		{
			vEvaluator.RESULTS = s;
		}

		private uint startTime = 0;
		
		public void DoScript()
		{
			startTime = _Library.WinmmTimer.TickCount;
			state.lineCount = 0;
			bool systemProcRunning = true;
			try
			{
				while (true)
				{
					methodStack = 0;
					systemProcRunning = true;
                    while (state.ScriptEnd && console.IsRunning)
                    {
                        if (state.SystemState == SystemStateCode.Normal)
                            return;//End Method
                        runSystemProc();
                    }
					if (!console.IsRunning)
						break;
					systemProcRunning = false;
					runScriptProc();
				}
			}
			catch (Exception ec)
			{
				LogicalLine currentLine = state.ErrorLine;
				if (currentLine != null && currentLine is NullLine)
					currentLine = null;
			}
		}
		
		public void BeginTitle()
		{
			vEvaluator.ResetData();
			state = originalState;
			state.Begin(BeginType.TITLE);
		}

		private void checkInfiniteLoop()
		{
			//うまく動かない。BEEP音が鳴るのを止められないのでこの処理なかったことに（1.51）
			////フリーズ防止。処理中でも履歴を見たりできる
			//System.Windows.Forms.Application.DoEvents();
			////System.Threading.Thread.Sleep(0);

			//if (!console.Enabled)
			//{
			//    //DoEvents()の間にウインドウが閉じられたらおしまい。
			//    console.ReadAnyKey();
			//    return;
			//}
			uint time = _Library.WinmmTimer.TickCount - startTime;
			if (time < Config.InfiniteLoopAlertTime)
				return;
			LogicalLine currentLine = state.CurrentLine;
			if ((currentLine == null) || (currentLine is NullLine))
				return;//現在の行が特殊な状態ならスルー
			if (!console.Enabled)
				return;//クローズしてるとMessageBox.Showができないので。
			string caption = string.Format("無限ループの可能性があります");
			string text = string.Format(
				"現在、{0}の{1}行目を実行中です。\n最後の入力から{3}ミリ秒経過し{2}行が実行されました。\n処理を中断し強制終了しますか？",
				currentLine.Position.Filename, currentLine.Position.LineNo, state.lineCount, time);
		}

		int methodStack = 0;
		public SingleTerm GetValue(SuperUserDefinedMethodTerm udmt)
		{
			methodStack++;
            if (methodStack > 100)
            {
                //StackOverflowExceptionはcatchできない上に再現性がないので発生前に一定数で打ち切る。
                //環境によっては100以前にStackOverflowExceptionがでるかも？
                throw new CodeEE("関数の呼び出しスタックが溢れました(無限に再帰呼び出しされていませんか？)");
            }
            SingleTerm ret = null;
            int temp_current = state.currentMin;
            state.currentMin = state.functionCount;
            udmt.Call.updateRetAddress(state.CurrentLine);
            try
            {
				state.IntoFunction(udmt.Call, udmt.Argument, exm);
                //do whileの中でthrow されたエラーはここではキャッチされない。
				//#functionを全て抜けてDoScriptでキャッチされる。
    			runScriptProc();
                ret = state.MethodReturnValue;
			}
			finally
			{
				if (udmt.Call.TopLabel.hasPrivDynamicVar)
					udmt.Call.TopLabel.Out();
                //1756beta2+v3:こいつらはここにないとデバッグコンソールで式中関数が事故った時に大事故になる
                state.currentMin = temp_current;
                methodStack--;
            }
			return ret;
		}

        public void clearMethodStack()
        {
            methodStack = 0;
        }

        public int MethodStack()
        {
            return methodStack;
        }

		public ScriptPosition GetRunningPosition()
		{
			LogicalLine line = state.ErrorLine;
			if (line == null)
				return null;
			return line.Position;
		}

		private readonly string scaningScope = null;
		private string GetScaningScope()
		{
			if (scaningScope != null)
				return scaningScope;
			return state.Scope;
		}

		public LogicalLine scaningLine = null;
		internal LogicalLine GetScaningLine()
		{
			if (scaningLine != null)
				return scaningLine;
			LogicalLine line = state.ErrorLine;
			if (line == null)
				return null;
			return line;
		}


	}
}
