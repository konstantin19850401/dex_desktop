namespace Kassa3
{
    partial class MatchForm
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
            this.cbField = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbOp = new System.Windows.Forms.ComboBox();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Поле";
            // 
            // cbField
            // 
            this.cbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbField.FormattingEnabled = true;
            this.cbField.Location = new System.Drawing.Point(51, 12);
            this.cbField.Name = "cbField";
            this.cbField.Size = new System.Drawing.Size(357, 21);
            this.cbField.TabIndex = 1;
            this.cbField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbField_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Действие";
            // 
            // cbOp
            // 
            this.cbOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOp.FormattingEnabled = true;
            this.cbOp.Items.AddRange(new object[] {
            "Равно",
            "Не равно",
            "Содержит",
            "Не содержит",
            "Регулярное выражение"});
            this.cbOp.Location = new System.Drawing.Point(75, 39);
            this.cbOp.Name = "cbOp";
            this.cbOp.Size = new System.Drawing.Size(333, 21);
            this.cbOp.TabIndex = 3;
            this.cbOp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbField_KeyDown);
            // 
            // tbValue
            // 
            this.tbValue.Location = new System.Drawing.Point(75, 66);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(333, 20);
            this.tbValue.TabIndex = 4;
            this.tbValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbField_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Значение";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(252, 92);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 6;
            this.bOk.Text = "ОК";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(333, 92);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // MatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 126);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.cbOp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbField);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MatchForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактор критерия";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbOp;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
    }
}