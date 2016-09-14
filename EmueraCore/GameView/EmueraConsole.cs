using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.IO;
using MinorShift._Library;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameData;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameProc.Function;
using System.Timers;

namespace MinorShift.Emuera.GameView
{
	//入出力待ちの状況。
	//難読化用属性。enum.ToString()やenum.Parse()を行うなら(Exclude=true)にすること。
	[global::System.Reflection.Obfuscation(Exclude=false)]
	internal enum ConsoleState
	{
		Initializing = 0,
		Quit = 5,//QUIT
		Error = 6,//Exceptionによる強制終了
		Running = 7,
		WaitInput = 20,

		//WaitKey = 1,//WAIT
		//WaitSystemInteger = 2,//Systemが要求するInput
		//WaitInteger = 3,//INPUT
		//WaitString = 4,//INPUTS
		//WaitIntegerWithTimer = 8,
		//WaitStringWithTimer = 9,
		//Timeout = 10,
		//Timeouts = 11,
		//WaitKeyWithTimer = 12,
		//WaitKeyWithTimerF = 13,
		//WaitOneInteger = 14,
		//WaitOneString = 15,
		//WaitOneIntegerWithTimer = 16,
		//WaitOneStringWithTimer = 17,
		//WaitAnyKey = 18,

    }

	//難読化用属性。enum.ToString()やenum.Parse()を行うなら(Exclude=true)にすること。
	[global::System.Reflection.Obfuscation(Exclude=false)]
	internal enum ConsoleRedraw
	{
		None = 0,
		Normal = 1,
	}

	internal sealed partial class EmueraConsole
	{
		public EmueraConsole()
		{

			//1.713 この段階でsetStBarを使用してはいけない
			//setStBar(StaticConfig.DrawLineString);
			state = ConsoleState.Initializing;
        }
		const string ErrorButtonsText = "__openFileWithDebug__";

		MinorShift.Emuera.GameProc.Process emuera;
		ConsoleState state = ConsoleState.Initializing;
		public bool Enabled { get { return true; } }

		/// <summary>
		/// スクリプトが継続中かどうか
		/// 入力系はメッセージスキップやマクロも含めてIsInProcessを参照すべき
		/// </summary>
		internal bool IsRunning
		{
			get
			{
				if (state == ConsoleState.Initializing)
					return true;
				return (state == ConsoleState.Running);
			}
		}

		internal bool IsInProcess
		{
			get
			{
				if (state == ConsoleState.Initializing)
					return true;
				return (state == ConsoleState.Running);
			}
		}

		internal bool IsError
		{
			get
			{
				return state == ConsoleState.Error;
			}
		}

		public void Initialize()
		{
			GlobalStatic.Console = this;
            emuera = new GameProc.Process(this);
			GlobalStatic.Process = emuera;
			if (!emuera.Initialize())
			{
				state = ConsoleState.Error;
				return;
			}
		}
		

        public void Quit() { state = ConsoleState.Quit; }
		public void ThrowTitleError(bool error)
		{
			state = ConsoleState.Error;
		}
		public void ThrowError(bool playSound)
		{
			if (playSound)
				System.Media.SystemSounds.Hand.Play();
		}

		bool force_temporary = false;
        bool timer_suspended = false;
		ConsoleState prevState;
		InputRequest prevReq;

		public void ReloadErb()
		{
            prevState = state;
			state = ConsoleState.Initializing;
			force_temporary = true;
			emuera.ReloadErb();
			force_temporary = false;
        }

		public void ReloadErbFinished()
		{
			state = prevState;
		}

		public void ReloadPartialErb(List<string> path)
		{
			prevState = state;
			state = ConsoleState.Initializing;
			force_temporary = true;
			emuera.ReloadPartialErb(path);
			force_temporary = false;
        }

		public void ReloadFolder(string erbPath)
		{
            List<string> paths = new List<string>();
			SearchOption op = SearchOption.AllDirectories;
			if (!Config.SearchSubdirectory)
				op = SearchOption.TopDirectoryOnly;
			string[] fnames = Directory.GetFiles(erbPath, "*.ERB", op);
			for (int i = 0; i < fnames.Length; i++)
				if (Path.GetExtension(fnames[i]).ToUpper() == ".ERB")
					paths.Add(fnames[i]);
			prevState = state;
			state = ConsoleState.Initializing;
			force_temporary = true;
            emuera.ReloadPartialErb(paths);
			force_temporary = false;
            //強制的にボタン世代が切り替わるのを防ぐ
        }
	}
}