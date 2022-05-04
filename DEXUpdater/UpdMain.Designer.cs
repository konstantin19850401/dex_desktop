namespace DEXUpdater
{
    partial class UpdMain
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
            this.tbSelectUpDir = new System.Windows.Forms.TextBox();
            this.bSelectUpDir = new System.Windows.Forms.Button();
            this.bDoUpdate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lMsg = new System.Windows.Forms.Label();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.cbForceUpdate = new System.Windows.Forms.CheckBox();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Каталог загрузки обновлений";
            // 
            // tbSelectUpDir
            // 
            this.tbSelectUpDir.Location = new System.Drawing.Point(15, 25);
            this.tbSelectUpDir.Name = "tbSelectUpDir";
            this.tbSelectUpDir.Size = new System.Drawing.Size(397, 20);
            this.tbSelectUpDir.TabIndex = 1;
            // 
            // bSelectUpDir
            // 
            this.bSelectUpDir.Location = new System.Drawing.Point(418, 22);
            this.bSelectUpDir.Name = "bSelectUpDir";
            this.bSelectUpDir.Size = new System.Drawing.Size(28, 23);
            this.bSelectUpDir.TabIndex = 2;
            this.bSelectUpDir.Text = "...";
            this.bSelectUpDir.UseVisualStyleBackColor = true;
            this.bSelectUpDir.Click += new System.EventHandler(this.bSelectUpDir_Click);
            // 
            // bDoUpdate
            // 
            this.bDoUpdate.Location = new System.Drawing.Point(15, 51);
            this.bDoUpdate.Name = "bDoUpdate";
            this.bDoUpdate.Size = new System.Drawing.Size(75, 23);
            this.bDoUpdate.TabIndex = 3;
            this.bDoUpdate.Text = "Обновить";
            this.bDoUpdate.UseVisualStyleBackColor = true;
            this.bDoUpdate.Click += new System.EventHandler(this.bDoUpdate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbLog);
            this.groupBox1.Controls.Add(this.lMsg);
            this.groupBox1.Controls.Add(this.pb);
            this.groupBox1.Location = new System.Drawing.Point(12, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 245);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Состояние";
            // 
            // lMsg
            // 
            this.lMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lMsg.Location = new System.Drawing.Point(6, 16);
            this.lMsg.Name = "lMsg";
            this.lMsg.Size = new System.Drawing.Size(422, 23);
            this.lMsg.TabIndex = 1;
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(6, 50);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(422, 23);
            this.pb.TabIndex = 0;
            // 
            // cbForceUpdate
            // 
            this.cbForceUpdate.AutoSize = true;
            this.cbForceUpdate.Location = new System.Drawing.Point(121, 55);
            this.cbForceUpdate.Name = "cbForceUpdate";
            this.cbForceUpdate.Size = new System.Drawing.Size(173, 17);
            this.cbForceUpdate.TabIndex = 5;
            this.cbForceUpdate.Text = "Принудительное обновление";
            this.cbForceUpdate.UseVisualStyleBackColor = true;
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(6, 79);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(422, 160);
            this.lbLog.TabIndex = 2;
            // 
            // UpdMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 337);
            this.Controls.Add(this.cbForceUpdate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bDoUpdate);
            this.Controls.Add(this.bSelectUpDir);
            this.Controls.Add(this.tbSelectUpDir);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обновление DEX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSelectUpDir;
        private System.Windows.Forms.Button bSelectUpDir;
        private System.Windows.Forms.Button bDoUpdate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lMsg;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.CheckBox cbForceUpdate;
        private System.Windows.Forms.ListBox lbLog;
    }
}