namespace dexol
{
    partial class DocsReportForm 
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.deDateEnd = new DEXExtendLib.DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.deDateStart = new DEXExtendLib.DateEdit();
            this.bReport = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbSortBy = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Дата начала";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.deDateEnd);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.deDateStart);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 78);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Интервал отчёта";
            // 
            // deDateEnd
            // 
            this.deDateEnd.FormattingEnabled = true;
            this.deDateEnd.InputChar = '#';
            this.deDateEnd.Location = new System.Drawing.Point(104, 48);
            this.deDateEnd.MaxLength = 10;
            this.deDateEnd.Name = "deDateEnd";
            this.deDateEnd.Size = new System.Drawing.Size(100, 21);
            this.deDateEnd.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Дата конца";
            // 
            // deDateStart
            // 
            this.deDateStart.FormattingEnabled = true;
            this.deDateStart.InputChar = '#';
            this.deDateStart.Location = new System.Drawing.Point(104, 19);
            this.deDateStart.MaxLength = 10;
            this.deDateStart.Name = "deDateStart";
            this.deDateStart.Size = new System.Drawing.Size(100, 21);
            this.deDateStart.TabIndex = 1;
            // 
            // bReport
            // 
            this.bReport.Location = new System.Drawing.Point(12, 157);
            this.bReport.Name = "bReport";
            this.bReport.Size = new System.Drawing.Size(135, 23);
            this.bReport.TabIndex = 2;
            this.bReport.Text = "Сформировать отчёт";
            this.bReport.UseVisualStyleBackColor = true;
            this.bReport.Click += new System.EventHandler(this.bReport_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(153, 157);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Выход";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbSortBy);
            this.groupBox2.Location = new System.Drawing.Point(12, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 54);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сортировка по";
            // 
            // cbSortBy
            // 
            this.cbSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSortBy.FormattingEnabled = true;
            this.cbSortBy.Items.AddRange(new object[] {
            "Дате документа",
            "Статусу",
            "№ документа",
            "Типу документа",
            "Содержанию"});
            this.cbSortBy.Location = new System.Drawing.Point(9, 19);
            this.cbSortBy.Name = "cbSortBy";
            this.cbSortBy.Size = new System.Drawing.Size(195, 21);
            this.cbSortBy.TabIndex = 0;
            // 
            // DocsReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(239, 190);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bReport);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DocsReportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Отчёт по внесённым документам";
            this.Shown += new System.EventHandler(this.DocsReportForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DocsReportForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DEXExtendLib.DateEdit deDateStart;
        private DEXExtendLib.DateEdit deDateEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bReport;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbSortBy;
    }
}