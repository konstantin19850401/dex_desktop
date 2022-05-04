namespace DEXPlugin.Dictionary.Sim
{
    partial class FBalanceMain
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
            this.lbBal = new System.Windows.Forms.ListBox();
            this.bAdd = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbBal
            // 
            this.lbBal.FormattingEnabled = true;
            this.lbBal.Location = new System.Drawing.Point(12, 12);
            this.lbBal.Name = "lbBal";
            this.lbBal.Size = new System.Drawing.Size(345, 277);
            this.lbBal.TabIndex = 0;
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(12, 295);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(75, 23);
            this.bAdd.TabIndex = 1;
            this.bAdd.Text = "Добавить";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(93, 295);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(75, 23);
            this.bDelete.TabIndex = 2;
            this.bDelete.Text = "Удалить";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // FBalanceMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 328);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.lbBal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FBalanceMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Справочник значений баланса";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbBal;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bDelete;
    }
}