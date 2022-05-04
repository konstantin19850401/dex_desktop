namespace DEXPlugin.Dictionary.People
{
    partial class FPeopleMain
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
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.bFilter = new System.Windows.Forms.Button();
            this.deBirth = new DEXExtendLib.DateEdit();
            this.tbSecondname = new System.Windows.Forms.TextBox();
            this.tbFirstname = new System.Windows.Forms.TextBox();
            this.tbLastname = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.bDelete = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Lastname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Firstname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Secondname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Birth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.label5);
            this.gbFilter.Controls.Add(this.bFilter);
            this.gbFilter.Controls.Add(this.deBirth);
            this.gbFilter.Controls.Add(this.tbSecondname);
            this.gbFilter.Controls.Add(this.tbFirstname);
            this.gbFilter.Controls.Add(this.tbLastname);
            this.gbFilter.Controls.Add(this.label4);
            this.gbFilter.Controls.Add(this.label3);
            this.gbFilter.Controls.Add(this.label2);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Location = new System.Drawing.Point(12, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(502, 81);
            this.gbFilter.TabIndex = 0;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Фильтр";
            // 
            // bFilter
            // 
            this.bFilter.Location = new System.Drawing.Point(416, 30);
            this.bFilter.Name = "bFilter";
            this.bFilter.Size = new System.Drawing.Size(75, 23);
            this.bFilter.TabIndex = 8;
            this.bFilter.Text = "Отбор";
            this.bFilter.UseVisualStyleBackColor = true;
            this.bFilter.Click += new System.EventHandler(this.bFilter_Click);
            // 
            // deBirth
            // 
            this.deBirth.FormattingEnabled = true;
            this.deBirth.InputChar = '_';
            this.deBirth.Location = new System.Drawing.Point(327, 32);
            this.deBirth.MaxLength = 10;
            this.deBirth.Name = "deBirth";
            this.deBirth.Size = new System.Drawing.Size(83, 21);
            this.deBirth.TabIndex = 7;
            // 
            // tbSecondname
            // 
            this.tbSecondname.Location = new System.Drawing.Point(221, 32);
            this.tbSecondname.Name = "tbSecondname";
            this.tbSecondname.Size = new System.Drawing.Size(100, 20);
            this.tbSecondname.TabIndex = 6;
            // 
            // tbFirstname
            // 
            this.tbFirstname.Location = new System.Drawing.Point(115, 32);
            this.tbFirstname.Name = "tbFirstname";
            this.tbFirstname.Size = new System.Drawing.Size(100, 20);
            this.tbFirstname.TabIndex = 5;
            // 
            // tbLastname
            // 
            this.tbLastname.Location = new System.Drawing.Point(9, 32);
            this.tbLastname.Name = "tbLastname";
            this.tbLastname.Size = new System.Drawing.Size(100, 20);
            this.tbLastname.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(324, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Дата рождения";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(218, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Отчество";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Имя";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Фамилия";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Lastname,
            this.Firstname,
            this.Secondname,
            this.Birth});
            this.dgv.Location = new System.Drawing.Point(12, 99);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(502, 244);
            this.dgv.TabIndex = 1;
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(439, 349);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(75, 23);
            this.bDelete.TabIndex = 3;
            this.bDelete.Text = "Удалить";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(9, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(283, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Для отбора по маске можно использовать символ \'%\'";
            // 
            // Lastname
            // 
            this.Lastname.DataPropertyName = "lastname";
            this.Lastname.HeaderText = "Фамилия";
            this.Lastname.Name = "Lastname";
            // 
            // Firstname
            // 
            this.Firstname.DataPropertyName = "firstname";
            this.Firstname.HeaderText = "Имя";
            this.Firstname.Name = "Firstname";
            // 
            // Secondname
            // 
            this.Secondname.DataPropertyName = "secondname";
            this.Secondname.HeaderText = "Отчество";
            this.Secondname.Name = "Secondname";
            // 
            // Birth
            // 
            this.Birth.DataPropertyName = "birth";
            this.Birth.FillWeight = 60F;
            this.Birth.HeaderText = "Дата рождения";
            this.Birth.Name = "Birth";
            // 
            // FPeopleMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 380);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.gbFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPeopleMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Справочник абонентов";
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button bFilter;
        private DEXExtendLib.DateEdit deBirth;
        private System.Windows.Forms.TextBox tbSecondname;
        private System.Windows.Forms.TextBox tbFirstname;
        private System.Windows.Forms.TextBox tbLastname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lastname;
        private System.Windows.Forms.DataGridViewTextBoxColumn Firstname;
        private System.Windows.Forms.DataGridViewTextBoxColumn Secondname;
        private System.Windows.Forms.DataGridViewTextBoxColumn Birth;
    }
}