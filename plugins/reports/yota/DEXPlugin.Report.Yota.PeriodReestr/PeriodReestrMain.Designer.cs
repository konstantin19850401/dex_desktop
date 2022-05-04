namespace DEXPlugin.Report.Yota.PeriodReestr
{
    partial class PeriodReestrMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.deEnd = new DEXExtendLib.DateEdit();
            this.deStart = new DEXExtendLib.DateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.cbDocType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbRegion = new System.Windows.Forms.ComboBox();
            this.cbDuty = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Начальная дата";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Конечная дата";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(104, 250);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 10;
            this.bOk.Text = "ОК";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(185, 250);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 11;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Отделение";
            // 
            // cbUnit
            // 
            this.cbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(15, 75);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(248, 21);
            this.cbUnit.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Фильтр статуса";
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Items.AddRange(new object[] {
            "Любой",
            "Черновик",
            "На подтверждение",
            "Подтверждён",
            "На отправку",
            "Отправлен",
            "Возвращён"});
            this.cbFilter.Location = new System.Drawing.Point(15, 115);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(248, 21);
            this.cbFilter.TabIndex = 4;
            // 
            // deEnd
            // 
            this.deEnd.FormattingEnabled = true;
            this.deEnd.InputChar = '_';
            this.deEnd.Location = new System.Drawing.Point(142, 31);
            this.deEnd.MaxLength = 10;
            this.deEnd.Name = "deEnd";
            this.deEnd.Size = new System.Drawing.Size(121, 21);
            this.deEnd.TabIndex = 1;
            // 
            // deStart
            // 
            this.deStart.FormattingEnabled = true;
            this.deStart.InputChar = '_';
            this.deStart.Location = new System.Drawing.Point(15, 31);
            this.deStart.MaxLength = 10;
            this.deStart.Name = "deStart";
            this.deStart.Size = new System.Drawing.Size(121, 21);
            this.deStart.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Тип документа";
            // 
            // cbDocType
            // 
            this.cbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocType.FormattingEnabled = true;
            this.cbDocType.Location = new System.Drawing.Point(15, 160);
            this.cbDocType.Name = "cbDocType";
            this.cbDocType.Size = new System.Drawing.Size(248, 21);
            this.cbDocType.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Регион";
            // 
            // cbRegion
            // 
            this.cbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRegion.FormattingEnabled = true;
            this.cbRegion.Location = new System.Drawing.Point(15, 200);
            this.cbRegion.Name = "cbRegion";
            this.cbRegion.Size = new System.Drawing.Size(248, 21);
            this.cbRegion.TabIndex = 7;
            // 
            // cbDuty
            // 
            this.cbDuty.AutoSize = true;
            this.cbDuty.Location = new System.Drawing.Point(15, 227);
            this.cbDuty.Name = "cbDuty";
            this.cbDuty.Size = new System.Drawing.Size(148, 17);
            this.cbDuty.TabIndex = 21;
            this.cbDuty.Text = "Отчёт только по долгам";
            this.cbDuty.UseVisualStyleBackColor = true;
            // 
            // PeriodReestrMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(277, 289);
            this.Controls.Add(this.cbDuty);
            this.Controls.Add(this.cbRegion);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbDocType);
            this.Controls.Add(this.cbFilter);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbUnit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deEnd);
            this.Controls.Add(this.deStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PeriodReestrMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Периодичный реестр договоров";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DEXExtendLib.DateEdit deStart;
        private DEXExtendLib.DateEdit deEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDocType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbRegion;
        private System.Windows.Forms.CheckBox cbDuty;
    }
}