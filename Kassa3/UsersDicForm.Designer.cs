namespace Kassa3
{
    partial class UsersDicForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bDeleteUser = new System.Windows.Forms.Button();
            this.bEditUser = new System.Windows.Forms.Button();
            this.bNewUser = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.login = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(417, 394);
            this.dataGridView1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bRefresh);
            this.panel1.Controls.Add(this.bDeleteUser);
            this.panel1.Controls.Add(this.bEditUser);
            this.panel1.Controls.Add(this.bNewUser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 348);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(417, 46);
            this.panel1.TabIndex = 1;
            // 
            // bRefresh
            // 
            this.bRefresh.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bRefresh.Location = new System.Drawing.Point(330, 12);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(75, 23);
            this.bRefresh.TabIndex = 3;
            this.bRefresh.Text = "Обновить";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bDeleteUser
            // 
            this.bDeleteUser.Location = new System.Drawing.Point(174, 12);
            this.bDeleteUser.Name = "bDeleteUser";
            this.bDeleteUser.Size = new System.Drawing.Size(75, 23);
            this.bDeleteUser.TabIndex = 2;
            this.bDeleteUser.Text = "Удалить";
            this.bDeleteUser.UseVisualStyleBackColor = true;
            this.bDeleteUser.Click += new System.EventHandler(this.bDeleteUser_Click);
            // 
            // bEditUser
            // 
            this.bEditUser.Location = new System.Drawing.Point(93, 12);
            this.bEditUser.Name = "bEditUser";
            this.bEditUser.Size = new System.Drawing.Size(75, 23);
            this.bEditUser.TabIndex = 1;
            this.bEditUser.Text = "Изменить";
            this.bEditUser.UseVisualStyleBackColor = true;
            this.bEditUser.Click += new System.EventHandler(this.bEditUser_Click);
            // 
            // bNewUser
            // 
            this.bNewUser.Location = new System.Drawing.Point(12, 12);
            this.bNewUser.Name = "bNewUser";
            this.bNewUser.Size = new System.Drawing.Size(75, 23);
            this.bNewUser.TabIndex = 0;
            this.bNewUser.Text = "Новый";
            this.bNewUser.UseVisualStyleBackColor = true;
            this.bNewUser.Click += new System.EventHandler(this.bNewUser_Click);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.login,
            this.active});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 0);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(417, 348);
            this.dgv.TabIndex = 2;
            this.dgv.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellMouseDoubleClick);
            this.dgv.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgv_KeyUp);
            // 
            // login
            // 
            this.login.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.login.DataPropertyName = "login";
            this.login.FillWeight = 80F;
            this.login.HeaderText = "Пользователь";
            this.login.Name = "login";
            this.login.ReadOnly = true;
            // 
            // active
            // 
            this.active.DataPropertyName = "active";
            this.active.FillWeight = 20F;
            this.active.HeaderText = "Активен";
            this.active.Name = "active";
            this.active.ReadOnly = true;
            // 
            // UsersDicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 394);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.MinimizeBox = false;
            this.Name = "UsersDicForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Справочник пользователей";
            this.Shown += new System.EventHandler(this.UsersDicForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UsersDicForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bNewUser;
        private System.Windows.Forms.Button bDeleteUser;
        private System.Windows.Forms.Button bEditUser;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn login;
        private System.Windows.Forms.DataGridViewCheckBoxColumn active;
    }
}