namespace DEXPlugin.Dictionary.Users
{
    partial class FUsersEd
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.CheckBox();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.tbUID = new System.Windows.Forms.TextBox();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clbRights = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbDefNewDocState = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин пользователя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Имя пользователя";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Пароль пользователя";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "ID пользователя";
            // 
            // cbStatus
            // 
            this.cbStatus.AutoSize = true;
            this.cbStatus.Location = new System.Drawing.Point(13, 352);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(143, 17);
            this.cbStatus.TabIndex = 10;
            this.cbStatus.Text = "Пользователь активен";
            this.cbStatus.UseVisualStyleBackColor = true;
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(135, 12);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(164, 20);
            this.tbLogin.TabIndex = 4;
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(135, 38);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(287, 20);
            this.tbTitle.TabIndex = 5;
            // 
            // tbUID
            // 
            this.tbUID.Enabled = false;
            this.tbUID.Location = new System.Drawing.Point(135, 132);
            this.tbUID.Name = "tbUID";
            this.tbUID.Size = new System.Drawing.Size(287, 20);
            this.tbUID.TabIndex = 7;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(266, 348);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 11;
            this.bOk.Text = "Сохранить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(347, 348);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 12;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbRights);
            this.groupBox1.Location = new System.Drawing.Point(13, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(409, 157);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Разрешения";
            // 
            // clbRights
            // 
            this.clbRights.FormattingEnabled = true;
            this.clbRights.Location = new System.Drawing.Point(6, 19);
            this.clbRights.Name = "clbRights";
            this.clbRights.Size = new System.Drawing.Size(397, 124);
            this.clbRights.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(132, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(290, 31);
            this.label5.TabIndex = 14;
            this.label5.Text = "Если нет необходимости устанавливать или изменять пароль, оставьте это поле пусты" +
                "м.";
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(135, 64);
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(164, 20);
            this.tbPass.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(210, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Статус нового документа по умолчанию";
            // 
            // cbDefNewDocState
            // 
            this.cbDefNewDocState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefNewDocState.FormattingEnabled = true;
            this.cbDefNewDocState.Items.AddRange(new object[] {
            "Черновик",
            "На подтверждение",
            "Подтверждён",
            "На отправку"});
            this.cbDefNewDocState.Location = new System.Drawing.Point(242, 158);
            this.cbDefNewDocState.Name = "cbDefNewDocState";
            this.cbDefNewDocState.Size = new System.Drawing.Size(180, 21);
            this.cbDefNewDocState.TabIndex = 16;
            // 
            // FUsersEd
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(436, 381);
            this.Controls.Add(this.cbDefNewDocState);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.tbUID);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FUsersEd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Данные пользователя";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbStatus;
        private System.Windows.Forms.TextBox tbLogin;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.TextBox tbUID;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox clbRights;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbDefNewDocState;
    }
}