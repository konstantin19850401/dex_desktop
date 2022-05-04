namespace DEXPlugin.Function.ExportDocData
{
    partial class ExportDocDataMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbDocType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gb2 = new System.Windows.Forms.GroupBox();
            this.clbFields = new System.Windows.Forms.CheckedListBox();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.deEnd = new DEXExtendLib.DateEdit();
            this.deStart = new DEXExtendLib.DateEdit();
            this.gb3 = new System.Windows.Forms.GroupBox();
            this.cbSeparator = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbQuotes = new System.Windows.Forms.CheckBox();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.gb1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.gb2.SuspendLayout();
            this.gb3.SuspendLayout();
            this.gb1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(231, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "по";
            // 
            // cbDocType
            // 
            this.cbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocType.FormattingEnabled = true;
            this.cbDocType.Location = new System.Drawing.Point(99, 44);
            this.cbDocType.Name = "cbDocType";
            this.cbDocType.Size = new System.Drawing.Size(248, 21);
            this.cbDocType.TabIndex = 1;
            this.cbDocType.SelectedIndexChanged += new System.EventHandler(this.cbDocType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Интервал: с";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Тип документа";
            // 
            // gb2
            // 
            this.gb2.Controls.Add(this.clbFields);
            this.gb2.Location = new System.Drawing.Point(12, 122);
            this.gb2.Name = "gb2";
            this.gb2.Size = new System.Drawing.Size(359, 234);
            this.gb2.TabIndex = 6;
            this.gb2.TabStop = false;
            this.gb2.Text = "Выгружаемые поля";
            // 
            // clbFields
            // 
            this.clbFields.FormattingEnabled = true;
            this.clbFields.Location = new System.Drawing.Point(6, 19);
            this.clbFields.Name = "clbFields";
            this.clbFields.Size = new System.Drawing.Size(341, 199);
            this.clbFields.TabIndex = 0;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(215, 416);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 7;
            this.bOk.Text = "Выгрузить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(296, 416);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 8;
            this.bCancel.Text = "Выход";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // deEnd
            // 
            this.deEnd.FormattingEnabled = true;
            this.deEnd.InputChar = '*';
            this.deEnd.Location = new System.Drawing.Point(256, 17);
            this.deEnd.MaxLength = 10;
            this.deEnd.Name = "deEnd";
            this.deEnd.Size = new System.Drawing.Size(91, 21);
            this.deEnd.TabIndex = 3;
            // 
            // deStart
            // 
            this.deStart.FormattingEnabled = true;
            this.deStart.InputChar = '*';
            this.deStart.Location = new System.Drawing.Point(134, 17);
            this.deStart.MaxLength = 10;
            this.deStart.Name = "deStart";
            this.deStart.Size = new System.Drawing.Size(91, 21);
            this.deStart.TabIndex = 2;
            // 
            // gb3
            // 
            this.gb3.Controls.Add(this.cbSeparator);
            this.gb3.Controls.Add(this.label4);
            this.gb3.Controls.Add(this.cbQuotes);
            this.gb3.Location = new System.Drawing.Point(12, 362);
            this.gb3.Name = "gb3";
            this.gb3.Size = new System.Drawing.Size(359, 48);
            this.gb3.TabIndex = 9;
            this.gb3.TabStop = false;
            this.gb3.Text = "Параметры экспорта";
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
            this.cbSeparator.Location = new System.Drawing.Point(267, 15);
            this.cbSeparator.Name = "cbSeparator";
            this.cbSeparator.Size = new System.Drawing.Size(86, 21);
            this.cbSeparator.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(188, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Разделитель";
            // 
            // cbQuotes
            // 
            this.cbQuotes.AutoSize = true;
            this.cbQuotes.Location = new System.Drawing.Point(6, 19);
            this.cbQuotes.Name = "cbQuotes";
            this.cbQuotes.Size = new System.Drawing.Size(134, 17);
            this.cbQuotes.TabIndex = 0;
            this.cbQuotes.Text = "Значения в кавычках";
            this.cbQuotes.UseVisualStyleBackColor = true;
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "csv";
            this.sfd.Filter = "Файлы CSV|*.csv|Все файлы|*.*";
            // 
            // gb1
            // 
            this.gb1.Controls.Add(this.cbUnit);
            this.gb1.Controls.Add(this.label5);
            this.gb1.Controls.Add(this.cbDocType);
            this.gb1.Controls.Add(this.label1);
            this.gb1.Controls.Add(this.deStart);
            this.gb1.Controls.Add(this.deEnd);
            this.gb1.Controls.Add(this.label2);
            this.gb1.Controls.Add(this.label3);
            this.gb1.Location = new System.Drawing.Point(12, 12);
            this.gb1.Name = "gb1";
            this.gb1.Size = new System.Drawing.Size(359, 104);
            this.gb1.TabIndex = 10;
            this.gb1.TabStop = false;
            this.gb1.Text = "Выборка";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Отделение";
            // 
            // cbUnit
            // 
            this.cbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(99, 71);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(248, 21);
            this.cbUnit.TabIndex = 7;
            // 
            // ExportDocDataMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(379, 445);
            this.Controls.Add(this.gb1);
            this.Controls.Add(this.gb3);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.gb2);
            this.Name = "ExportDocDataMain";
            this.Text = "Выгрузка полей документов журнала";
            this.gb2.ResumeLayout(false);
            this.gb3.ResumeLayout(false);
            this.gb3.PerformLayout();
            this.gb1.ResumeLayout(false);
            this.gb1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDocType;
        private DEXExtendLib.DateEdit deStart;
        private DEXExtendLib.DateEdit deEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gb2;
        private System.Windows.Forms.CheckedListBox clbFields;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.GroupBox gb3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbQuotes;
        private System.Windows.Forms.ComboBox cbSeparator;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.GroupBox gb1;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.Label label5;
    }
}