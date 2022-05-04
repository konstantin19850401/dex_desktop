namespace Kassa3
{
    partial class OpRuleEdForm
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
            this.gbRuleType = new System.Windows.Forms.GroupBox();
            this.cbClient = new System.Windows.Forms.ComboBox();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.rbCategory = new System.Windows.Forms.RadioButton();
            this.cbOperation = new System.Windows.Forms.ComboBox();
            this.rbOp = new System.Windows.Forms.RadioButton();
            this.cbAcc = new System.Windows.Forms.ComboBox();
            this.cbFirm = new System.Windows.Forms.ComboBox();
            this.rbFirm = new System.Windows.Forms.RadioButton();
            this.gbAction = new System.Windows.Forms.GroupBox();
            this.rbPermit = new System.Windows.Forms.RadioButton();
            this.rbProhibit = new System.Windows.Forms.RadioButton();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.rbAcc = new System.Windows.Forms.RadioButton();
            this.rbClient = new System.Windows.Forms.RadioButton();
            this.gbRuleType.SuspendLayout();
            this.gbAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbRuleType
            // 
            this.gbRuleType.Controls.Add(this.rbClient);
            this.gbRuleType.Controls.Add(this.rbAcc);
            this.gbRuleType.Controls.Add(this.cbClient);
            this.gbRuleType.Controls.Add(this.cbCategory);
            this.gbRuleType.Controls.Add(this.rbCategory);
            this.gbRuleType.Controls.Add(this.cbOperation);
            this.gbRuleType.Controls.Add(this.rbOp);
            this.gbRuleType.Controls.Add(this.cbAcc);
            this.gbRuleType.Controls.Add(this.cbFirm);
            this.gbRuleType.Controls.Add(this.rbFirm);
            this.gbRuleType.Location = new System.Drawing.Point(12, 12);
            this.gbRuleType.Name = "gbRuleType";
            this.gbRuleType.Size = new System.Drawing.Size(384, 157);
            this.gbRuleType.TabIndex = 0;
            this.gbRuleType.TabStop = false;
            this.gbRuleType.Text = "Тип и значение";
            // 
            // cbClient
            // 
            this.cbClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClient.FormattingEnabled = true;
            this.cbClient.Location = new System.Drawing.Point(95, 127);
            this.cbClient.Name = "cbClient";
            this.cbClient.Size = new System.Drawing.Size(283, 21);
            this.cbClient.TabIndex = 9;
            this.cbClient.SelectedValueChanged += new System.EventHandler(this.cbClient_SelectedValueChanged);
            // 
            // cbCategory
            // 
            this.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Location = new System.Drawing.Point(156, 100);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(222, 21);
            this.cbCategory.TabIndex = 7;
            // 
            // rbCategory
            // 
            this.rbCategory.AutoSize = true;
            this.rbCategory.Location = new System.Drawing.Point(6, 101);
            this.rbCategory.Name = "rbCategory";
            this.rbCategory.Size = new System.Drawing.Size(144, 17);
            this.rbCategory.TabIndex = 3;
            this.rbCategory.TabStop = true;
            this.rbCategory.Tag = "3";
            this.rbCategory.Text = "Категория контрагента";
            this.rbCategory.UseVisualStyleBackColor = true;
            this.rbCategory.CheckedChanged += new System.EventHandler(this.rbFirm_CheckedChanged);
            // 
            // cbOperation
            // 
            this.cbOperation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperation.FormattingEnabled = true;
            this.cbOperation.Location = new System.Drawing.Point(95, 73);
            this.cbOperation.Name = "cbOperation";
            this.cbOperation.Size = new System.Drawing.Size(283, 21);
            this.cbOperation.TabIndex = 5;
            // 
            // rbOp
            // 
            this.rbOp.AutoSize = true;
            this.rbOp.Location = new System.Drawing.Point(6, 74);
            this.rbOp.Name = "rbOp";
            this.rbOp.Size = new System.Drawing.Size(75, 17);
            this.rbOp.TabIndex = 2;
            this.rbOp.TabStop = true;
            this.rbOp.Tag = "2";
            this.rbOp.Text = "Операция";
            this.rbOp.UseVisualStyleBackColor = true;
            this.rbOp.CheckedChanged += new System.EventHandler(this.rbFirm_CheckedChanged);
            // 
            // cbAcc
            // 
            this.cbAcc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAcc.FormattingEnabled = true;
            this.cbAcc.Location = new System.Drawing.Point(95, 46);
            this.cbAcc.Name = "cbAcc";
            this.cbAcc.Size = new System.Drawing.Size(283, 21);
            this.cbAcc.TabIndex = 2;
            // 
            // cbFirm
            // 
            this.cbFirm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFirm.FormattingEnabled = true;
            this.cbFirm.Location = new System.Drawing.Point(95, 19);
            this.cbFirm.Name = "cbFirm";
            this.cbFirm.Size = new System.Drawing.Size(283, 21);
            this.cbFirm.TabIndex = 1;
            // 
            // rbFirm
            // 
            this.rbFirm.AutoSize = true;
            this.rbFirm.Location = new System.Drawing.Point(6, 20);
            this.rbFirm.Name = "rbFirm";
            this.rbFirm.Size = new System.Drawing.Size(62, 17);
            this.rbFirm.TabIndex = 0;
            this.rbFirm.TabStop = true;
            this.rbFirm.Tag = "0";
            this.rbFirm.Text = "Фирма";
            this.rbFirm.UseVisualStyleBackColor = true;
            this.rbFirm.CheckedChanged += new System.EventHandler(this.rbFirm_CheckedChanged);
            // 
            // gbAction
            // 
            this.gbAction.Controls.Add(this.rbPermit);
            this.gbAction.Controls.Add(this.rbProhibit);
            this.gbAction.Location = new System.Drawing.Point(12, 175);
            this.gbAction.Name = "gbAction";
            this.gbAction.Size = new System.Drawing.Size(384, 47);
            this.gbAction.TabIndex = 1;
            this.gbAction.TabStop = false;
            this.gbAction.Text = "Действие";
            // 
            // rbPermit
            // 
            this.rbPermit.AutoSize = true;
            this.rbPermit.Location = new System.Drawing.Point(190, 19);
            this.rbPermit.Name = "rbPermit";
            this.rbPermit.Size = new System.Drawing.Size(81, 17);
            this.rbPermit.TabIndex = 1;
            this.rbPermit.TabStop = true;
            this.rbPermit.Tag = "1";
            this.rbPermit.Text = "Разрешить";
            this.rbPermit.UseVisualStyleBackColor = true;
            // 
            // rbProhibit
            // 
            this.rbProhibit.AutoSize = true;
            this.rbProhibit.Location = new System.Drawing.Point(6, 19);
            this.rbProhibit.Name = "rbProhibit";
            this.rbProhibit.Size = new System.Drawing.Size(78, 17);
            this.rbProhibit.TabIndex = 0;
            this.rbProhibit.TabStop = true;
            this.rbProhibit.Tag = "0";
            this.rbProhibit.Text = "Запретить";
            this.rbProhibit.UseVisualStyleBackColor = true;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(240, 228);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 2;
            this.bOk.Text = "Сохранить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(321, 228);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // rbAcc
            // 
            this.rbAcc.AutoSize = true;
            this.rbAcc.Location = new System.Drawing.Point(6, 47);
            this.rbAcc.Name = "rbAcc";
            this.rbAcc.Size = new System.Drawing.Size(48, 17);
            this.rbAcc.TabIndex = 1;
            this.rbAcc.TabStop = true;
            this.rbAcc.Tag = "1";
            this.rbAcc.Text = "Счёт";
            this.rbAcc.UseVisualStyleBackColor = true;
            this.rbAcc.CheckedChanged += new System.EventHandler(this.rbFirm_CheckedChanged);
            // 
            // rbClient
            // 
            this.rbClient.AutoSize = true;
            this.rbClient.Location = new System.Drawing.Point(6, 128);
            this.rbClient.Name = "rbClient";
            this.rbClient.Size = new System.Drawing.Size(83, 17);
            this.rbClient.TabIndex = 4;
            this.rbClient.TabStop = true;
            this.rbClient.Tag = "4";
            this.rbClient.Text = "Контрагент";
            this.rbClient.UseVisualStyleBackColor = true;
            this.rbClient.CheckedChanged += new System.EventHandler(this.rbFirm_CheckedChanged);
            // 
            // OpRuleEdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(407, 259);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.gbAction);
            this.Controls.Add(this.gbRuleType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpRuleEdForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Операционное правило";
            this.gbRuleType.ResumeLayout(false);
            this.gbRuleType.PerformLayout();
            this.gbAction.ResumeLayout(false);
            this.gbAction.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbRuleType;
        private System.Windows.Forms.RadioButton rbFirm;
        private System.Windows.Forms.ComboBox cbFirm;
        private System.Windows.Forms.ComboBox cbAcc;
        private System.Windows.Forms.RadioButton rbOp;
        private System.Windows.Forms.ComboBox cbOperation;
        private System.Windows.Forms.ComboBox cbClient;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.RadioButton rbCategory;
        private System.Windows.Forms.GroupBox gbAction;
        private System.Windows.Forms.RadioButton rbPermit;
        private System.Windows.Forms.RadioButton rbProhibit;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.RadioButton rbAcc;
        private System.Windows.Forms.RadioButton rbClient;
    }
}