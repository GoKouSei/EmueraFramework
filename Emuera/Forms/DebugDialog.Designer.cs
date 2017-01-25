namespace MinorShift.Emuera.Forms
{
	partial class DebugDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ウォッチリストの保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ウォッチリストの読込ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.閉じるToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.設定ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageWatch = new System.Windows.Forms.TabPage();
            this.listViewWatch = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageTrace = new System.Windows.Forms.TabPage();
            this.textBoxTrace = new System.Windows.Forms.TextBox();
            this.tabPageConsole = new System.Windows.Forms.TabPage();
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.currentProcessLine = new System.Windows.Forms.TabPage();
            this.breakPoint = new System.Windows.Forms.PictureBox();
            this.sourceText = new System.Windows.Forms.RichTextBox();
            this.checkBoxTopMost = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageWatch.SuspendLayout();
            this.tabPageTrace.SuspendLayout();
            this.tabPageConsole.SuspendLayout();
            this.currentProcessLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.breakPoint)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.設定ToolStripMenuItem,
            this.runToolStripMenuItem,
            this.stepToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(448, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ウォッチリストの保存ToolStripMenuItem,
            this.ウォッチリストの読込ToolStripMenuItem,
            this.閉じるToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(81, 20);
            this.toolStripMenuItem1.Text = "ファイル(&F)";
            // 
            // ウォッチリストの保存ToolStripMenuItem
            // 
            this.ウォッチリストの保存ToolStripMenuItem.Name = "ウォッチリストの保存ToolStripMenuItem";
            this.ウォッチリストの保存ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.ウォッチリストの保存ToolStripMenuItem.Text = "ウォッチリストの保存";
            this.ウォッチリストの保存ToolStripMenuItem.Click += new System.EventHandler(this.ウォッチリストの保存ToolStripMenuItem_Click);
            // 
            // ウォッチリストの読込ToolStripMenuItem
            // 
            this.ウォッチリストの読込ToolStripMenuItem.Name = "ウォッチリストの読込ToolStripMenuItem";
            this.ウォッチリストの読込ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.ウォッチリストの読込ToolStripMenuItem.Text = "ウォッチリストの読込";
            this.ウォッチリストの読込ToolStripMenuItem.Click += new System.EventHandler(this.ウォッチリストの読込ToolStripMenuItem_Click);
            // 
            // 閉じるToolStripMenuItem
            // 
            this.閉じるToolStripMenuItem.Name = "閉じるToolStripMenuItem";
            this.閉じるToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.閉じるToolStripMenuItem.Text = "閉じる";
            this.閉じるToolStripMenuItem.Click += new System.EventHandler(this.閉じるToolStripMenuItem_Click);
            // 
            // 設定ToolStripMenuItem
            // 
            this.設定ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.設定ToolStripMenuItem1});
            this.設定ToolStripMenuItem.Name = "設定ToolStripMenuItem";
            this.設定ToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.設定ToolStripMenuItem.Text = "設定(&C)";
            // 
            // 設定ToolStripMenuItem1
            // 
            this.設定ToolStripMenuItem1.Name = "設定ToolStripMenuItem1";
            this.設定ToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.設定ToolStripMenuItem1.Text = "コンフィグ(&C)";
            this.設定ToolStripMenuItem1.Click += new System.EventHandler(this.設定ToolStripMenuItem1_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.runToolStripMenuItem.Text = "Run(F5)";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // stepToolStripMenuItem
            // 
            this.stepToolStripMenuItem.Name = "stepToolStripMenuItem";
            this.stepToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.stepToolStripMenuItem.Text = "Step(F11)";
            this.stepToolStripMenuItem.Click += new System.EventHandler(this.StepToolStripMenuItemClick);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageWatch);
            this.tabControlMain.Controls.Add(this.tabPageTrace);
            this.tabControlMain.Controls.Add(this.tabPageConsole);
            this.tabControlMain.Controls.Add(this.currentProcessLine);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControlMain.Location = new System.Drawing.Point(0, 24);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(448, 197);
            this.tabControlMain.TabIndex = 4;
            this.tabControlMain.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControlMain_Selected);
            // 
            // tabPageWatch
            // 
            this.tabPageWatch.Controls.Add(this.listViewWatch);
            this.tabPageWatch.Location = new System.Drawing.Point(4, 22);
            this.tabPageWatch.Name = "tabPageWatch";
            this.tabPageWatch.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageWatch.Size = new System.Drawing.Size(440, 171);
            this.tabPageWatch.TabIndex = 0;
            this.tabPageWatch.Text = "変数ウォッチ";
            this.tabPageWatch.UseVisualStyleBackColor = true;
            // 
            // listViewWatch
            // 
            this.listViewWatch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.listViewWatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewWatch.LabelEdit = true;
            this.listViewWatch.Location = new System.Drawing.Point(3, 3);
            this.listViewWatch.Name = "listViewWatch";
            this.listViewWatch.Size = new System.Drawing.Size(434, 165);
            this.listViewWatch.TabIndex = 0;
            this.listViewWatch.UseCompatibleStateImageBehavior = false;
            this.listViewWatch.View = System.Windows.Forms.View.Details;
            this.listViewWatch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listViewWatch_KeyUp);
            this.listViewWatch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewWatch_MouseUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "対象";
            this.columnHeader1.Width = 147;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "値";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 97;
            // 
            // tabPageTrace
            // 
            this.tabPageTrace.Controls.Add(this.textBoxTrace);
            this.tabPageTrace.Location = new System.Drawing.Point(4, 22);
            this.tabPageTrace.Name = "tabPageTrace";
            this.tabPageTrace.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageTrace.Size = new System.Drawing.Size(440, 171);
            this.tabPageTrace.TabIndex = 1;
            this.tabPageTrace.Text = "スタックトレース";
            this.tabPageTrace.UseVisualStyleBackColor = true;
            // 
            // textBoxTrace
            // 
            this.textBoxTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTrace.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxTrace.Location = new System.Drawing.Point(3, 3);
            this.textBoxTrace.Multiline = true;
            this.textBoxTrace.Name = "textBoxTrace";
            this.textBoxTrace.ReadOnly = true;
            this.textBoxTrace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxTrace.Size = new System.Drawing.Size(434, 165);
            this.textBoxTrace.TabIndex = 0;
            // 
            // tabPageConsole
            // 
            this.tabPageConsole.Controls.Add(this.textBoxCommand);
            this.tabPageConsole.Controls.Add(this.textBoxConsole);
            this.tabPageConsole.Location = new System.Drawing.Point(4, 22);
            this.tabPageConsole.Name = "tabPageConsole";
            this.tabPageConsole.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageConsole.Size = new System.Drawing.Size(440, 171);
            this.tabPageConsole.TabIndex = 2;
            this.tabPageConsole.Text = "コンソール";
            this.tabPageConsole.UseVisualStyleBackColor = true;
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxCommand.Location = new System.Drawing.Point(3, 147);
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.Size = new System.Drawing.Size(434, 21);
            this.textBoxCommand.TabIndex = 0;
            this.textBoxCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCommand_KeyDown);
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxConsole.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxConsole.Location = new System.Drawing.Point(3, 3);
            this.textBoxConsole.Multiline = true;
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ReadOnly = true;
            this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxConsole.Size = new System.Drawing.Size(434, 143);
            this.textBoxConsole.TabIndex = 1;
            // 
            // currentProcessLine
            // 
            this.currentProcessLine.Controls.Add(this.breakPoint);
            this.currentProcessLine.Controls.Add(this.sourceText);
            this.currentProcessLine.Location = new System.Drawing.Point(4, 22);
            this.currentProcessLine.Name = "currentProcessLine";
            this.currentProcessLine.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.currentProcessLine.Size = new System.Drawing.Size(440, 171);
            this.currentProcessLine.TabIndex = 3;
            this.currentProcessLine.Text = "Current Process Line";
            this.currentProcessLine.UseVisualStyleBackColor = true;
            // 
            // breakPoint
            // 
            this.breakPoint.Location = new System.Drawing.Point(0, 0);
            this.breakPoint.Name = "breakPoint";
            this.breakPoint.Size = new System.Drawing.Size(20, 175);
            this.breakPoint.TabIndex = 1;
            this.breakPoint.TabStop = false;
            this.breakPoint.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BreakPointMouseClick);
            // 
            // sourceText
            // 
            this.sourceText.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceText.Location = new System.Drawing.Point(20, 0);
            this.sourceText.Name = "sourceText";
            this.sourceText.ReadOnly = true;
            this.sourceText.Size = new System.Drawing.Size(421, 175);
            this.sourceText.TabIndex = 0;
            this.sourceText.Text = "";
            this.sourceText.WordWrap = false;
            this.sourceText.VScroll += new System.EventHandler(this.sourceText_VScroll);
            this.sourceText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SourceTextKeyDown);
            // 
            // checkBoxTopMost
            // 
            this.checkBoxTopMost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxTopMost.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxTopMost.AutoSize = true;
            this.checkBoxTopMost.Location = new System.Drawing.Point(14, 229);
            this.checkBoxTopMost.Name = "checkBoxTopMost";
            this.checkBoxTopMost.Size = new System.Drawing.Size(87, 22);
            this.checkBoxTopMost.TabIndex = 6;
            this.checkBoxTopMost.Text = "最前面に表示";
            this.checkBoxTopMost.UseVisualStyleBackColor = true;
            this.checkBoxTopMost.CheckedChanged += new System.EventHandler(this.checkBoxTopMost_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(330, 229);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 22);
            this.button1.TabIndex = 7;
            this.button1.Text = "閉じる";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(219, 229);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 22);
            this.button2.TabIndex = 8;
            this.button2.Text = "データ更新";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DebugDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 263);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxTopMost);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(371, 240);
            this.Name = "DebugDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Emuera - デバッグウインドウ";
            this.Activated += new System.EventHandler(this.DebugDialog_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugDialog_FormClosing);
            this.Resize += new System.EventHandler(this.DebugDialog_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageWatch.ResumeLayout(false);
            this.tabPageTrace.ResumeLayout(false);
            this.tabPageTrace.PerformLayout();
            this.tabPageConsole.ResumeLayout(false);
            this.tabPageConsole.PerformLayout();
            this.currentProcessLine.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.breakPoint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ウォッチリストの保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ウォッチリストの読込ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 閉じるToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageWatch;
        private System.Windows.Forms.ListView listViewWatch;
        private System.Windows.Forms.TabPage tabPageTrace;
        private System.Windows.Forms.CheckBox checkBoxTopMost;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.TextBox textBoxTrace;
		private System.Windows.Forms.TabPage tabPageConsole;
		private System.Windows.Forms.TextBox textBoxConsole;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox textBoxCommand;
		private System.Windows.Forms.ToolStripMenuItem 設定ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 設定ToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem stepToolStripMenuItem;
		private System.Windows.Forms.TabPage currentProcessLine;
		private System.Windows.Forms.RichTextBox sourceText;
		private System.Windows.Forms.PictureBox breakPoint;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
    }
}