namespace Kassa3
{
    partial class RuleEditForm
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
            this.tbRPrim = new System.Windows.Forms.TextBox();
            this.cmsPrim = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.cbDst = new System.Windows.Forms.ComboBox();
            this.cbDstType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbSrc = new System.Windows.Forms.ComboBox();
            this.cbSrcType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbOp = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.cbStatus = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bEdit = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.bDel = new System.Windows.Forms.Button();
            this.lbMatches = new System.Windows.Forms.ListBox();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lWarning = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lWarning);
            this.groupBox1.Controls.Add(this.tbRPrim);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbDst);
            this.groupBox1.Controls.Add(this.cbDstType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbSrc);
            this.groupBox1.Controls.Add(this.cbSrcType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbOp);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(579, 143);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры операции";
            // 
            // tbRPrim
            // 
            this.tbRPrim.ContextMenuStrip = this.cmsPrim;
            this.tbRPrim.Location = new System.Drawing.Point(82, 113);
            this.tbRPrim.Name = "tbRPrim";
            this.tbRPrim.Size = new System.Drawing.Size(491, 20);
            this.tbRPrim.TabIndex = 11;
            this.toolTip1.SetToolTip(this.tbRPrim, "В контекстном меню список допустимых макросов");
            // 
            // cmsPrim
            // 
            this.cmsPrim.Name = "cmsPrim";
            this.cmsPrim.Size = new System.Drawing.Size(61, 4);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Примечание";
            // 
            // cbDst
            // 
            this.cbDst.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDst.FormattingEnabled = true;
            this.cbDst.Location = new System.Drawing.Point(233, 73);
            this.cbDst.Name = "cbDst";
            this.cbDst.Size = new System.Drawing.Size(340, 21);
            this.cbDst.TabIndex = 9;
            // 
            // cbDstType
            // 
            this.cbDstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDstType.FormattingEnabled = true;
            this.cbDstType.Items.AddRange(new object[] {
            "Счёт",
            "Контрагент"});
            this.cbDstType.Location = new System.Drawing.Point(82, 73);
            this.cbDstType.Name = "cbDstType";
            this.cbDstType.Size = new System.Drawing.Size(145, 21);
            this.cbDstType.TabIndex = 7;
            this.cbDstType.SelectedIndexChanged += new System.EventHandler(this.cbDstType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Приёмник";
            // 
            // cbSrc
            // 
            this.cbSrc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSrc.FormattingEnabled = true;
            this.cbSrc.Location = new System.Drawing.Point(233, 46);
            this.cbSrc.Name = "cbSrc";
            this.cbSrc.Size = new System.Drawing.Size(340, 21);
            this.cbSrc.TabIndex = 5;
            // 
            // cbSrcType
            // 
            this.cbSrcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSrcType.FormattingEnabled = true;
            this.cbSrcType.Items.AddRange(new object[] {
            "Счёт",
            "Контрагент"});
            this.cbSrcType.Location = new System.Drawing.Point(82, 46);
            this.cbSrcType.Name = "cbSrcType";
            this.cbSrcType.Size = new System.Drawing.Size(145, 21);
            this.cbSrcType.TabIndex = 3;
            this.cbSrcType.SelectedIndexChanged += new System.EventHandler(this.cbSrcType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Источник";
            // 
            // cbOp
            // 
            this.cbOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOp.FormattingEnabled = true;
            this.cbOp.Location = new System.Drawing.Point(82, 19);
            this.cbOp.Name = "cbOp";
            this.cbOp.Size = new System.Drawing.Size(491, 21);
            this.cbOp.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Операция";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Наименование правила";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(166, 10);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(419, 20);
            this.tbTitle.TabIndex = 2;
            // 
            // cbStatus
            // 
            this.cbStatus.AutoSize = true;
            this.cbStatus.Location = new System.Drawing.Point(166, 36);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(114, 17);
            this.cbStatus.TabIndex = 3;
            this.cbStatus.Text = "Правило активно";
            this.cbStatus.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bEdit);
            this.groupBox2.Controls.Add(this.bNew);
            this.groupBox2.Controls.Add(this.bDel);
            this.groupBox2.Controls.Add(this.lbMatches);
            this.groupBox2.Location = new System.Drawing.Point(12, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(579, 218);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Критерии импорта";
            // 
            // bEdit
            // 
            this.bEdit.Location = new System.Drawing.Point(87, 185);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(75, 23);
            this.bEdit.TabIndex = 3;
            this.bEdit.Text = "Изменить";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // bNew
            // 
            this.bNew.Location = new System.Drawing.Point(6, 185);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(75, 23);
            this.bNew.TabIndex = 2;
            this.bNew.Text = "Новый";
            this.bNew.UseVisualStyleBackColor = true;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // bDel
            // 
            this.bDel.Location = new System.Drawing.Point(168, 185);
            this.bDel.Name = "bDel";
            this.bDel.Size = new System.Drawing.Size(75, 23);
            this.bDel.TabIndex = 1;
            this.bDel.Text = "Удалить";
            this.bDel.UseVisualStyleBackColor = true;
            this.bDel.Click += new System.EventHandler(this.bDel_Click);
            // 
            // lbMatches
            // 
            this.lbMatches.FormattingEnabled = true;
            this.lbMatches.HorizontalScrollbar = true;
            this.lbMatches.Location = new System.Drawing.Point(6, 19);
            this.lbMatches.Name = "lbMatches";
            this.lbMatches.Size = new System.Drawing.Size(567, 160);
            this.lbMatches.TabIndex = 0;
            this.lbMatches.DoubleClick += new System.EventHandler(this.bEdit_Click);
            this.lbMatches.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbMatches_KeyDown);
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(435, 432);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 5;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(516, 432);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // lWarning
            // 
            this.lWarning.AutoSize = true;
            this.lWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lWarning.ForeColor = System.Drawing.Color.Red;
            this.lWarning.Location = new System.Drawing.Point(79, 97);
            this.lWarning.Name = "lWarning";
            this.lWarning.Size = new System.Drawing.Size(446, 13);
            this.lWarning.TabIndex = 12;
            this.lWarning.Text = "Внимание! Источник и приёмник должны иметь одинаковый тип валюты.";
            // 
            // RuleEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(603, 466);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RuleEditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактор правила импорта";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RuleEditForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbRPrim;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbDst;
        private System.Windows.Forms.ComboBox cbDstType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbSrc;
        private System.Windows.Forms.ComboBox cbSrcType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbOp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.CheckBox cbStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bEdit;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button bDel;
        private System.Windows.Forms.ListBox lbMatches;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.ContextMenuStrip cmsPrim;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lWarning;
    }
}