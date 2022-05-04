namespace DEXPlugin.Report.Common.Sverka
{
    partial class SverkaMain
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
            this.deStart = new DEXExtendLib.DateEdit();
            this.deEnd = new DEXExtendLib.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bReport = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRepType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDocType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbJournalOption = new System.Windows.Forms.ComboBox();
            this.cbDevidePeriod = new System.Windows.Forms.CheckBox();
            this.cbShowRegion = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // deStart
            // 
            this.deStart.FormattingEnabled = true;
            this.deStart.InputChar = '*';
            this.deStart.Location = new System.Drawing.Point(12, 25);
            this.deStart.MaxLength = 10;
            this.deStart.Name = "deStart";
            this.deStart.Size = new System.Drawing.Size(121, 21);
            this.deStart.TabIndex = 0;
            // 
            // deEnd
            // 
            this.deEnd.FormattingEnabled = true;
            this.deEnd.InputChar = '*';
            this.deEnd.Location = new System.Drawing.Point(139, 25);
            this.deEnd.MaxLength = 10;
            this.deEnd.Name = "deEnd";
            this.deEnd.Size = new System.Drawing.Size(120, 21);
            this.deEnd.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Начальная дата";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Конечная дата";
            // 
            // bReport
            // 
            this.bReport.Location = new System.Drawing.Point(103, 238);
            this.bReport.Name = "bReport";
            this.bReport.Size = new System.Drawing.Size(75, 23);
            this.bReport.TabIndex = 5;
            this.bReport.Text = "Отчёт";
            this.bReport.UseVisualStyleBackColor = true;
            this.bReport.Click += new System.EventHandler(this.bReport_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(184, 238);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Выход";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Тип отчёта";
            // 
            // cbRepType
            // 
            this.cbRepType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRepType.FormattingEnabled = true;
            this.cbRepType.Items.AddRange(new object[] {
            "Количество",
            "Разбивка по ТП",
            "Разбивка по регионам",
            "Разбивка по балансу"});
            this.cbRepType.Location = new System.Drawing.Point(15, 94);
            this.cbRepType.Name = "cbRepType";
            this.cbRepType.Size = new System.Drawing.Size(244, 21);
            this.cbRepType.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Тип документа";
            // 
            // cbDocType
            // 
            this.cbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocType.FormattingEnabled = true;
            this.cbDocType.Location = new System.Drawing.Point(15, 140);
            this.cbDocType.Name = "cbDocType";
            this.cbDocType.Size = new System.Drawing.Size(244, 21);
            this.cbDocType.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Параметры журнала";
            // 
            // cbJournalOption
            // 
            this.cbJournalOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbJournalOption.FormattingEnabled = true;
            this.cbJournalOption.Items.AddRange(new object[] {
            "Только данные архива",
            "Только данные журнала",
            "Данные журнала и архива"});
            this.cbJournalOption.Location = new System.Drawing.Point(15, 191);
            this.cbJournalOption.Name = "cbJournalOption";
            this.cbJournalOption.Size = new System.Drawing.Size(244, 21);
            this.cbJournalOption.TabIndex = 17;
            // 
            // cbDevidePeriod
            // 
            this.cbDevidePeriod.AutoSize = true;
            this.cbDevidePeriod.Location = new System.Drawing.Point(15, 54);
            this.cbDevidePeriod.Name = "cbDevidePeriod";
            this.cbDevidePeriod.Size = new System.Drawing.Size(126, 17);
            this.cbDevidePeriod.TabIndex = 18;
            this.cbDevidePeriod.Text = "Разбить помесячно";
            this.cbDevidePeriod.UseVisualStyleBackColor = true;
            this.cbDevidePeriod.CheckedChanged += new System.EventHandler(this.cbDevidePeriod_CheckedChanged);
            // 
            // cbShowRegion
            // 
            this.cbShowRegion.AutoSize = true;
            this.cbShowRegion.Location = new System.Drawing.Point(147, 54);
            this.cbShowRegion.Name = "cbShowRegion";
            this.cbShowRegion.Size = new System.Drawing.Size(121, 17);
            this.cbShowRegion.TabIndex = 19;
            this.cbShowRegion.Text = "Отображать адрес";
            this.cbShowRegion.UseVisualStyleBackColor = true;
            // 
            // SverkaMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(271, 271);
            this.Controls.Add(this.cbShowRegion);
            this.Controls.Add(this.cbDevidePeriod);
            this.Controls.Add(this.cbJournalOption);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbDocType);
            this.Controls.Add(this.cbRepType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bReport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deEnd);
            this.Controls.Add(this.deStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SverkaMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сверка по ТП и документам";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DEXExtendLib.DateEdit deStart;
        private DEXExtendLib.DateEdit deEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bReport;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRepType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDocType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbJournalOption;
        private System.Windows.Forms.CheckBox cbDevidePeriod;
        private System.Windows.Forms.CheckBox cbShowRegion;
    }
}