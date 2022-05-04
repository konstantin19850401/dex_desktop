namespace DEXPlugin.Dictionary.Units
{
    partial class FUnitsMain
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
            this.dgvUnits = new System.Windows.Forms.DataGridView();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.foreign_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.documentstate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.region = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bStatus = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.bEdit = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnits)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvUnits
            // 
            this.dgvUnits.AllowUserToAddRows = false;
            this.dgvUnits.AllowUserToDeleteRows = false;
            this.dgvUnits.AllowUserToResizeRows = false;
            this.dgvUnits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUnits.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUnits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.title,
            this.uid,
            this.foreign_id,
            this.desc,
            this.status,
            this.documentstate,
            this.region});
            this.dgvUnits.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvUnits.Location = new System.Drawing.Point(12, 12);
            this.dgvUnits.MultiSelect = false;
            this.dgvUnits.Name = "dgvUnits";
            this.dgvUnits.RowHeadersVisible = false;
            this.dgvUnits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUnits.ShowEditingIcon = false;
            this.dgvUnits.Size = new System.Drawing.Size(1092, 300);
            this.dgvUnits.TabIndex = 0;
            this.dgvUnits.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnits_CellDoubleClick);
            this.dgvUnits.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvUnits_CellFormatting);
            this.dgvUnits.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvUnits_MouseClick);
            // 
            // title
            // 
            this.title.DataPropertyName = "title";
            this.title.FillWeight = 177.665F;
            this.title.HeaderText = "Наименование отделения";
            this.title.Name = "title";
            // 
            // uid
            // 
            this.uid.DataPropertyName = "uid";
            this.uid.FillWeight = 46.69724F;
            this.uid.HeaderText = "ID отделения";
            this.uid.Name = "uid";
            // 
            // foreign_id
            // 
            this.foreign_id.DataPropertyName = "foreign_id";
            this.foreign_id.FillWeight = 46.69724F;
            this.foreign_id.HeaderText = "Внешний ID";
            this.foreign_id.Name = "foreign_id";
            // 
            // desc
            // 
            this.desc.DataPropertyName = "desc";
            this.desc.FillWeight = 46.69724F;
            this.desc.HeaderText = "Описание";
            this.desc.Name = "desc";
            // 
            // status
            // 
            this.status.DataPropertyName = "status";
            this.status.FillWeight = 46.69724F;
            this.status.HeaderText = "Активно";
            this.status.Name = "status";
            // 
            // documentstate
            // 
            this.documentstate.DataPropertyName = "documentstate";
            this.documentstate.FillWeight = 46.69724F;
            this.documentstate.HeaderText = "Статус импортируемого документа";
            this.documentstate.Name = "documentstate";
            // 
            // region
            // 
            this.region.DataPropertyName = "region";
            this.region.FillWeight = 288.8488F;
            this.region.HeaderText = "Район";
            this.region.Name = "region";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbSearch);
            this.panel1.Controls.Add(this.bRefresh);
            this.panel1.Controls.Add(this.bStatus);
            this.panel1.Controls.Add(this.bDelete);
            this.panel1.Controls.Add(this.bEdit);
            this.panel1.Controls.Add(this.bNew);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 326);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1116, 44);
            this.panel1.TabIndex = 1;
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(476, 11);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(193, 20);
            this.tbSearch.TabIndex = 5;
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // bRefresh
            // 
            this.bRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bRefresh.Location = new System.Drawing.Point(1029, 9);
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
            // FUnitsMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 370);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvUnits);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FUnitsMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник отделений";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnits)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUnits;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bEdit;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.Button bStatus;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn title;
        private System.Windows.Forms.DataGridViewTextBoxColumn uid;
        private System.Windows.Forms.DataGridViewTextBoxColumn foreign_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn desc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn documentstate;
        private System.Windows.Forms.DataGridViewTextBoxColumn region;
    }
}