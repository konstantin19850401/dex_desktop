namespace DEXOL_NEW
{
    partial class DictItem
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
            this.sb_ok = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // sb_ok
            // 
            this.sb_ok.Location = new System.Drawing.Point(633, 439);
            this.sb_ok.Name = "sb_ok";
            this.sb_ok.Size = new System.Drawing.Size(75, 23);
            this.sb_ok.TabIndex = 1;
            this.sb_ok.Text = "Сохранить";
            this.sb_ok.Click += new System.EventHandler(this.sb_ok_Click);
            // 
            // DictItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 474);
            this.Controls.Add(this.sb_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DictItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DictItem";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DictItem_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton sb_ok;
    }
}