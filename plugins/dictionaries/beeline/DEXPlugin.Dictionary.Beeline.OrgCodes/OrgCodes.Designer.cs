namespace DEXPlugin.Dictionary.Beeline.OrgCodes
{
    partial class OrgCodes
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.bEdit = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.dgvOrgCodes = new System.Windows.Forms.DataGridView();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgCodes)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bRefresh);
            this.panel1.Controls.Add(this.bDelete);
            this.panel1.Controls.Add(this.bEdit);
            this.panel1.Controls.Add(this.bNew);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 331);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(577, 44);
            this.panel1.TabIndex = 5;
            // 
            // bRefresh
            // 
            this.bRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bRefresh.Location = new System.Drawing.Point(490, 9);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(75, 23);
            this.bRefresh.TabIndex = 4;
            this.bRefresh.Text = "Обновить";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(244, 9);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(110, 23);
            this.bDelete.TabIndex = 2;
            this.bDelete.Text = "Удалить запись";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bEdit
            // 
            this.bEdit.Location = new System.Drawing.Point(128, 9);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(110, 23);
            this.bEdit.TabIndex = 1;
            this.bEdit.Text = "Изменить запись";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // bNew
            // 
            this.bNew.Location = new System.Drawing.Point(12, 9);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(110, 23);
            this.bNew.TabIndex = 0;
            this.bNew.Text = "Новая запись";
            this.bNew.UseVisualStyleBackColor = true;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // dgvOrgCodes
            // 
            this.dgvOrgCodes.AllowUserToAddRows = false;
            this.dgvOrgCodes.AllowUserToDeleteRows = false;
            this.dgvOrgCodes.AllowUserToResizeRows = false;
            this.dgvOrgCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrgCodes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrgCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrgCodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.title});
            this.dgvOrgCodes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOrgCodes.Location = new System.Drawing.Point(12, 12);
            this.dgvOrgCodes.MultiSelect = false;
            this.dgvOrgCodes.Name = "dgvOrgCodes";
            this.dgvOrgCodes.RowHeadersVisible = false;
            this.dgvOrgCodes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrgCodes.ShowEditingIcon = false;
            this.dgvOrgCodes.Size = new System.Drawing.Size(553, 302);
            this.dgvOrgCodes.TabIndex = 4;
            this.dgvOrgCodes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrgCodes_CellDoubleClick);
            // 
            // code
            // 
            this.code.FillWeight = 25.78474F;
            this.code.HeaderText = "Код подразделения";
            this.code.Name = "code";
            // 
            // title
            // 
            this.title.FillWeight = 48.73096F;
            this.title.HeaderText = "Название подразделения";
            this.title.Name = "title";
            // 
            // OrgCodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 375);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvOrgCodes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrgCodes";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник кодов подразделений";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrgCodes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bEdit;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.DataGridView dgvOrgCodes;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn title;
    }
}