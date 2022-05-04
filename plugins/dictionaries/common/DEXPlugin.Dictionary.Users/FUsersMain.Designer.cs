namespace DEXPlugin.Dictionary.Users
{
    partial class FUsersMain
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
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.status = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.login = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datecreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bStatus = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.bEdit = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.status,
            this.login,
            this.title,
            this.datecreated});
            this.dgvUsers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvUsers.Location = new System.Drawing.Point(12, 12);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.ShowEditingIcon = false;
            this.dgvUsers.Size = new System.Drawing.Size(632, 300);
            this.dgvUsers.TabIndex = 0;
            this.dgvUsers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvUsers_CellFormatting);
            this.dgvUsers.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUsers_CellMouseDoubleClick);
            // 
            // status
            // 
            this.status.HeaderText = "Активен";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // login
            // 
            this.login.HeaderText = "Логин пользователя";
            this.login.Name = "login";
            this.login.ReadOnly = true;
            // 
            // title
            // 
            this.title.HeaderText = "Имя пользователя";
            this.title.Name = "title";
            this.title.ReadOnly = true;
            // 
            // datecreated
            // 
            this.datecreated.HeaderText = "Дата создания";
            this.datecreated.Name = "datecreated";
            this.datecreated.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bRefresh);
            this.panel1.Controls.Add(this.bStatus);
            this.panel1.Controls.Add(this.bDelete);
            this.panel1.Controls.Add(this.bEdit);
            this.panel1.Controls.Add(this.bNew);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 326);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(656, 44);
            this.panel1.TabIndex = 2;
            // 
            // bRefresh
            // 
            this.bRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bRefresh.Location = new System.Drawing.Point(569, 9);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(75, 23);
            this.bRefresh.TabIndex = 4;
            this.bRefresh.Text = "Обновить";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bStatus
            // 
            this.bStatus.Location = new System.Drawing.Point(375, 9);
            this.bStatus.Name = "bStatus";
            this.bStatus.Size = new System.Drawing.Size(75, 23);
            this.bStatus.TabIndex = 3;
            this.bStatus.Text = "Статус";
            this.bStatus.UseVisualStyleBackColor = true;
            this.bStatus.Click += new System.EventHandler(this.bStatus_Click);
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
            // FUsersMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 370);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FUsersMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник пользователей";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bStatus;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bEdit;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.DataGridViewCheckBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn login;
        private System.Windows.Forms.DataGridViewTextBoxColumn title;
        private System.Windows.Forms.DataGridViewTextBoxColumn datecreated;
    }
}