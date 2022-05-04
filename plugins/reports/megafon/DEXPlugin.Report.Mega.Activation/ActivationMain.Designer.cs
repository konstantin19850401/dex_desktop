namespace DEXPlugin.Report.Mega.Activation
{
    partial class ActivationMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbShowBalance = new System.Windows.Forms.CheckBox();
            this.cbJournalOption = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDocType = new System.Windows.Forms.ComboBox();
            this.nudMsisdnPos = new System.Windows.Forms.NumericUpDown();
            this.cbMsisdnSubstr = new System.Windows.Forms.CheckBox();
            this.cbEnc = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.bClearTable = new System.Windows.Forms.Button();
            this.bSrcFromBuffer = new System.Windows.Forms.Button();
            this.bSrcFromFile = new System.Windows.Forms.Button();
            this.cbQuotes = new System.Windows.Forms.CheckBox();
            this.cbSeparator = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSrc = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.bStartCheck = new System.Windows.Forms.Button();
            this.bSaveToFile = new System.Windows.Forms.Button();
            this.bCopyToClipboard = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.cbSearchByIcc = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMsisdnPos)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSearchByIcc);
            this.groupBox1.Controls.Add(this.cbShowBalance);
            this.groupBox1.Controls.Add(this.cbJournalOption);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbDocType);
            this.groupBox1.Controls.Add(this.nudMsisdnPos);
            this.groupBox1.Controls.Add(this.cbMsisdnSubstr);
            this.groupBox1.Controls.Add(this.cbEnc);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.bClearTable);
            this.groupBox1.Controls.Add(this.bSrcFromBuffer);
            this.groupBox1.Controls.Add(this.bSrcFromFile);
            this.groupBox1.Controls.Add(this.cbQuotes);
            this.groupBox1.Controls.Add(this.cbSeparator);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbSrc);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 190);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Источник";
            // 
            // cbShowBalance
            // 
            this.cbShowBalance.AutoSize = true;
            this.cbShowBalance.Location = new System.Drawing.Point(6, 96);
            this.cbShowBalance.Name = "cbShowBalance";
            this.cbShowBalance.Size = new System.Drawing.Size(184, 17);
            this.cbShowBalance.TabIndex = 20;
            this.cbShowBalance.Text = "Отображать баланс сим-карты";
            this.cbShowBalance.UseVisualStyleBackColor = true;
            // 
            // cbJournalOption
            // 
            this.cbJournalOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbJournalOption.FormattingEnabled = true;
            this.cbJournalOption.Items.AddRange(new object[] {
            "Только данные архива",
            "Только данные журнала",
            "Данные журнала и архива"});
            this.cbJournalOption.Location = new System.Drawing.Point(130, 155);
            this.cbJournalOption.Name = "cbJournalOption";
            this.cbJournalOption.Size = new System.Drawing.Size(406, 21);
            this.cbJournalOption.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Параметры журнала";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Тип документа";
            // 
            // cbDocType
            // 
            this.cbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocType.FormattingEnabled = true;
            this.cbDocType.Location = new System.Drawing.Point(130, 128);
            this.cbDocType.Name = "cbDocType";
            this.cbDocType.Size = new System.Drawing.Size(406, 21);
            this.cbDocType.TabIndex = 12;
            // 
            // nudMsisdnPos
            // 
            this.nudMsisdnPos.Location = new System.Drawing.Point(254, 72);
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
            this.nudMsisdnPos.TabIndex = 11;
            this.nudMsisdnPos.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbMsisdnSubstr
            // 
            this.cbMsisdnSubstr.AutoSize = true;
            this.cbMsisdnSubstr.Location = new System.Drawing.Point(6, 73);
            this.cbMsisdnSubstr.Name = "cbMsisdnSubstr";
            this.cbMsisdnSubstr.Size = new System.Drawing.Size(244, 17);
            this.cbMsisdnSubstr.TabIndex = 9;
            this.cbMsisdnSubstr.Text = "Подстрока MSISDN начинается с позиции:";
            this.cbMsisdnSubstr.UseVisualStyleBackColor = true;
            this.cbMsisdnSubstr.CheckedChanged += new System.EventHandler(this.cbMsisdnSubstr_CheckedChanged);
            // 
            // cbEnc
            // 
            this.cbEnc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEnc.FormattingEnabled = true;
            this.cbEnc.Items.AddRange(new object[] {
            "UTF-8",
            "Windows-1251",
            "DOS (866)"});
            this.cbEnc.Location = new System.Drawing.Point(74, 45);
            this.cbEnc.Name = "cbEnc";
            this.cbEnc.Size = new System.Drawing.Size(164, 21);
            this.cbEnc.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Кодировка";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(268, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(25, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bClearTable
            // 
            this.bClearTable.Location = new System.Drawing.Point(461, 16);
            this.bClearTable.Name = "bClearTable";
            this.bClearTable.Size = new System.Drawing.Size(75, 23);
            this.bClearTable.TabIndex = 6;
            this.bClearTable.Text = "Очистить";
            this.bClearTable.UseVisualStyleBackColor = true;
            this.bClearTable.Click += new System.EventHandler(this.bClearTable_Click);
            // 
            // bSrcFromBuffer
            // 
            this.bSrcFromBuffer.Location = new System.Drawing.Point(380, 16);
            this.bSrcFromBuffer.Name = "bSrcFromBuffer";
            this.bSrcFromBuffer.Size = new System.Drawing.Size(75, 23);
            this.bSrcFromBuffer.TabIndex = 5;
            this.bSrcFromBuffer.Text = "Из буфера";
            this.bSrcFromBuffer.UseVisualStyleBackColor = true;
            this.bSrcFromBuffer.Click += new System.EventHandler(this.bSrcFromBuffer_Click);
            // 
            // bSrcFromFile
            // 
            this.bSrcFromFile.Location = new System.Drawing.Point(299, 16);
            this.bSrcFromFile.Name = "bSrcFromFile";
            this.bSrcFromFile.Size = new System.Drawing.Size(75, 23);
            this.bSrcFromFile.TabIndex = 4;
            this.bSrcFromFile.Text = "Из файла";
            this.bSrcFromFile.UseVisualStyleBackColor = true;
            this.bSrcFromFile.Click += new System.EventHandler(this.bSrcFromFile_Click);
            // 
            // cbQuotes
            // 
            this.cbQuotes.AutoSize = true;
            this.cbQuotes.Location = new System.Drawing.Point(402, 47);
            this.cbQuotes.Name = "cbQuotes";
            this.cbQuotes.Size = new System.Drawing.Size(134, 17);
            this.cbQuotes.TabIndex = 3;
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
            this.cbSeparator.Location = new System.Drawing.Point(323, 45);
            this.cbSeparator.Name = "cbSeparator";
            this.cbSeparator.Size = new System.Drawing.Size(73, 21);
            this.cbSeparator.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Разделитель";
            // 
            // tbSrc
            // 
            this.tbSrc.Location = new System.Drawing.Point(6, 18);
            this.tbSrc.Name = "tbSrc";
            this.tbSrc.Size = new System.Drawing.Size(256, 20);
            this.tbSrc.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgv);
            this.groupBox2.Location = new System.Drawing.Point(12, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(542, 259);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Данные";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv.Location = new System.Drawing.Point(9, 19);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(527, 229);
            this.dgv.TabIndex = 0;
            // 
            // bStartCheck
            // 
            this.bStartCheck.Location = new System.Drawing.Point(12, 473);
            this.bStartCheck.Name = "bStartCheck";
            this.bStartCheck.Size = new System.Drawing.Size(108, 23);
            this.bStartCheck.TabIndex = 2;
            this.bStartCheck.Text = "Начать проверку";
            this.bStartCheck.UseVisualStyleBackColor = true;
            this.bStartCheck.Click += new System.EventHandler(this.bStartCheck_Click);
            // 
            // bSaveToFile
            // 
            this.bSaveToFile.Location = new System.Drawing.Point(126, 473);
            this.bSaveToFile.Name = "bSaveToFile";
            this.bSaveToFile.Size = new System.Drawing.Size(184, 23);
            this.bSaveToFile.TabIndex = 3;
            this.bSaveToFile.Text = "Сохранить результаты в файл";
            this.bSaveToFile.UseVisualStyleBackColor = true;
            this.bSaveToFile.Click += new System.EventHandler(this.bSaveToFile_Click);
            // 
            // bCopyToClipboard
            // 
            this.bCopyToClipboard.Location = new System.Drawing.Point(316, 473);
            this.bCopyToClipboard.Name = "bCopyToClipboard";
            this.bCopyToClipboard.Size = new System.Drawing.Size(157, 23);
            this.bCopyToClipboard.TabIndex = 4;
            this.bCopyToClipboard.Text = "или скопировать в буфер";
            this.bCopyToClipboard.UseVisualStyleBackColor = true;
            this.bCopyToClipboard.Click += new System.EventHandler(this.bCopyToClipboard_Click);
            // 
            // bExit
            // 
            this.bExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bExit.Location = new System.Drawing.Point(479, 473);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(75, 23);
            this.bExit.TabIndex = 5;
            this.bExit.Text = "Выход";
            this.bExit.UseVisualStyleBackColor = true;
            // 
            // ofd
            // 
            this.ofd.Filter = "Текстовые файлы|*.*";
            // 
            // sfd
            // 
            this.sfd.Filter = "Файлы CSV|*.csv";
            // 
            // cbSearchByIcc
            // 
            this.cbSearchByIcc.AutoSize = true;
            this.cbSearchByIcc.Location = new System.Drawing.Point(443, 73);
            this.cbSearchByIcc.Name = "cbSearchByIcc";
            this.cbSearchByIcc.Size = new System.Drawing.Size(93, 17);
            this.cbSearchByIcc.TabIndex = 21;
            this.cbSearchByIcc.Text = "Поиск по ICC";
            this.cbSearchByIcc.UseVisualStyleBackColor = true;
            // 
            // ActivationMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bExit;
            this.ClientSize = new System.Drawing.Size(567, 508);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bCopyToClipboard);
            this.Controls.Add(this.bSaveToFile);
            this.Controls.Add(this.bStartCheck);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActivationMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сверка по активации Мегафон";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMsisdnPos)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSrc;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbSeparator;
        private System.Windows.Forms.Button bClearTable;
        private System.Windows.Forms.Button bSrcFromBuffer;
        private System.Windows.Forms.Button bSrcFromFile;
        private System.Windows.Forms.CheckBox cbQuotes;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button bStartCheck;
        private System.Windows.Forms.Button bSaveToFile;
        private System.Windows.Forms.Button bCopyToClipboard;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.ComboBox cbEnc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudMsisdnPos;
        private System.Windows.Forms.CheckBox cbMsisdnSubstr;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDocType;
        private System.Windows.Forms.ComboBox cbJournalOption;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbShowBalance;
        private System.Windows.Forms.CheckBox cbSearchByIcc;
    }
}