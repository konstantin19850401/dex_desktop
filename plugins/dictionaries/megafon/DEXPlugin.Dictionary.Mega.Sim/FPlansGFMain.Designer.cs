namespace DEXPlugin.Dictionary.Mega.Sim
{
    partial class FPlansGFMain
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
            this.dgvPlans = new System.Windows.Forms.DataGridView();
            this.plan_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.allowed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.allow_m = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlans)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bRefresh);
            this.panel1.Controls.Add(this.bDelete);
            this.panel1.Controls.Add(this.bEdit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 326);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(656, 44);
            this.panel1.TabIndex = 4;
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
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(128, 9);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(110, 23);
            this.bDelete.TabIndex = 2;
            this.bDelete.Text = "Удалить запись";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bEdit
            // 
            this.bEdit.Location = new System.Drawing.Point(12, 9);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(110, 23);
            this.bEdit.TabIndex = 1;
            this.bEdit.Text = "Изменить запись";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // dgvPlans
            // 
            this.dgvPlans.AllowUserToAddRows = false;
            this.dgvPlans.AllowUserToDeleteRows = false;
            this.dgvPlans.AllowUserToResizeRows = false;
            this.dgvPlans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPlans.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlans.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.plan_id,
            this.title,
            this.allowed,
            this.allow_m});
            this.dgvPlans.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPlans.Location = new System.Drawing.Point(12, 6);
            this.dgvPlans.MultiSelect = false;
            this.dgvPlans.Name = "dgvPlans";
            this.dgvPlans.RowHeadersVisible = false;
            this.dgvPlans.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlans.ShowEditingIcon = false;
            this.dgvPlans.Size = new System.Drawing.Size(632, 300);
            this.dgvPlans.TabIndex = 3;
            this.dgvPlans.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvPlans_MouseDoubleClick);
            // 
            // plan_id
            // 
            this.plan_id.FillWeight = 38.68021F;
            this.plan_id.HeaderText = "Идентификатор";
            this.plan_id.Name = "plan_id";
            // 
            // title
            // 
            this.title.FillWeight = 90.25381F;
            this.title.HeaderText = "Наименование";
            this.title.Name = "title";
            // 
            // allowed
            // 
            this.allowed.FillWeight = 71.06599F;
            this.allowed.HeaderText = "Разрешен к использованию в ФС";
            this.allowed.Name = "allowed";
            this.allowed.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.allowed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // allow_m
            // 
            this.allow_m.HeaderText = "Разрешен к использованию";
            this.allow_m.Name = "allow_m";
            // 
            // FPlansGFMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 370);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvPlans);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPlansGFMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник тарифных планов GF";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlans)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bEdit;
        private System.Windows.Forms.DataGridView dgvPlans;
        private System.Windows.Forms.DataGridViewTextBoxColumn plan_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn title;
        private System.Windows.Forms.DataGridViewCheckBoxColumn allowed;
        private System.Windows.Forms.DataGridViewCheckBoxColumn allow_m;
    }
}