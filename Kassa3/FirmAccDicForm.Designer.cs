namespace Kassa3
{
    partial class FirmAccDicForm
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
            this.cbFirm = new System.Windows.Forms.ComboBox();
            this.bFirmAdd = new System.Windows.Forms.Button();
            this.bFirmEdit = new System.Windows.Forms.Button();
            this.bFirmDelete = new System.Windows.Forms.Button();
            this.gbAcc = new System.Windows.Forms.GroupBox();
            this.bAccDelete = new System.Windows.Forms.Button();
            this.bAccEdit = new System.Windows.Forms.Button();
            this.bAccAdd = new System.Windows.Forms.Button();
            this.lbAcc = new System.Windows.Forms.ListBox();
            this.gbFirm = new System.Windows.Forms.GroupBox();
            this.gbAcc.SuspendLayout();
            this.gbFirm.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbFirm
            // 
            this.cbFirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFirm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFirm.FormattingEnabled = true;
            this.cbFirm.Location = new System.Drawing.Point(6, 19);
            this.cbFirm.Name = "cbFirm";
            this.cbFirm.Size = new System.Drawing.Size(375, 21);
            this.cbFirm.TabIndex = 0;
            this.cbFirm.SelectedIndexChanged += new System.EventHandler(this.cbFirm_SelectedIndexChanged);
            // 
            // bFirmAdd
            // 
            this.bFirmAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bFirmAdd.Location = new System.Drawing.Point(144, 46);
            this.bFirmAdd.Name = "bFirmAdd";
            this.bFirmAdd.Size = new System.Drawing.Size(75, 23);
            this.bFirmAdd.TabIndex = 2;
            this.bFirmAdd.Text = "Новая";
            this.bFirmAdd.UseVisualStyleBackColor = true;
            this.bFirmAdd.Click += new System.EventHandler(this.bFirmAdd_Click);
            // 
            // bFirmEdit
            // 
            this.bFirmEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bFirmEdit.Location = new System.Drawing.Point(225, 46);
            this.bFirmEdit.Name = "bFirmEdit";
            this.bFirmEdit.Size = new System.Drawing.Size(75, 23);
            this.bFirmEdit.TabIndex = 3;
            this.bFirmEdit.Text = "Изменить";
            this.bFirmEdit.UseVisualStyleBackColor = true;
            this.bFirmEdit.Click += new System.EventHandler(this.bFirmEdit_Click);
            // 
            // bFirmDelete
            // 
            this.bFirmDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bFirmDelete.Location = new System.Drawing.Point(306, 46);
            this.bFirmDelete.Name = "bFirmDelete";
            this.bFirmDelete.Size = new System.Drawing.Size(75, 23);
            this.bFirmDelete.TabIndex = 4;
            this.bFirmDelete.Text = "Удалить";
            this.bFirmDelete.UseVisualStyleBackColor = true;
            this.bFirmDelete.Click += new System.EventHandler(this.bFirmDelete_Click);
            // 
            // gbAcc
            // 
            this.gbAcc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAcc.Controls.Add(this.bAccDelete);
            this.gbAcc.Controls.Add(this.bAccEdit);
            this.gbAcc.Controls.Add(this.bAccAdd);
            this.gbAcc.Controls.Add(this.lbAcc);
            this.gbAcc.Location = new System.Drawing.Point(15, 97);
            this.gbAcc.Name = "gbAcc";
            this.gbAcc.Size = new System.Drawing.Size(387, 288);
            this.gbAcc.TabIndex = 5;
            this.gbAcc.TabStop = false;
            this.gbAcc.Text = "Список счетов";
            // 
            // bAccDelete
            // 
            this.bAccDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bAccDelete.Location = new System.Drawing.Point(306, 259);
            this.bAccDelete.Name = "bAccDelete";
            this.bAccDelete.Size = new System.Drawing.Size(75, 23);
            this.bAccDelete.TabIndex = 3;
            this.bAccDelete.Text = "Удалить";
            this.bAccDelete.UseVisualStyleBackColor = true;
            this.bAccDelete.Click += new System.EventHandler(this.bAccDelete_Click);
            // 
            // bAccEdit
            // 
            this.bAccEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bAccEdit.Location = new System.Drawing.Point(225, 259);
            this.bAccEdit.Name = "bAccEdit";
            this.bAccEdit.Size = new System.Drawing.Size(75, 23);
            this.bAccEdit.TabIndex = 2;
            this.bAccEdit.Text = "Изменить";
            this.bAccEdit.UseVisualStyleBackColor = true;
            this.bAccEdit.Click += new System.EventHandler(this.bAccEdit_Click);
            // 
            // bAccAdd
            // 
            this.bAccAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bAccAdd.Location = new System.Drawing.Point(144, 259);
            this.bAccAdd.Name = "bAccAdd";
            this.bAccAdd.Size = new System.Drawing.Size(75, 23);
            this.bAccAdd.TabIndex = 1;
            this.bAccAdd.Text = "Новый";
            this.bAccAdd.UseVisualStyleBackColor = true;
            this.bAccAdd.Click += new System.EventHandler(this.bAccAdd_Click);
            // 
            // lbAcc
            // 
            this.lbAcc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAcc.FormattingEnabled = true;
            this.lbAcc.Location = new System.Drawing.Point(6, 19);
            this.lbAcc.Name = "lbAcc";
            this.lbAcc.Size = new System.Drawing.Size(375, 225);
            this.lbAcc.TabIndex = 0;
            this.lbAcc.DoubleClick += new System.EventHandler(this.bAccEdit_Click);
            // 
            // gbFirm
            // 
            this.gbFirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFirm.Controls.Add(this.cbFirm);
            this.gbFirm.Controls.Add(this.bFirmAdd);
            this.gbFirm.Controls.Add(this.bFirmDelete);
            this.gbFirm.Controls.Add(this.bFirmEdit);
            this.gbFirm.Location = new System.Drawing.Point(15, 12);
            this.gbFirm.Name = "gbFirm";
            this.gbFirm.Size = new System.Drawing.Size(387, 79);
            this.gbFirm.TabIndex = 6;
            this.gbFirm.TabStop = false;
            this.gbFirm.Text = "Фирма";
            // 
            // FirmAccDicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 397);
            this.Controls.Add(this.gbFirm);
            this.Controls.Add(this.gbAcc);
            this.Name = "FirmAccDicForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Фирмы и счета";
            this.Shown += new System.EventHandler(this.FirmAccDicForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FirmAccDicForm_FormClosing);
            this.gbAcc.ResumeLayout(false);
            this.gbFirm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbFirm;
        private System.Windows.Forms.Button bFirmAdd;
        private System.Windows.Forms.Button bFirmEdit;
        private System.Windows.Forms.Button bFirmDelete;
        private System.Windows.Forms.GroupBox gbAcc;
        private System.Windows.Forms.Button bAccDelete;
        private System.Windows.Forms.Button bAccEdit;
        private System.Windows.Forms.Button bAccAdd;
        private System.Windows.Forms.ListBox lbAcc;
        private System.Windows.Forms.GroupBox gbFirm;
    }
}