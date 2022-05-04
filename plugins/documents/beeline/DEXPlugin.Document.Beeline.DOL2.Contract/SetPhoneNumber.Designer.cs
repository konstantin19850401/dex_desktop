namespace DEXPlugin.Document.Beeline.DOL2.Contract
{
    partial class SetPhoneNumber
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
            this.bOk = new System.Windows.Forms.Button();
            this.tbNewNumber = new System.Windows.Forms.TextBox();
            this.tbDocNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(245, 62);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(132, 27);
            this.bOk.TabIndex = 0;
            this.bOk.Text = "Применить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // tbNewNumber
            // 
            this.tbNewNumber.Location = new System.Drawing.Point(172, 9);
            this.tbNewNumber.Name = "tbNewNumber";
            this.tbNewNumber.Size = new System.Drawing.Size(205, 20);
            this.tbNewNumber.TabIndex = 1;
            // 
            // tbDocNum
            // 
            this.tbDocNum.Location = new System.Drawing.Point(170, 36);
            this.tbDocNum.Name = "tbDocNum";
            this.tbDocNum.Size = new System.Drawing.Size(205, 20);
            this.tbDocNum.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Полученный номер телефона";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Полученный номер договора";
            // 
            // SetPhoneNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 104);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDocNum);
            this.Controls.Add(this.tbNewNumber);
            this.Controls.Add(this.bOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetPhoneNumber";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Введите присвоенный номер телефона";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOk;
        public System.Windows.Forms.TextBox tbNewNumber;
        public System.Windows.Forms.TextBox tbDocNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}