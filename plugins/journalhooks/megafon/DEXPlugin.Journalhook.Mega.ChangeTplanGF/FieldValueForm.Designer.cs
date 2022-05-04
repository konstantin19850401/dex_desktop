namespace DEXPlugin.Journalhook.Mega.ChangeTplanGF
{
    partial class FieldValueForm
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
            this.lFieldTitle = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.cbList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lFieldTitle
            // 
            this.lFieldTitle.AutoSize = true;
            this.lFieldTitle.Location = new System.Drawing.Point(12, 15);
            this.lFieldTitle.Name = "lFieldTitle";
            this.lFieldTitle.Size = new System.Drawing.Size(51, 13);
            this.lFieldTitle.TabIndex = 0;
            this.lFieldTitle.Text = "lFieldTitle";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(327, 39);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 4;
            this.bOk.Text = "ОК";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(408, 39);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // cbList
            // 
            this.cbList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbList.FormattingEnabled = true;
            this.cbList.Location = new System.Drawing.Point(192, 15);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(291, 21);
            this.cbList.TabIndex = 6;
            // 
            // FieldValueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(495, 73);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.lFieldTitle);
            this.Controls.Add(this.cbList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FieldValueForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Укажите значение поля";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lFieldTitle;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.ComboBox cbList;
    }
}