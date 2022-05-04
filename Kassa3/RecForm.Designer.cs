namespace Kassa3
{
    partial class RecForm
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
            this.gbOperation = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.deRDate = new DEXExtendLib.DateEdit();
            this.cbOp = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSrc = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSrcType = new System.Windows.Forms.ComboBox();
            this.bSrcCurrValueGet = new System.Windows.Forms.Button();
            this.nudSrcCurrValue = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbDst = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDstType = new System.Windows.Forms.ComboBox();
            this.bDstCurrValueGet = new System.Windows.Forms.Button();
            this.nudDstCurrValue = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.bEditPrim = new System.Windows.Forms.Button();
            this.bAddPrim = new System.Windows.Forms.Button();
            this.tbRSum = new System.Windows.Forms.TextBox();
            this.tbRPrim = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lCurrCode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.gbOperation.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSrcCurrValue)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDstCurrValue)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOperation
            // 
            this.gbOperation.Controls.Add(this.label1);
            this.gbOperation.Controls.Add(this.deRDate);
            this.gbOperation.Controls.Add(this.cbOp);
            this.gbOperation.Location = new System.Drawing.Point(12, 12);
            this.gbOperation.Name = "gbOperation";
            this.gbOperation.Size = new System.Drawing.Size(456, 49);
            this.gbOperation.TabIndex = 0;
            this.gbOperation.TabStop = false;
            this.gbOperation.Text = "Дата";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Операция";
            // 
            // deRDate
            // 
            this.deRDate.FormattingEnabled = true;
            this.deRDate.InputChar = '#';
            this.deRDate.Location = new System.Drawing.Point(6, 19);
            this.deRDate.MaxLength = 10;
            this.deRDate.Name = "deRDate";
            this.deRDate.Size = new System.Drawing.Size(94, 21);
            this.deRDate.TabIndex = 0;
            this.deRDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deRDate_KeyDown);
            // 
            // cbOp
            // 
            this.cbOp.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbOp.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbOp.FormattingEnabled = true;
            this.cbOp.Location = new System.Drawing.Point(106, 19);
            this.cbOp.Name = "cbOp";
            this.cbOp.Size = new System.Drawing.Size(344, 21);
            this.cbOp.TabIndex = 1;
            this.cbOp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deRDate_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSrc);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbSrcType);
            this.groupBox1.Controls.Add(this.bSrcCurrValueGet);
            this.groupBox1.Controls.Add(this.nudSrcCurrValue);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(12, 67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 115);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Источник";
            // 
            // cbSrc
            // 
            this.cbSrc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSrc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSrc.FormattingEnabled = true;
            this.cbSrc.Location = new System.Drawing.Point(6, 59);
            this.cbSrc.Name = "cbSrc";
            this.cbSrc.Size = new System.Drawing.Size(213, 21);
            this.cbSrc.TabIndex = 1;
            this.cbSrc.SelectedIndexChanged += new System.EventHandler(this.cbSrc_SelectedIndexChanged);
            this.cbSrc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deRDate_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Контрагент";
            // 
            // cbSrcType
            // 
            this.cbSrcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSrcType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbSrcType.FormattingEnabled = true;
            this.cbSrcType.Items.AddRange(new object[] {
            "Счёт",
            "Контрагент"});
            this.cbSrcType.Location = new System.Drawing.Point(6, 19);
            this.cbSrcType.Name = "cbSrcType";
            this.cbSrcType.Size = new System.Drawing.Size(213, 21);
            this.cbSrcType.TabIndex = 0;
            this.cbSrcType.SelectedIndexChanged += new System.EventHandler(this.cbSrcType_SelectedIndexChanged);
            this.cbSrcType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deRDate_KeyDown);
            // 
            // bSrcCurrValueGet
            // 
            this.bSrcCurrValueGet.Location = new System.Drawing.Point(192, 84);
            this.bSrcCurrValueGet.Name = "bSrcCurrValueGet";
            this.bSrcCurrValueGet.Size = new System.Drawing.Size(27, 20);
            this.bSrcCurrValueGet.TabIndex = 3;
            this.bSrcCurrValueGet.TabStop = false;
            this.bSrcCurrValueGet.Text = "?";
            this.bSrcCurrValueGet.UseVisualStyleBackColor = true;
            this.bSrcCurrValueGet.Click += new System.EventHandler(this.bSrcCurrValueGet_Click);
            // 
            // nudSrcCurrValue
            // 
            this.nudSrcCurrValue.DecimalPlaces = 4;
            this.nudSrcCurrValue.Location = new System.Drawing.Point(86, 86);
            this.nudSrcCurrValue.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudSrcCurrValue.Name = "nudSrcCurrValue";
            this.nudSrcCurrValue.Size = new System.Drawing.Size(100, 20);
            this.nudSrcCurrValue.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Курс к рублю";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbDst);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cbDstType);
            this.groupBox2.Controls.Add(this.bDstCurrValueGet);
            this.groupBox2.Controls.Add(this.nudDstCurrValue);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(243, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 115);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Приёмник";
            // 
            // cbDst
            // 
            this.cbDst.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbDst.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbDst.FormattingEnabled = true;
            this.cbDst.Location = new System.Drawing.Point(6, 59);
            this.cbDst.Name = "cbDst";
            this.cbDst.Size = new System.Drawing.Size(213, 21);
            this.cbDst.TabIndex = 1;
            this.cbDst.SelectedIndexChanged += new System.EventHandler(this.cbDst_SelectedIndexChanged);
            this.cbDst.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deRDate_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Контрагент";
            // 
            // cbDstType
            // 
            this.cbDstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDstType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbDstType.FormattingEnabled = true;
            this.cbDstType.Items.AddRange(new object[] {
            "Счёт",
            "Контрагент"});
            this.cbDstType.Location = new System.Drawing.Point(6, 19);
            this.cbDstType.Name = "cbDstType";
            this.cbDstType.Size = new System.Drawing.Size(213, 21);
            this.cbDstType.TabIndex = 0;
            this.cbDstType.SelectedIndexChanged += new System.EventHandler(this.cbDstType_SelectedIndexChanged);
            this.cbDstType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deRDate_KeyDown);
            // 
            // bDstCurrValueGet
            // 
            this.bDstCurrValueGet.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bDstCurrValueGet.Location = new System.Drawing.Point(192, 86);
            this.bDstCurrValueGet.Name = "bDstCurrValueGet";
            this.bDstCurrValueGet.Size = new System.Drawing.Size(27, 20);
            this.bDstCurrValueGet.TabIndex = 3;
            this.bDstCurrValueGet.TabStop = false;
            this.bDstCurrValueGet.Text = "?";
            this.bDstCurrValueGet.UseVisualStyleBackColor = true;
            this.bDstCurrValueGet.Click += new System.EventHandler(this.bDstCurrValueGet_Click);
            // 
            // nudDstCurrValue
            // 
            this.nudDstCurrValue.DecimalPlaces = 4;
            this.nudDstCurrValue.Location = new System.Drawing.Point(86, 86);
            this.nudDstCurrValue.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudDstCurrValue.Name = "nudDstCurrValue";
            this.nudDstCurrValue.Size = new System.Drawing.Size(100, 20);
            this.nudDstCurrValue.TabIndex = 2;
            this.nudDstCurrValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deRDate_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Курс к рублю";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bEditPrim);
            this.groupBox3.Controls.Add(this.bAddPrim);
            this.groupBox3.Controls.Add(this.tbRSum);
            this.groupBox3.Controls.Add(this.tbRPrim);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.lCurrCode);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(12, 188);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(456, 75);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Детали";
            // 
            // bEditPrim
            // 
            this.bEditPrim.Location = new System.Drawing.Point(430, 46);
            this.bEditPrim.Name = "bEditPrim";
            this.bEditPrim.Size = new System.Drawing.Size(20, 20);
            this.bEditPrim.TabIndex = 11;
            this.bEditPrim.Text = "?";
            this.bEditPrim.UseVisualStyleBackColor = true;
            this.bEditPrim.Click += new System.EventHandler(this.bEditPrim_Click);
            // 
            // bAddPrim
            // 
            this.bAddPrim.Location = new System.Drawing.Point(404, 46);
            this.bAddPrim.Name = "bAddPrim";
            this.bAddPrim.Size = new System.Drawing.Size(20, 20);
            this.bAddPrim.TabIndex = 10;
            this.bAddPrim.TabStop = false;
            this.bAddPrim.Text = "+";
            this.bAddPrim.UseVisualStyleBackColor = true;
            this.bAddPrim.Click += new System.EventHandler(this.bAddPrim_Click);
            // 
            // tbRSum
            // 
            this.tbRSum.CausesValidation = false;
            this.tbRSum.Location = new System.Drawing.Point(101, 19);
            this.tbRSum.Name = "tbRSum";
            this.tbRSum.Size = new System.Drawing.Size(100, 20);
            this.tbRSum.TabIndex = 1;
            this.tbRSum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deRDate_KeyDown);
            this.tbRSum.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // tbRPrim
            // 
            this.tbRPrim.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbRPrim.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbRPrim.Location = new System.Drawing.Point(101, 46);
            this.tbRPrim.Name = "tbRPrim";
            this.tbRPrim.Size = new System.Drawing.Size(297, 20);
            this.tbRPrim.TabIndex = 9;
            this.tbRPrim.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbRPrim_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Примечание";
            // 
            // lCurrCode
            // 
            this.lCurrCode.AutoSize = true;
            this.lCurrCode.Location = new System.Drawing.Point(207, 22);
            this.lCurrCode.Name = "lCurrCode";
            this.lCurrCode.Size = new System.Drawing.Size(100, 13);
            this.lCurrCode.TabIndex = 2;
            this.lCurrCode.Text = "Российские рубли";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Сумма операции";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(312, 269);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 4;
            this.bOk.Text = "Сохранить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(393, 269);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // RecForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(477, 300);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbOperation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Операция";
            this.Shown += new System.EventHandler(this.RecForm_Shown);
            this.gbOperation.ResumeLayout(false);
            this.gbOperation.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSrcCurrValue)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDstCurrValue)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOperation;
        private System.Windows.Forms.ComboBox cbOp;
        private DEXExtendLib.DateEdit deRDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lCurrCode;
        private System.Windows.Forms.TextBox tbRPrim;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudSrcCurrValue;
        private System.Windows.Forms.Button bSrcCurrValueGet;
        private System.Windows.Forms.Button bDstCurrValueGet;
        private System.Windows.Forms.NumericUpDown nudDstCurrValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbSrcType;
        private System.Windows.Forms.ComboBox cbDstType;
        private System.Windows.Forms.TextBox tbRSum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbSrc;
        private System.Windows.Forms.ComboBox cbDst;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bEditPrim;
        private System.Windows.Forms.Button bAddPrim;
    }
}