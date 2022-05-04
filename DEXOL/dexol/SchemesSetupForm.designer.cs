namespace dexol
{
    partial class SchemesSetupForm
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
            this.gbSelParams = new System.Windows.Forms.GroupBox();
            this.bSelPrnScheme = new System.Windows.Forms.Button();
            this.cbScheme = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPrinter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbSelParams.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSelParams
            // 
            this.gbSelParams.Controls.Add(this.bSelPrnScheme);
            this.gbSelParams.Controls.Add(this.cbScheme);
            this.gbSelParams.Controls.Add(this.label2);
            this.gbSelParams.Controls.Add(this.cbPrinter);
            this.gbSelParams.Controls.Add(this.label1);
            this.gbSelParams.Location = new System.Drawing.Point(12, 12);
            this.gbSelParams.Name = "gbSelParams";
            this.gbSelParams.Size = new System.Drawing.Size(582, 107);
            this.gbSelParams.TabIndex = 0;
            this.gbSelParams.TabStop = false;
            this.gbSelParams.Text = "Принтер и схема";
            // 
            // bSelPrnScheme
            // 
            this.bSelPrnScheme.Location = new System.Drawing.Point(419, 73);
            this.bSelPrnScheme.Name = "bSelPrnScheme";
            this.bSelPrnScheme.Size = new System.Drawing.Size(157, 23);
            this.bSelPrnScheme.TabIndex = 4;
            this.bSelPrnScheme.Text = "Редактирование";
            this.bSelPrnScheme.UseVisualStyleBackColor = true;
            this.bSelPrnScheme.Click += new System.EventHandler(this.bSelPrnScheme_Click);
            // 
            // cbScheme
            // 
            this.cbScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScheme.FormattingEnabled = true;
            this.cbScheme.Location = new System.Drawing.Point(62, 46);
            this.cbScheme.Name = "cbScheme";
            this.cbScheme.Size = new System.Drawing.Size(514, 21);
            this.cbScheme.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Схема";
            // 
            // cbPrinter
            // 
            this.cbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrinter.FormattingEnabled = true;
            this.cbPrinter.Location = new System.Drawing.Point(62, 19);
            this.cbPrinter.Name = "cbPrinter";
            this.cbPrinter.Size = new System.Drawing.Size(514, 21);
            this.cbPrinter.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Принтер";
            // 
            // SchemesSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 128);
            this.Controls.Add(this.gbSelParams);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SchemesSetupForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройка схем печати";
            this.gbSelParams.ResumeLayout(false);
            this.gbSelParams.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSelParams;
        private System.Windows.Forms.ComboBox cbPrinter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bSelPrnScheme;
        private System.Windows.Forms.ComboBox cbScheme;
        private System.Windows.Forms.Label label2;
    }
}