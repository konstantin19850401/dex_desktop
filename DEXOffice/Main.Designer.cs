namespace DEXOffice
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetupDocuments = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetupReports = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetupFunctions = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetupDictionaries = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetupSchedules = new System.Windows.Forms.ToolStripMenuItem();
            this.miReports = new System.Windows.Forms.ToolStripMenuItem();
            this.miFunctions = new System.Windows.Forms.ToolStripMenuItem();
            this.miDictionaries = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMainButtons = new System.Windows.Forms.ToolStrip();
            this.pNav = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.cbScan = new System.Windows.Forms.ComboBox();
            this.cbTypeCreateDoc = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnIgnorReg = new System.Windows.Forms.Button();
            this.cbPlans = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbSearchMethod = new System.Windows.Forms.ComboBox();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbDocSearch = new System.Windows.Forms.TextBox();
            this.bMultiUnits = new System.Windows.Forms.Button();
            this.bAllJournal = new System.Windows.Forms.Button();
            this.cbFilterImmediate = new System.Windows.Forms.CheckBox();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDocType = new System.Windows.Forms.ComboBox();
            this.cbRecLimit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.cbDateTo = new System.Windows.Forms.CheckBox();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.bNavSetFilter = new System.Windows.Forms.Button();
            this.bNavOpenClose = new System.Windows.Forms.Button();
            this.pJournal = new System.Windows.Forms.Panel();
            this.dgvJournal = new System.Windows.Forms.DataGridView();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vdocdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unittitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vdoctype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.digest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.usertitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsJournalhook = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsDocControl = new System.Windows.Forms.ToolStrip();
            this.tsddbPagination = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsddbDocuments = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbEditDocument = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteDocument = new System.Windows.Forms.ToolStripButton();
            this.tsbApprove = new System.Windows.Forms.ToolStripButton();
            this.tsbCloneDocument = new System.Windows.Forms.ToolStripButton();
            this.tsbPrintDoc = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiPreviewDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPrintDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPronterSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSchemesSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDataFields = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbChangeStatus = new System.Windows.Forms.ToolStripButton();
            this.toolStripPrintTest = new System.Windows.Forms.ToolStripButton();
            this.lNoRecords = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslDocCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSelCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsddbJournalType = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiJournalModeJournal = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiJournalModeArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.pLog = new System.Windows.Forms.Panel();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spLog = new System.Windows.Forms.Splitter();
            this.msMain.SuspendLayout();
            this.pNav.SuspendLayout();
            this.pJournal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournal)).BeginInit();
            this.cmsJournalhook.SuspendLayout();
            this.tsDocControl.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSettings,
            this.miReports,
            this.miFunctions,
            this.miDictionaries});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(1246, 24);
            this.msMain.TabIndex = 0;
            this.msMain.Text = "menuStrip1";
            // 
            // miSettings
            // 
            this.miSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSetupDocuments,
            this.miSetupReports,
            this.miSetupFunctions,
            this.miSetupDictionaries,
            this.miSetupSchedules});
            this.miSettings.Name = "miSettings";
            this.miSettings.Size = new System.Drawing.Size(79, 20);
            this.miSettings.Text = "Настройки";
            // 
            // miSetupDocuments
            // 
            this.miSetupDocuments.Name = "miSetupDocuments";
            this.miSetupDocuments.Size = new System.Drawing.Size(206, 22);
            this.miSetupDocuments.Text = "Документы";
            // 
            // miSetupReports
            // 
            this.miSetupReports.Name = "miSetupReports";
            this.miSetupReports.Size = new System.Drawing.Size(206, 22);
            this.miSetupReports.Text = "Отчёты";
            // 
            // miSetupFunctions
            // 
            this.miSetupFunctions.Name = "miSetupFunctions";
            this.miSetupFunctions.Size = new System.Drawing.Size(206, 22);
            this.miSetupFunctions.Text = "Функции";
            // 
            // miSetupDictionaries
            // 
            this.miSetupDictionaries.Name = "miSetupDictionaries";
            this.miSetupDictionaries.Size = new System.Drawing.Size(206, 22);
            this.miSetupDictionaries.Text = "Справочники";
            // 
            // miSetupSchedules
            // 
            this.miSetupSchedules.Name = "miSetupSchedules";
            this.miSetupSchedules.Size = new System.Drawing.Size(206, 22);
            this.miSetupSchedules.Text = "Задания планировщика";
            // 
            // miReports
            // 
            this.miReports.Name = "miReports";
            this.miReports.Size = new System.Drawing.Size(60, 20);
            this.miReports.Text = "Отчёты";
            // 
            // miFunctions
            // 
            this.miFunctions.Name = "miFunctions";
            this.miFunctions.Size = new System.Drawing.Size(68, 20);
            this.miFunctions.Text = "Функции";
            // 
            // miDictionaries
            // 
            this.miDictionaries.Name = "miDictionaries";
            this.miDictionaries.Size = new System.Drawing.Size(94, 20);
            this.miDictionaries.Text = "Справочники";
            // 
            // tsMainButtons
            // 
            this.tsMainButtons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMainButtons.Location = new System.Drawing.Point(0, 24);
            this.tsMainButtons.Name = "tsMainButtons";
            this.tsMainButtons.Size = new System.Drawing.Size(1246, 25);
            this.tsMainButtons.TabIndex = 1;
            this.tsMainButtons.Text = "toolStrip1";
            // 
            // pNav
            // 
            this.pNav.BackColor = System.Drawing.SystemColors.Control;
            this.pNav.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pNav.Controls.Add(this.label9);
            this.pNav.Controls.Add(this.cbScan);
            this.pNav.Controls.Add(this.cbTypeCreateDoc);
            this.pNav.Controls.Add(this.label8);
            this.pNav.Controls.Add(this.btnIgnorReg);
            this.pNav.Controls.Add(this.cbPlans);
            this.pNav.Controls.Add(this.label7);
            this.pNav.Controls.Add(this.cbSearchMethod);
            this.pNav.Controls.Add(this.cbStatus);
            this.pNav.Controls.Add(this.label6);
            this.pNav.Controls.Add(this.tbDocSearch);
            this.pNav.Controls.Add(this.bMultiUnits);
            this.pNav.Controls.Add(this.bAllJournal);
            this.pNav.Controls.Add(this.cbFilterImmediate);
            this.pNav.Controls.Add(this.cbUnit);
            this.pNav.Controls.Add(this.label5);
            this.pNav.Controls.Add(this.label4);
            this.pNav.Controls.Add(this.cbDocType);
            this.pNav.Controls.Add(this.cbRecLimit);
            this.pNav.Controls.Add(this.label3);
            this.pNav.Controls.Add(this.label2);
            this.pNav.Controls.Add(this.dtpDateTo);
            this.pNav.Controls.Add(this.cbDateTo);
            this.pNav.Controls.Add(this.dtpDateFrom);
            this.pNav.Controls.Add(this.label1);
            this.pNav.Controls.Add(this.bNavSetFilter);
            this.pNav.Controls.Add(this.bNavOpenClose);
            this.pNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pNav.Location = new System.Drawing.Point(0, 49);
            this.pNav.Name = "pNav";
            this.pNav.Padding = new System.Windows.Forms.Padding(1);
            this.pNav.Size = new System.Drawing.Size(1246, 87);
            this.pNav.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(769, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Наличие скана";
            // 
            // cbScan
            // 
            this.cbScan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScan.FormattingEnabled = true;
            this.cbScan.Location = new System.Drawing.Point(854, 3);
            this.cbScan.Name = "cbScan";
            this.cbScan.Size = new System.Drawing.Size(95, 21);
            this.cbScan.TabIndex = 26;
            this.cbScan.SelectedIndexChanged += new System.EventHandler(this.cbScan_SelectedIndexChanged);
            // 
            // cbTypeCreateDoc
            // 
            this.cbTypeCreateDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeCreateDoc.FormattingEnabled = true;
            this.cbTypeCreateDoc.Location = new System.Drawing.Point(643, 2);
            this.cbTypeCreateDoc.Name = "cbTypeCreateDoc";
            this.cbTypeCreateDoc.Size = new System.Drawing.Size(121, 21);
            this.cbTypeCreateDoc.TabIndex = 25;
            this.cbTypeCreateDoc.SelectedIndexChanged += new System.EventHandler(this.cbTypeCreateDocChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(492, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Метод создания документа ";
            // 
            // btnIgnorReg
            // 
            this.btnIgnorReg.Location = new System.Drawing.Point(923, 29);
            this.btnIgnorReg.Name = "btnIgnorReg";
            this.btnIgnorReg.Size = new System.Drawing.Size(266, 21);
            this.btnIgnorReg.TabIndex = 23;
            this.btnIgnorReg.Text = "Показавать с привязкой сим-карт к регионам";
            this.btnIgnorReg.UseVisualStyleBackColor = true;
            this.btnIgnorReg.Click += new System.EventHandler(this.bMultiUnitsIgnorReg_Click_);
            // 
            // cbPlans
            // 
            this.cbPlans.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlans.DropDownWidth = 220;
            this.cbPlans.FormattingEnabled = true;
            this.cbPlans.Location = new System.Drawing.Point(797, 58);
            this.cbPlans.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cbPlans.MaximumSize = new System.Drawing.Size(131, 0);
            this.cbPlans.MinimumSize = new System.Drawing.Size(131, 0);
            this.cbPlans.Name = "cbPlans";
            this.cbPlans.Size = new System.Drawing.Size(131, 21);
            this.cbPlans.TabIndex = 22;
            this.cbPlans.SelectedIndexChanged += new System.EventHandler(this.cbPlans_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(709, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Тарифный план";
            // 
            // cbSearchMethod
            // 
            this.cbSearchMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearchMethod.DropDownWidth = 97;
            this.cbSearchMethod.FormattingEnabled = true;
            this.cbSearchMethod.Items.AddRange(new object[] {
            "Полностью",
            "\"И\"",
            "\"ИЛИ\""});
            this.cbSearchMethod.Location = new System.Drawing.Point(419, 57);
            this.cbSearchMethod.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cbSearchMethod.MinimumSize = new System.Drawing.Size(97, 0);
            this.cbSearchMethod.Name = "cbSearchMethod";
            this.cbSearchMethod.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbSearchMethod.Size = new System.Drawing.Size(97, 21);
            this.cbSearchMethod.TabIndex = 16;
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(583, 57);
            this.cbStatus.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cbStatus.MaximumSize = new System.Drawing.Size(120, 0);
            this.cbStatus.MinimumSize = new System.Drawing.Size(120, 0);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(120, 21);
            this.cbStatus.TabIndex = 20;
            this.cbStatus.SelectedIndexChanged += new System.EventHandler(this.cbStatus_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(536, 60);
            this.label6.MaximumSize = new System.Drawing.Size(100, 14);
            this.label6.MinimumSize = new System.Drawing.Size(20, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 14);
            this.label6.TabIndex = 19;
            this.label6.Text = "Статус";
            // 
            // tbDocSearch
            // 
            this.tbDocSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDocSearch.Location = new System.Drawing.Point(115, 55);
            this.tbDocSearch.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tbDocSearch.MaximumSize = new System.Drawing.Size(298, 21);
            this.tbDocSearch.MinimumSize = new System.Drawing.Size(298, 21);
            this.tbDocSearch.Name = "tbDocSearch";
            this.tbDocSearch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbDocSearch.Size = new System.Drawing.Size(298, 20);
            this.tbDocSearch.TabIndex = 7;
            this.tbDocSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocSearch_KeyDown);
            // 
            // bMultiUnits
            // 
            this.bMultiUnits.Location = new System.Drawing.Point(775, 29);
            this.bMultiUnits.Name = "bMultiUnits";
            this.bMultiUnits.Size = new System.Drawing.Size(142, 21);
            this.bMultiUnits.TabIndex = 18;
            this.bMultiUnits.Text = "Множественный выбор";
            this.bMultiUnits.UseVisualStyleBackColor = true;
            this.bMultiUnits.Click += new System.EventHandler(this.bMultiUnits_Click);
            // 
            // bAllJournal
            // 
            this.bAllJournal.Location = new System.Drawing.Point(403, 2);
            this.bAllJournal.Name = "bAllJournal";
            this.bAllJournal.Size = new System.Drawing.Size(83, 21);
            this.bAllJournal.TabIndex = 17;
            this.bAllJournal.Text = "Весь журнал";
            this.bAllJournal.UseVisualStyleBackColor = true;
            this.bAllJournal.Click += new System.EventHandler(this.bAllJournal_Click);
            // 
            // cbFilterImmediate
            // 
            this.cbFilterImmediate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFilterImmediate.AutoSize = true;
            this.cbFilterImmediate.Location = new System.Drawing.Point(955, 7);
            this.cbFilterImmediate.Name = "cbFilterImmediate";
            this.cbFilterImmediate.Size = new System.Drawing.Size(160, 17);
            this.cbFilterImmediate.TabIndex = 14;
            this.cbFilterImmediate.Text = "Немедленная фильтрация";
            this.cbFilterImmediate.UseVisualStyleBackColor = true;
            // 
            // cbUnit
            // 
            this.cbUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(522, 29);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(247, 21);
            this.cbUnit.TabIndex = 13;
            this.cbUnit.SelectedIndexChanged += new System.EventHandler(this.cbDocType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(454, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Отделение";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Тип документа";
            // 
            // cbDocType
            // 
            this.cbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocType.FormattingEnabled = true;
            this.cbDocType.Location = new System.Drawing.Point(115, 29);
            this.cbDocType.Name = "cbDocType";
            this.cbDocType.Size = new System.Drawing.Size(298, 21);
            this.cbDocType.TabIndex = 10;
            this.cbDocType.SelectedIndexChanged += new System.EventHandler(this.cbDocType_SelectedIndexChanged);
            // 
            // cbRecLimit
            // 
            this.cbRecLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRecLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRecLimit.FormattingEnabled = true;
            this.cbRecLimit.Items.AddRange(new object[] {
            "Все",
            "10000",
            "5000",
            "1000",
            "500",
            "100",
            "50",
            "10",
            "5"});
            this.cbRecLimit.Location = new System.Drawing.Point(1121, 56);
            this.cbRecLimit.Name = "cbRecLimit";
            this.cbRecLimit.Size = new System.Drawing.Size(119, 21);
            this.cbRecLimit.TabIndex = 9;
            this.cbRecLimit.SelectedIndexChanged += new System.EventHandler(this.cbDocType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(934, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Количество записей на странице";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 60);
            this.label2.MinimumSize = new System.Drawing.Size(105, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Поиск в документе";
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateTo.Location = new System.Drawing.Point(285, 3);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(110, 20);
            this.dtpDateTo.TabIndex = 5;
            // 
            // cbDateTo
            // 
            this.cbDateTo.AutoSize = true;
            this.cbDateTo.Location = new System.Drawing.Point(244, 6);
            this.cbDateTo.Name = "cbDateTo";
            this.cbDateTo.Size = new System.Drawing.Size(38, 17);
            this.cbDateTo.TabIndex = 4;
            this.cbDateTo.Text = "по";
            this.cbDateTo.UseVisualStyleBackColor = true;
            this.cbDateTo.CheckStateChanged += new System.EventHandler(this.cbDateTo_CheckStateChanged);
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.CustomFormat = "";
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateFrom.Location = new System.Drawing.Point(115, 3);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(110, 20);
            this.dtpDateFrom.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Дата: С";
            // 
            // bNavSetFilter
            // 
            this.bNavSetFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bNavSetFilter.Location = new System.Drawing.Point(1121, 4);
            this.bNavSetFilter.Name = "bNavSetFilter";
            this.bNavSetFilter.Size = new System.Drawing.Size(120, 23);
            this.bNavSetFilter.TabIndex = 1;
            this.bNavSetFilter.Text = "Установить фильтр";
            this.bNavSetFilter.UseVisualStyleBackColor = true;
            this.bNavSetFilter.Click += new System.EventHandler(this.bNavSetFilter_Click);
            // 
            // bNavOpenClose
            // 
            this.bNavOpenClose.Location = new System.Drawing.Point(3, 3);
            this.bNavOpenClose.Name = "bNavOpenClose";
            this.bNavOpenClose.Size = new System.Drawing.Size(23, 23);
            this.bNavOpenClose.TabIndex = 0;
            this.bNavOpenClose.Text = ">";
            this.bNavOpenClose.UseVisualStyleBackColor = true;
            this.bNavOpenClose.Click += new System.EventHandler(this.bNavOpeClose_Click);
            // 
            // pJournal
            // 
            this.pJournal.Controls.Add(this.dgvJournal);
            this.pJournal.Controls.Add(this.tsDocControl);
            this.pJournal.Controls.Add(this.lNoRecords);
            this.pJournal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pJournal.Location = new System.Drawing.Point(0, 136);
            this.pJournal.Name = "pJournal";
            this.pJournal.Size = new System.Drawing.Size(1246, 322);
            this.pJournal.TabIndex = 1;
            // 
            // dgvJournal
            // 
            this.dgvJournal.AllowUserToAddRows = false;
            this.dgvJournal.AllowUserToDeleteRows = false;
            this.dgvJournal.AllowUserToResizeRows = false;
            this.dgvJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJournal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.status,
            this.vdocdate,
            this.unittitle,
            this.vdoctype,
            this.digest,
            this.usertitle});
            this.dgvJournal.ContextMenuStrip = this.cmsJournalhook;
            this.dgvJournal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvJournal.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvJournal.Location = new System.Drawing.Point(0, 0);
            this.dgvJournal.Name = "dgvJournal";
            this.dgvJournal.ReadOnly = true;
            this.dgvJournal.RowHeadersVisible = false;
            this.dgvJournal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvJournal.Size = new System.Drawing.Size(1246, 297);
            this.dgvJournal.TabIndex = 3;
            this.dgvJournal.VirtualMode = true;
            this.dgvJournal.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvJournal_CellFormatting);
            this.dgvJournal.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvJournal_ColumnDisplayIndexChanged);
            this.dgvJournal.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvJournal_ColumnWidthChanged);
            this.dgvJournal.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJournal_RowEnter);
            this.dgvJournal.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvJournal_RowPrePaint);
            this.dgvJournal.SelectionChanged += new System.EventHandler(this.dgvJournal_SelectionChanged);
            this.dgvJournal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvJournal_KeyDown);
            this.dgvJournal.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvJournal_MouseDoubleClick);
            // 
            // status
            // 
            this.status.HeaderText = "Статус";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 32;
            // 
            // vdocdate
            // 
            this.vdocdate.HeaderText = "Дата документа";
            this.vdocdate.Name = "vdocdate";
            this.vdocdate.ReadOnly = true;
            // 
            // unittitle
            // 
            this.unittitle.HeaderText = "Отделение";
            this.unittitle.Name = "unittitle";
            this.unittitle.ReadOnly = true;
            // 
            // vdoctype
            // 
            this.vdoctype.HeaderText = "Тип документа";
            this.vdoctype.Name = "vdoctype";
            this.vdoctype.ReadOnly = true;
            // 
            // digest
            // 
            this.digest.HeaderText = "Сведения";
            this.digest.Name = "digest";
            this.digest.ReadOnly = true;
            // 
            // usertitle
            // 
            this.usertitle.HeaderText = "Пользователь";
            this.usertitle.Name = "usertitle";
            this.usertitle.ReadOnly = true;
            // 
            // cmsJournalhook
            // 
            this.cmsJournalhook.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5});
            this.cmsJournalhook.Name = "cmsJournalhook";
            this.cmsJournalhook.Size = new System.Drawing.Size(99, 26);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(98, 22);
            this.toolStripMenuItem5.Text = "TEST";
            // 
            // tsDocControl
            // 
            this.tsDocControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tsDocControl.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsDocControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbPagination,
            this.tsddbDocuments,
            this.tsbEditDocument,
            this.tsbDeleteDocument,
            this.tsbApprove,
            this.tsbCloneDocument,
            this.tsbPrintDoc,
            this.toolStripSeparator4,
            this.toolStripSeparator5,
            this.tsbDataFields,
            this.tsbExport,
            this.toolStripSeparator6,
            this.tsbChangeStatus,
            this.toolStripPrintTest});
            this.tsDocControl.Location = new System.Drawing.Point(0, 297);
            this.tsDocControl.Name = "tsDocControl";
            this.tsDocControl.Size = new System.Drawing.Size(1246, 25);
            this.tsDocControl.TabIndex = 0;
            this.tsDocControl.Text = "toolStrip1";
            // 
            // tsddbPagination
            // 
            this.tsddbPagination.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsddbPagination.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbPagination.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.tsddbPagination.Image = ((System.Drawing.Image)(resources.GetObject("tsddbPagination.Image")));
            this.tsddbPagination.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbPagination.Name = "tsddbPagination";
            this.tsddbPagination.Size = new System.Drawing.Size(115, 22);
            this.tsddbPagination.Text = "Страница: 1 из 10";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(138, 22);
            this.toolStripMenuItem2.Text = "1 (1 - 100)";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(138, 22);
            this.toolStripMenuItem3.Text = "2 (100 - 200)";
            // 
            // tsddbDocuments
            // 
            this.tsddbDocuments.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbDocuments.Image = ((System.Drawing.Image)(resources.GetObject("tsddbDocuments.Image")));
            this.tsddbDocuments.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbDocuments.Name = "tsddbDocuments";
            this.tsddbDocuments.Size = new System.Drawing.Size(113, 22);
            this.tsddbDocuments.Text = "Новый документ";
            // 
            // tsbEditDocument
            // 
            this.tsbEditDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbEditDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsbEditDocument.Image")));
            this.tsbEditDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditDocument.Name = "tsbEditDocument";
            this.tsbEditDocument.Size = new System.Drawing.Size(120, 22);
            this.tsbEditDocument.Text = "Изменить документ";
            this.tsbEditDocument.Click += new System.EventHandler(this.tsbEditDocument_Click);
            // 
            // tsbDeleteDocument
            // 
            this.tsbDeleteDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDeleteDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsbDeleteDocument.Image")));
            this.tsbDeleteDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteDocument.Name = "tsbDeleteDocument";
            this.tsbDeleteDocument.Size = new System.Drawing.Size(110, 22);
            this.tsbDeleteDocument.Text = "Удалить документ";
            this.tsbDeleteDocument.Click += new System.EventHandler(this.tsbDeleteDocument_Click);
            // 
            // tsbApprove
            // 
            this.tsbApprove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbApprove.Image = ((System.Drawing.Image)(resources.GetObject("tsbApprove.Image")));
            this.tsbApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbApprove.Name = "tsbApprove";
            this.tsbApprove.Size = new System.Drawing.Size(153, 22);
            this.tsbApprove.Text = "Подтвердить документ(ы)";
            this.tsbApprove.Click += new System.EventHandler(this.tsbApprove_Click);
            // 
            // tsbCloneDocument
            // 
            this.tsbCloneDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCloneDocument.Image = ((System.Drawing.Image)(resources.GetObject("tsbCloneDocument.Image")));
            this.tsbCloneDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCloneDocument.Name = "tsbCloneDocument";
            this.tsbCloneDocument.Size = new System.Drawing.Size(138, 22);
            this.tsbCloneDocument.Text = "Клонировать документ";
            this.tsbCloneDocument.Click += new System.EventHandler(this.tsbCloneDocument_Click);
            // 
            // tsbPrintDoc
            // 
            this.tsbPrintDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPrintDoc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPreviewDoc,
            this.toolStripMenuItem1,
            this.tsmiPrintDoc,
            this.toolStripMenuItem4,
            this.tsmiPronterSettings,
            this.tsmiSchemesSetup});
            this.tsbPrintDoc.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrintDoc.Image")));
            this.tsbPrintDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrintDoc.Name = "tsbPrintDoc";
            this.tsbPrintDoc.Size = new System.Drawing.Size(127, 22);
            this.tsbPrintDoc.Text = "Печать документов";
            // 
            // tsmiPreviewDoc
            // 
            this.tsmiPreviewDoc.Name = "tsmiPreviewDoc";
            this.tsmiPreviewDoc.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.tsmiPreviewDoc.Size = new System.Drawing.Size(241, 22);
            this.tsmiPreviewDoc.Text = "Просмотр перед печатью";
            this.tsmiPreviewDoc.Click += new System.EventHandler(this.tsmiPreviewDoc_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiPrintDoc
            // 
            this.tsmiPrintDoc.Name = "tsmiPrintDoc";
            this.tsmiPrintDoc.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F12)));
            this.tsmiPrintDoc.Size = new System.Drawing.Size(241, 22);
            this.tsmiPrintDoc.Text = "Печать";
            this.tsmiPrintDoc.Click += new System.EventHandler(this.tsmiPrintDoc_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiPronterSettings
            // 
            this.tsmiPronterSettings.Name = "tsmiPronterSettings";
            this.tsmiPronterSettings.Size = new System.Drawing.Size(241, 22);
            this.tsmiPronterSettings.Text = "Настройки принтера";
            this.tsmiPronterSettings.Click += new System.EventHandler(this.tsmiPrinterSettings_Click);
            // 
            // tsmiSchemesSetup
            // 
            this.tsmiSchemesSetup.Name = "tsmiSchemesSetup";
            this.tsmiSchemesSetup.Size = new System.Drawing.Size(241, 22);
            this.tsmiSchemesSetup.Text = "Настройка схем печати";
            this.tsmiSchemesSetup.Click += new System.EventHandler(this.tsmiSchemesSetup_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbDataFields
            // 
            this.tsbDataFields.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbDataFields.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDataFields.Image = ((System.Drawing.Image)(resources.GetObject("tsbDataFields.Image")));
            this.tsbDataFields.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDataFields.Name = "tsbDataFields";
            this.tsbDataFields.Size = new System.Drawing.Size(40, 22);
            this.tsbDataFields.Text = "Поля";
            this.tsbDataFields.Click += new System.EventHandler(this.tsbDataFields_Click);
            // 
            // tsbExport
            // 
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExport.Image = ((System.Drawing.Image)(resources.GetObject("tsbExport.Image")));
            this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(56, 22);
            this.tsbExport.Text = "Экспорт";
            this.tsbExport.Visible = false;
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator6.Visible = false;
            // 
            // tsbChangeStatus
            // 
            this.tsbChangeStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbChangeStatus.Image = ((System.Drawing.Image)(resources.GetObject("tsbChangeStatus.Image")));
            this.tsbChangeStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbChangeStatus.Name = "tsbChangeStatus";
            this.tsbChangeStatus.Size = new System.Drawing.Size(47, 22);
            this.tsbChangeStatus.Text = "Статус";
            this.tsbChangeStatus.ToolTipText = "Изменить статус выделенных документов";
            this.tsbChangeStatus.Visible = false;
            this.tsbChangeStatus.Click += new System.EventHandler(this.tsbChangeStatus_Click);
            // 
            // toolStripPrintTest
            // 
            this.toolStripPrintTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripPrintTest.Image = ((System.Drawing.Image)(resources.GetObject("toolStripPrintTest.Image")));
            this.toolStripPrintTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPrintTest.Name = "toolStripPrintTest";
            this.toolStripPrintTest.Size = new System.Drawing.Size(109, 22);
            this.toolStripPrintTest.Text = "Печать (Тестовая)";
            this.toolStripPrintTest.Click += new System.EventHandler(this.toolStripPrintTest_Click);
            // 
            // lNoRecords
            // 
            this.lNoRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lNoRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lNoRecords.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lNoRecords.Location = new System.Drawing.Point(0, 0);
            this.lNoRecords.Name = "lNoRecords";
            this.lNoRecords.Size = new System.Drawing.Size(1246, 322);
            this.lNoRecords.TabIndex = 2;
            this.lNoRecords.Text = "Документов нет";
            this.lNoRecords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspbProgress,
            this.tsslStatus,
            this.tsslDocCount,
            this.tsslSelCount,
            this.tsddbJournalType});
            this.statusStrip1.Location = new System.Drawing.Point(0, 603);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1246, 24);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.DoubleClick += new System.EventHandler(this.tsslStatus_DoubleClick);
            // 
            // tspbProgress
            // 
            this.tspbProgress.Name = "tspbProgress";
            this.tspbProgress.Size = new System.Drawing.Size(100, 18);
            // 
            // tsslStatus
            // 
            this.tsslStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsslStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsslStatus.Name = "tsslStatus";
            this.tsslStatus.Size = new System.Drawing.Size(897, 19);
            this.tsslStatus.Spring = true;
            this.tsslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsslStatus.DoubleClick += new System.EventHandler(this.tsslStatus_DoubleClick);
            // 
            // tsslDocCount
            // 
            this.tsslDocCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsslDocCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsslDocCount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslDocCount.Name = "tsslDocCount";
            this.tsslDocCount.Size = new System.Drawing.Size(90, 19);
            this.tsslDocCount.Text = "Документов: 0";
            this.tsslDocCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tsslSelCount
            // 
            this.tsslSelCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tsslSelCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsslSelCount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslSelCount.Name = "tsslSelCount";
            this.tsslSelCount.Size = new System.Drawing.Size(78, 19);
            this.tsslSelCount.Text = "Выделено: 0";
            // 
            // tsddbJournalType
            // 
            this.tsddbJournalType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbJournalType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiJournalModeJournal,
            this.tsmiJournalModeArchive});
            this.tsddbJournalType.Image = ((System.Drawing.Image)(resources.GetObject("tsddbJournalType.Image")));
            this.tsddbJournalType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbJournalType.Name = "tsddbJournalType";
            this.tsddbJournalType.Size = new System.Drawing.Size(64, 22);
            this.tsddbJournalType.Text = "Журнал";
            // 
            // tsmiJournalModeJournal
            // 
            this.tsmiJournalModeJournal.Name = "tsmiJournalModeJournal";
            this.tsmiJournalModeJournal.Size = new System.Drawing.Size(118, 22);
            this.tsmiJournalModeJournal.Text = "Журнал";
            this.tsmiJournalModeJournal.Click += new System.EventHandler(this.tsmiJournalModeJournal_Click);
            // 
            // tsmiJournalModeArchive
            // 
            this.tsmiJournalModeArchive.Name = "tsmiJournalModeArchive";
            this.tsmiJournalModeArchive.Size = new System.Drawing.Size(118, 22);
            this.tsmiJournalModeArchive.Text = "Архив";
            this.tsmiJournalModeArchive.Click += new System.EventHandler(this.tsmiJournalModeArchive_Click);
            // 
            // pLog
            // 
            this.pLog.Controls.Add(this.dgvLog);
            this.pLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pLog.Location = new System.Drawing.Point(0, 458);
            this.pLog.Name = "pLog";
            this.pLog.Size = new System.Drawing.Size(1246, 145);
            this.pLog.TabIndex = 4;
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvLog.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.time,
            this.text});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLog.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLog.Location = new System.Drawing.Point(0, 0);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.RowHeadersVisible = false;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLog.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvLog.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLog.Size = new System.Drawing.Size(1246, 145);
            this.dgvLog.TabIndex = 1;
            this.dgvLog.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvLog_RowPrePaint);
            // 
            // time
            // 
            this.time.DataPropertyName = "time";
            this.time.FillWeight = 30F;
            this.time.HeaderText = "Время";
            this.time.Name = "time";
            this.time.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // text
            // 
            this.text.DataPropertyName = "text";
            this.text.FillWeight = 70F;
            this.text.HeaderText = "Текст";
            this.text.Name = "text";
            this.text.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // spLog
            // 
            this.spLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.spLog.Location = new System.Drawing.Point(0, 456);
            this.spLog.Name = "spLog";
            this.spLog.Size = new System.Drawing.Size(1246, 2);
            this.spLog.TabIndex = 5;
            this.spLog.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 627);
            this.Controls.Add(this.spLog);
            this.Controls.Add(this.pJournal);
            this.Controls.Add(this.pNav);
            this.Controls.Add(this.tsMainButtons);
            this.Controls.Add(this.msMain);
            this.Controls.Add(this.pLog);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMain;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Журнал документов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.pNav.ResumeLayout(false);
            this.pNav.PerformLayout();
            this.pJournal.ResumeLayout(false);
            this.pJournal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournal)).EndInit();
            this.cmsJournalhook.ResumeLayout(false);
            this.tsDocControl.ResumeLayout(false);
            this.tsDocControl.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pNav;
        private System.Windows.Forms.Panel pJournal;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripMenuItem miDictionaries;
        private System.Windows.Forms.ToolStripMenuItem miSetupDictionaries;
        private System.Windows.Forms.Button bNavOpenClose;
        private System.Windows.Forms.Button bNavSetFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.CheckBox cbDateTo;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDocSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbRecLimit;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label lNoRecords;
        private System.Windows.Forms.DataGridView dgvJournal;
        private System.Windows.Forms.ToolStrip tsDocControl;
        private System.Windows.Forms.ToolStripDropDownButton tsddbPagination;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripDropDownButton tsddbDocuments;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDocType;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.ToolStripStatusLabel tsslDocCount;
        private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
        private System.Windows.Forms.ToolStripButton tsbEditDocument;
        private System.Windows.Forms.ToolStripMenuItem miSetupDocuments;
        private System.Windows.Forms.ToolStripButton tsbDeleteDocument;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbDataFields;
        private System.Windows.Forms.ToolStripDropDownButton tsbPrintDoc;
        private System.Windows.Forms.ToolStripMenuItem tsmiPreviewDoc;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrintDoc;
        private System.Windows.Forms.ToolStripMenuItem miReports;
        private System.Windows.Forms.ToolStripMenuItem miSetupReports;
        private System.Windows.Forms.ToolStripMenuItem miFunctions;
        private System.Windows.Forms.ToolStripMenuItem miSetupFunctions;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripMenuItem miSetupSchedules;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbChangeStatus;
        private System.Windows.Forms.ToolStripButton tsbApprove;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem tsmiPronterSettings;
        private System.Windows.Forms.CheckBox cbFilterImmediate;
        private System.Windows.Forms.ToolStripProgressBar tspbProgress;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn vdocdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn unittitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn vdoctype;
        private System.Windows.Forms.DataGridViewTextBoxColumn digest;
        private System.Windows.Forms.DataGridViewTextBoxColumn usertitle;
        private System.Windows.Forms.ToolStripMenuItem tsmiSchemesSetup;
        private System.Windows.Forms.ComboBox cbSearchMethod;
        private System.Windows.Forms.ToolStripStatusLabel tsslSelCount;
        private System.Windows.Forms.ContextMenuStrip cmsJournalhook;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.Button bAllJournal;
        private System.Windows.Forms.ToolStrip tsMainButtons;
        private System.Windows.Forms.ToolStripDropDownButton tsddbJournalType;
        private System.Windows.Forms.ToolStripMenuItem tsmiJournalModeArchive;
        private System.Windows.Forms.ToolStripMenuItem tsmiJournalModeJournal;
        private System.Windows.Forms.Panel pLog;
        private System.Windows.Forms.Splitter spLog;
        private System.Windows.Forms.DataGridView dgvLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn text;
        private System.Windows.Forms.Button bMultiUnits;




        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbPlans;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnIgnorReg;
        private System.Windows.Forms.ComboBox cbTypeCreateDoc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripButton tsbCloneDocument;
        private System.Windows.Forms.ToolStripButton toolStripPrintTest;
        private System.Windows.Forms.ComboBox cbScan;
        private System.Windows.Forms.Label label9;
    }
}

