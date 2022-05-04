namespace Kassa3
{
    partial class ExchangeImportForm
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
            this.lbRules = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bNew = new System.Windows.Forms.Button();
            this.bEdit = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.bImport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbRules
            // 
            this.lbRules.FormattingEnabled = true;
            this.lbRules.Location = new System.Drawing.Point(530, 29);
            this.lbRules.Name = "lbRules";
            this.lbRules.Size = new System.Drawing.Size(255, 472);
            this.lbRules.TabIndex = 0;
            this.lbRules.DoubleClick += new System.EventHandler(this.bEdit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(527, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Правила импорта";
            // 
            // bNew
            // 
            this.bNew.Location = new System.Drawing.Point(530, 507);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(81, 23);
            this.bNew.TabIndex = 2;
            this.bNew.Text = "Новое";
            this.bNew.UseVisualStyleBackColor = true;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // bEdit
            // 
            this.bEdit.Location = new System.Drawing.Point(617, 507);
            this.bEdit.Name = "bEdit";
            this.bEdit.Size = new System.Drawing.Size(81, 23);
            this.bEdit.TabIndex = 3;
            this.bEdit.Text = "Изменить";
            this.bEdit.UseVisualStyleBackColor = true;
            this.bEdit.Click += new System.EventHandler(this.bEdit_Click);
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(704, 507);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(81, 23);
            this.bDelete.TabIndex = 4;
            this.bDelete.Text = "Удалить";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Журнал импорта";
            // 
            // lbLog
            // 
            this.lbLog.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(15, 29);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(509, 472);
            this.lbLog.TabIndex = 6;
            this.lbLog.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbLog_DrawItem);
            // 
            // bImport
            // 
            this.bImport.Location = new System.Drawing.Point(15, 507);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(122, 23);
            this.bImport.TabIndex = 7;
            this.bImport.Text = "Импорт из файла";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 507);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Для тестов";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ExchangeImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 542);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bImport);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.bEdit);
            this.Controls.Add(this.bNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbRules);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExchangeImportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Импорт из файла iBank2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbRules;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button bEdit;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.Button bImport;
        private System.Windows.Forms.Button button1;
    }
}