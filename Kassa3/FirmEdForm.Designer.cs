﻿namespace Kassa3
{
    partial class FirmEdForm
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
            this.bCancel = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.lShortcutStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.tbShortcut = new System.Windows.Forms.TextBox();
            this.tShortcutChanged = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(316, 64);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 13;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(235, 64);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 12;
            this.bOk.Text = "Сохранить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // lShortcutStatus
            // 
            this.lShortcutStatus.AutoSize = true;
            this.lShortcutStatus.Location = new System.Drawing.Point(300, 15);
            this.lShortcutStatus.Name = "lShortcutStatus";
            this.lShortcutStatus.Size = new System.Drawing.Size(10, 13);
            this.lShortcutStatus.TabIndex = 11;
            this.lShortcutStatus.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Наименование";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Код";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(118, 38);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(273, 20);
            this.tbTitle.TabIndex = 8;
            // 
            // tbShortcut
            // 
            this.tbShortcut.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.tbShortcut.Location = new System.Drawing.Point(118, 12);
            this.tbShortcut.MaxLength = 16;
            this.tbShortcut.Name = "tbShortcut";
            this.tbShortcut.Size = new System.Drawing.Size(176, 20);
            this.tbShortcut.TabIndex = 7;
            this.tbShortcut.TextChanged += new System.EventHandler(this.tbShortcut_TextChanged);
            // 
            // tShortcutChanged
            // 
            this.tShortcutChanged.Tick += new System.EventHandler(this.tShortcutChanged_Tick);
            // 
            // FirmEdForm
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(407, 98);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.lShortcutStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.tbShortcut);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FirmEdForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сведения о фирме";
            this.Deactivate += new System.EventHandler(this.FirmEdForm_Deactivate);
            this.Activated += new System.EventHandler(this.FirmEdForm_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Label lShortcutStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tbTitle;
        public System.Windows.Forms.TextBox tbShortcut;
        private System.Windows.Forms.Timer tShortcutChanged;
    }
}