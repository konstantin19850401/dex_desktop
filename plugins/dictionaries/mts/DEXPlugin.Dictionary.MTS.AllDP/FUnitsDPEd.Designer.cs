namespace DEXPlugin.Dictionary.MTS.AllDP
{
    partial class FUnitsDPEd
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
            this.tbDPCode = new System.Windows.Forms.TextBox();
            this.cbKind = new System.Windows.Forms.CheckBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Точка продаж МТС";
            // 
            // tbDPCode
            // 
            this.tbDPCode.Location = new System.Drawing.Point(122, 12);
            this.tbDPCode.MaxLength = 32;
            this.tbDPCode.Name = "tbDPCode";
            this.tbDPCode.Size = new System.Drawing.Size(234, 20);
            this.tbDPCode.TabIndex = 19;
            // 
            // cbKind
            // 
            this.cbKind.AutoSize = true;
            this.cbKind.Location = new System.Drawing.Point(122, 38);
            this.cbKind.Name = "cbKind";
            this.cbKind.Size = new System.Drawing.Size(91, 17);
            this.cbKind.TabIndex = 20;
            this.cbKind.Text = "Статус точки";
            this.cbKind.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(281, 63);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 22;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(200, 63);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 21;
            this.bOK.Text = "Сохранить";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // FUnitsDPEd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 94);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.cbKind);
            this.Controls.Add(this.tbDPCode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FUnitsDPEd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметры точки продаж";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDPCode;
        private System.Windows.Forms.CheckBox cbKind;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
    }
}