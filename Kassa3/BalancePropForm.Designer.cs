namespace Kassa3
{
    partial class BalancePropForm
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
            this.gbFirmsAcc = new System.Windows.Forms.GroupBox();
            this.clbAccounts = new System.Windows.Forms.CheckedListBox();
            this.gbItoCource = new System.Windows.Forms.GroupBox();
            this.lbCurr = new System.Windows.Forms.ListBox();
            this.gbFirmsAcc.SuspendLayout();
            this.gbItoCource.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFirmsAcc
            // 
            this.gbFirmsAcc.Controls.Add(this.clbAccounts);
            this.gbFirmsAcc.Location = new System.Drawing.Point(12, 12);
            this.gbFirmsAcc.Name = "gbFirmsAcc";
            this.gbFirmsAcc.Size = new System.Drawing.Size(482, 192);
            this.gbFirmsAcc.TabIndex = 2;
            this.gbFirmsAcc.TabStop = false;
            this.gbFirmsAcc.Text = "Фирмы и счета";
            // 
            // clbAccounts
            // 
            this.clbAccounts.FormattingEnabled = true;
            this.clbAccounts.Location = new System.Drawing.Point(6, 19);
            this.clbAccounts.Name = "clbAccounts";
            this.clbAccounts.Size = new System.Drawing.Size(470, 169);
            this.clbAccounts.TabIndex = 2;
            this.clbAccounts.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbAccounts_ItemCheck);
            this.clbAccounts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.clbAccounts_MouseDown);
            // 
            // gbItoCource
            // 
            this.gbItoCource.Controls.Add(this.lbCurr);
            this.gbItoCource.Location = new System.Drawing.Point(12, 210);
            this.gbItoCource.Name = "gbItoCource";
            this.gbItoCource.Size = new System.Drawing.Size(482, 118);
            this.gbItoCource.TabIndex = 3;
            this.gbItoCource.TabStop = false;
            this.gbItoCource.Text = "Курсы валют";
            // 
            // lbCurr
            // 
            this.lbCurr.FormattingEnabled = true;
            this.lbCurr.Location = new System.Drawing.Point(6, 19);
            this.lbCurr.Name = "lbCurr";
            this.lbCurr.Size = new System.Drawing.Size(470, 95);
            this.lbCurr.TabIndex = 0;
            this.lbCurr.DoubleClick += new System.EventHandler(this.lbCurr_DoubleClick);
            // 
            // BalancePropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 335);
            this.Controls.Add(this.gbItoCource);
            this.Controls.Add(this.gbFirmsAcc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BalancePropForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Дополнительные параметры";
            this.gbFirmsAcc.ResumeLayout(false);
            this.gbItoCource.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFirmsAcc;
        private System.Windows.Forms.GroupBox gbItoCource;
        public System.Windows.Forms.CheckedListBox clbAccounts;
        public System.Windows.Forms.ListBox lbCurr;
    }
}