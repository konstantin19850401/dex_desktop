namespace DEXPlugin.Report.Yota.DocDates
{
    partial class DocDatesMain
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
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.bLoadCheckSim = new System.Windows.Forms.Button();
            this.bReport = new System.Windows.Forms.Button();
            this.bExportClipboard = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.icc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_own = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Отделение";
            // 
            // cbUnit
            // 
            this.cbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(80, 6);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(563, 21);
            this.cbUnit.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Список карт";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.icc,
            this.DocDate,
            this.date_own,
            this.unit});
            this.dgv.Location = new System.Drawing.Point(12, 56);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.ShowCellErrors = false;
            this.dgv.ShowEditingIcon = false;
            this.dgv.ShowRowErrors = false;
            this.dgv.Size = new System.Drawing.Size(628, 255);
            this.dgv.TabIndex = 3;
            // 
            // bLoadCheckSim
            // 
            this.bLoadCheckSim.Location = new System.Drawing.Point(15, 317);
            this.bLoadCheckSim.Name = "bLoadCheckSim";
            this.bLoadCheckSim.Size = new System.Drawing.Size(628, 23);
            this.bLoadCheckSim.TabIndex = 4;
            this.bLoadCheckSim.Text = "Загрузить список SIM-карт из буфера обмена и проверить их";
            this.bLoadCheckSim.UseVisualStyleBackColor = true;
            this.bLoadCheckSim.Click += new System.EventHandler(this.bLoadCheckSim_Click);
            // 
            // bReport
            // 
            this.bReport.Location = new System.Drawing.Point(15, 346);
            this.bReport.Name = "bReport";
            this.bReport.Size = new System.Drawing.Size(179, 23);
            this.bReport.TabIndex = 5;
            this.bReport.Text = "Отчёт";
            this.bReport.UseVisualStyleBackColor = true;
            this.bReport.Click += new System.EventHandler(this.bReport_Click);
            // 
            // bExportClipboard
            // 
            this.bExportClipboard.Location = new System.Drawing.Point(200, 346);
            this.bExportClipboard.Name = "bExportClipboard";
            this.bExportClipboard.Size = new System.Drawing.Size(317, 23);
            this.bExportClipboard.TabIndex = 6;
            this.bExportClipboard.Text = "Выгрузить в буфер обмена";
            this.bExportClipboard.UseVisualStyleBackColor = true;
            this.bExportClipboard.Click += new System.EventHandler(this.bExportClipboard_Click);
            // 
            // bExit
            // 
            this.bExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bExit.Location = new System.Drawing.Point(523, 346);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(120, 23);
            this.bExit.TabIndex = 7;
            this.bExit.Text = "Выход";
            this.bExit.UseVisualStyleBackColor = true;
            // 
            // icc
            // 
            this.icc.DataPropertyName = "icc";
            this.icc.HeaderText = "ICC";
            this.icc.Name = "icc";
            // 
            // DocDate
            // 
            this.DocDate.DataPropertyName = "DocDate";
            this.DocDate.HeaderText = "Дата док.";
            this.DocDate.Name = "DocDate";
            // 
            // date_own
            // 
            this.date_own.DataPropertyName = "date_own";
            this.date_own.HeaderText = "Дата отгр.";
            this.date_own.Name = "date_own";
            // 
            // unit
            // 
            this.unit.DataPropertyName = "unit";
            this.unit.FillWeight = 200F;
            this.unit.HeaderText = "Отделение";
            this.unit.Name = "unit";
            this.unit.Width = 200;
            // 
            // DocDatesMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bExit;
            this.ClientSize = new System.Drawing.Size(655, 378);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bExportClipboard);
            this.Controls.Add(this.bReport);
            this.Controls.Add(this.bLoadCheckSim);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbUnit);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DocDatesMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметры отчёта";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button bLoadCheckSim;
        private System.Windows.Forms.Button bReport;
        private System.Windows.Forms.Button bExportClipboard;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn icc;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_own;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
    }
}