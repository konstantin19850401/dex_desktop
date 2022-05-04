namespace Kassa3
{
    partial class UserEdForm
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
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpCommonRights = new System.Windows.Forms.TabPage();
            this.cbFieldsEdit = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbMarkNewRec = new System.Windows.Forms.CheckBox();
            this.cbDicFirmAcc = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDicOps = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDicClients = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbAppSettings = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbDicCurrencies = new System.Windows.Forms.ComboBox();
            this.cbDicUsers = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tpOpRules = new System.Windows.Forms.TabPage();
            this.bRuleSet = new System.Windows.Forms.Button();
            this.bMoveDown = new System.Windows.Forms.Button();
            this.bMoveUp = new System.Windows.Forms.Button();
            this.bDeleteRule = new System.Windows.Forms.Button();
            this.bEditRule = new System.Windows.Forms.Button();
            this.bNewRule = new System.Windows.Forms.Button();
            this.lbOpRules = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbGlobalRule = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cbChangePassword = new System.Windows.Forms.CheckBox();
            this.cbActive = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tpCommonRights.SuspendLayout();
            this.tpOpRules.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя пользователя";
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(16, 25);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(166, 20);
            this.tbLogin.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Пароль пользователя";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(188, 25);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(158, 20);
            this.tbPassword.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpCommonRights);
            this.tabControl1.Controls.Add(this.tpOpRules);
            this.tabControl1.Location = new System.Drawing.Point(12, 51);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(461, 266);
            this.tabControl1.TabIndex = 4;
            // 
            // tpCommonRights
            // 
            this.tpCommonRights.AutoScroll = true;
            this.tpCommonRights.Controls.Add(this.cbFieldsEdit);
            this.tpCommonRights.Controls.Add(this.label11);
            this.tpCommonRights.Controls.Add(this.cbMarkNewRec);
            this.tpCommonRights.Controls.Add(this.cbDicFirmAcc);
            this.tpCommonRights.Controls.Add(this.label5);
            this.tpCommonRights.Controls.Add(this.cbDicOps);
            this.tpCommonRights.Controls.Add(this.label9);
            this.tpCommonRights.Controls.Add(this.label4);
            this.tpCommonRights.Controls.Add(this.cbDicClients);
            this.tpCommonRights.Controls.Add(this.label8);
            this.tpCommonRights.Controls.Add(this.cbAppSettings);
            this.tpCommonRights.Controls.Add(this.label7);
            this.tpCommonRights.Controls.Add(this.cbDicCurrencies);
            this.tpCommonRights.Controls.Add(this.cbDicUsers);
            this.tpCommonRights.Controls.Add(this.label3);
            this.tpCommonRights.Location = new System.Drawing.Point(4, 22);
            this.tpCommonRights.Name = "tpCommonRights";
            this.tpCommonRights.Padding = new System.Windows.Forms.Padding(3);
            this.tpCommonRights.Size = new System.Drawing.Size(453, 240);
            this.tpCommonRights.TabIndex = 1;
            this.tpCommonRights.Text = "Общие разрешения";
            this.tpCommonRights.UseVisualStyleBackColor = true;
            // 
            // cbFieldsEdit
            // 
            this.cbFieldsEdit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFieldsEdit.FormattingEnabled = true;
            this.cbFieldsEdit.Items.AddRange(new object[] {
            "Запрещено изменять",
            "Разрешено изменять"});
            this.cbFieldsEdit.Location = new System.Drawing.Point(194, 170);
            this.cbFieldsEdit.Name = "cbFieldsEdit";
            this.cbFieldsEdit.Size = new System.Drawing.Size(252, 21);
            this.cbFieldsEdit.TabIndex = 14;
            this.cbFieldsEdit.SelectedIndexChanged += new System.EventHandler(this.cbFieldsEdit_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(168, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Редактирование чужих записей";
            // 
            // cbMarkNewRec
            // 
            this.cbMarkNewRec.AutoSize = true;
            this.cbMarkNewRec.Location = new System.Drawing.Point(194, 204);
            this.cbMarkNewRec.Name = "cbMarkNewRec";
            this.cbMarkNewRec.Size = new System.Drawing.Size(252, 17);
            this.cbMarkNewRec.TabIndex = 12;
            this.cbMarkNewRec.Text = "Помечать новые записи как непрочитанные";
            this.cbMarkNewRec.UseVisualStyleBackColor = true;
            // 
            // cbDicFirmAcc
            // 
            this.cbDicFirmAcc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDicFirmAcc.FormattingEnabled = true;
            this.cbDicFirmAcc.Items.AddRange(new object[] {
            "Недоступен",
            "Доступен для просмотра",
            "Доступен для изменения"});
            this.cbDicFirmAcc.Location = new System.Drawing.Point(194, 141);
            this.cbDicFirmAcc.Name = "cbDicFirmAcc";
            this.cbDicFirmAcc.Size = new System.Drawing.Size(252, 21);
            this.cbDicFirmAcc.TabIndex = 11;
            this.cbDicFirmAcc.SelectedIndexChanged += new System.EventHandler(this.cbDicFirmAcc_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Справочник фирм и счетов";
            // 
            // cbDicOps
            // 
            this.cbDicOps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDicOps.FormattingEnabled = true;
            this.cbDicOps.Items.AddRange(new object[] {
            "Недоступен",
            "Доступен для просмотра",
            "Доступен для изменения"});
            this.cbDicOps.Location = new System.Drawing.Point(194, 114);
            this.cbDicOps.Name = "cbDicOps";
            this.cbDicOps.Size = new System.Drawing.Size(252, 21);
            this.cbDicOps.TabIndex = 9;
            this.cbDicOps.SelectedIndexChanged += new System.EventHandler(this.cbDicOps_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(118, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Справочник операций";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Справочник контрагентов";
            // 
            // cbDicClients
            // 
            this.cbDicClients.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDicClients.FormattingEnabled = true;
            this.cbDicClients.Items.AddRange(new object[] {
            "Недоступен",
            "Доступен для просмотра",
            "Доступен для изменения"});
            this.cbDicClients.Location = new System.Drawing.Point(194, 87);
            this.cbDicClients.Name = "cbDicClients";
            this.cbDicClients.Size = new System.Drawing.Size(252, 21);
            this.cbDicClients.TabIndex = 6;
            this.cbDicClients.SelectedIndexChanged += new System.EventHandler(this.cbDicClients_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Установки приложения";
            // 
            // cbAppSettings
            // 
            this.cbAppSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAppSettings.FormattingEnabled = true;
            this.cbAppSettings.Items.AddRange(new object[] {
            "Запрещено изменять",
            "Разрешено изменять"});
            this.cbAppSettings.Location = new System.Drawing.Point(194, 6);
            this.cbAppSettings.Name = "cbAppSettings";
            this.cbAppSettings.Size = new System.Drawing.Size(252, 21);
            this.cbAppSettings.TabIndex = 1;
            this.cbAppSettings.SelectedIndexChanged += new System.EventHandler(this.cbAppSettings_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Справочник валют";
            // 
            // cbDicCurrencies
            // 
            this.cbDicCurrencies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDicCurrencies.FormattingEnabled = true;
            this.cbDicCurrencies.Items.AddRange(new object[] {
            "Недоступен",
            "Доступен для просмотра",
            "Доступен для изменения"});
            this.cbDicCurrencies.Location = new System.Drawing.Point(194, 60);
            this.cbDicCurrencies.Name = "cbDicCurrencies";
            this.cbDicCurrencies.Size = new System.Drawing.Size(252, 21);
            this.cbDicCurrencies.TabIndex = 3;
            this.cbDicCurrencies.SelectedIndexChanged += new System.EventHandler(this.cbDicCurrencies_SelectedIndexChanged);
            // 
            // cbDicUsers
            // 
            this.cbDicUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDicUsers.FormattingEnabled = true;
            this.cbDicUsers.Items.AddRange(new object[] {
            "Недоступен",
            "Доступен для просмотра",
            "Доступен для изменения"});
            this.cbDicUsers.Location = new System.Drawing.Point(194, 33);
            this.cbDicUsers.Name = "cbDicUsers";
            this.cbDicUsers.Size = new System.Drawing.Size(252, 21);
            this.cbDicUsers.TabIndex = 2;
            this.cbDicUsers.SelectedIndexChanged += new System.EventHandler(this.cbDicUsers_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Справочник пользователей";
            // 
            // tpOpRules
            // 
            this.tpOpRules.Controls.Add(this.bRuleSet);
            this.tpOpRules.Controls.Add(this.bMoveDown);
            this.tpOpRules.Controls.Add(this.bMoveUp);
            this.tpOpRules.Controls.Add(this.bDeleteRule);
            this.tpOpRules.Controls.Add(this.bEditRule);
            this.tpOpRules.Controls.Add(this.bNewRule);
            this.tpOpRules.Controls.Add(this.lbOpRules);
            this.tpOpRules.Controls.Add(this.label10);
            this.tpOpRules.Controls.Add(this.cbGlobalRule);
            this.tpOpRules.Controls.Add(this.label6);
            this.tpOpRules.Location = new System.Drawing.Point(4, 22);
            this.tpOpRules.Name = "tpOpRules";
            this.tpOpRules.Padding = new System.Windows.Forms.Padding(3);
            this.tpOpRules.Size = new System.Drawing.Size(453, 240);
            this.tpOpRules.TabIndex = 2;
            this.tpOpRules.Text = "Операционные правила";
            this.tpOpRules.UseVisualStyleBackColor = true;
            // 
            // bRuleSet
            // 
            this.bRuleSet.Location = new System.Drawing.Point(9, 194);
            this.bRuleSet.Name = "bRuleSet";
            this.bRuleSet.Size = new System.Drawing.Size(128, 23);
            this.bRuleSet.TabIndex = 14;
            this.bRuleSet.Text = "Карта допуска";
            this.bRuleSet.UseVisualStyleBackColor = true;
            this.bRuleSet.Click += new System.EventHandler(this.bRuleSet_Click);
            // 
            // bMoveDown
            // 
            this.bMoveDown.Location = new System.Drawing.Point(9, 165);
            this.bMoveDown.Name = "bMoveDown";
            this.bMoveDown.Size = new System.Drawing.Size(128, 23);
            this.bMoveDown.TabIndex = 13;
            this.bMoveDown.Text = "Сдвинуть ниже";
            this.bMoveDown.UseVisualStyleBackColor = true;
            this.bMoveDown.Click += new System.EventHandler(this.bMoveDown_Click);
            // 
            // bMoveUp
            // 
            this.bMoveUp.Location = new System.Drawing.Point(9, 136);
            this.bMoveUp.Name = "bMoveUp";
            this.bMoveUp.Size = new System.Drawing.Size(128, 23);
            this.bMoveUp.TabIndex = 12;
            this.bMoveUp.Text = "Сдвинуть выше";
            this.bMoveUp.UseVisualStyleBackColor = true;
            this.bMoveUp.Click += new System.EventHandler(this.bMoveUp_Click);
            // 
            // bDeleteRule
            // 
            this.bDeleteRule.Location = new System.Drawing.Point(9, 107);
            this.bDeleteRule.Name = "bDeleteRule";
            this.bDeleteRule.Size = new System.Drawing.Size(128, 23);
            this.bDeleteRule.TabIndex = 11;
            this.bDeleteRule.Text = "Удалить правило";
            this.bDeleteRule.UseVisualStyleBackColor = true;
            this.bDeleteRule.Click += new System.EventHandler(this.bDeleteRule_Click);
            // 
            // bEditRule
            // 
            this.bEditRule.Location = new System.Drawing.Point(9, 78);
            this.bEditRule.Name = "bEditRule";
            this.bEditRule.Size = new System.Drawing.Size(128, 23);
            this.bEditRule.TabIndex = 10;
            this.bEditRule.Text = "Изменить правило";
            this.bEditRule.UseVisualStyleBackColor = true;
            this.bEditRule.Click += new System.EventHandler(this.bEditRule_Click);
            // 
            // bNewRule
            // 
            this.bNewRule.Location = new System.Drawing.Point(9, 49);
            this.bNewRule.Name = "bNewRule";
            this.bNewRule.Size = new System.Drawing.Size(128, 23);
            this.bNewRule.TabIndex = 9;
            this.bNewRule.Text = "Новое правило";
            this.bNewRule.UseVisualStyleBackColor = true;
            this.bNewRule.Click += new System.EventHandler(this.bNewRule_Click);
            // 
            // lbOpRules
            // 
            this.lbOpRules.FormattingEnabled = true;
            this.lbOpRules.Location = new System.Drawing.Point(143, 33);
            this.lbOpRules.Name = "lbOpRules";
            this.lbOpRules.Size = new System.Drawing.Size(303, 186);
            this.lbOpRules.TabIndex = 3;
            this.lbOpRules.DoubleClick += new System.EventHandler(this.bEditRule_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Уточняющие правила:";
            // 
            // cbGlobalRule
            // 
            this.cbGlobalRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGlobalRule.FormattingEnabled = true;
            this.cbGlobalRule.Items.AddRange(new object[] {
            "Всё запрещено",
            "Всё разрешено"});
            this.cbGlobalRule.Location = new System.Drawing.Point(143, 6);
            this.cbGlobalRule.Name = "cbGlobalRule";
            this.cbGlobalRule.Size = new System.Drawing.Size(303, 21);
            this.cbGlobalRule.TabIndex = 1;
            this.cbGlobalRule.SelectedIndexChanged += new System.EventHandler(this.cbGlobalRule_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Общее правило:";
            // 
            // bOk
            // 
            this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOk.Location = new System.Drawing.Point(317, 323);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 5;
            this.bOk.Text = "ОК";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(398, 323);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // cbChangePassword
            // 
            this.cbChangePassword.AutoSize = true;
            this.cbChangePassword.Location = new System.Drawing.Point(352, 27);
            this.cbChangePassword.Name = "cbChangePassword";
            this.cbChangePassword.Size = new System.Drawing.Size(116, 17);
            this.cbChangePassword.TabIndex = 7;
            this.cbChangePassword.Text = "Изменить пароль";
            this.cbChangePassword.UseVisualStyleBackColor = true;
            // 
            // cbActive
            // 
            this.cbActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbActive.AutoSize = true;
            this.cbActive.Location = new System.Drawing.Point(12, 327);
            this.cbActive.Name = "cbActive";
            this.cbActive.Size = new System.Drawing.Size(143, 17);
            this.cbActive.TabIndex = 8;
            this.cbActive.Text = "Пользователь активен";
            this.cbActive.UseVisualStyleBackColor = true;
            // 
            // UserEdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(485, 358);
            this.Controls.Add(this.cbActive);
            this.Controls.Add(this.cbChangePassword);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserEdForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Данные пользователя";
            this.Shown += new System.EventHandler(this.UserEdForm_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tpCommonRights.ResumeLayout(false);
            this.tpCommonRights.PerformLayout();
            this.tpOpRules.ResumeLayout(false);
            this.tpOpRules.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.CheckBox cbChangePassword;
        private System.Windows.Forms.CheckBox cbActive;
        private System.Windows.Forms.TabPage tpCommonRights;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbDicCurrencies;
        public System.Windows.Forms.ComboBox cbDicUsers;
        private System.Windows.Forms.ComboBox cbAppSettings;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDicClients;
        private System.Windows.Forms.ComboBox cbDicOps;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbDicFirmAcc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tpOpRules;
        private System.Windows.Forms.ComboBox cbGlobalRule;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox lbOpRules;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button bRuleSet;
        private System.Windows.Forms.Button bMoveDown;
        private System.Windows.Forms.Button bMoveUp;
        private System.Windows.Forms.Button bDeleteRule;
        private System.Windows.Forms.Button bEditRule;
        private System.Windows.Forms.Button bNewRule;
        private System.Windows.Forms.CheckBox cbMarkNewRec;
        private System.Windows.Forms.ComboBox cbFieldsEdit;
        private System.Windows.Forms.Label label11;
    }
}