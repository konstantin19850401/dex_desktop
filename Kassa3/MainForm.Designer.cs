namespace Kassa3
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.tsmiFirmsList = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDictionaries = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDicUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDicCurrencies = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDicClients = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDicOps = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDicFirmAcc = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAppSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNewRec = new System.Windows.Forms.ToolStripMenuItem();
            this.импортToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiIbank2Import = new System.Windows.Forms.ToolStripMenuItem();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.tsslUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.pBalance = new System.Windows.Forms.Panel();
            this.gc = new DevExpress.XtraGrid.GridControl();
            this.gv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pBalCaption = new System.Windows.Forms.Panel();
            this.cbBalType = new System.Windows.Forms.ComboBox();
            this.deBalEnd = new DEXExtendLib.DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.deBalStart = new DEXExtendLib.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.cbBalFrom = new System.Windows.Forms.CheckBox();
            this.bBalanceParameters = new System.Windows.Forms.Button();
            this.bBalanceFold = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tsmiJournal = new System.Windows.Forms.ToolStripMenuItem();
            this.msMain.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.pBalance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
            this.pBalCaption.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFirmsList,
            this.tsmiDictionaries,
            this.tsmiWindows,
            this.tsmiAppSettings,
            this.tsmiNewRec,
            this.импортToolStripMenuItem,
            this.tsmiJournal});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(1112, 24);
            this.msMain.TabIndex = 1;
            this.msMain.Text = "menuStrip1";
            // 
            // tsmiFirmsList
            // 
            this.tsmiFirmsList.Name = "tsmiFirmsList";
            this.tsmiFirmsList.Size = new System.Drawing.Size(60, 20);
            this.tsmiFirmsList.Text = "Фирмы";
            // 
            // tsmiDictionaries
            // 
            this.tsmiDictionaries.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDicUsers,
            this.tsmiDicCurrencies,
            this.tsmiDicClients,
            this.tsmiDicOps,
            this.tsmiDicFirmAcc});
            this.tsmiDictionaries.Name = "tsmiDictionaries";
            this.tsmiDictionaries.Size = new System.Drawing.Size(94, 20);
            this.tsmiDictionaries.Text = "Справочники";
            // 
            // tsmiDicUsers
            // 
            this.tsmiDicUsers.Name = "tsmiDicUsers";
            this.tsmiDicUsers.Size = new System.Drawing.Size(158, 22);
            this.tsmiDicUsers.Text = "Пользователи";
            this.tsmiDicUsers.Click += new System.EventHandler(this.tsmiDicUsers_Click);
            // 
            // tsmiDicCurrencies
            // 
            this.tsmiDicCurrencies.Name = "tsmiDicCurrencies";
            this.tsmiDicCurrencies.Size = new System.Drawing.Size(158, 22);
            this.tsmiDicCurrencies.Text = "Валюты";
            this.tsmiDicCurrencies.Click += new System.EventHandler(this.tsmiDicCurrencies_Click);
            // 
            // tsmiDicClients
            // 
            this.tsmiDicClients.Name = "tsmiDicClients";
            this.tsmiDicClients.Size = new System.Drawing.Size(158, 22);
            this.tsmiDicClients.Text = "Контрагенты";
            this.tsmiDicClients.Click += new System.EventHandler(this.tsmiDicClients_Click);
            // 
            // tsmiDicOps
            // 
            this.tsmiDicOps.Name = "tsmiDicOps";
            this.tsmiDicOps.Size = new System.Drawing.Size(158, 22);
            this.tsmiDicOps.Text = "Операции";
            this.tsmiDicOps.Click += new System.EventHandler(this.tsmiDicOps_Click);
            // 
            // tsmiDicFirmAcc
            // 
            this.tsmiDicFirmAcc.Name = "tsmiDicFirmAcc";
            this.tsmiDicFirmAcc.Size = new System.Drawing.Size(158, 22);
            this.tsmiDicFirmAcc.Text = "Фирмы и счета";
            this.tsmiDicFirmAcc.Click += new System.EventHandler(this.tsmiDicFirmAcc_Click);
            // 
            // tsmiWindows
            // 
            this.tsmiWindows.Name = "tsmiWindows";
            this.tsmiWindows.Size = new System.Drawing.Size(47, 20);
            this.tsmiWindows.Text = "Окна";
            // 
            // tsmiAppSettings
            // 
            this.tsmiAppSettings.Name = "tsmiAppSettings";
            this.tsmiAppSettings.Size = new System.Drawing.Size(149, 20);
            this.tsmiAppSettings.Text = "Установки приложения";
            this.tsmiAppSettings.Click += new System.EventHandler(this.tsmiAppSettings_Click);
            // 
            // tsmiNewRec
            // 
            this.tsmiNewRec.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmiNewRec.Name = "tsmiNewRec";
            this.tsmiNewRec.Size = new System.Drawing.Size(93, 20);
            this.tsmiNewRec.Text = "Новая запись";
            this.tsmiNewRec.Click += new System.EventHandler(this.tsmiNewRec_Click);
            // 
            // импортToolStripMenuItem
            // 
            this.импортToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiIbank2Import});
            this.импортToolStripMenuItem.Name = "импортToolStripMenuItem";
            this.импортToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.импортToolStripMenuItem.Text = "Импорт";
            // 
            // tsmiIbank2Import
            // 
            this.tsmiIbank2Import.Name = "tsmiIbank2Import";
            this.tsmiIbank2Import.Size = new System.Drawing.Size(209, 22);
            this.tsmiIbank2Import.Text = "Импорт из файла iBank2";
            this.tsmiIbank2Import.Click += new System.EventHandler(this.tsmiIbank2Import_Click);
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslUser});
            this.ssStatus.Location = new System.Drawing.Point(0, 619);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(1112, 24);
            this.ssStatus.TabIndex = 3;
            this.ssStatus.Text = "statusStrip1";
            // 
            // tsslUser
            // 
            this.tsslUser.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslUser.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsslUser.Name = "tsslUser";
            this.tsslUser.Size = new System.Drawing.Size(122, 19);
            this.tsslUser.Text = "toolStripStatusLabel1";
            // 
            // pBalance
            // 
            this.pBalance.Controls.Add(this.gc);
            this.pBalance.Controls.Add(this.pBalCaption);
            this.pBalance.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBalance.Location = new System.Drawing.Point(0, 445);
            this.pBalance.Name = "pBalance";
            this.pBalance.Size = new System.Drawing.Size(1112, 174);
            this.pBalance.TabIndex = 5;
            // 
            // gc
            // 
            this.gc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gc.EmbeddedNavigator.Enabled = false;
            this.gc.Location = new System.Drawing.Point(0, 30);
            this.gc.MainView = this.gv;
            this.gc.Name = "gc";
            this.gc.Size = new System.Drawing.Size(1112, 144);
            this.gc.TabIndex = 3;
            this.gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv});
            // 
            // gv
            // 
            this.gv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gv.GridControl = this.gc;
            this.gv.Name = "gv";
            this.gv.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gv.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gv.OptionsBehavior.AutoPopulateColumns = false;
            this.gv.OptionsBehavior.Editable = false;
            this.gv.OptionsBehavior.ReadOnly = true;
            this.gv.OptionsCustomization.AllowGroup = false;
            this.gv.OptionsCustomization.AllowQuickHideColumns = false;
            this.gv.OptionsMenu.EnableColumnMenu = false;
            this.gv.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gv.OptionsView.EnableAppearanceEvenRow = true;
            this.gv.OptionsView.EnableAppearanceOddRow = true;
            this.gv.OptionsView.ShowFooter = true;
            this.gv.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gv.OptionsView.ShowGroupPanel = false;
            this.gv.OptionsView.ShowIndicator = false;
            this.gv.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gv_CustomDrawCell);
            this.gv.CustomDrawFooterCell += new DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventHandler(this.gv_CustomDrawFooterCell);
            this.gv.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gv_CustomColumnDisplayText);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // pBalCaption
            // 
            this.pBalCaption.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pBalCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBalCaption.Controls.Add(this.cbBalType);
            this.pBalCaption.Controls.Add(this.deBalEnd);
            this.pBalCaption.Controls.Add(this.label2);
            this.pBalCaption.Controls.Add(this.deBalStart);
            this.pBalCaption.Controls.Add(this.label1);
            this.pBalCaption.Controls.Add(this.cbBalFrom);
            this.pBalCaption.Controls.Add(this.bBalanceParameters);
            this.pBalCaption.Controls.Add(this.bBalanceFold);
            this.pBalCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.pBalCaption.Location = new System.Drawing.Point(0, 0);
            this.pBalCaption.Name = "pBalCaption";
            this.pBalCaption.Size = new System.Drawing.Size(1112, 30);
            this.pBalCaption.TabIndex = 0;
            // 
            // cbBalType
            // 
            this.cbBalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBalType.FormattingEnabled = true;
            this.cbBalType.Items.AddRange(new object[] {
            "Отчёт по валютам",
            "Отчёт по фин.группам"});
            this.cbBalType.Location = new System.Drawing.Point(383, 3);
            this.cbBalType.Name = "cbBalType";
            this.cbBalType.Size = new System.Drawing.Size(146, 21);
            this.cbBalType.TabIndex = 10;
            // 
            // deBalEnd
            // 
            this.deBalEnd.FormattingEnabled = true;
            this.deBalEnd.InputChar = '_';
            this.deBalEnd.Location = new System.Drawing.Point(294, 3);
            this.deBalEnd.MaxLength = 10;
            this.deBalEnd.Name = "deBalEnd";
            this.deBalEnd.Size = new System.Drawing.Size(83, 21);
            this.deBalEnd.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(269, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "по";
            // 
            // deBalStart
            // 
            this.deBalStart.FormattingEnabled = true;
            this.deBalStart.InputChar = '_';
            this.deBalStart.Location = new System.Drawing.Point(180, 3);
            this.deBalStart.MaxLength = 10;
            this.deBalStart.Name = "deBalStart";
            this.deBalStart.Size = new System.Drawing.Size(83, 21);
            this.deBalStart.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(90, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Период";
            // 
            // cbBalFrom
            // 
            this.cbBalFrom.AutoSize = true;
            this.cbBalFrom.Location = new System.Drawing.Point(141, 5);
            this.cbBalFrom.Name = "cbBalFrom";
            this.cbBalFrom.Size = new System.Drawing.Size(33, 17);
            this.cbBalFrom.TabIndex = 7;
            this.cbBalFrom.Text = "С";
            this.cbBalFrom.UseVisualStyleBackColor = true;
            this.cbBalFrom.CheckedChanged += new System.EventHandler(this.cbBalFrom_CheckedChanged);
            // 
            // bBalanceParameters
            // 
            this.bBalanceParameters.Dock = System.Windows.Forms.DockStyle.Right;
            this.bBalanceParameters.Location = new System.Drawing.Point(940, 0);
            this.bBalanceParameters.Name = "bBalanceParameters";
            this.bBalanceParameters.Size = new System.Drawing.Size(170, 28);
            this.bBalanceParameters.TabIndex = 6;
            this.bBalanceParameters.Text = "Дополнительные параметры";
            this.bBalanceParameters.UseVisualStyleBackColor = true;
            this.bBalanceParameters.Click += new System.EventHandler(this.bBalanceParameters_Click);
            // 
            // bBalanceFold
            // 
            this.bBalanceFold.Dock = System.Windows.Forms.DockStyle.Left;
            this.bBalanceFold.Location = new System.Drawing.Point(0, 0);
            this.bBalanceFold.Name = "bBalanceFold";
            this.bBalanceFold.Size = new System.Drawing.Size(84, 28);
            this.bBalanceFold.TabIndex = 0;
            this.bBalanceFold.Text = "Обновить";
            this.bBalanceFold.UseVisualStyleBackColor = true;
            this.bBalanceFold.Click += new System.EventHandler(this.bBalanceFold_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 442);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1112, 3);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // tsmiJournal
            // 
            this.tsmiJournal.Name = "tsmiJournal";
            this.tsmiJournal.Size = new System.Drawing.Size(127, 20);
            this.tsmiJournal.Text = "Журнал изменений";
            this.tsmiJournal.Click += new System.EventHandler(this.tsmiJournal_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 643);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pBalance);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.msMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.msMain;
            this.Name = "MainForm";
            this.Text = "Касса 3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.pBalance.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
            this.pBalCaption.ResumeLayout(false);
            this.pBalCaption.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiDictionaries;
        private System.Windows.Forms.ToolStripMenuItem tsmiDicUsers;
        private System.Windows.Forms.StatusStrip ssStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsslUser;
        private System.Windows.Forms.ToolStripMenuItem tsmiDicCurrencies;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiDicClients;
        private System.Windows.Forms.ToolStripMenuItem tsmiDicOps;
        private System.Windows.Forms.ToolStripMenuItem tsmiDicFirmAcc;
        private System.Windows.Forms.ToolStripMenuItem tsmiFirmsList;
        private System.Windows.Forms.ToolStripMenuItem tsmiWindows;
        private System.Windows.Forms.ToolStripMenuItem tsmiNewRec;
        private System.Windows.Forms.Panel pBalance;
        private System.Windows.Forms.Panel pBalCaption;
        private System.Windows.Forms.Button bBalanceFold;
        private System.Windows.Forms.Button bBalanceParameters;
        private System.Windows.Forms.Splitter splitter1;
        private DEXExtendLib.DateEdit deBalEnd;
        private System.Windows.Forms.Label label2;
        private DEXExtendLib.DateEdit deBalStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbBalFrom;
        private DevExpress.XtraGrid.GridControl gc;
        private DevExpress.XtraGrid.Views.Grid.GridView gv;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.ComboBox cbBalType;
        private System.Windows.Forms.ToolStripMenuItem импортToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiIbank2Import;
        private System.Windows.Forms.ToolStripMenuItem tsmiJournal;
    }
}

