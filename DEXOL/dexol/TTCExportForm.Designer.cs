namespace dexol
{
    partial class TTCExportForm
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
            this.tbTTCDir = new System.Windows.Forms.TextBox();
            this.bSelectDir = new System.Windows.Forms.Button();
            this.bExport = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Каталог программы TinyTrade";
            // 
            // tbTTCDir
            // 
            this.tbTTCDir.Location = new System.Drawing.Point(12, 25);
            this.tbTTCDir.Name = "tbTTCDir";
            this.tbTTCDir.ReadOnly = true;
            this.tbTTCDir.Size = new System.Drawing.Size(323, 20);
            this.tbTTCDir.TabIndex = 1;
            // 
            // bSelectDir
            // 
            this.bSelectDir.Location = new System.Drawing.Point(341, 25);
            this.bSelectDir.Name = "bSelectDir";
            this.bSelectDir.Size = new System.Drawing.Size(30, 20);
            this.bSelectDir.TabIndex = 2;
            this.bSelectDir.Text = "...";
            this.bSelectDir.UseVisualStyleBackColor = true;
            this.bSelectDir.Click += new System.EventHandler(this.bSelectDir_Click);
            // 
            // bExport
            // 
            this.bExport.Location = new System.Drawing.Point(152, 51);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(138, 23);
            this.bExport.TabIndex = 3;
            this.bExport.Text = "Выгрузить документы";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(296, 51);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 4;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // TTCExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(379, 83);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.bSelectDir);
            this.Controls.Add(this.tbTTCDir);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TTCExportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Экспорт в TinyTrade";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTTCDir;
        private System.Windows.Forms.Button bSelectDir;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bCancel;
    }
}