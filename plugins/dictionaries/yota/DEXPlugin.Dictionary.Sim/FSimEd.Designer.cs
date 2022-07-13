namespace DEXPlugin.Dictionary.Yota.Sim
{
    partial class FSimEd
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbICC = new System.Windows.Forms.TextBox();
            this.nudParty_id = new System.Windows.Forms.NumericUpDown();
            this.dtpDate_in = new System.Windows.Forms.DateTimePicker();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.cbUnit_id = new System.Windows.Forms.ComboBox();
            this.dtpDate_own = new System.Windows.Forms.DateTimePicker();
            this.dtpDate_sold = new System.Windows.Forms.DateTimePicker();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbBalance = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lbTypeSim = new System.Windows.Forms.Label();
            this.cbTypeSim = new System.Windows.Forms.ComboBox();
            this.lbMsisdn = new System.Windows.Forms.Label();
            this.tbMSISDN = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudParty_id)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(101, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ICC";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(84, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Статус";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Дата поступления";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 214);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Отделение";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Дата распределения";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(45, 269);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Дата продажи";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(81, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Партия";
            // 
            // tbICC
            // 
            this.tbICC.Location = new System.Drawing.Point(131, 32);
            this.tbICC.MaxLength = 32;
            this.tbICC.Name = "tbICC";
            this.tbICC.Size = new System.Drawing.Size(291, 20);
            this.tbICC.TabIndex = 11;
            // 
            // nudParty_id
            // 
            this.nudParty_id.Location = new System.Drawing.Point(131, 80);
            this.nudParty_id.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudParty_id.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudParty_id.Name = "nudParty_id";
            this.nudParty_id.Size = new System.Drawing.Size(120, 20);
            this.nudParty_id.TabIndex = 13;
            this.nudParty_id.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dtpDate_in
            // 
            this.dtpDate_in.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate_in.Location = new System.Drawing.Point(131, 106);
            this.dtpDate_in.Name = "dtpDate_in";
            this.dtpDate_in.Size = new System.Drawing.Size(120, 20);
            this.dtpDate_in.TabIndex = 16;
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "Поступила",
            "Распределена",
            "Продана"});
            this.cbStatus.Location = new System.Drawing.Point(131, 183);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(291, 21);
            this.cbStatus.TabIndex = 17;
            // 
            // cbUnit_id
            // 
            this.cbUnit_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnit_id.FormattingEnabled = true;
            this.cbUnit_id.Location = new System.Drawing.Point(131, 211);
            this.cbUnit_id.Name = "cbUnit_id";
            this.cbUnit_id.Size = new System.Drawing.Size(291, 21);
            this.cbUnit_id.TabIndex = 18;
            // 
            // dtpDate_own
            // 
            this.dtpDate_own.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate_own.Location = new System.Drawing.Point(131, 239);
            this.dtpDate_own.Name = "dtpDate_own";
            this.dtpDate_own.Size = new System.Drawing.Size(120, 20);
            this.dtpDate_own.TabIndex = 19;
            // 
            // dtpDate_sold
            // 
            this.dtpDate_sold.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate_sold.Location = new System.Drawing.Point(131, 265);
            this.dtpDate_sold.Name = "dtpDate_sold";
            this.dtpDate_sold.Size = new System.Drawing.Size(120, 20);
            this.dtpDate_sold.TabIndex = 20;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(266, 325);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 22;
            this.bOk.Text = "Сохранить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(347, 325);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 23;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(131, 291);
            this.tbComment.MaxLength = 255;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(291, 20);
            this.tbComment.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(55, 294);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Примечание";
            // 
            // tbBalance
            // 
            this.tbBalance.Location = new System.Drawing.Point(131, 157);
            this.tbBalance.Name = "tbBalance";
            this.tbBalance.Size = new System.Drawing.Size(291, 20);
            this.tbBalance.TabIndex = 25;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(81, 160);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 26;
            this.label12.Text = "Баланс";
            // 
            // lbTypeSim
            // 
            this.lbTypeSim.AutoSize = true;
            this.lbTypeSim.Location = new System.Drawing.Point(42, 134);
            this.lbTypeSim.Name = "lbTypeSim";
            this.lbTypeSim.Size = new System.Drawing.Size(83, 13);
            this.lbTypeSim.TabIndex = 27;
            this.lbTypeSim.Text = "Тип сим-карты";
            // 
            // cbTypeSim
            // 
            this.cbTypeSim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeSim.FormattingEnabled = true;
            this.cbTypeSim.Items.AddRange(new object[] {
            "Голос",
            "Модем",
            "Для модема",
            "Саморегистрация"});
            this.cbTypeSim.Location = new System.Drawing.Point(131, 131);
            this.cbTypeSim.Name = "cbTypeSim";
            this.cbTypeSim.Size = new System.Drawing.Size(120, 21);
            this.cbTypeSim.TabIndex = 28;
            // 
            // lbMsisdn
            // 
            this.lbMsisdn.AutoSize = true;
            this.lbMsisdn.Location = new System.Drawing.Point(76, 60);
            this.lbMsisdn.Name = "lbMsisdn";
            this.lbMsisdn.Size = new System.Drawing.Size(49, 13);
            this.lbMsisdn.TabIndex = 29;
            this.lbMsisdn.Text = "MSISDN";
            // 
            // tbMSISDN
            // 
            this.tbMSISDN.Location = new System.Drawing.Point(131, 57);
            this.tbMSISDN.MaxLength = 32;
            this.tbMSISDN.Name = "tbMSISDN";
            this.tbMSISDN.Size = new System.Drawing.Size(291, 20);
            this.tbMSISDN.TabIndex = 30;
            // 
            // FSimEd
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(434, 357);
            this.Controls.Add(this.tbMSISDN);
            this.Controls.Add(this.lbMsisdn);
            this.Controls.Add(this.cbTypeSim);
            this.Controls.Add(this.lbTypeSim);
            this.Controls.Add(this.tbBalance);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.dtpDate_sold);
            this.Controls.Add(this.dtpDate_own);
            this.Controls.Add(this.cbUnit_id);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.dtpDate_in);
            this.Controls.Add(this.nudParty_id);
            this.Controls.Add(this.tbICC);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FSimEd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойства SIM-карты";
            ((System.ComponentModel.ISupportInitialize)(this.nudParty_id)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        public System.Windows.Forms.TextBox tbICC;
        public System.Windows.Forms.NumericUpDown nudParty_id;
        public System.Windows.Forms.DateTimePicker dtpDate_in;
        public System.Windows.Forms.ComboBox cbStatus;
        public System.Windows.Forms.ComboBox cbUnit_id;
        public System.Windows.Forms.DateTimePicker dtpDate_own;
        public System.Windows.Forms.DateTimePicker dtpDate_sold;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.TextBox tbComment;
        public System.Windows.Forms.TextBox tbBalance;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbTypeSim;
        public System.Windows.Forms.ComboBox cbTypeSim;
        private System.Windows.Forms.Label lbMsisdn;
        public System.Windows.Forms.TextBox tbMSISDN;
    }
}