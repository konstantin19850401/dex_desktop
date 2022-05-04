namespace DEXOffice
{
    partial class DataFieldsForm
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
            this.lbAll = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbSel = new System.Windows.Forms.ListBox();
            this.bAdd = new System.Windows.Forms.Button();
            this.bRemove = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.bUp = new System.Windows.Forms.Button();
            this.bDown = new System.Windows.Forms.Button();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbAll
            // 
            this.lbAll.FormattingEnabled = true;
            this.lbAll.Location = new System.Drawing.Point(12, 25);
            this.lbAll.Name = "lbAll";
            this.lbAll.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAll.Size = new System.Drawing.Size(170, 199);
            this.lbAll.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Доступные для просмотра поля";
            // 
            // lbSel
            // 
            this.lbSel.FormattingEnabled = true;
            this.lbSel.Location = new System.Drawing.Point(224, 25);
            this.lbSel.Name = "lbSel";
            this.lbSel.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSel.Size = new System.Drawing.Size(170, 199);
            this.lbSel.TabIndex = 2;
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(188, 25);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(30, 23);
            this.bAdd.TabIndex = 4;
            this.bAdd.Text = ">>";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bRemove
            // 
            this.bRemove.Location = new System.Drawing.Point(188, 54);
            this.bRemove.Name = "bRemove";
            this.bRemove.Size = new System.Drawing.Size(30, 23);
            this.bRemove.TabIndex = 5;
            this.bRemove.Text = "<<";
            this.bRemove.UseVisualStyleBackColor = true;
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Видимые поля";
            // 
            // bUp
            // 
            this.bUp.Location = new System.Drawing.Point(400, 25);
            this.bUp.Name = "bUp";
            this.bUp.Size = new System.Drawing.Size(50, 23);
            this.bUp.TabIndex = 7;
            this.bUp.Text = "Выше";
            this.bUp.UseVisualStyleBackColor = true;
            this.bUp.Click += new System.EventHandler(this.bUp_Click);
            // 
            // bDown
            // 
            this.bDown.Location = new System.Drawing.Point(400, 54);
            this.bDown.Name = "bDown";
            this.bDown.Size = new System.Drawing.Size(50, 23);
            this.bDown.TabIndex = 8;
            this.bDown.Text = "Ниже";
            this.bDown.UseVisualStyleBackColor = true;
            this.bDown.Click += new System.EventHandler(this.bDown_Click);
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(294, 240);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 9;
            this.bOk.Text = "Сохранить";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(375, 240);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 10;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // DataFieldsForm
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(462, 275);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.bDown);
            this.Controls.Add(this.bUp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bRemove);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.lbSel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataFieldsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Поля документов в таблице";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbSel;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bRemove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bUp;
        private System.Windows.Forms.Button bDown;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;

    }
}