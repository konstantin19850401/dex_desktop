namespace Kassa3
{
    partial class PrimForm
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
            this.tbPrim = new System.Windows.Forms.TextBox();
            this.bSort = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbPrim
            // 
            this.tbPrim.Location = new System.Drawing.Point(12, 12);
            this.tbPrim.Multiline = true;
            this.tbPrim.Name = "tbPrim";
            this.tbPrim.Size = new System.Drawing.Size(460, 268);
            this.tbPrim.TabIndex = 0;
            // 
            // bSort
            // 
            this.bSort.Location = new System.Drawing.Point(12, 286);
            this.bSort.Name = "bSort";
            this.bSort.Size = new System.Drawing.Size(164, 23);
            this.bSort.TabIndex = 1;
            this.bSort.Text = "Сортировать по алфавиту";
            this.bSort.UseVisualStyleBackColor = true;
            this.bSort.Click += new System.EventHandler(this.bSort_Click);
            // 
            // bSave
            // 
            this.bSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bSave.Location = new System.Drawing.Point(316, 286);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 23);
            this.bSave.TabIndex = 2;
            this.bSave.Text = "Сохранить";
            this.bSave.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(397, 286);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // PrimForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(484, 320);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bSort);
            this.Controls.Add(this.tbPrim);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrimForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактор шаблонов примечаний";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bSort;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bCancel;
        public System.Windows.Forms.TextBox tbPrim;
    }
}