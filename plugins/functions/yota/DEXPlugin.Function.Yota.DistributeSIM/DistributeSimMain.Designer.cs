namespace DEXPlugin.Function.Yota.DistributeSIM
{
    partial class DistributeSimMain
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSortField = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.bFields = new System.Windows.Forms.Button();
            this.cmsFields = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiICC = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiParty = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDate = new System.Windows.Forms.ToolStripMenuItem();
            this.cbParty = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbUnsold = new System.Windows.Forms.ListBox();
            this.bExportUnsold = new System.Windows.Forms.Button();
            this.bPrintUnsold = new System.Windows.Forms.Button();
            this.bUnassign = new System.Windows.Forms.Button();
            this.lAssignedSim = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbUnassigned = new System.Windows.Forms.ListBox();
            this.bDistributeFromFile = new System.Windows.Forms.Button();
            this.bPrintUnassigned = new System.Windows.Forms.Button();
            this.cbAssignToClipboard = new System.Windows.Forms.CheckBox();
            this.bAssign = new System.Windows.Forms.Button();
            this.lFreeSim = new System.Windows.Forms.Label();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.cmsFields.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbSortField);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.bFields);
            this.groupBox1.Controls.Add(this.cbParty);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbUnit);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(852, 73);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Фильтр";
            // 
            // cbSortField
            // 
            this.cbSortField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSortField.FormattingEnabled = true;
            this.cbSortField.Items.AddRange(new object[] {
            "ICC",
            "Партия",
            "Дата"});
            this.cbSortField.Location = new System.Drawing.Point(358, 38);
            this.cbSortField.Name = "cbSortField";
            this.cbSortField.Size = new System.Drawing.Size(121, 21);
            this.cbSortField.TabIndex = 6;
            this.cbSortField.SelectedIndexChanged += new System.EventHandler(this.cbUnit_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(355, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Сортировка по полю";
            // 
            // bFields
            // 
            this.bFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bFields.ContextMenuStrip = this.cmsFields;
            this.bFields.Location = new System.Drawing.Point(732, 36);
            this.bFields.Name = "bFields";
            this.bFields.Size = new System.Drawing.Size(114, 23);
            this.bFields.TabIndex = 4;
            this.bFields.Text = "Видимые поля";
            this.bFields.UseVisualStyleBackColor = true;
            this.bFields.Click += new System.EventHandler(this.bFields_Click);
            // 
            // cmsFields
            // 
            this.cmsFields.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiICC,
            this.tsmiParty,
            this.tsmiDate});
            this.cmsFields.Name = "cmsFields";
            this.cmsFields.Size = new System.Drawing.Size(115, 70);
            this.cmsFields.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsFields_ItemClicked);
            // 
            // tsmiICC
            // 
            this.tsmiICC.CheckOnClick = true;
            this.tsmiICC.Name = "tsmiICC";
            this.tsmiICC.Size = new System.Drawing.Size(114, 22);
            this.tsmiICC.Text = "ICC";
            this.tsmiICC.CheckedChanged += new System.EventHandler(this.tsmiMsisdn_CheckedChanged);
            // 
            // tsmiParty
            // 
            this.tsmiParty.CheckOnClick = true;
            this.tsmiParty.Name = "tsmiParty";
            this.tsmiParty.Size = new System.Drawing.Size(114, 22);
            this.tsmiParty.Text = "Партия";
            this.tsmiParty.CheckedChanged += new System.EventHandler(this.tsmiMsisdn_CheckedChanged);
            // 
            // tsmiDate
            // 
            this.tsmiDate.CheckOnClick = true;
            this.tsmiDate.Name = "tsmiDate";
            this.tsmiDate.Size = new System.Drawing.Size(114, 22);
            this.tsmiDate.Text = "Дата";
            this.tsmiDate.CheckedChanged += new System.EventHandler(this.tsmiMsisdn_CheckedChanged);
            // 
            // cbParty
            // 
            this.cbParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParty.FormattingEnabled = true;
            this.cbParty.Location = new System.Drawing.Point(266, 38);
            this.cbParty.Name = "cbParty";
            this.cbParty.Size = new System.Drawing.Size(86, 21);
            this.cbParty.TabIndex = 3;
            this.cbParty.SelectedIndexChanged += new System.EventHandler(this.cbUnit_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(263, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Партия";
            // 
            // cbUnit
            // 
            this.cbUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(9, 38);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(251, 21);
            this.cbUnit.TabIndex = 1;
            this.cbUnit.SelectedIndexChanged += new System.EventHandler(this.cbUnit_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Отделение";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.splitter1);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new System.Drawing.Point(12, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(852, 473);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SIM-карты";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbUnsold);
            this.panel2.Controls.Add(this.bExportUnsold);
            this.panel2.Controls.Add(this.bPrintUnsold);
            this.panel2.Controls.Add(this.bUnassign);
            this.panel2.Controls.Add(this.lAssignedSim);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(399, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(450, 454);
            this.panel2.TabIndex = 2;
            // 
            // lbUnsold
            // 
            this.lbUnsold.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUnsold.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbUnsold.FormattingEnabled = true;
            this.lbUnsold.Location = new System.Drawing.Point(9, 22);
            this.lbUnsold.Name = "lbUnsold";
            this.lbUnsold.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbUnsold.Size = new System.Drawing.Size(438, 355);
            this.lbUnsold.TabIndex = 5;
            this.lbUnsold.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbUnassigned_DrawItem);
            this.lbUnsold.SelectedIndexChanged += new System.EventHandler(this.lbUnsold_SelectedIndexChanged);
            this.lbUnsold.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbUnassigned_MouseDown);
            // 
            // bExportUnsold
            // 
            this.bExportUnsold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bExportUnsold.Location = new System.Drawing.Point(137, 419);
            this.bExportUnsold.Name = "bExportUnsold";
            this.bExportUnsold.Size = new System.Drawing.Size(145, 23);
            this.bExportUnsold.TabIndex = 4;
            this.bExportUnsold.Text = "Экспорт списка карт";
            this.bExportUnsold.UseVisualStyleBackColor = true;
            this.bExportUnsold.Click += new System.EventHandler(this.bExportUnsold_Click);
            // 
            // bPrintUnsold
            // 
            this.bPrintUnsold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bPrintUnsold.Location = new System.Drawing.Point(9, 419);
            this.bPrintUnsold.Name = "bPrintUnsold";
            this.bPrintUnsold.Size = new System.Drawing.Size(122, 23);
            this.bPrintUnsold.TabIndex = 3;
            this.bPrintUnsold.Text = "Печать списка карт";
            this.bPrintUnsold.UseVisualStyleBackColor = true;
            this.bPrintUnsold.Click += new System.EventHandler(this.bPrintUnsold_Click);
            // 
            // bUnassign
            // 
            this.bUnassign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bUnassign.Location = new System.Drawing.Point(9, 390);
            this.bUnassign.Name = "bUnassign";
            this.bUnassign.Size = new System.Drawing.Size(146, 23);
            this.bUnassign.TabIndex = 2;
            this.bUnassign.Text = "<<< F5 - Вернуть";
            this.bUnassign.UseVisualStyleBackColor = true;
            this.bUnassign.Click += new System.EventHandler(this.bUnassign_Click);
            // 
            // lAssignedSim
            // 
            this.lAssignedSim.AutoSize = true;
            this.lAssignedSim.Location = new System.Drawing.Point(6, 6);
            this.lAssignedSim.Name = "lAssignedSim";
            this.lAssignedSim.Size = new System.Drawing.Size(111, 13);
            this.lAssignedSim.TabIndex = 0;
            this.lAssignedSim.Text = "Непроданные карты";
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Location = new System.Drawing.Point(396, 16);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 454);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbUnassigned);
            this.panel1.Controls.Add(this.bDistributeFromFile);
            this.panel1.Controls.Add(this.bPrintUnassigned);
            this.panel1.Controls.Add(this.cbAssignToClipboard);
            this.panel1.Controls.Add(this.bAssign);
            this.panel1.Controls.Add(this.lFreeSim);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(393, 454);
            this.panel1.TabIndex = 0;
            // 
            // lbUnassigned
            // 
            this.lbUnassigned.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUnassigned.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbUnassigned.FormattingEnabled = true;
            this.lbUnassigned.Location = new System.Drawing.Point(6, 22);
            this.lbUnassigned.Name = "lbUnassigned";
            this.lbUnassigned.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbUnassigned.Size = new System.Drawing.Size(381, 355);
            this.lbUnassigned.TabIndex = 7;
            this.lbUnassigned.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbUnassigned_DrawItem);
            this.lbUnassigned.SelectedIndexChanged += new System.EventHandler(this.lbUnassigned_SelectedIndexChanged);
            this.lbUnassigned.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbUnassigned_MouseDown);
            // 
            // bDistributeFromFile
            // 
            this.bDistributeFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDistributeFromFile.Location = new System.Drawing.Point(138, 419);
            this.bDistributeFromFile.Name = "bDistributeFromFile";
            this.bDistributeFromFile.Size = new System.Drawing.Size(150, 23);
            this.bDistributeFromFile.TabIndex = 6;
            this.bDistributeFromFile.Text = "Распределить из файла";
            this.bDistributeFromFile.UseVisualStyleBackColor = true;
            this.bDistributeFromFile.Click += new System.EventHandler(this.bDistributeFromFile_Click);
            // 
            // bPrintUnassigned
            // 
            this.bPrintUnassigned.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bPrintUnassigned.Location = new System.Drawing.Point(10, 419);
            this.bPrintUnassigned.Name = "bPrintUnassigned";
            this.bPrintUnassigned.Size = new System.Drawing.Size(122, 23);
            this.bPrintUnassigned.TabIndex = 4;
            this.bPrintUnassigned.Text = "Печать списка карт";
            this.bPrintUnassigned.UseVisualStyleBackColor = true;
            this.bPrintUnassigned.Click += new System.EventHandler(this.bPrintUnassigned_Click);
            // 
            // cbAssignToClipboard
            // 
            this.cbAssignToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbAssignToClipboard.AutoSize = true;
            this.cbAssignToClipboard.Location = new System.Drawing.Point(172, 394);
            this.cbAssignToClipboard.Name = "cbAssignToClipboard";
            this.cbAssignToClipboard.Size = new System.Drawing.Size(116, 17);
            this.cbAssignToClipboard.TabIndex = 3;
            this.cbAssignToClipboard.Text = "и в буфер обмена";
            this.cbAssignToClipboard.UseVisualStyleBackColor = true;
            // 
            // bAssign
            // 
            this.bAssign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAssign.Location = new System.Drawing.Point(10, 390);
            this.bAssign.Name = "bAssign";
            this.bAssign.Size = new System.Drawing.Size(146, 23);
            this.bAssign.TabIndex = 2;
            this.bAssign.Text = "F5 - Присвоить >>>";
            this.bAssign.UseVisualStyleBackColor = true;
            this.bAssign.Click += new System.EventHandler(this.bAssign_Click);
            // 
            // lFreeSim
            // 
            this.lFreeSim.AutoSize = true;
            this.lFreeSim.Location = new System.Drawing.Point(7, 6);
            this.lFreeSim.Name = "lFreeSim";
            this.lFreeSim.Size = new System.Drawing.Size(98, 13);
            this.lFreeSim.TabIndex = 0;
            this.lFreeSim.Text = "Свободные карты";
            // 
            // ofd
            // 
            this.ofd.FileName = "openFileDialog1";
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "csv";
            this.sfd.Filter = "Файлы CSV|*.csv";
            // 
            // DistributeSimMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 576);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DistributeSimMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Распределение SIM-карт";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.cmsFields.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbParty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lAssignedSim;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lFreeSim;
        private System.Windows.Forms.CheckBox cbAssignToClipboard;
        private System.Windows.Forms.Button bAssign;
        private System.Windows.Forms.Button bPrintUnassigned;
        private System.Windows.Forms.Button bDistributeFromFile;
        private System.Windows.Forms.Button bExportUnsold;
        private System.Windows.Forms.Button bPrintUnsold;
        private System.Windows.Forms.Button bUnassign;
        private System.Windows.Forms.Button bFields;
        private System.Windows.Forms.ContextMenuStrip cmsFields;
        private System.Windows.Forms.ComboBox cbSortField;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem tsmiICC;
        private System.Windows.Forms.ToolStripMenuItem tsmiParty;
        private System.Windows.Forms.ToolStripMenuItem tsmiDate;
        private System.Windows.Forms.ListBox lbUnassigned;
        private System.Windows.Forms.ListBox lbUnsold;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.SaveFileDialog sfd;

    }
}