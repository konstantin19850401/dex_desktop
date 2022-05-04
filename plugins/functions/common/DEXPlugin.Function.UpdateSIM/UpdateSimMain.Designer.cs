namespace DEXPlugin.Function.UpdateSIM
{
    partial class UpdateSimMain
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
            this.de1 = new DEXExtendLib.DateEdit();
            this.de2 = new DEXExtendLib.DateEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cb1 = new System.Windows.Forms.CheckBox();
            this.cb2 = new System.Windows.Forms.CheckBox();
            this.cb3 = new System.Windows.Forms.CheckBox();
            this.cb4 = new System.Windows.Forms.CheckBox();
            this.cb5 = new System.Windows.Forms.CheckBox();
            this.cb6 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // de1
            // 
            this.de1.FormattingEnabled = true;
            this.de1.InputChar = '_';
            this.de1.Location = new System.Drawing.Point(15, 25);
            this.de1.MaxLength = 10;
            this.de1.Name = "de1";
            this.de1.Size = new System.Drawing.Size(121, 21);
            this.de1.TabIndex = 0;
            // 
            // de2
            // 
            this.de2.FormattingEnabled = true;
            this.de2.InputChar = '_';
            this.de2.Location = new System.Drawing.Point(142, 25);
            this.de2.MaxLength = 10;
            this.de2.Name = "de2";
            this.de2.Size = new System.Drawing.Size(121, 21);
            this.de2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Начало периода";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Окончание периода";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(64, 168);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Синхронизировать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(188, 168);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cb1
            // 
            this.cb1.AutoSize = true;
            this.cb1.Location = new System.Drawing.Point(15, 81);
            this.cb1.Name = "cb1";
            this.cb1.Size = new System.Drawing.Size(76, 17);
            this.cb1.TabIndex = 6;
            this.cb1.Text = "Черновик";
            this.cb1.UseVisualStyleBackColor = true;
            // 
            // cb2
            // 
            this.cb2.AutoSize = true;
            this.cb2.Location = new System.Drawing.Point(142, 81);
            this.cb2.Name = "cb2";
            this.cb2.Size = new System.Drawing.Size(122, 17);
            this.cb2.TabIndex = 7;
            this.cb2.Text = "На подтверждение";
            this.cb2.UseVisualStyleBackColor = true;
            // 
            // cb3
            // 
            this.cb3.AutoSize = true;
            this.cb3.Location = new System.Drawing.Point(15, 104);
            this.cb3.Name = "cb3";
            this.cb3.Size = new System.Drawing.Size(95, 17);
            this.cb3.TabIndex = 8;
            this.cb3.Text = "Подтверждён";
            this.cb3.UseVisualStyleBackColor = true;
            // 
            // cb4
            // 
            this.cb4.AutoSize = true;
            this.cb4.Location = new System.Drawing.Point(142, 104);
            this.cb4.Name = "cb4";
            this.cb4.Size = new System.Drawing.Size(89, 17);
            this.cb4.TabIndex = 9;
            this.cb4.Text = "На отправку";
            this.cb4.UseVisualStyleBackColor = true;
            // 
            // cb5
            // 
            this.cb5.AutoSize = true;
            this.cb5.Location = new System.Drawing.Point(15, 127);
            this.cb5.Name = "cb5";
            this.cb5.Size = new System.Drawing.Size(81, 17);
            this.cb5.TabIndex = 10;
            this.cb5.Text = "Отправлен";
            this.cb5.UseVisualStyleBackColor = true;
            // 
            // cb6
            // 
            this.cb6.AutoSize = true;
            this.cb6.Location = new System.Drawing.Point(142, 127);
            this.cb6.Name = "cb6";
            this.cb6.Size = new System.Drawing.Size(84, 17);
            this.cb6.TabIndex = 11;
            this.cb6.Text = "Возвращен";
            this.cb6.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(214, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Обрабатывать документы со статусами:";
            // 
            // UpdateSimMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(279, 203);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cb6);
            this.Controls.Add(this.cb5);
            this.Controls.Add(this.cb4);
            this.Controls.Add(this.cb3);
            this.Controls.Add(this.cb2);
            this.Controls.Add(this.cb1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.de2);
            this.Controls.Add(this.de1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateSimMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Синхронизация SIM-карт из договоров";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DEXExtendLib.DateEdit de1;
        private DEXExtendLib.DateEdit de2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox cb1;
        private System.Windows.Forms.CheckBox cb2;
        private System.Windows.Forms.CheckBox cb3;
        private System.Windows.Forms.CheckBox cb4;
        private System.Windows.Forms.CheckBox cb5;
        private System.Windows.Forms.CheckBox cb6;
        private System.Windows.Forms.Label label3;
    }
}