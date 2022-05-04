namespace Kassa3
{
    partial class NetLoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetLoginForm));
            this.bConnectNetworkDB = new System.Windows.Forms.Button();
            this.gbKassa = new System.Windows.Forms.GroupBox();
            this.cbUnlockUser = new System.Windows.Forms.CheckBox();
            this.bSearchNetworkDb = new System.Windows.Forms.Button();
            this.tbKassaPassword = new System.Windows.Forms.TextBox();
            this.tbKassaUser = new System.Windows.Forms.TextBox();
            this.cbKassaBase = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.gbMysql = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMysqlPassword = new System.Windows.Forms.TextBox();
            this.tbMysqlHost = new System.Windows.Forms.TextBox();
            this.tbMysqlUser = new System.Windows.Forms.TextBox();
            this.nudMysqlPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bExitFromKassa = new System.Windows.Forms.Button();
            this.cbSavePassword = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bLiteForget = new System.Windows.Forms.Button();
            this.bLiteCreate = new System.Windows.Forms.Button();
            this.bLiteOpen = new System.Windows.Forms.Button();
            this.cbLiteRemember = new System.Windows.Forms.CheckBox();
            this.tbLitePass = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbLiteUser = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbLiteBases = new System.Windows.Forms.ListBox();
            this.gbKassa.SuspendLayout();
            this.gbMysql.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMysqlPort)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bConnectNetworkDB
            // 
            this.bConnectNetworkDB.Location = new System.Drawing.Point(214, 165);
            this.bConnectNetworkDB.Name = "bConnectNetworkDB";
            this.bConnectNetworkDB.Size = new System.Drawing.Size(179, 23);
            this.bConnectNetworkDB.TabIndex = 10;
            this.bConnectNetworkDB.Text = "Подключиться к кассе в сети";
            this.bConnectNetworkDB.UseVisualStyleBackColor = true;
            this.bConnectNetworkDB.Click += new System.EventHandler(this.bConnectNetworkDB_Click);
            // 
            // gbKassa
            // 
            this.gbKassa.Controls.Add(this.cbUnlockUser);
            this.gbKassa.Controls.Add(this.bSearchNetworkDb);
            this.gbKassa.Controls.Add(this.tbKassaPassword);
            this.gbKassa.Controls.Add(this.tbKassaUser);
            this.gbKassa.Controls.Add(this.cbKassaBase);
            this.gbKassa.Controls.Add(this.label7);
            this.gbKassa.Controls.Add(this.label6);
            this.gbKassa.Controls.Add(this.label5);
            this.gbKassa.Location = new System.Drawing.Point(6, 76);
            this.gbKassa.Name = "gbKassa";
            this.gbKassa.Size = new System.Drawing.Size(455, 83);
            this.gbKassa.TabIndex = 9;
            this.gbKassa.TabStop = false;
            this.gbKassa.Text = "База данных \"Касса\"";
            // 
            // cbUnlockUser
            // 
            this.cbUnlockUser.AutoSize = true;
            this.cbUnlockUser.ForeColor = System.Drawing.Color.Red;
            this.cbUnlockUser.Location = new System.Drawing.Point(6, 59);
            this.cbUnlockUser.Name = "cbUnlockUser";
            this.cbUnlockUser.Size = new System.Drawing.Size(323, 17);
            this.cbUnlockUser.TabIndex = 7;
            this.cbUnlockUser.Text = "Разблокировать записи в журнале (аварийная процедура)";
            this.cbUnlockUser.UseVisualStyleBackColor = true;
            // 
            // bSearchNetworkDb
            // 
            this.bSearchNetworkDb.Location = new System.Drawing.Point(210, 30);
            this.bSearchNetworkDb.Name = "bSearchNetworkDb";
            this.bSearchNetworkDb.Size = new System.Drawing.Size(23, 23);
            this.bSearchNetworkDb.TabIndex = 6;
            this.bSearchNetworkDb.Text = "?";
            this.bSearchNetworkDb.UseVisualStyleBackColor = true;
            this.bSearchNetworkDb.Click += new System.EventHandler(this.bSearchNetworkDb_Click);
            // 
            // tbKassaPassword
            // 
            this.tbKassaPassword.Location = new System.Drawing.Point(345, 32);
            this.tbKassaPassword.Name = "tbKassaPassword";
            this.tbKassaPassword.PasswordChar = '*';
            this.tbKassaPassword.Size = new System.Drawing.Size(100, 20);
            this.tbKassaPassword.TabIndex = 5;
            // 
            // tbKassaUser
            // 
            this.tbKassaUser.Location = new System.Drawing.Point(239, 32);
            this.tbKassaUser.Name = "tbKassaUser";
            this.tbKassaUser.Size = new System.Drawing.Size(100, 20);
            this.tbKassaUser.TabIndex = 4;
            // 
            // cbKassaBase
            // 
            this.cbKassaBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKassaBase.FormattingEnabled = true;
            this.cbKassaBase.Location = new System.Drawing.Point(6, 32);
            this.cbKassaBase.Name = "cbKassaBase";
            this.cbKassaBase.Size = new System.Drawing.Size(198, 21);
            this.cbKassaBase.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(342, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Пароль";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(236, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Пользователь";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Конфигурация";
            // 
            // gbMysql
            // 
            this.gbMysql.Controls.Add(this.label1);
            this.gbMysql.Controls.Add(this.label4);
            this.gbMysql.Controls.Add(this.tbMysqlPassword);
            this.gbMysql.Controls.Add(this.tbMysqlHost);
            this.gbMysql.Controls.Add(this.tbMysqlUser);
            this.gbMysql.Controls.Add(this.nudMysqlPort);
            this.gbMysql.Controls.Add(this.label2);
            this.gbMysql.Controls.Add(this.label3);
            this.gbMysql.Location = new System.Drawing.Point(6, 6);
            this.gbMysql.Name = "gbMysql";
            this.gbMysql.Size = new System.Drawing.Size(455, 64);
            this.gbMysql.TabIndex = 8;
            this.gbMysql.TabStop = false;
            this.gbMysql.Text = "Параметры сервера";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Адрес";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(342, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Пароль";
            // 
            // tbMysqlPassword
            // 
            this.tbMysqlPassword.Location = new System.Drawing.Point(345, 32);
            this.tbMysqlPassword.Name = "tbMysqlPassword";
            this.tbMysqlPassword.PasswordChar = '*';
            this.tbMysqlPassword.Size = new System.Drawing.Size(100, 20);
            this.tbMysqlPassword.TabIndex = 7;
            // 
            // tbMysqlHost
            // 
            this.tbMysqlHost.Location = new System.Drawing.Point(6, 32);
            this.tbMysqlHost.Name = "tbMysqlHost";
            this.tbMysqlHost.Size = new System.Drawing.Size(138, 20);
            this.tbMysqlHost.TabIndex = 4;
            this.tbMysqlHost.Text = "192.168.2.50";
            // 
            // tbMysqlUser
            // 
            this.tbMysqlUser.Location = new System.Drawing.Point(239, 32);
            this.tbMysqlUser.Name = "tbMysqlUser";
            this.tbMysqlUser.Size = new System.Drawing.Size(100, 20);
            this.tbMysqlUser.TabIndex = 6;
            this.tbMysqlUser.Text = "kassa";
            // 
            // nudMysqlPort
            // 
            this.nudMysqlPort.Location = new System.Drawing.Point(150, 33);
            this.nudMysqlPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudMysqlPort.Name = "nudMysqlPort";
            this.nudMysqlPort.Size = new System.Drawing.Size(83, 20);
            this.nudMysqlPort.TabIndex = 5;
            this.nudMysqlPort.Value = new decimal(new int[] {
            3306,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Порт";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(236, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Пользователь";
            // 
            // bExitFromKassa
            // 
            this.bExitFromKassa.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bExitFromKassa.Location = new System.Drawing.Point(399, 165);
            this.bExitFromKassa.Name = "bExitFromKassa";
            this.bExitFromKassa.Size = new System.Drawing.Size(62, 23);
            this.bExitFromKassa.TabIndex = 1;
            this.bExitFromKassa.Text = "Выйти";
            this.bExitFromKassa.UseVisualStyleBackColor = true;
            this.bExitFromKassa.Click += new System.EventHandler(this.bExitFromKassa_Click);
            // 
            // cbSavePassword
            // 
            this.cbSavePassword.AutoSize = true;
            this.cbSavePassword.Location = new System.Drawing.Point(6, 169);
            this.cbSavePassword.Name = "cbSavePassword";
            this.cbSavePassword.Size = new System.Drawing.Size(156, 17);
            this.cbSavePassword.TabIndex = 7;
            this.cbSavePassword.Text = "Запомнить пользователя";
            this.cbSavePassword.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(479, 223);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.gbMysql);
            this.tabPage1.Controls.Add(this.gbKassa);
            this.tabPage1.Controls.Add(this.cbSavePassword);
            this.tabPage1.Controls.Add(this.bExitFromKassa);
            this.tabPage1.Controls.Add(this.bConnectNetworkDB);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(471, 197);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Сетевая база данных";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.bLiteForget);
            this.tabPage2.Controls.Add(this.bLiteCreate);
            this.tabPage2.Controls.Add(this.bLiteOpen);
            this.tabPage2.Controls.Add(this.cbLiteRemember);
            this.tabPage2.Controls.Add(this.tbLitePass);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.tbLiteUser);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(471, 197);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Локальная база данных";
            // 
            // bLiteForget
            // 
            this.bLiteForget.Location = new System.Drawing.Point(405, 87);
            this.bLiteForget.Name = "bLiteForget";
            this.bLiteForget.Size = new System.Drawing.Size(60, 24);
            this.bLiteForget.TabIndex = 20;
            this.bLiteForget.Text = "Забыть";
            this.bLiteForget.UseVisualStyleBackColor = true;
            this.bLiteForget.Click += new System.EventHandler(this.bLiteForget_Click);
            // 
            // bLiteCreate
            // 
            this.bLiteCreate.Location = new System.Drawing.Point(317, 162);
            this.bLiteCreate.Name = "bLiteCreate";
            this.bLiteCreate.Size = new System.Drawing.Size(148, 23);
            this.bLiteCreate.TabIndex = 19;
            this.bLiteCreate.Text = "Новая база данных";
            this.bLiteCreate.UseVisualStyleBackColor = true;
            this.bLiteCreate.Click += new System.EventHandler(this.bLiteCreate_Click);
            // 
            // bLiteOpen
            // 
            this.bLiteOpen.Location = new System.Drawing.Point(317, 117);
            this.bLiteOpen.Name = "bLiteOpen";
            this.bLiteOpen.Size = new System.Drawing.Size(148, 23);
            this.bLiteOpen.TabIndex = 18;
            this.bLiteOpen.Text = "Открыть базу данных";
            this.bLiteOpen.UseVisualStyleBackColor = true;
            this.bLiteOpen.Click += new System.EventHandler(this.bLiteOpen_Click);
            // 
            // cbLiteRemember
            // 
            this.cbLiteRemember.AutoSize = true;
            this.cbLiteRemember.Location = new System.Drawing.Point(317, 92);
            this.cbLiteRemember.Name = "cbLiteRemember";
            this.cbLiteRemember.Size = new System.Drawing.Size(82, 17);
            this.cbLiteRemember.TabIndex = 17;
            this.cbLiteRemember.Text = "Запомнить";
            this.cbLiteRemember.UseVisualStyleBackColor = true;
            // 
            // tbLitePass
            // 
            this.tbLitePass.Location = new System.Drawing.Point(317, 61);
            this.tbLitePass.Name = "tbLitePass";
            this.tbLitePass.Size = new System.Drawing.Size(148, 20);
            this.tbLitePass.TabIndex = 16;
            this.tbLitePass.UseSystemPasswordChar = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(314, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Пароль";
            // 
            // tbLiteUser
            // 
            this.tbLiteUser.Location = new System.Drawing.Point(317, 22);
            this.tbLiteUser.Name = "tbLiteUser";
            this.tbLiteUser.Size = new System.Drawing.Size(148, 20);
            this.tbLiteUser.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(314, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Пользователь";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbLiteBases);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 185);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Базы данных";
            // 
            // lbLiteBases
            // 
            this.lbLiteBases.FormattingEnabled = true;
            this.lbLiteBases.Location = new System.Drawing.Point(6, 19);
            this.lbLiteBases.Name = "lbLiteBases";
            this.lbLiteBases.Size = new System.Drawing.Size(293, 160);
            this.lbLiteBases.TabIndex = 0;
            this.lbLiteBases.SelectedIndexChanged += new System.EventHandler(this.lbLiteBases_SelectedIndexChanged);
            this.lbLiteBases.DoubleClick += new System.EventHandler(this.bLiteOpen_Click);
            // 
            // NetLoginForm
            // 
            this.AcceptButton = this.bConnectNetworkDB;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bExitFromKassa;
            this.ClientSize = new System.Drawing.Size(500, 250);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetLoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Подключение к БД";
            this.Shown += new System.EventHandler(this.NetLoginForm_Shown);
            this.gbKassa.ResumeLayout(false);
            this.gbKassa.PerformLayout();
            this.gbMysql.ResumeLayout(false);
            this.gbMysql.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMysqlPort)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bExitFromKassa;
        private System.Windows.Forms.GroupBox gbMysql;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMysqlPassword;
        private System.Windows.Forms.TextBox tbMysqlHost;
        private System.Windows.Forms.TextBox tbMysqlUser;
        private System.Windows.Forms.NumericUpDown nudMysqlPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbKassa;
        private System.Windows.Forms.Button bSearchNetworkDb;
        private System.Windows.Forms.TextBox tbKassaPassword;
        private System.Windows.Forms.TextBox tbKassaUser;
        private System.Windows.Forms.ComboBox cbKassaBase;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bConnectNetworkDB;
        private System.Windows.Forms.CheckBox cbSavePassword;
        private System.Windows.Forms.CheckBox cbUnlockUser;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox cbLiteRemember;
        private System.Windows.Forms.TextBox tbLitePass;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbLiteUser;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bLiteForget;
        private System.Windows.Forms.Button bLiteCreate;
        private System.Windows.Forms.Button bLiteOpen;
        private System.Windows.Forms.ListBox lbLiteBases;
    }
}