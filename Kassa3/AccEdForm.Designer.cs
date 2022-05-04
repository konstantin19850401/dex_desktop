namespace Kassa3
{
    partial class AccEdForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.lOwner = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.tbShortcut = new System.Windows.Forms.TextBox();
            this.cbCurrency = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.tShortcutChanged = new System.Windows.Forms.Timer(this.components);
            this.lShortcutStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbBankTag = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Принадлежит фирме:";
            // 
            // lOwner
            // 
            this.lOwner.AutoSize = true;
            this.lOwner.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lOwner.Location = new System.Drawing.Point(134, 15);
            this.lOwner.Name = "lOwner";
            this.lOwner.Size = new System.Drawing.Size(109, 13);
            this.lOwner.TabIndex = 1;
            this.lOwner.Text = "Фирма-владелец";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(137, 64);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(343, 20);
            this.tbTitle.TabIndex = 2;
            // 
            // tbShortcut
            // 
            this.tbShortcut.Location = new System.Drawing.Point(137, 38);
            this.tbShortcut.Name = "tbShortcut";
            this.tbShortcut.Size = new System.Drawing.Size(174, 20);
            this.tbShortcut.TabIndex = 1;
            this.tbShortcut.TextChanged += new System.EventHandler(this.tbShortcut_TextChanged);
            // 
            // cbCurrency
            // 
            this.cbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrency.FormattingEnabled = true;
            this.cbCurrency.Location = new System.Drawing.Point(137, 90);
            this.cbCurrency.Name = "cbCurrency";
            this.cbCurrency.Size = new System.Drawing.Size(343, 21);
            this.cbCurrency.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Код";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Наименование";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Валюта";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(324, 144);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 8;
            this.bOk.Text = "Сохранить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(405, 144);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 9;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // tShortcutChanged
            // 
            this.tShortcutChanged.Tick += new System.EventHandler(this.tShortcutChanged_Tick);
            // 
            // lShortcutStatus
            // 
            this.lShortcutStatus.AutoSize = true;
            this.lShortcutStatus.Location = new System.Drawing.Point(317, 41);
            this.lShortcutStatus.Name = "lShortcutStatus";
            this.lShortcutStatus.Size = new System.Drawing.Size(10, 13);
            this.lShortcutStatus.TabIndex = 12;
            this.lShortcutStatus.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Финансовая группа";
            // 
            // cbBankTag
            // 
            this.cbBankTag.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbBankTag.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbBankTag.FormattingEnabled = true;
            this.cbBankTag.Location = new System.Drawing.Point(137, 117);
            this.cbBankTag.Name = "cbBankTag";
            this.cbBankTag.Size = new System.Drawing.Size(343, 21);
            this.cbBankTag.TabIndex = 14;
            // 
            // AccEdForm
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(492, 179);
            this.Controls.Add(this.cbBankTag);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lShortcutStatus);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCurrency);
            this.Controls.Add(this.tbShortcut);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.lOwner);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AccEdForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Счёт";
            this.Deactivate += new System.EventHandler(this.AccEdForm_Deactivate);
            this.Activated += new System.EventHandler(this.AccEdForm_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lOwner;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Timer tShortcutChanged;
        private System.Windows.Forms.Label lShortcutStatus;
        internal System.Windows.Forms.TextBox tbTitle;
        internal System.Windows.Forms.TextBox tbShortcut;
        internal System.Windows.Forms.ComboBox cbCurrency;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox cbBankTag;
    }
}