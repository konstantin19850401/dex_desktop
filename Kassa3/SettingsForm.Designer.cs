namespace Kassa3
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.gbLegalDate = new System.Windows.Forms.GroupBox();
            this.deLegalDate = new DEXExtendLib.DateEdit();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbDBTitle = new System.Windows.Forms.TextBox();
            this.gbLegalDate.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLegalDate
            // 
            this.gbLegalDate.Controls.Add(this.deLegalDate);
            this.gbLegalDate.Location = new System.Drawing.Point(12, 67);
            this.gbLegalDate.Name = "gbLegalDate";
            this.gbLegalDate.Size = new System.Drawing.Size(203, 50);
            this.gbLegalDate.TabIndex = 0;
            this.gbLegalDate.TabStop = false;
            this.gbLegalDate.Text = "Дата начала открытого периода";
            // 
            // deLegalDate
            // 
            this.deLegalDate.FormattingEnabled = true;
            this.deLegalDate.InputChar = '#';
            this.deLegalDate.Location = new System.Drawing.Point(6, 19);
            this.deLegalDate.MaxLength = 10;
            this.deLegalDate.Name = "deLegalDate";
            this.deLegalDate.Size = new System.Drawing.Size(121, 21);
            this.deLegalDate.TabIndex = 0;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(59, 123);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 1;
            this.bOk.Text = "ОК";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(140, 123);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbDBTitle);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 49);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Наименование базы";
            // 
            // tbDBTitle
            // 
            this.tbDBTitle.Location = new System.Drawing.Point(6, 19);
            this.tbDBTitle.Name = "tbDBTitle";
            this.tbDBTitle.Size = new System.Drawing.Size(191, 20);
            this.tbDBTitle.TabIndex = 0;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(227, 156);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.gbLegalDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Установки приложения";
            this.gbLegalDate.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLegalDate;
        private DEXExtendLib.DateEdit deLegalDate;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbDBTitle;
    }
}