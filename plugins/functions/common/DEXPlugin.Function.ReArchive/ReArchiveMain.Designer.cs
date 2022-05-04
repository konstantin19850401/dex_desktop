namespace DEXPlugin.Function.ReArchive
{
    partial class ReArchiveMain
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
            this.gb1 = new System.Windows.Forms.GroupBox();
            this.tb_listMSISDN = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.deStart = new DEXExtendLib.DateEdit();
            this.deEnd = new DEXExtendLib.DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.bMakeArchive = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.lDocCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bCheckCount = new System.Windows.Forms.Button();
            this.gb1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb1
            // 
            this.gb1.Controls.Add(this.tb_listMSISDN);
            this.gb1.Controls.Add(this.label4);
            this.gb1.Controls.Add(this.label1);
            this.gb1.Controls.Add(this.deStart);
            this.gb1.Controls.Add(this.deEnd);
            this.gb1.Controls.Add(this.label2);
            this.gb1.Location = new System.Drawing.Point(12, 12);
            this.gb1.Name = "gb1";
            this.gb1.Size = new System.Drawing.Size(359, 206);
            this.gb1.TabIndex = 11;
            this.gb1.TabStop = false;
            this.gb1.Text = "Период разархивирования";
            // 
            // tb_listMSISDN
            // 
            this.tb_listMSISDN.Location = new System.Drawing.Point(13, 70);
            this.tb_listMSISDN.Multiline = true;
            this.tb_listMSISDN.Name = "tb_listMSISDN";
            this.tb_listMSISDN.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_listMSISDN.Size = new System.Drawing.Size(334, 130);
            this.tb_listMSISDN.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(338, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Вставте номера(через запятую), которые необходимо проверить";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(231, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "по";
            // 
            // deStart
            // 
            this.deStart.FormattingEnabled = true;
            this.deStart.InputChar = '*';
            this.deStart.Location = new System.Drawing.Point(134, 17);
            this.deStart.MaxLength = 10;
            this.deStart.Name = "deStart";
            this.deStart.Size = new System.Drawing.Size(91, 21);
            this.deStart.TabIndex = 2;
            // 
            // deEnd
            // 
            this.deEnd.FormattingEnabled = true;
            this.deEnd.InputChar = '*';
            this.deEnd.Location = new System.Drawing.Point(256, 17);
            this.deEnd.MaxLength = 10;
            this.deEnd.Name = "deEnd";
            this.deEnd.Size = new System.Drawing.Size(91, 21);
            this.deEnd.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Интервал: с";
            // 
            // bMakeArchive
            // 
            this.bMakeArchive.Location = new System.Drawing.Point(12, 265);
            this.bMakeArchive.Name = "bMakeArchive";
            this.bMakeArchive.Size = new System.Drawing.Size(288, 23);
            this.bMakeArchive.TabIndex = 12;
            this.bMakeArchive.Text = "Произвести разархивирование документов";
            this.bMakeArchive.UseVisualStyleBackColor = true;
            this.bMakeArchive.Click += new System.EventHandler(this.bMakeArchive_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(306, 265);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(65, 23);
            this.bCancel.TabIndex = 13;
            this.bCancel.Text = "Выход";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // lDocCount
            // 
            this.lDocCount.AutoSize = true;
            this.lDocCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lDocCount.Location = new System.Drawing.Point(160, 232);
            this.lDocCount.Name = "lDocCount";
            this.lDocCount.Size = new System.Drawing.Size(11, 13);
            this.lDocCount.TabIndex = 15;
            this.lDocCount.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Количество документов:";
            // 
            // bCheckCount
            // 
            this.bCheckCount.Location = new System.Drawing.Point(268, 227);
            this.bCheckCount.Name = "bCheckCount";
            this.bCheckCount.Size = new System.Drawing.Size(91, 23);
            this.bCheckCount.TabIndex = 14;
            this.bCheckCount.Text = "Проверить";
            this.bCheckCount.UseVisualStyleBackColor = true;
            this.bCheckCount.Click += new System.EventHandler(this.bCheckCount_Click);
            // 
            // ReArchiveMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(385, 299);
            this.Controls.Add(this.lDocCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bCheckCount);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bMakeArchive);
            this.Controls.Add(this.gb1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReArchiveMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Возврат документов из архива (аварийная процедура)";
            this.gb1.ResumeLayout(false);
            this.gb1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb1;
        private System.Windows.Forms.Label label1;
        private DEXExtendLib.DateEdit deStart;
        private DEXExtendLib.DateEdit deEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bMakeArchive;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_listMSISDN;
        private System.Windows.Forms.Label lDocCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bCheckCount;
    }
}