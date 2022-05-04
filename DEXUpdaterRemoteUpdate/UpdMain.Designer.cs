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
            this.lbLog = new System.Windows.Forms.ListBox();
            this.lMsg = new System.Windows.Forms.Label();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.cbForceUpdate = new System.Windows.Forms.CheckBox();
            this.gbTypeSW = new System.Windows.Forms.GroupBox();
            this.rb_dex = new System.Windows.Forms.RadioButton();
            this.rb_dexol = new System.Windows.Forms.RadioButton();
            this.rb_kassa = new System.Windows.Forms.RadioButton();
            this.gbTypeRepo = new System.Windows.Forms.GroupBox();
            this.rbRem = new System.Windows.Forms.RadioButton();
            this.rbLoc = new System.Windows.Forms.RadioButton();
            this.btn_saveconf = new System.Windows.Forms.Button();
            this.rb_adapt_mega = new System.Windows.Forms.RadioButton();
            this.btn_recoveryVer = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.gbTypeSW.SuspendLayout();
            this.gbTypeRepo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Каталог загрузки обновлений";
            // 
            // tbSelectUpDir
            // 
            this.tbSelectUpDir.Location = new System.Drawing.Point(23, 84);
            this.tbSelectUpDir.Name = "tbSelectUpDir";
            this.tbSelectUpDir.Size = new System.Drawing.Size(356, 20);
            this.tbSelectUpDir.TabIndex = 1;
            // 
            // bSelectUpDir
            // 
            this.bSelectUpDir.Location = new System.Drawing.Point(385, 82);
            this.bSelectUpDir.Name = "bSelectUpDir";
            this.bSelectUpDir.Size = new System.Drawing.Size(28, 23);
            this.bSelectUpDir.TabIndex = 2;
            this.bSelectUpDir.Text = "...";
            this.bSelectUpDir.UseVisualStyleBackColor = true;
            this.bSelectUpDir.Click += new System.EventHandler(this.bSelectUpDir_Click);
            // 
            // bDoUpdate
            // 
            this.bDoUpdate.Location = new System.Drawing.Point(23, 110);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 243);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 245);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Состояние";
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(6, 79);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(422, 160);
            this.lbLog.TabIndex = 2;
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
            this.cbForceUpdate.Location = new System.Drawing.Point(206, 116);
            this.cbForceUpdate.Name = "cbForceUpdate";
            this.cbForceUpdate.Size = new System.Drawing.Size(173, 17);
            this.cbForceUpdate.TabIndex = 5;
            this.cbForceUpdate.Text = "Принудительное обновление";
            this.cbForceUpdate.UseVisualStyleBackColor = true;
            // 
            // gbTypeSW
            // 
            this.gbTypeSW.Controls.Add(this.rb_adapt_mega);
            this.gbTypeSW.Controls.Add(this.rb_dex);
            this.gbTypeSW.Controls.Add(this.rb_dexol);
            this.gbTypeSW.Controls.Add(this.rb_kassa);
            this.gbTypeSW.Location = new System.Drawing.Point(12, 3);
            this.gbTypeSW.Name = "gbTypeSW";
            this.gbTypeSW.Size = new System.Drawing.Size(283, 89);
            this.gbTypeSW.TabIndex = 6;
            this.gbTypeSW.TabStop = false;
            this.gbTypeSW.Text = "Выберите тип обновляемого ПО";
            // 
            // rb_dex
            // 
            this.rb_dex.AutoSize = true;
            this.rb_dex.Checked = true;
            this.rb_dex.Location = new System.Drawing.Point(23, 20);
            this.rb_dex.Name = "rb_dex";
            this.rb_dex.Size = new System.Drawing.Size(47, 17);
            this.rb_dex.TabIndex = 2;
            this.rb_dex.TabStop = true;
            this.rb_dex.Text = "DEX";
            this.rb_dex.UseVisualStyleBackColor = true;
            // 
            // rb_dexol
            // 
            this.rb_dexol.AutoSize = true;
            this.rb_dexol.Location = new System.Drawing.Point(23, 43);
            this.rb_dexol.Name = "rb_dexol";
            this.rb_dexol.Size = new System.Drawing.Size(61, 17);
            this.rb_dexol.TabIndex = 1;
            this.rb_dexol.TabStop = true;
            this.rb_dexol.Text = "DEXOL";
            this.rb_dexol.UseVisualStyleBackColor = true;
            // 
            // rb_kassa
            // 
            this.rb_kassa.AutoSize = true;
            this.rb_kassa.Location = new System.Drawing.Point(23, 66);
            this.rb_kassa.Name = "rb_kassa";
            this.rb_kassa.Size = new System.Drawing.Size(56, 17);
            this.rb_kassa.TabIndex = 0;
            this.rb_kassa.TabStop = true;
            this.rb_kassa.Text = "Касса";
            this.rb_kassa.UseVisualStyleBackColor = true;
            // 
            // gbTypeRepo
            // 
            this.gbTypeRepo.Controls.Add(this.rbRem);
            this.gbTypeRepo.Controls.Add(this.rbLoc);
            this.gbTypeRepo.Controls.Add(this.cbForceUpdate);
            this.gbTypeRepo.Controls.Add(this.label1);
            this.gbTypeRepo.Controls.Add(this.tbSelectUpDir);
            this.gbTypeRepo.Controls.Add(this.bDoUpdate);
            this.gbTypeRepo.Controls.Add(this.bSelectUpDir);
            this.gbTypeRepo.Location = new System.Drawing.Point(12, 98);
            this.gbTypeRepo.Name = "gbTypeRepo";
            this.gbTypeRepo.Size = new System.Drawing.Size(434, 139);
            this.gbTypeRepo.TabIndex = 7;
            this.gbTypeRepo.TabStop = false;
            this.gbTypeRepo.Text = "Тип используемого репозитория";
            // 
            // rbRem
            // 
            this.rbRem.AutoSize = true;
            this.rbRem.Location = new System.Drawing.Point(23, 17);
            this.rbRem.Name = "rbRem";
            this.rbRem.Size = new System.Drawing.Size(83, 17);
            this.rbRem.TabIndex = 4;
            this.rbRem.TabStop = true;
            this.rbRem.Text = "Удаленный";
            this.rbRem.UseVisualStyleBackColor = true;
            this.rbRem.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // rbLoc
            // 
            this.rbLoc.AutoSize = true;
            this.rbLoc.Checked = true;
            this.rbLoc.Location = new System.Drawing.Point(23, 40);
            this.rbLoc.Name = "rbLoc";
            this.rbLoc.Size = new System.Drawing.Size(83, 17);
            this.rbLoc.TabIndex = 3;
            this.rbLoc.TabStop = true;
            this.rbLoc.Text = "Локальный";
            this.rbLoc.UseVisualStyleBackColor = true;
            this.rbLoc.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // btn_saveconf
            // 
            this.btn_saveconf.Location = new System.Drawing.Point(301, 12);
            this.btn_saveconf.Name = "btn_saveconf";
            this.btn_saveconf.Size = new System.Drawing.Size(145, 28);
            this.btn_saveconf.TabIndex = 3;
            this.btn_saveconf.Text = "Сохранить конфигурацию";
            this.btn_saveconf.UseVisualStyleBackColor = true;
            this.btn_saveconf.Click += new System.EventHandler(this.SaveConfigugation);
            // 
            // rb_adapt_mega
            // 
            this.rb_adapt_mega.AutoSize = true;
            this.rb_adapt_mega.Location = new System.Drawing.Point(95, 20);
            this.rb_adapt_mega.Name = "rb_adapt_mega";
            this.rb_adapt_mega.Size = new System.Drawing.Size(121, 17);
            this.rb_adapt_mega.TabIndex = 8;
            this.rb_adapt_mega.TabStop = true;
            this.rb_adapt_mega.Text = "Адаптер мегафона";
            this.rb_adapt_mega.UseVisualStyleBackColor = true;
            // 
            // btn_recoveryVer
            // 
            this.btn_recoveryVer.Location = new System.Drawing.Point(301, 46);
            this.btn_recoveryVer.Name = "btn_recoveryVer";
            this.btn_recoveryVer.Size = new System.Drawing.Size(145, 40);
            this.btn_recoveryVer.TabIndex = 8;
            this.btn_recoveryVer.Text = "Восстановление предыдущей версии обновления";
            this.btn_recoveryVer.UseVisualStyleBackColor = true;
            this.btn_recoveryVer.Click += new System.EventHandler(this.button1_Click);
            // 
            // UpdMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 498);
            this.Controls.Add(this.btn_recoveryVer);
            this.Controls.Add(this.btn_saveconf);
            this.Controls.Add(this.gbTypeRepo);
            this.Controls.Add(this.gbTypeSW);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обновление ПО";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.gbTypeSW.ResumeLayout(false);
            this.gbTypeSW.PerformLayout();
            this.gbTypeRepo.ResumeLayout(false);
            this.gbTypeRepo.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.GroupBox gbTypeSW;
        private System.Windows.Forms.GroupBox gbTypeRepo;
        private System.Windows.Forms.RadioButton rbRem;
        private System.Windows.Forms.RadioButton rbLoc;
        private System.Windows.Forms.RadioButton rb_dex;
        private System.Windows.Forms.RadioButton rb_dexol;
        private System.Windows.Forms.RadioButton rb_kassa;
        private System.Windows.Forms.Button btn_saveconf;
        private System.Windows.Forms.RadioButton rb_adapt_mega;
        private System.Windows.Forms.Button btn_recoveryVer;
    }
}