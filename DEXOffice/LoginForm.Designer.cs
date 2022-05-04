namespace DEXOffice
{
    partial class LoginForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSaveLogin = new System.Windows.Forms.CheckBox();
            this.bLogin = new System.Windows.Forms.Button();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nudTimeout = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.tbDbName = new System.Windows.Forms.TextBox();
            this.tbDbServer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bCheckConnection = new System.Windows.Forms.Button();
            this.tbDbPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDbUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbSMClearSearchSettings = new System.Windows.Forms.CheckBox();
            this.cbClearPrinterSettings = new System.Windows.Forms.CheckBox();
            this.cbSMIgnoreDateInterval = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbPasspRem = new System.Windows.Forms.RadioButton();
            this.rbPasspLoc = new System.Windows.Forms.RadioButton();
            this.tbPasspData = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbPasspHost = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbPasspName = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tbPasspUser = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tbPasspPass = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeout)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tbPasspData.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSaveLogin);
            this.groupBox1.Controls.Add(this.bLogin);
            this.groupBox1.Controls.Add(this.tbPass);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbLogin);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 89);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Пользователь";
            // 
            // cbSaveLogin
            // 
            this.cbSaveLogin.AutoSize = true;
            this.cbSaveLogin.Location = new System.Drawing.Point(6, 62);
            this.cbSaveLogin.Name = "cbSaveLogin";
            this.cbSaveLogin.Size = new System.Drawing.Size(187, 17);
            this.cbSaveLogin.TabIndex = 3;
            this.cbSaveLogin.Text = "Запомнить этого пользователя";
            this.cbSaveLogin.UseVisualStyleBackColor = true;
            // 
            // bLogin
            // 
            this.bLogin.Location = new System.Drawing.Point(242, 58);
            this.bLogin.Name = "bLogin";
            this.bLogin.Size = new System.Drawing.Size(106, 23);
            this.bLogin.TabIndex = 4;
            this.bLogin.Text = "Вход в систему";
            this.bLogin.UseVisualStyleBackColor = true;
            this.bLogin.Click += new System.EventHandler(this.bLogin_Click);
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(180, 32);
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(168, 20);
            this.tbPass.TabIndex = 2;
            this.tbPass.Text = "admin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Пароль";
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(6, 32);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(168, 20);
            this.tbLogin.TabIndex = 1;
            this.tbLogin.Text = "admin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Имя пользователя";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudTimeout);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbDbName);
            this.groupBox2.Controls.Add(this.tbDbServer);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.bCheckConnection);
            this.groupBox2.Controls.Add(this.tbDbPass);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbDbUser);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(6, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(354, 143);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "База данных";
            // 
            // nudTimeout
            // 
            this.nudTimeout.Location = new System.Drawing.Point(213, 87);
            this.nudTimeout.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.nudTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTimeout.Name = "nudTimeout";
            this.nudTimeout.Size = new System.Drawing.Size(135, 20);
            this.nudTimeout.TabIndex = 13;
            this.nudTimeout.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(210, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Тайм-аут (сек)";
            // 
            // tbDbName
            // 
            this.tbDbName.Location = new System.Drawing.Point(213, 38);
            this.tbDbName.Name = "tbDbName";
            this.tbDbName.Size = new System.Drawing.Size(135, 20);
            this.tbDbName.TabIndex = 7;
            this.tbDbName.Text = "dex";
            // 
            // tbDbServer
            // 
            this.tbDbServer.Location = new System.Drawing.Point(9, 38);
            this.tbDbServer.Name = "tbDbServer";
            this.tbDbServer.Size = new System.Drawing.Size(198, 20);
            this.tbDbServer.TabIndex = 6;
            this.tbDbServer.Text = "192.168.0.23";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(210, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Наименование БД";
            // 
            // bCheckConnection
            // 
            this.bCheckConnection.Location = new System.Drawing.Point(213, 113);
            this.bCheckConnection.Name = "bCheckConnection";
            this.bCheckConnection.Size = new System.Drawing.Size(135, 23);
            this.bCheckConnection.TabIndex = 10;
            this.bCheckConnection.Text = "Проверка соединения";
            this.bCheckConnection.UseVisualStyleBackColor = true;
            this.bCheckConnection.Click += new System.EventHandler(this.bCheckConnection_Click);
            // 
            // tbDbPass
            // 
            this.tbDbPass.Location = new System.Drawing.Point(114, 87);
            this.tbDbPass.Name = "tbDbPass";
            this.tbDbPass.PasswordChar = '*';
            this.tbDbPass.Size = new System.Drawing.Size(93, 20);
            this.tbDbPass.TabIndex = 9;
            this.tbDbPass.Text = "dex";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(111, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Пароль к БД";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Пользователь БД";
            // 
            // tbDbUser
            // 
            this.tbDbUser.Location = new System.Drawing.Point(9, 87);
            this.tbDbUser.Name = "tbDbUser";
            this.tbDbUser.Size = new System.Drawing.Size(97, 20);
            this.tbDbUser.TabIndex = 8;
            this.tbDbUser.Text = "dex";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Сервер";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbSMClearSearchSettings);
            this.groupBox3.Controls.Add(this.cbClearPrinterSettings);
            this.groupBox3.Controls.Add(this.cbSMIgnoreDateInterval);
            this.groupBox3.Location = new System.Drawing.Point(6, 341);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(354, 91);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Безопасный режим";
            // 
            // cbSMClearSearchSettings
            // 
            this.cbSMClearSearchSettings.AutoSize = true;
            this.cbSMClearSearchSettings.Location = new System.Drawing.Point(9, 42);
            this.cbSMClearSearchSettings.Name = "cbSMClearSearchSettings";
            this.cbSMClearSearchSettings.Size = new System.Drawing.Size(168, 17);
            this.cbSMClearSearchSettings.TabIndex = 2;
            this.cbSMClearSearchSettings.Text = "Сбросить установки поиска";
            this.cbSMClearSearchSettings.UseVisualStyleBackColor = true;
            // 
            // cbClearPrinterSettings
            // 
            this.cbClearPrinterSettings.AutoSize = true;
            this.cbClearPrinterSettings.Location = new System.Drawing.Point(9, 65);
            this.cbClearPrinterSettings.Name = "cbClearPrinterSettings";
            this.cbClearPrinterSettings.Size = new System.Drawing.Size(179, 17);
            this.cbClearPrinterSettings.TabIndex = 1;
            this.cbClearPrinterSettings.Text = "Сбросить установки принтера";
            this.cbClearPrinterSettings.UseVisualStyleBackColor = true;
            // 
            // cbSMIgnoreDateInterval
            // 
            this.cbSMIgnoreDateInterval.AutoSize = true;
            this.cbSMIgnoreDateInterval.Location = new System.Drawing.Point(9, 19);
            this.cbSMIgnoreDateInterval.Name = "cbSMIgnoreDateInterval";
            this.cbSMIgnoreDateInterval.Size = new System.Drawing.Size(310, 17);
            this.cbSMIgnoreDateInterval.TabIndex = 0;
            this.cbSMIgnoreDateInterval.Text = "Сбросить установленный временной интервал журнала";
            this.cbSMIgnoreDateInterval.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbPasspRem);
            this.groupBox4.Controls.Add(this.rbPasspLoc);
            this.groupBox4.Controls.Add(this.tbPasspData);
            this.groupBox4.Location = new System.Drawing.Point(6, 256);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(354, 79);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Сервер проверки паспортных данных";
            // 
            // rbPasspRem
            // 
            this.rbPasspRem.AutoSize = true;
            this.rbPasspRem.Checked = true;
            this.rbPasspRem.Location = new System.Drawing.Point(4, 41);
            this.rbPasspRem.Name = "rbPasspRem";
            this.rbPasspRem.Size = new System.Drawing.Size(83, 17);
            this.rbPasspRem.TabIndex = 9;
            this.rbPasspRem.TabStop = true;
            this.rbPasspRem.Text = "Удаленный";
            this.rbPasspRem.UseVisualStyleBackColor = true;
            this.rbPasspRem.CheckedChanged += new System.EventHandler(this.ChangedPasspServer);
            // 
            // rbPasspLoc
            // 
            this.rbPasspLoc.AutoSize = true;
            this.rbPasspLoc.Location = new System.Drawing.Point(4, 19);
            this.rbPasspLoc.Name = "rbPasspLoc";
            this.rbPasspLoc.Size = new System.Drawing.Size(83, 17);
            this.rbPasspLoc.TabIndex = 8;
            this.rbPasspLoc.Text = "Локальный";
            this.rbPasspLoc.UseVisualStyleBackColor = true;
            this.rbPasspLoc.CheckedChanged += new System.EventHandler(this.ChangedPasspServer);
            // 
            // tbPasspData
            // 
            this.tbPasspData.Controls.Add(this.tabPage1);
            this.tbPasspData.Controls.Add(this.tabPage2);
            this.tbPasspData.Controls.Add(this.tabPage3);
            this.tbPasspData.Controls.Add(this.tabPage4);
            this.tbPasspData.Location = new System.Drawing.Point(91, 19);
            this.tbPasspData.Name = "tbPasspData";
            this.tbPasspData.SelectedIndex = 0;
            this.tbPasspData.Size = new System.Drawing.Size(257, 47);
            this.tbPasspData.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbPasspHost);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(249, 21);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Сервер";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbPasspHost
            // 
            this.tbPasspHost.Location = new System.Drawing.Point(0, 0);
            this.tbPasspHost.Name = "tbPasspHost";
            this.tbPasspHost.Size = new System.Drawing.Size(249, 20);
            this.tbPasspHost.TabIndex = 3;
            this.tbPasspHost.Text = "192.168.0.64";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbPasspName);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(249, 21);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Наименование бд";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbPasspName
            // 
            this.tbPasspName.Location = new System.Drawing.Point(0, 1);
            this.tbPasspName.Name = "tbPasspName";
            this.tbPasspName.Size = new System.Drawing.Size(249, 20);
            this.tbPasspName.TabIndex = 0;
            this.tbPasspName.Text = "passports";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tbPasspUser);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(249, 21);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Пользователь";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tbPasspUser
            // 
            this.tbPasspUser.Location = new System.Drawing.Point(0, 1);
            this.tbPasspUser.Name = "tbPasspUser";
            this.tbPasspUser.Size = new System.Drawing.Size(251, 20);
            this.tbPasspUser.TabIndex = 0;
            this.tbPasspUser.Text = "passport";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tbPasspPass);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(249, 21);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Пароль";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tbPasspPass
            // 
            this.tbPasspPass.Location = new System.Drawing.Point(0, 1);
            this.tbPasspPass.Name = "tbPasspPass";
            this.tbPasspPass.PasswordChar = '*';
            this.tbPasspPass.Size = new System.Drawing.Size(249, 20);
            this.tbPasspPass.TabIndex = 0;
            this.tbPasspPass.Text = "12473513";
            // 
            // LoginForm
            // 
            this.AcceptButton = this.bLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 443);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вход в систему";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeout)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tbPasspData.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bLogin;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bCheckConnection;
        private System.Windows.Forms.TextBox tbDbPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDbUser;
        private System.Windows.Forms.TextBox tbDbName;
        private System.Windows.Forms.TextBox tbDbServer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbSaveLogin;
        private System.Windows.Forms.NumericUpDown nudTimeout;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.CheckBox cbClearPrinterSettings;
        public System.Windows.Forms.CheckBox cbSMIgnoreDateInterval;
        public System.Windows.Forms.CheckBox cbSMClearSearchSettings;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabControl tbPasspData;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox tbPasspHost;
        private System.Windows.Forms.TextBox tbPasspName;
        private System.Windows.Forms.TextBox tbPasspUser;
        private System.Windows.Forms.TextBox tbPasspPass;
        private System.Windows.Forms.RadioButton rbPasspRem;
        private System.Windows.Forms.RadioButton rbPasspLoc;

    }
}