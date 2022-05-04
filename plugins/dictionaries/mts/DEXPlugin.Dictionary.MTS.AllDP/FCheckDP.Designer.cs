using System.Windows.Forms;
namespace DEXPlugin.Dictionary.MTS.AllDP
{
    partial class FCheckDP
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
            this.cbEnc = new System.Windows.Forms.ComboBox();
            this.cbQuotes = new System.Windows.Forms.CheckBox();
            this.cbSeparator = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bClearTable = new System.Windows.Forms.Button();
            this.bSrcFromBuffer = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bStartCheck = new System.Windows.Forms.Button();
            this.bExit = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.btnAddDp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbEnc);
            this.groupBox1.Controls.Add(this.cbQuotes);
            this.groupBox1.Controls.Add(this.cbSeparator);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.bClearTable);
            this.groupBox1.Controls.Add(this.bSrcFromBuffer);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 88);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Источник";
            // 
            // cbEnc
            // 
            this.cbEnc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEnc.FormattingEnabled = true;
            this.cbEnc.Items.AddRange(new object[] {
            "UTF-8",
            "Windows-1251",
            "DOS (866)"});
            this.cbEnc.Location = new System.Drawing.Point(74, 51);
            this.cbEnc.Name = "cbEnc";
            this.cbEnc.Size = new System.Drawing.Size(164, 21);
            this.cbEnc.TabIndex = 11;
            // 
            // cbQuotes
            // 
            this.cbQuotes.AutoSize = true;
            this.cbQuotes.Location = new System.Drawing.Point(402, 53);
            this.cbQuotes.Name = "cbQuotes";
            this.cbQuotes.Size = new System.Drawing.Size(134, 17);
            this.cbQuotes.TabIndex = 13;
            this.cbQuotes.Text = "Значения в кавычках";
            this.cbQuotes.UseVisualStyleBackColor = true;
            // 
            // cbSeparator
            // 
            this.cbSeparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeparator.FormattingEnabled = true;
            this.cbSeparator.Items.AddRange(new object[] {
            "<Tab>",
            "<;>",
            "<:>",
            "<|>",
            "<.>",
            "<,>",
            "<!>",
            "<&>"});
            this.cbSeparator.Location = new System.Drawing.Point(323, 51);
            this.cbSeparator.Name = "cbSeparator";
            this.cbSeparator.Size = new System.Drawing.Size(73, 21);
            this.cbSeparator.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Разделитель";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Кодировка";
            // 
            // bClearTable
            // 
            this.bClearTable.Location = new System.Drawing.Point(461, 16);
            this.bClearTable.Name = "bClearTable";
            this.bClearTable.Size = new System.Drawing.Size(75, 23);
            this.bClearTable.TabIndex = 6;
            this.bClearTable.Text = "Очистить";
            this.bClearTable.UseVisualStyleBackColor = true;
            this.bClearTable.Click += new System.EventHandler(this.bClearTable_Click);
            // 
            // bSrcFromBuffer
            // 
            this.bSrcFromBuffer.Location = new System.Drawing.Point(323, 16);
            this.bSrcFromBuffer.Name = "bSrcFromBuffer";
            this.bSrcFromBuffer.Size = new System.Drawing.Size(128, 23);
            this.bSrcFromBuffer.TabIndex = 5;
            this.bSrcFromBuffer.Text = "Вставить из буфера";
            this.bSrcFromBuffer.UseVisualStyleBackColor = true;
            this.bSrcFromBuffer.Click += new System.EventHandler(this.bSrcFromBuffer_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgv);
            this.groupBox2.Location = new System.Drawing.Point(12, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(542, 259);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Данные";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.ContextMenuStrip = this.cms;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv.Location = new System.Drawing.Point(9, 19);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(527, 229);
            this.dgv.TabIndex = 0;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.checkBoxClick);
            this.dgv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            // 
            // cms
            // 
            this.cms.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cms.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(61, 4);
            // 
            // bStartCheck
            // 
            this.bStartCheck.Location = new System.Drawing.Point(12, 389);
            this.bStartCheck.Name = "bStartCheck";
            this.bStartCheck.Size = new System.Drawing.Size(108, 23);
            this.bStartCheck.TabIndex = 2;
            this.bStartCheck.Text = "Начать проверку";
            this.bStartCheck.UseVisualStyleBackColor = true;
            this.bStartCheck.Click += new System.EventHandler(this.bStartCheck_Click);
            // 
            // bExit
            // 
            this.bExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bExit.Location = new System.Drawing.Point(479, 389);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(75, 23);
            this.bExit.TabIndex = 5;
            this.bExit.Text = "Выход";
            this.bExit.UseVisualStyleBackColor = true;
            // 
            // ofd
            // 
            this.ofd.Filter = "Текстовые файлы|*.*";
            // 
            // sfd
            // 
            this.sfd.Filter = "Файлы CSV|*.csv";
            // 
            // btnAddDp
            // 
            this.btnAddDp.Enabled = false;
            this.btnAddDp.Location = new System.Drawing.Point(139, 389);
            this.btnAddDp.Name = "btnAddDp";
            this.btnAddDp.Size = new System.Drawing.Size(111, 23);
            this.btnAddDp.TabIndex = 6;
            this.btnAddDp.Text = "Добавить";
            this.btnAddDp.UseVisualStyleBackColor = true;
            this.btnAddDp.Click += new System.EventHandler(this.btnAddDp_Click);
            // 
            // FCheckDP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bExit;
            this.ClientSize = new System.Drawing.Size(566, 433);
            this.Controls.Add(this.btnAddDp);
            this.Controls.Add(this.bExit);
            this.Controls.Add(this.bStartCheck);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCheckDP";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сравнение и добавление новых точек";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bClearTable;
        private System.Windows.Forms.Button bSrcFromBuffer;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button bStartCheck;
        private System.Windows.Forms.Button bExit;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbEnc;
        private System.Windows.Forms.CheckBox cbQuotes;
        private System.Windows.Forms.ComboBox cbSeparator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip cms;
        private Button btnAddDp;
    }
}