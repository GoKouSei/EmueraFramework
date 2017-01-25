﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameProc;
using System.IO;
using MinorShift.Emuera.GameView;

namespace MinorShift.Emuera.Forms
{
	public partial class DebugDialog : Form
	{
		public DebugDialog()
		{
			InitializeComponent();
			listViewWatch.AfterLabelEdit += new LabelEditEventHandler(listViewWatch_AfterLabelEdit);

			this.TopMost = Config.DebugWindowTopMost;
			int width = Math.Max(this.MinimumSize.Width, Config.DebugWindowWidth);
			int height = Math.Max(this.MinimumSize.Height, Config.DebugWindowHeight);
			this.Size = new Size(width, height);
			if (Config.DebugSetWindowPos)
			{
				this.StartPosition = FormStartPosition.Manual;
				this.Location = new Point(Config.DebugWindowPosX, Config.DebugWindowPosY);
			}
			updateSize();
			checkBoxTopMost.Checked = this.TopMost;
			loadWatchList();
            LoadBreakPoint();
            breakPoint.Image = new Bitmap(breakPoint.Width, breakPoint.Height);
            sourceText.BackColor = Color.White;
            sourceText.ForeColor = Color.Black;
            breakPoint.BackColor = BackColor;
		}
		private Process emuera = null;
		private EmueraConsole mainConsole = null;

		internal void SetParent(EmueraConsole console, Process process)
		{
			emuera = process;
			mainConsole = console;
		}

		public string ConsoleText
		{
			get { return textBoxConsole.Text; }
			set { textBoxConsole.Text = value; }
		}
		public string TraceText
		{
			get { return textBoxTrace.Text; }
			set { textBoxTrace.Text = value; }
		}
		public void AddTraceText(string str)
		{
			this.SuspendLayout();
			textBoxTrace.Text += str;
			this.ResumeLayout(false);
		}

		public void UpdateData()
		{
			if (tabControlMain.SelectedTab == tabPageWatch)
				updateVarWatch();
			else if (tabControlMain.SelectedTab == tabPageTrace)
				updateTrace();
			else if (tabControlMain.SelectedTab == tabPageConsole)
				updateConsole();
		}

        private bool wait = false;
		private bool nextBreak = false;
		private Dictionary<string,string[]>lines=new Dictionary<string, string[]>();
        internal List<ScriptPosition> breakPoints = new List<ScriptPosition>();
		private ScriptPosition lastPos = null;

        private void SaveBreakPoint()
        {
            using(FileStream debugDat=new FileStream(Program.DebugDir + "Debug.dat", FileMode.Create))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(debugDat, breakPoints);
            }
        }

        private void LoadBreakPoint()
        {
            if (!File.Exists(Program.DebugDir + "Debug.dat"))
                return;
            try
            {
                using (FileStream debugDat = new FileStream(Program.DebugDir + "Debug.dat", FileMode.Open))
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    breakPoints = formatter.Deserialize(debugDat) as List<ScriptPosition>;
                }
            }
            catch
            {
                return;
            }
        }

		private void InitSource(string fileName)
		{
			if(!lines.ContainsKey(fileName))
			{
				lines.Add(fileName, File.ReadAllLines(Program.ErbDir+fileName));
			}
			
			sourceText.Clear();
			sourceText.Lines=lines[fileName];
		}

		internal void UpdatePosition(ScriptPosition pos)
		{
            if (lastPos?.Filename != pos.Filename)
            {
                InitSource(pos.Filename);
            }
            else if (lastPos != null)
            {
                if (breakPoints.Contains(lastPos))
                    MarkBreakPoint(lastPos);
                else
                    CleanLine(lastPos);
            }
            lastPos = pos;
            SelectLine(pos.LineNo - 1, Color.FromArgb(0x00C6C971), Color.Black, true);
            if (wait)
            {
                //sourceText.Refresh();
                System.Threading.Tasks.Task.Delay(600).Wait();
                wait = false;
            }
            if (nextBreak)
            {
                nextBreak = false;
                SetBreak();
            }

            Application.DoEvents();
        }
        
        private void MarkBreakPoint(ScriptPosition pos) => SelectLine(pos.LineNo - 1, Color.DarkRed, Color.White);
        private void CleanLine(ScriptPosition pos) => SelectLine(pos.LineNo - 1, Color.White, Color.Black);

        private void SelectLine(int lineNo,Color back,Color text,bool scroll=false)
        {
            var lineLength = sourceText.Lines[lineNo].Length;
            sourceText.Select(sourceText.GetFirstCharIndexFromLine(lineNo), lineLength);
            sourceText.SelectionColor = text;
            sourceText.SelectionBackColor = back;
            if(scroll)
                sourceText.ScrollToCaret();
            sourceText.DeselectAll();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainConsole.PressEnterKey(false, "", false);
        }

        void StepToolStripMenuItemClick(object sender, EventArgs e)
		{
            wait = true;
            nextBreak = true;
            if (mainConsole.IsWaitingEnterKey)
			{
				mainConsole.PressEnterKey(false, "", false);
            }
		}

		void SetBreak()
		{
            mainConsole.ReadAnyKey(anykey: true);
		}

        void UpdateBreakPoint()
        {
            if (lastPos == null)
                return;
            var bitmap = new Bitmap(breakPoint.Width, breakPoint.Height);
            var g = Graphics.FromImage(bitmap);
            var b = new SolidBrush(breakPoint.BackColor);
            g.FillRectangle(b, 0, 0, breakPoint.Width, breakPoint.Height);

            var topPoint = sourceText.Location;
            var firstLine = sourceText.GetLineFromCharIndex(sourceText.GetCharIndexFromPosition(topPoint));
            var lastLine = sourceText.GetLineFromCharIndex(sourceText.GetCharIndexFromPosition(new Point(topPoint.X, topPoint.Y + sourceText.Height)));

            var lineHeight = sourceText.Font.Height;
            b.Color = Color.DarkRed;
            for (int i = firstLine; i <= lastLine; i++)
            {
                var currentPos = new ScriptPosition(lastPos.Filename, i+1);
                
                if(breakPoints.Contains(currentPos))
                {
                    if (lastPos != currentPos)
                        MarkBreakPoint(currentPos);
                    var circle = new Rectangle((int)(sourceText.Font.Size / 3),
                    sourceText.GetPositionFromCharIndex(sourceText.GetFirstCharIndexFromLine(i)).Y + (int)(sourceText.Font.Size / 2), (int)(sourceText.Font.Size), (int)(sourceText.Font.Size));
                    g.FillEllipse(b, circle);
                }
            }
            breakPoint.Image = bitmap;
        }

        private void sourceText_VScroll(object sender, EventArgs e)
        {
            UpdateBreakPoint();
        }

        void BreakPointMouseClick(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
                var lineNo = sourceText.GetLineFromCharIndex(
                    sourceText.GetCharIndexFromPosition(new Point(0, e.Y + sourceText.Location.Y)));
			    var selectedPos = new ScriptPosition(lastPos.Filename, lineNo + 1);
                if (breakPoints.Contains(selectedPos))
                {
                    CleanLine(selectedPos);
                    breakPoints.Remove(selectedPos);
                }
                else
                {
                    breakPoints.Add(selectedPos);
                }
                UpdateBreakPoint();
			}
		}

		void SourceTextKeyDown(object sender, KeyEventArgs e)
		{
            if (e.KeyCode == Keys.F11)
                StepToolStripMenuItemClick(sender, e);
            else if (e.KeyCode == Keys.F5)
                runToolStripMenuItem_Click(sender, e);
		}

		private void tabControlMain_Selected(object sender, TabControlEventArgs e)
		{
			UpdateData();
			updateSize();
		}

		private void updateTrace()
		{
			string str = mainConsole.GetDebugTraceLog(false);
			if(str != null)
				textBoxTrace.Text = str;
			//textBoxTrace.SelectionStart = textBoxTrace.Text.Length;
			//textBoxTrace.Focus();
			//textBoxTrace.ScrollToCaret();
		}

		private void updateConsole()
		{
			textBoxConsole.Text = mainConsole.DebugConsoleLog;
			//textBoxConsole.SelectionStart = textBoxConsole.Text.Length;
			//textBoxConsole.Focus();
			//textBoxConsole.ScrollToCaret();
		}

		private void updateVarWatch()
		{
            GlobalStatic.Process.saveCurrentState(false);
			for (int i = 0; i < listViewWatch.Items.Count - 1; i++)
			{//無名のアイテムを削除
				if (listViewWatch.Items[i].Text.Length == 0)
				{
					listViewWatch.Items.RemoveAt(i);
					i--;
				}
			}
			if ((listViewWatch.Items.Count == 0) || (!string.IsNullOrEmpty(listViewWatch.Items[listViewWatch.Items.Count - 1].Text)))
			{
				ListViewItem newLVI = new ListViewItem("");
				newLVI.SubItems.Add(new ListViewItem.ListViewSubItem(newLVI, ""));
				listViewWatch.Items.Add(newLVI);
			}
			foreach(ListViewItem lvi in listViewWatch.Items)
			{
				lvi.SubItems[1].Text = getValueString(lvi.Text);
			}
			GlobalStatic.Process.clearMethodStack();
            GlobalStatic.Process.loadPrevState();
			this.Update();
		}
		private string getValueString(string str)
		{
			if ((emuera == null) || (GlobalStatic.EMediator == null))
				return "";
			if (string.IsNullOrEmpty(str))
				return "";
			mainConsole.RunERBFromMemory = true;
			try
			{
				StringStream st = new StringStream(str);
				WordCollection wc = LexicalAnalyzer.Analyse(st, LexEndWith.EoL, LexAnalyzeFlag.None);
				IOperandTerm term = ExpressionParser.ReduceExpressionTerm(wc, TermEndWith.EoL);
				SingleTerm value = term.GetValue(GlobalStatic.EMediator);
				return value.ToString();
			}
			catch (CodeEE e)
			{
				return e.Message;
			}
			catch (Exception e)
			{
				return e.GetType().ToString() + ":" + e.Message;
			}
			finally
			{
				mainConsole.RunERBFromMemory = false;
			}

		}
		private void listViewWatch_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (string.IsNullOrEmpty(e.Label))
			{
			//	if (e.Item != listViewWatch.Items.Count - 1)
			//		listViewWatch.Items.RemoveAt(e.Item);
			}
			else
			{
				listViewWatch.Items[e.Item].SubItems[1].Text = getValueString(e.Label);
				if (e.Item == listViewWatch.Items.Count - 1)
				{
					ListViewItem newLVI = new ListViewItem("");
					newLVI.SubItems.Add(new ListViewItem.ListViewSubItem(newLVI, ""));
					listViewWatch.Items.Add(newLVI);
				}
			}
		}

        private void checkBoxTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBoxTopMost.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 閉じるToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ウォッチリストの読込ToolStripMenuItem_Click(object sender, EventArgs e)
        {
			loadWatchList();
			updateVarWatch();
        }

        private void ウォッチリストの保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
			saveWatchList();
        }


		private readonly string watchFilepath = Program.DebugDir + "watchlist.csv";
		private readonly string traceFilepath = Program.DebugDir + "trace.log";
		private readonly string consoleFilepath = Program.DebugDir + "console.log";

		private void saveData()
		{
			saveWatchList();
            SaveBreakPoint();

			StreamWriter writer = null;
			//トレースの仕様をいじってるうちに保存する意味が無いものになった
			//try
			//{
			//    writer = new StreamWriter(traceFilepath, false, StaticConfig.Encode);
			//    writer.Write(mainConsole.GetDebugTraceLog(true));
			//}
			//catch
			//{
			//    MessageBox.Show("トレースログの保存に失敗しました", "デバッグウインドウ");
			//    return;
			//}
			//finally
			//{
			//    if (writer != null)
			//        writer.Close();
			//}
			writer = null;
			try
			{
				writer = new StreamWriter(consoleFilepath, false, Config.Encode);
				writer.Write(mainConsole.DebugConsoleLog);
			}
			catch
			{
				MessageBox.Show("コンソールログの保存に失敗しました", "デバッグウインドウ");
				return;
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}

		private void saveWatchList()
		{
			StreamWriter writer = null;
			try
			{
				writer = new StreamWriter(watchFilepath, false, Config.Encode);
				foreach (ListViewItem lvi in listViewWatch.Items)
					if (!string.IsNullOrEmpty(lvi.Text))
						writer.WriteLine(lvi.Text);
			}
			catch
			{
				MessageBox.Show("変数ウォッチリストの保存に失敗しました", "デバッグウインドウ");
				return;
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}

		private void loadWatchList()
		{
			if (!File.Exists(watchFilepath))
				return;
			List<string> saveStrList = new List<string>();

			StreamReader reader = null;
			try
			{
				reader = new StreamReader(watchFilepath, Config.Encode);
				string line = null;
				while ((line = reader.ReadLine()) != null)
					if (line.Length > 0)
						saveStrList.Add(line);
			}
			catch
			{
				MessageBox.Show("変数ウォッチリストの読込に失敗しました", "デバッグウインドウ");
				return;
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}

			listViewWatch.Items.Clear();
			foreach (string str in saveStrList)
			{
				if (!string.IsNullOrEmpty(str))
				{
					ListViewItem newLVI = new ListViewItem(str);
					newLVI.SubItems.Add(new ListViewItem.ListViewSubItem(newLVI, ""));
					listViewWatch.Items.Add(newLVI);
				}
			}
		}

        private void DebugDialog_Activated(object sender, EventArgs e)
        {
            UpdateData();
        }


		private void updateSize()
		{
			if (this.WindowState == FormWindowState.Minimized)
				return;

			if (tabControlMain.SelectedTab == tabPageConsole)
			{//タブ切り替え直後の時点ではtabPageConsole.Heightは更新されていないのでtabControlMain.Heightより推定するしかない
				//textBoxConsole.Height = tabPageConsole.Height - textBoxCommand.Height - 9;
				textBoxConsole.Height = tabControlMain.Height - 26 - textBoxCommand.Height - 9;
			}
		}

		private void DebugDialog_Resize(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
				return;
			//環境依存かもしれない。誰かに指摘されたら考えよう。
			tabControlMain.Height = this.Size.Height - 103;
			sourceText.Height = tabControlMain.Height - 26;
			updateSize();
			
		}

		private void listViewWatch_KeyUp(object sender, KeyEventArgs e)
		{
			//F2キーで名前の変更。
			if (e.KeyCode == Keys.F2 && listViewWatch.FocusedItem != null)
			{
				listViewWatch.FocusedItem.BeginEdit();
			}
		}

		private void DebugDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			saveData();
		}


		private void listViewWatch_MouseUp(object sender, MouseEventArgs e)
		{
			ListViewItem item = listViewWatch.GetItemAt(e.X, e.Y);
			if (item != null)
			{
				item.Selected = true;
				item.BeginEdit();
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			//これをクリックする時点で情報が最新でないことは普通ないので実はあんまり意味が無い。
			//最新の情報であることを確認するためのボタンってことで
			UpdateData();
		}

		private void textBoxCommand_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				e.SuppressKeyPress = true;
				if (!mainConsole.IsInProcess && textBoxCommand.Text.Length > 0)
				{
					mainConsole.DebugPrint(textBoxCommand.Text);
					mainConsole.DebugNewLine();
					mainConsole.DebugCommand(textBoxCommand.Text, false, true);
					updateConsole();
					textBoxConsole.SelectionStart = textBoxConsole.Text.Length;
					textBoxConsole.Focus();
					textBoxConsole.ScrollToCaret();
					updateInputs();
					textBoxCommand.Focus();
				}
				return;
			}
			if (e.KeyCode == Keys.Up)
			{
				e.SuppressKeyPress = true;
				if (mainConsole.IsInProcess)
					return;
				movePrev(-1);
				return;
			}
			if (e.KeyCode == Keys.Down)
			{
				e.SuppressKeyPress = true;
				if (mainConsole.IsInProcess)
					return;
				movePrev(1);
				return;
			}
		}

		//1750 MainWindowからほぼコピペ
		string[] prevInputs = new string[100];
		int selectedInputs = 100;
		int lastSelected = 100;
		void updateInputs()
		{
			string cur = textBoxCommand.Text;
			if (string.IsNullOrEmpty(cur))
				return;
			for (int i = 0; i < prevInputs.Length - 1; i++)
			{
				prevInputs[i] = prevInputs[i + 1];
			}
			prevInputs[prevInputs.Length - 1] = cur;
			//entered = console.IsWaintingOnePhrase;
			textBoxCommand.Text = "";
			//1729a eramakerと同じ処理系に変更 1730a 再修正
			if (selectedInputs != prevInputs.Length && cur == prevInputs[selectedInputs - 1])
				lastSelected = --selectedInputs;
			else
				lastSelected = 100;
			selectedInputs = prevInputs.Length;
		}
		void movePrev(int move)
		{
			if (move == 0)
				return;
			//if((selectedInputs != prevInputs.Length) &&(prevInputs[selectedInputs] != richTextBox1.Text))
			//	selectedInputs =  prevInputs.Length;
			int next;
			if (lastSelected != prevInputs.Length && selectedInputs == prevInputs.Length)
			{
				if (move == -1)
					move = 0;
				next = lastSelected + move;
			}
			else
				next = selectedInputs + move;
			if ((next < 0) || (next > prevInputs.Length))
				return;
			if (next == prevInputs.Length)
			{
				selectedInputs = next;
				textBoxCommand.Text = "";
				return;
			}
			if (string.IsNullOrEmpty(prevInputs[next]))
				if (++next == prevInputs.Length)
					return;

			selectedInputs = next;
			textBoxCommand.Text = prevInputs[next];
			textBoxCommand.SelectionStart = 0;
			textBoxCommand.SelectionLength = textBoxCommand.Text.Length;
			return;
		}

		private void 設定ToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			bool tempTopMost = TopMost;
			this.TopMost = false;
			DebugConfigDialog dialog = new DebugConfigDialog();
            dialog.StartPosition = FormStartPosition.CenterParent;
			dialog.SetConfig(this);
			dialog.ShowDialog();
			this.TopMost = tempTopMost;
		}

    }
}
