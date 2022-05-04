namespace DEXPlugin.Function.CreateDocsFromList
{
    partial class CreateDocsFromListMain
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
            this.dgvSrc = new System.Windows.Forms.DataGridView();
            this.bLoadFromClipboard = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbEnc = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbQuotes = new System.Windows.Forms.CheckBox();
            this.cbSeparator = new System.Windows.Forms.ComboBox();
            this.bMakeDocs = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.deDocDate = new DEXExtendLib.DateEdit();
            this.cbFixedDocDate = new System.Windows.Forms.CheckBox();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.cbUnitAsSIM = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbStatusDoc = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSrc)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSrc
            // 
            this.dgvSrc.AllowUserToAddRows = false;
            this.dgvSrc.AllowUserToDeleteRows = false;
            this.dgvSrc.AllowUserToResizeRows = false;
            this.dgvSrc.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSrc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSrc.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSrc.Location = new System.Drawing.Point(9, 12);
            this.dgvSrc.MultiSelect = false;
            this.dgvSrc.Name = "dgvSrc";
            this.dgvSrc.RowHeadersVisible = false;
            this.dgvSrc.RowHeadersWidth = 60;
            this.dgvSrc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSrc.ShowEditingIcon = false;
            this.dgvSrc.Size = new System.Drawing.Size(769, 414);
            this.dgvSrc.TabIndex = 2;
            this.dgvSrc.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvSrc_RowPrePaint);
            // 
            // bLoadFromClipboard
            // 
            this.bLoadFromClipboard.Location = new System.Drawing.Point(340, 587);
            this.bLoadFromClipboard.Name = "bLoadFromClipboard";
            this.bLoadFromClipboard.Size = new System.Drawing.Size(177, 23);
            this.bLoadFromClipboard.TabIndex = 6;
            this.bLoadFromClipboard.Text = "Заполнить из буфера обмена";
            this.bLoadFromClipboard.UseVisualStyleBackColor = true;
            this.bLoadFromClipboard.Click += new System.EventHandler(this.bLoadFromClipboard_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbEnc);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbQuotes);
            this.groupBox2.Controls.Add(this.cbSeparator);
            this.groupBox2.Location = new System.Drawing.Point(9, 432);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(769, 48);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры";
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
            // bMakeDocs
            // 
            this.bMakeDocs.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bMakeDocs.Enabled = false;
            this.bMakeDocs.Location = new System.Drawing.Point(523, 587);
            this.bMakeDocs.Name = "bMakeDocs";
            this.bMakeDocs.Size = new System.Drawing.Size(174, 23);
            this.bMakeDocs.TabIndex = 8;
            this.bMakeDocs.Text = "Сформировать документы";
            this.bMakeDocs.UseVisualStyleBackColor = true;
            this.bMakeDocs.Click += new System.EventHandler(this.bMakeDocs_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(703, 587);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 9;
            this.bCancel.Text = "Выход";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbStatusDoc);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.deDocDate);
            this.groupBox3.Controls.Add(this.cbFixedDocDate);
            this.groupBox3.Controls.Add(this.cbUnit);
            this.groupBox3.Controls.Add(this.cbUnitAsSIM);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(9, 486);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(769, 95);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Данные документа";
            // 
            // deDocDate
            // 
            this.deDocDate.Enabled = false;
            this.deDocDate.FormattingEnabled = true;
            this.deDocDate.InputChar = '*';
            this.deDocDate.Location = new System.Drawing.Point(136, 19);
            this.deDocDate.MaxLength = 10;
            this.deDocDate.Name = "deDocDate";
            this.deDocDate.Size = new System.Drawing.Size(81, 21);
            this.deDocDate.TabIndex = 22;
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
            this.cbUnit.Enabled = false;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(380, 48);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(234, 21);
            this.cbUnit.TabIndex = 21;
            // 
            // cbUnitAsSIM
            // 
            this.cbUnitAsSIM.AutoSize = true;
            this.cbUnitAsSIM.Checked = true;
            this.cbUnitAsSIM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUnitAsSIM.Location = new System.Drawing.Point(207, 50);
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
            this.label4.Location = new System.Drawing.Point(6, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(195, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Отделение создаваемого документа";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(438, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Статус создаваемых документов";
            // 
            // cbStatusDoc
            // 
            this.cbStatusDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatusDoc.FormattingEnabled = true;
            this.cbStatusDoc.Items.AddRange(new object[] {
            "Черновик",
            "На подтверждение",
            "Подтвержден",
            "На отправку",
            "Отправлен"});
            this.cbStatusDoc.Location = new System.Drawing.Point(620, 16);
            this.cbStatusDoc.Name = "cbStatusDoc";
            this.cbStatusDoc.Size = new System.Drawing.Size(143, 21);
            this.cbStatusDoc.TabIndex = 24;
            // 
            // CreateDocsFromListMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 622);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bMakeDocs);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bLoadFromClipboard);
            this.Controls.Add(this.dgvSrc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateDocsFromListMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Создание договора из файла";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSrc)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSrc;
        private System.Windows.Forms.Button bLoadFromClipboard;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbEnc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbQuotes;
        private System.Windows.Forms.ComboBox cbSeparator;
        private System.Windows.Forms.Button bMakeDocs;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbFixedDocDate;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.CheckBox cbUnitAsSIM;
        private System.Windows.Forms.Label label4;
        private DEXExtendLib.DateEdit deDocDate;
        private System.Windows.Forms.ComboBox cbStatusDoc;
        private System.Windows.Forms.Label label3;

    }
}