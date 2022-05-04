namespace DEXPlugin.Dictionary.Mega.SenderProfiles
{
    partial class FProfilesMain
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
            this.dgvProfiles = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.bEdit = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.pname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ppassword = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.psubscribers = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfiles)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvProfiles
            // 
            this.dgvProfiles.AllowUserToAddRows = false;
            this.dgvProfiles.AllowUserToDeleteRows = false;
            this.dgvProfiles.AllowUserToResizeRows = false;
            this.dgvProfiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProfiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProfiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pname,
            this.plogin,
            this.ppassword,
            this.plink,
            this.psubscribers});
            this.dgvProfiles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvProfiles.Location = new System.Drawing.Point(12, 12);
            this.dgvProfiles.Name = "dgvProfiles";
            this.dgvProfiles.RowHeadersVisible = false;
            this.dgvProfiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfiles.ShowEditingIcon = false;
            this.dgvProfiles.Size = new System.Drawing.Size(553, 300);
            this.dgvProfiles.TabIndex = 0;
            this.dgvProfiles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProfiles_CellDoubleClick);
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
            this.panel1.TabIndex = 1;
            // 
            // bRefresh
            // 
            this.bRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bRefresh.Location = new System.Drawing.Point(490, 9);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(75, 23);
            this.bRefresh.TabIndex = 6;
            this.bRefresh.Text = "Обновить";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(244, 9);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(110, 23);
            this.bDelete.TabIndex = 5;
            this.bDelete.Text = "Удалить запись";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bEdit
            // 
            this.bEdit.Location = new System.Drawing.Point(128, 9);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(110, 23);
            this.bEdit.TabIndex = 4;
            this.bEdit.Text = "Изменить запись";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // bNew
            // 
            this.bNew.Location = new System.Drawing.Point(12, 9);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(110, 23);
            this.bNew.TabIndex = 3;
            this.bNew.Text = "Новая запись";
            this.bNew.UseVisualStyleBackColor = true;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // pname
            // 
            this.pname.HeaderText = "Профиль";
            this.pname.Name = "pname";
            // 
            // plogin
            // 
            this.plogin.HeaderText = "Пользователь";
            this.plogin.Name = "plogin";
            // 
            // ppassword
            // 
            this.ppassword.HeaderText = "Пароль";
            this.ppassword.Name = "ppassword";
            // 
            // plink
            // 
            this.plink.HeaderText = "URL";
            this.plink.Name = "plink";
            // 
            // psubscribers
            // 
            this.psubscribers.FillWeight = 20F;
            this.psubscribers.HeaderText = "S";
            this.psubscribers.Name = "psubscribers";
            this.psubscribers.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.psubscribers.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // FProfilesMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 375);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvProfiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FProfilesMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Справочник профилей отправки";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfiles)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvProfiles;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bEdit;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn pname;
        private System.Windows.Forms.DataGridViewTextBoxColumn plogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn ppassword;
        private System.Windows.Forms.DataGridViewTextBoxColumn plink;
        private System.Windows.Forms.DataGridViewCheckBoxColumn psubscribers;
    }
}