using System;
using System.Drawing;
using System.Collections.Generic;
using MinorShift._Library;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.GameData.Expression;
using System.IO;
using System.Linq;

namespace MinorShift.Emuera
{
	public static class Program
	{
		/*
		コードの開始地点。
		ここでMainWindowを作り、
		MainWindowがProcessを作り、
		ProcessがGameBase・ConstantData・Variableを作る。
		
		
		*.ERBの読み込み、実行、その他の処理をProcessが、
		入出力をMainWindowが、
		定数の保存をConstantDataが、
		変数の管理をVariableが行う。
		 
		と言う予定だったが改変するうちに境界が曖昧になってしまった。
		 
		後にEmueraConsoleを追加し、それに入出力を担当させることに。
        
        1750 DebugConsole追加
         Debugを全て切り離すことはできないので一部EmueraConsoleにも担当させる
		
		TODO: 1819 MainWindow & Consoleの入力・表示組とProcess&Dataのデータ処理組だけでも分離したい

		*/
		public static SharedLibrary.IPlatform Main(string root)
		{

            ExeDir = root;
#if DEBUG
			//debugMode = true;
#endif
			CsvDir = ExeDir + "CSV\\";
			ErbDir = ExeDir + "ERB\\";
			DebugDir = ExeDir + "debug\\";
			DatDir = ExeDir + "dat\\";
			ContentDir = ExeDir + "resources\\";
			//エラー出力用
			//1815 .exeが東方板のNGワードに引っかかるそうなので除去
			ExeName = "Emuera.exe";
            
			ConfigData.Instance.LoadConfig();

            EmueraConsole console = new EmueraConsole();
            console.Initialize();

            return new EmueraPlatform(GlobalStatic.LabelDictionary.GetAllLabelName());
		}

		/// <summary>
		/// 実行ファイルのディレクトリ。最後に\を付けたstring
		/// </summary>
		public static string ExeDir { get; private set; }
		public static string CsvDir { get; private set; }
		public static string ErbDir { get; private set; }
		public static string DebugDir { get; private set; }
		public static string DatDir { get; private set; }
		public static string ContentDir { get; private set; }
		public static string ExeName { get; private set; }

		public static bool Reboot = false;
		//public static int RebootClientX = 0;
		public static int RebootClientY = 0;

        public static bool AnalysisMode = false;
        public static List<string> AnalysisFiles = null;

		public static bool debugMode = false;
		public static bool DebugMode { get { return debugMode; } }


		public static uint StartTime { get; private set; }

	}
}