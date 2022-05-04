namespace DEXPlugin.Function.Mega.Autodoc
{
    partial class AutodocMain
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbOnlyForeigner = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbRegions = new System.Windows.Forms.ComboBox();
            this.nudMsisdnPos = new System.Windows.Forms.NumericUpDown();
            this.cbMsisdnSubstr = new System.Windows.Forms.CheckBox();
            this.cbEnc = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbQuotes = new System.Windows.Forms.CheckBox();
            this.cbSeparator = new System.Windows.Forms.ComboBox();
            this.bLoadFromClipboard = new System.Windows.Forms.Button();
            this.bLoadFromFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bSelectSrcFile = new System.Windows.Forms.Button();
            this.tbSrcFile = new System.Windows.Forms.TextBox();
            this.dgvSrc = new System.Windows.Forms.DataGridView();
            this.bFillTable = new System.Windows.Forms.Button();
            this.bMakeDocs = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbOnlyFDOC = new System.Windows.Forms.CheckBox();
            this.cbChTerrorists = new System.Windows.Forms.CheckBox();
            this.cbOnlyWomen = new System.Windows.Forms.CheckBox();
            this.bAnyPassText = new System.Windows.Forms.Button();
            this.tbAnyPassUnits = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbLockToUnitId = new System.Windows.Forms.CheckBox();
            this.cbFixedDocDate = new System.Windows.Forms.CheckBox();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.cbUnitAsSIM = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.deDocDate = new DEXExtendLib.DateEdit();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbCert = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbSBMSPass = new System.Windows.Forms.TextBox();
            this.tbSBMSUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMsisdnPos)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSrc)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cbRegions);
            this.groupBox2.Controls.Add(this.nudMsisdnPos);
            this.groupBox2.Controls.Add(this.cbMsisdnSubstr);
            this.groupBox2.Controls.Add(this.cbEnc);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbQuotes);
            this.groupBox2.Controls.Add(this.cbSeparator);
            this.groupBox2.Location = new System.Drawing.Point(12, 350);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(628, 73);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры";
            // 
            // cbOnlyForeigner
            // 
            this.cbOnlyForeigner.AutoSize = true;
            this.cbOnlyForeigner.Checked = true;
            this.cbOnlyForeigner.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOnlyForeigner.Location = new System.Drawing.Point(490, 96);
            this.cbOnlyForeigner.Name = "cbOnlyForeigner";
            this.cbOnlyForeigner.Size = new System.Drawing.Size(131, 17);
            this.cbOnlyForeigner.TabIndex = 35;
            this.cbOnlyForeigner.Text = "Только иностранцев";
            this.cbOnlyForeigner.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(335, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(164, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Регион для выбора абонентов:";
            // 
            // cbRegions
            // 
            this.cbRegions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRegions.FormattingEnabled = true;
            this.cbRegions.Location = new System.Drawing.Point(501, 38);
            this.cbRegions.Name = "cbRegions";
            this.cbRegions.Size = new System.Drawing.Size(121, 21);
            this.cbRegions.TabIndex = 16;
            // 
            // nudMsisdnPos
            // 
            this.nudMsisdnPos.Location = new System.Drawing.Point(254, 41);
            this.nudMsisdnPos.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudMsisdnPos.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMsisdnPos.Name = "nudMsisdnPos";
            this.nudMsisdnPos.Size = new System.Drawing.Size(63, 20);
            this.nudMsisdnPos.TabIndex = 15;
            this.nudMsisdnPos.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbMsisdnSubstr
            // 
            this.cbMsisdnSubstr.AutoSize = true;
            this.cbMsisdnSubstr.Location = new System.Drawing.Point(6, 42);
            this.cbMsisdnSubstr.Name = "cbMsisdnSubstr";
            this.cbMsisdnSubstr.Size = new System.Drawing.Size(244, 17);
            this.cbMsisdnSubstr.TabIndex = 14;
            this.cbMsisdnSubstr.Text = "Подстрока MSISDN начинается с позиции:";
            this.cbMsisdnSubstr.UseVisualStyleBackColor = true;
            // 
            // cbEnc
            // 
            this.cbEnc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEnc.FormattingEnabled = true;
            this.cbEnc.Items.AddRange(new object[] {
            "UTF-8",
            "Windows-1251",
            "DOS (866)"});
            this.cbEnc.Location = new System.Drawing.Point(74, 15);
            this.cbEnc.Name = "cbEnc";
            this.cbEnc.Size = new System.Drawing.Size(164, 21);
            this.cbEnc.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Кодировка";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Разделитель";
            // 
            // cbQuotes
            // 
            this.cbQuotes.AutoSize = true;
            this.cbQuotes.Location = new System.Drawing.Point(399, 17);
            this.cbQuotes.Name = "cbQuotes";
            this.cbQuotes.Size = new System.Drawing.Size(134, 17);
            this.cbQuotes.TabIndex = 12;
            this.cbQuotes.Text = "Значения в кавычках";
            this.cbQuotes.UseVisualStyleBackColor = true;
            // 
            // cbSeparator
            // 
            this.cbSeparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeparator.FormattingEnabled = true;
            this.cbSeparator.Items.AddRange(new object[] {
            "<Tab>",
            "<;>",
            "<:>",
            "<|>",
            "<.>",
            "<,>",
            "<!>",
            "<&>"});
            this.cbSeparator.Location = new System.Drawing.Point(320, 15);
            this.cbSeparator.Name = "cbSeparator";
            this.cbSeparator.Size = new System.Drawing.Size(73, 21);
            this.cbSeparator.TabIndex = 11;
            // 
            // bLoadFromClipboard
            // 
            this.bLoadFromClipboard.Location = new System.Drawing.Point(145, 702);
            this.bLoadFromClipboard.Name = "bLoadFromClipboard";
            this.bLoadFromClipboard.Size = new System.Drawing.Size(136, 23);
            this.bLoadFromClipboard.TabIndex = 5;
            this.bLoadFromClipboard.Text = "или из буфера обмена";
            this.bLoadFromClipboard.UseVisualStyleBackColor = true;
            this.bLoadFromClipboard.Click += new System.EventHandler(this.bLoadFromClipboard_Click);
            // 
            // bLoadFromFile
            // 
            this.bLoadFromFile.Location = new System.Drawing.Point(11, 702);
            this.bLoadFromFile.Name = "bLoadFromFile";
            this.bLoadFromFile.Size = new System.Drawing.Size(128, 23);
            this.bLoadFromFile.TabIndex = 4;
            this.bLoadFromFile.Text = "Загрузить из файла";
            this.bLoadFromFile.UseVisualStyleBackColor = true;
            this.bLoadFromFile.Click += new System.EventHandler(this.bLoadFromFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bSelectSrcFile);
            this.groupBox1.Controls.Add(this.tbSrcFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 296);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(628, 48);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Загрузка из файла";
            // 
            // bSelectSrcFile
            // 
            this.bSelectSrcFile.Location = new System.Drawing.Point(586, 16);
            this.bSelectSrcFile.Name = "bSelectSrcFile";
            this.bSelectSrcFile.Size = new System.Drawing.Size(36, 23);
            this.bSelectSrcFile.TabIndex = 1;
            this.bSelectSrcFile.Text = "...";
            this.bSelectSrcFile.UseVisualStyleBackColor = true;
            this.bSelectSrcFile.Click += new System.EventHandler(this.bSelectSrcFile_Click);
            // 
            // tbSrcFile
            // 
            this.tbSrcFile.Location = new System.Drawing.Point(6, 18);
            this.tbSrcFile.Name = "tbSrcFile";
            this.tbSrcFile.Size = new System.Drawing.Size(574, 20);
            this.tbSrcFile.TabIndex = 0;
            // 
            // dgvSrc
            // 
            this.dgvSrc.AllowUserToAddRows = false;
            this.dgvSrc.AllowUserToDeleteRows = false;
            this.dgvSrc.AllowUserToResizeRows = false;
            this.dgvSrc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSrc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSrc.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSrc.Location = new System.Drawing.Point(12, 12);
            this.dgvSrc.MultiSelect = false;
            this.dgvSrc.Name = "dgvSrc";
            this.dgvSrc.RowHeadersVisible = false;
            this.dgvSrc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSrc.ShowEditingIcon = false;
            this.dgvSrc.Size = new System.Drawing.Size(628, 278);
            this.dgvSrc.TabIndex = 1;
            this.dgvSrc.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvSrc_RowPrePaint);
            // 
            // bFillTable
            // 
            this.bFillTable.Location = new System.Drawing.Point(287, 702);
            this.bFillTable.Name = "bFillTable";
            this.bFillTable.Size = new System.Drawing.Size(172, 23);
            this.bFillTable.TabIndex = 0;
            this.bFillTable.Text = "Заполнить список абонентов";
            this.bFillTable.UseVisualStyleBackColor = true;
            this.bFillTable.Click += new System.EventHandler(this.button1_Click);
            // 
            // bMakeDocs
            // 
            this.bMakeDocs.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bMakeDocs.Location = new System.Drawing.Point(467, 702);
            this.bMakeDocs.Name = "bMakeDocs";
            this.bMakeDocs.Size = new System.Drawing.Size(174, 23);
            this.bMakeDocs.TabIndex = 1;
            this.bMakeDocs.Text = "Сформировать документы";
            this.bMakeDocs.UseVisualStyleBackColor = true;
            this.bMakeDocs.Click += new System.EventHandler(this.bMakeDocs_Click);
            // 
            // ofd
            // 
            this.ofd.FileName = "openFileDialog1";
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(565, 731);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = "Выход";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbOnlyForeigner);
            this.groupBox3.Controls.Add(this.cbOnlyFDOC);
            this.groupBox3.Controls.Add(this.cbChTerrorists);
            this.groupBox3.Controls.Add(this.cbOnlyWomen);
            this.groupBox3.Controls.Add(this.bAnyPassText);
            this.groupBox3.Controls.Add(this.tbAnyPassUnits);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cbLockToUnitId);
            this.groupBox3.Controls.Add(this.cbFixedDocDate);
            this.groupBox3.Controls.Add(this.cbUnit);
            this.groupBox3.Controls.Add(this.cbUnitAsSIM);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.deDocDate);
            this.groupBox3.Location = new System.Drawing.Point(12, 458);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(628, 119);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Данные документа";
            // 
            // cbOnlyFDOC
            // 
            this.cbOnlyFDOC.AutoSize = true;
            this.cbOnlyFDOC.Checked = true;
            this.cbOnlyFDOC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOnlyFDOC.Location = new System.Drawing.Point(302, 96);
            this.cbOnlyFDOC.Name = "cbOnlyFDOC";
            this.cbOnlyFDOC.Size = new System.Drawing.Size(188, 17);
            this.cbOnlyFDOC.TabIndex = 26;
            this.cbOnlyFDOC.Text = "Только с кодом подразделения";
            this.cbOnlyFDOC.UseVisualStyleBackColor = true;
            // 
            // cbChTerrorists
            // 
            this.cbChTerrorists.AutoSize = true;
            this.cbChTerrorists.Checked = true;
            this.cbChTerrorists.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbChTerrorists.Location = new System.Drawing.Point(114, 96);
            this.cbChTerrorists.Name = "cbChTerrorists";
            this.cbChTerrorists.Size = new System.Drawing.Size(190, 17);
            this.cbChTerrorists.TabIndex = 8;
            this.cbChTerrorists.Text = "Проверять по базе террористов";
            this.cbChTerrorists.UseVisualStyleBackColor = true;
            // 
            // cbOnlyWomen
            // 
            this.cbOnlyWomen.AutoSize = true;
            this.cbOnlyWomen.Checked = true;
            this.cbOnlyWomen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOnlyWomen.Location = new System.Drawing.Point(9, 96);
            this.cbOnlyWomen.Name = "cbOnlyWomen";
            this.cbOnlyWomen.Size = new System.Drawing.Size(107, 17);
            this.cbOnlyWomen.TabIndex = 18;
            this.cbOnlyWomen.Text = "Только женщин";
            this.cbOnlyWomen.UseVisualStyleBackColor = true;
            // 
            // bAnyPassText
            // 
            this.bAnyPassText.Location = new System.Drawing.Point(317, 69);
            this.bAnyPassText.Name = "bAnyPassText";
            this.bAnyPassText.Size = new System.Drawing.Size(17, 23);
            this.bAnyPassText.TabIndex = 25;
            this.bAnyPassText.Text = "?";
            this.bAnyPassText.UseVisualStyleBackColor = true;
            this.bAnyPassText.Click += new System.EventHandler(this.bAnyPassText_Click);
            // 
            // tbAnyPassUnits
            // 
            this.tbAnyPassUnits.Location = new System.Drawing.Point(340, 71);
            this.tbAnyPassUnits.Name = "tbAnyPassUnits";
            this.tbAnyPassUnits.Size = new System.Drawing.Size(282, 20);
            this.tbAnyPassUnits.TabIndex = 24;
            this.tbAnyPassUnits.TextChanged += new System.EventHandler(this.tbAnyPassUnits_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(305, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Отделения, для которых можно добирать любые паспорта";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // cbLockToUnitId
            // 
            this.cbLockToUnitId.AutoSize = true;
            this.cbLockToUnitId.Location = new System.Drawing.Point(9, 48);
            this.cbLockToUnitId.Name = "cbLockToUnitId";
            this.cbLockToUnitId.Size = new System.Drawing.Size(505, 17);
            this.cbLockToUnitId.TabIndex = 22;
            this.cbLockToUnitId.Text = "Абонентские данные должны принадлежать тому же отделению, что и формируемый догов" +
                "ор";
            this.cbLockToUnitId.UseVisualStyleBackColor = true;
            // 
            // cbFixedDocDate
            // 
            this.cbFixedDocDate.AutoSize = true;
            this.cbFixedDocDate.Location = new System.Drawing.Point(9, 23);
            this.cbFixedDocDate.Name = "cbFixedDocDate";
            this.cbFixedDocDate.Size = new System.Drawing.Size(131, 17);
            this.cbFixedDocDate.TabIndex = 9;
            this.cbFixedDocDate.Text = "Дата формирования";
            this.cbFixedDocDate.UseVisualStyleBackColor = true;
            this.cbFixedDocDate.CheckedChanged += new System.EventHandler(this.cbFixedDocDate_CheckedChanged);
            // 
            // cbUnit
            // 
            this.cbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(240, 21);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(209, 21);
            this.cbUnit.TabIndex = 21;
            // 
            // cbUnitAsSIM
            // 
            this.cbUnitAsSIM.AutoSize = true;
            this.cbUnitAsSIM.Location = new System.Drawing.Point(455, 23);
            this.cbUnitAsSIM.Name = "cbUnitAsSIM";
            this.cbUnitAsSIM.Size = new System.Drawing.Size(167, 17);
            this.cbUnitAsSIM.TabIndex = 20;
            this.cbUnitAsSIM.Text = "Как в справочнике MSISDN";
            this.cbUnitAsSIM.UseVisualStyleBackColor = true;
            this.cbUnitAsSIM.CheckedChanged += new System.EventHandler(this.cbUnitAsSIM_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(237, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Отделение";
            // 
            // deDocDate
            // 
            this.deDocDate.FormattingEnabled = true;
            this.deDocDate.InputChar = '*';
            this.deDocDate.Location = new System.Drawing.Point(146, 21);
            this.deDocDate.MaxLength = 10;
            this.deDocDate.Name = "deDocDate";
            this.deDocDate.Size = new System.Drawing.Size(81, 21);
            this.deDocDate.TabIndex = 17;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbServer);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.cbCert);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.tbSBMSPass);
            this.groupBox4.Controls.Add(this.tbSBMSUser);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(12, 583);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(628, 113);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Параметры СБМС";
            // 
            // cbServer
            // 
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Items.AddRange(new object[] {
            "kvk-dlr-sbms01.megafon.ru",
            "kvk-dlr-sbms02.megafon.ru"});
            this.cbServer.Location = new System.Drawing.Point(196, 82);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(270, 21);
            this.cbServer.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Сервер СБМС";
            // 
            // cbCert
            // 
            this.cbCert.FormattingEnabled = true;
            this.cbCert.Location = new System.Drawing.Point(196, 57);
            this.cbCert.Name = "cbCert";
            this.cbCert.Size = new System.Drawing.Size(270, 21);
            this.cbCert.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(177, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Cертификат для доступа к  SBMS";
            // 
            // tbSBMSPass
            // 
            this.tbSBMSPass.Location = new System.Drawing.Point(320, 19);
            this.tbSBMSPass.Name = "tbSBMSPass";
            this.tbSBMSPass.PasswordChar = '*';
            this.tbSBMSPass.Size = new System.Drawing.Size(146, 20);
            this.tbSBMSPass.TabIndex = 3;
            // 
            // tbSBMSUser
            // 
            this.tbSBMSUser.Location = new System.Drawing.Point(92, 19);
            this.tbSBMSUser.Name = "tbSBMSUser";
            this.tbSBMSUser.Size = new System.Drawing.Size(146, 20);
            this.tbSBMSUser.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(272, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Пароль";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Пользователь";
            // 
            // AutodocMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(649, 761);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bMakeDocs);
            this.Controls.Add(this.bLoadFromClipboard);
            this.Controls.Add(this.bLoadFromFile);
            this.Controls.Add(this.dgvSrc);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bFillTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutodocMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Формирование группы документов (ЕФД)";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMsisdnPos)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSrc)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bFillTable;
        private System.Windows.Forms.Button bMakeDocs;
        private System.Windows.Forms.DataGridView dgvSrc;
        private System.Windows.Forms.Button bLoadFromClipboard;
        private System.Windows.Forms.Button bLoadFromFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bSelectSrcFile;
        private System.Windows.Forms.TextBox tbSrcFile;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbEnc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbQuotes;
        private System.Windows.Forms.ComboBox cbSeparator;
        private System.Windows.Forms.NumericUpDown nudMsisdnPos;
        private System.Windows.Forms.CheckBox cbMsisdnSubstr;
        private System.Windows.Forms.Button bCancel;
        private DEXExtendLib.DateEdit deDocDate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.CheckBox cbUnitAsSIM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbFixedDocDate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSBMSPass;
        private System.Windows.Forms.TextBox tbSBMSUser;
        private System.Windows.Forms.CheckBox cbLockToUnitId;
        private System.Windows.Forms.Button bAnyPassText;
        private System.Windows.Forms.TextBox tbAnyPassUnits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbCert;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbServer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbRegions;
        private System.Windows.Forms.CheckBox cbOnlyWomen;
        private System.Windows.Forms.CheckBox cbChTerrorists;
        private System.Windows.Forms.CheckBox cbOnlyFDOC;
        private System.Windows.Forms.CheckBox cbOnlyForeigner;



    }
}