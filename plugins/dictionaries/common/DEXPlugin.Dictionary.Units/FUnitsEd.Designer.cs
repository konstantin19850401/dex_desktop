namespace DEXPlugin.Dictionary.Units
{
    partial class FUnitsEd
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.CheckBox();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.nudUID = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDocumentState = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbForeignId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbRegion = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudUID)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "UID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Наименование";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Примечания";
            // 
            // cbStatus
            // 
            this.cbStatus.AutoSize = true;
            this.cbStatus.Location = new System.Drawing.Point(101, 249);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(125, 17);
            this.cbStatus.TabIndex = 5;
            this.cbStatus.Text = "Отделение активно";
            this.cbStatus.UseVisualStyleBackColor = true;
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(101, 58);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(235, 20);
            this.tbTitle.TabIndex = 2;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(101, 159);
            this.tbDesc.Multiline = true;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(235, 84);
            this.tbDesc.TabIndex = 4;
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(181, 272);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 6;
            this.bOK.Text = "Сохранить";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(261, 272);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // nudUID
            // 
            this.nudUID.Location = new System.Drawing.Point(101, 6);
            this.nudUID.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudUID.Name = "nudUID";
            this.nudUID.Size = new System.Drawing.Size(235, 20);
            this.nudUID.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(186, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Статус импортируемого документа";
            // 
            // cbDocumentState
            // 
            this.cbDocumentState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDocumentState.FormattingEnabled = true;
            this.cbDocumentState.Location = new System.Drawing.Point(204, 132);
            this.cbDocumentState.Name = "cbDocumentState";
            this.cbDocumentState.Size = new System.Drawing.Size(132, 21);
            this.cbDocumentState.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Внешний ID";
            // 
            // tbForeignId
            // 
            this.tbForeignId.Location = new System.Drawing.Point(101, 32);
            this.tbForeignId.Name = "tbForeignId";
            this.tbForeignId.Size = new System.Drawing.Size(235, 20);
            this.tbForeignId.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Регион";
            // 
            // tbRegion
            // 
            this.tbRegion.Location = new System.Drawing.Point(101, 84);
            this.tbRegion.Multiline = true;
            this.tbRegion.Name = "tbRegion";
            this.tbRegion.Size = new System.Drawing.Size(235, 42);
            this.tbRegion.TabIndex = 14;
            // 
            // FUnitsEd
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(349, 304);
            this.Controls.Add(this.tbRegion);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbForeignId);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbDocumentState);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudUID);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FUnitsEd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Данные отделения";
            ((System.ComponentModel.ISupportInitialize)(this.nudUID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbStatus;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.TextBox tbDesc;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.NumericUpDown nudUID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDocumentState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbForeignId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbRegion;
    }
}