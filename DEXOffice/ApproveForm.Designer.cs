namespace DEXOffice
{
    partial class ApproveForm
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
            this.rbStToExport = new System.Windows.Forms.RadioButton();
            this.rbStApproved = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtbInfo = new System.Windows.Forms.RichTextBox();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbStReturnedStSent = new System.Windows.Forms.CheckBox();
            this.cbIgnoreStSent = new System.Windows.Forms.CheckBox();
            this.cbIgnoreStReturned = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbStToExport);
            this.groupBox1.Controls.Add(this.rbStApproved);
            this.groupBox1.Location = new System.Drawing.Point(12, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Перевести документы в состояние";
            // 
            // rbStToExport
            // 
            this.rbStToExport.AutoSize = true;
            this.rbStToExport.Location = new System.Drawing.Point(6, 42);
            this.rbStToExport.Name = "rbStToExport";
            this.rbStToExport.Size = new System.Drawing.Size(88, 17);
            this.rbStToExport.TabIndex = 1;
            this.rbStToExport.Text = "На отправку";
            this.rbStToExport.UseVisualStyleBackColor = true;
            // 
            // rbStApproved
            // 
            this.rbStApproved.AutoSize = true;
            this.rbStApproved.Checked = true;
            this.rbStApproved.Location = new System.Drawing.Point(6, 19);
            this.rbStApproved.Name = "rbStApproved";
            this.rbStApproved.Size = new System.Drawing.Size(94, 17);
            this.rbStApproved.TabIndex = 0;
            this.rbStApproved.TabStop = true;
            this.rbStApproved.Text = "Подтверждён";
            this.rbStApproved.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtbInfo);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(322, 114);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сведения";
            // 
            // rtbInfo
            // 
            this.rtbInfo.Location = new System.Drawing.Point(6, 19);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.ReadOnly = true;
            this.rtbInfo.Size = new System.Drawing.Size(310, 86);
            this.rtbInfo.TabIndex = 0;
            this.rtbInfo.Text = "Черновиков\n1\n2\n3\n4\n5";
            // 
            // bOk
            // 
            this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOk.Location = new System.Drawing.Point(178, 301);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 2;
            this.bOk.Text = "ОК";
            this.bOk.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(259, 301);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbStReturnedStSent);
            this.groupBox3.Controls.Add(this.cbIgnoreStSent);
            this.groupBox3.Controls.Add(this.cbIgnoreStReturned);
            this.groupBox3.Location = new System.Drawing.Point(12, 206);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(322, 89);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Опции";
            // 
            // cbStReturnedStSent
            // 
            this.cbStReturnedStSent.AutoSize = true;
            this.cbStReturnedStSent.Location = new System.Drawing.Point(6, 42);
            this.cbStReturnedStSent.Name = "cbStReturnedStSent";
            this.cbStReturnedStSent.Size = new System.Drawing.Size(253, 17);
            this.cbStReturnedStSent.TabIndex = 2;
            this.cbStReturnedStSent.Text = "Пометить возвращённые как отправленные";
            this.cbStReturnedStSent.UseVisualStyleBackColor = true;
            this.cbStReturnedStSent.CheckedChanged += new System.EventHandler(this.cbStReturnedStSent_CheckedChanged);
            // 
            // cbIgnoreStSent
            // 
            this.cbIgnoreStSent.AutoSize = true;
            this.cbIgnoreStSent.Location = new System.Drawing.Point(6, 65);
            this.cbIgnoreStSent.Name = "cbIgnoreStSent";
            this.cbIgnoreStSent.Size = new System.Drawing.Size(233, 17);
            this.cbIgnoreStSent.TabIndex = 1;
            this.cbIgnoreStSent.Text = "Игнорировать отправленные документы";
            this.cbIgnoreStSent.UseVisualStyleBackColor = true;
            // 
            // cbIgnoreStReturned
            // 
            this.cbIgnoreStReturned.AutoSize = true;
            this.cbIgnoreStReturned.Location = new System.Drawing.Point(6, 19);
            this.cbIgnoreStReturned.Name = "cbIgnoreStReturned";
            this.cbIgnoreStReturned.Size = new System.Drawing.Size(237, 17);
            this.cbIgnoreStReturned.TabIndex = 0;
            this.cbIgnoreStReturned.Text = "Игнорировать возвращённые документы";
            this.cbIgnoreStReturned.UseVisualStyleBackColor = true;
            this.cbIgnoreStReturned.CheckedChanged += new System.EventHandler(this.cbIgnoreStReturned_CheckedChanged);
            // 
            // ApproveForm
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(346, 335);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ApproveForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Подтверждение";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.RadioButton rbStToExport;
        public System.Windows.Forms.RadioButton rbStApproved;
        public System.Windows.Forms.RichTextBox rtbInfo;
        public System.Windows.Forms.CheckBox cbIgnoreStSent;
        public System.Windows.Forms.CheckBox cbIgnoreStReturned;
        public System.Windows.Forms.CheckBox cbStReturnedStSent;
    }
}