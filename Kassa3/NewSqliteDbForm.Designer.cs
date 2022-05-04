namespace Kassa3
{
    partial class NewSqliteDbForm
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
            this.components = new System.ComponentModel.Container();
            this.bCreate = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.lDbMsg = new System.Windows.Forms.Label();
            this.lUserMsg = new System.Windows.Forms.Label();
            this.lPassMsg = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lpb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bCreate
            // 
            this.bCreate.Location = new System.Drawing.Point(138, 299);
            this.bCreate.Name = "bCreate";
            this.bCreate.Size = new System.Drawing.Size(120, 23);
            this.bCreate.TabIndex = 0;
            this.bCreate.Text = "Создать базу";
            this.bCreate.UseVisualStyleBackColor = true;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(264, 299);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Чтобы создать новую базу данных, заполните поля:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Наименование базы данных";
            // 
            // tbDb
            // 
            this.tbDb.Location = new System.Drawing.Point(15, 55);
            this.tbDb.MaxLength = 32;
            this.tbDb.Name = "tbDb";
            this.tbDb.Size = new System.Drawing.Size(149, 20);
            this.tbDb.TabIndex = 4;
            this.tbDb.TextChanged += new System.EventHandler(this.tbDb_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Пользователь";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(15, 104);
            this.tbUser.MaxLength = 32;
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(149, 20);
            this.tbUser.TabIndex = 6;
            this.tbUser.TextChanged += new System.EventHandler(this.tbDb_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Пароль";
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(15, 153);
            this.tbPass.MaxLength = 32;
            this.tbPass.Name = "tbPass";
            this.tbPass.Size = new System.Drawing.Size(149, 20);
            this.tbPass.TabIndex = 8;
            this.tbPass.TextChanged += new System.EventHandler(this.tbDb_TextChanged);
            // 
            // lDbMsg
            // 
            this.lDbMsg.AutoSize = true;
            this.lDbMsg.Location = new System.Drawing.Point(170, 58);
            this.lDbMsg.Name = "lDbMsg";
            this.lDbMsg.Size = new System.Drawing.Size(43, 13);
            this.lDbMsg.TabIndex = 9;
            this.lDbMsg.Text = "lDbMsg";
            // 
            // lUserMsg
            // 
            this.lUserMsg.AutoSize = true;
            this.lUserMsg.Location = new System.Drawing.Point(170, 107);
            this.lUserMsg.Name = "lUserMsg";
            this.lUserMsg.Size = new System.Drawing.Size(51, 13);
            this.lUserMsg.TabIndex = 10;
            this.lUserMsg.Text = "lUserMsg";
            // 
            // lPassMsg
            // 
            this.lPassMsg.AutoSize = true;
            this.lPassMsg.Location = new System.Drawing.Point(170, 156);
            this.lPassMsg.Name = "lPassMsg";
            this.lPassMsg.Size = new System.Drawing.Size(52, 13);
            this.lPassMsg.TabIndex = 11;
            this.lPassMsg.Text = "lPassMsg";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(12, 198);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(327, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Указанный пользователь будет иметь права администратора.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label6.Location = new System.Drawing.Point(12, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(231, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "База будет создана в папке local_db кассы.";
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(103, 253);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(236, 23);
            this.pb.TabIndex = 0;
            this.pb.Value = 16;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lpb
            // 
            this.lpb.AutoSize = true;
            this.lpb.Location = new System.Drawing.Point(12, 258);
            this.lpb.Name = "lpb";
            this.lpb.Size = new System.Drawing.Size(85, 13);
            this.lpb.TabIndex = 14;
            this.lpb.Text = "Создание базы";
            // 
            // NewSqliteDbForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(351, 336);
            this.Controls.Add(this.lpb);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lPassMsg);
            this.Controls.Add(this.lUserMsg);
            this.Controls.Add(this.lDbMsg);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bCreate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewSqliteDbForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Создание локальной базы";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCreate;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Label lDbMsg;
        private System.Windows.Forms.Label lUserMsg;
        private System.Windows.Forms.Label lPassMsg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lpb;
    }
}