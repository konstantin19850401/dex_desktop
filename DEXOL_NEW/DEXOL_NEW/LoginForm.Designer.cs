namespace DEXOL_NEW
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.btn_ok = new System.Windows.Forms.Button();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.te_login = new DevExpress.XtraEditors.TextEdit();
            this.te_password = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.te_login.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_password.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_ok
            // 
            this.btn_ok.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_ok.Location = new System.Drawing.Point(172, 64);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "Применить";
            this.btn_ok.UseVisualStyleBackColor = false;
            this.btn_ok.Click += new System.EventHandler(this.loginClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(19, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 13);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Логин";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(19, 41);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(37, 13);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "Пароль";
            // 
            // te_login
            // 
            this.te_login.EditValue = "ermakova";
            this.te_login.Location = new System.Drawing.Point(67, 12);
            this.te_login.Name = "te_login";
            this.te_login.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.te_login.Size = new System.Drawing.Size(180, 20);
            this.te_login.TabIndex = 5;
            this.te_login.KeyDown += new System.Windows.Forms.KeyEventHandler(this.te_login_KeyDown);
            // 
            // te_password
            // 
            this.te_password.EditValue = "62584124736";
            this.te_password.Location = new System.Drawing.Point(67, 38);
            this.te_password.Name = "te_password";
            this.te_password.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.te_password.Properties.PasswordChar = '*';
            this.te_password.Size = new System.Drawing.Size(180, 20);
            this.te_password.TabIndex = 6;
            this.te_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.te_password_KeyDown);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 93);
            this.Controls.Add(this.te_password);
            this.Controls.Add(this.te_login);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btn_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            ((System.ComponentModel.ISupportInitialize)(this.te_login.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_password.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ok;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit te_login;
        private DevExpress.XtraEditors.TextEdit te_password;
    }
}