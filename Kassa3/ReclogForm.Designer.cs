namespace Kassa3
{
    partial class ReclogForm
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
            this.bShow = new System.Windows.Forms.Button();
            this.deEnd = new DEXExtendLib.DateEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.deStart = new DEXExtendLib.DateEdit();
            this.cbKind = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUser = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.op_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.op_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.op_kind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.op_info = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bShow);
            this.panel1.Controls.Add(this.deEnd);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.deStart);
            this.panel1.Controls.Add(this.cbKind);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbUser);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(880, 99);
            this.panel1.TabIndex = 0;
            // 
            // bShow
            // 
            this.bShow.Location = new System.Drawing.Point(321, 64);
            this.bShow.Name = "bShow";
            this.bShow.Size = new System.Drawing.Size(96, 23);
            this.bShow.TabIndex = 8;
            this.bShow.Text = "Показать";
            this.bShow.UseVisualStyleBackColor = true;
            this.bShow.Click += new System.EventHandler(this.bShow_Click);
            // 
            // deEnd
            // 
            this.deEnd.FormattingEnabled = true;
            this.deEnd.InputChar = '#';
            this.deEnd.Location = new System.Drawing.Point(321, 12);
            this.deEnd.MaxLength = 10;
            this.deEnd.Name = "deEnd";
            this.deEnd.Size = new System.Drawing.Size(96, 21);
            this.deEnd.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(234, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Конечная дата";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Начальная дата";
            // 
            // deStart
            // 
            this.deStart.FormattingEnabled = true;
            this.deStart.InputChar = '#';
            this.deStart.Location = new System.Drawing.Point(106, 12);
            this.deStart.MaxLength = 10;
            this.deStart.Name = "deStart";
            this.deStart.Size = new System.Drawing.Size(96, 21);
            this.deStart.TabIndex = 4;
            // 
            // cbKind
            // 
            this.cbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKind.FormattingEnabled = true;
            this.cbKind.Items.AddRange(new object[] {
            "Любое",
            "Создание",
            "Изменение"});
            this.cbKind.Location = new System.Drawing.Point(106, 66);
            this.cbKind.Name = "cbKind";
            this.cbKind.Size = new System.Drawing.Size(204, 21);
            this.cbKind.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Действие";
            // 
            // cbUser
            // 
            this.cbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUser.FormattingEnabled = true;
            this.cbUser.Location = new System.Drawing.Point(106, 39);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(311, 21);
            this.cbUser.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Пользователь";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbInfo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 441);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(880, 131);
            this.panel2.TabIndex = 1;
            // 
            // tbInfo
            // 
            this.tbInfo.AcceptsReturn = true;
            this.tbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbInfo.Location = new System.Drawing.Point(0, 0);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ReadOnly = true;
            this.tbInfo.Size = new System.Drawing.Size(880, 131);
            this.tbInfo.TabIndex = 0;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToOrderColumns = true;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.op_date,
            this.op_user,
            this.op_kind,
            this.op_info});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 99);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.ShowEditingIcon = false;
            this.dgv.Size = new System.Drawing.Size(880, 342);
            this.dgv.TabIndex = 2;
            this.dgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_CellFormatting);
            this.dgv.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_RowEnter);
            // 
            // op_date
            // 
            this.op_date.DataPropertyName = "op_date";
            this.op_date.HeaderText = "Дата";
            this.op_date.Name = "op_date";
            // 
            // op_user
            // 
            this.op_user.DataPropertyName = "op_user";
            this.op_user.HeaderText = "Пользователь";
            this.op_user.Name = "op_user";
            // 
            // op_kind
            // 
            this.op_kind.DataPropertyName = "op_kind";
            this.op_kind.HeaderText = "Тип";
            this.op_kind.Name = "op_kind";
            // 
            // op_info
            // 
            this.op_info.DataPropertyName = "op_info";
            this.op_info.HeaderText = "Информация";
            this.op_info.Name = "op_info";
            // 
            // ReclogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 572);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ReclogForm";
            this.Text = "Журнал изменений";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReclogForm_FormClosing);
            this.Load += new System.EventHandler(this.ReclogForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.DataGridView dgv;
        private DEXExtendLib.DateEdit deEnd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private DEXExtendLib.DateEdit deStart;
        private System.Windows.Forms.ComboBox cbKind;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn op_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn op_user;
        private System.Windows.Forms.DataGridViewTextBoxColumn op_kind;
        private System.Windows.Forms.DataGridViewTextBoxColumn op_info;
        private System.Windows.Forms.Button bShow;
    }
}