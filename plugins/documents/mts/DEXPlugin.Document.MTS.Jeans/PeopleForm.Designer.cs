namespace DEXPlugin.Document.MTS.Jeans
{
    partial class PeopleForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbPeople = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtbData = new System.Windows.Forms.RichTextBox();
            this.bSelect = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbPeople);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(383, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ф.И.О., дата рождения";
            // 
            // lbPeople
            // 
            this.lbPeople.FormattingEnabled = true;
            this.lbPeople.Location = new System.Drawing.Point(6, 19);
            this.lbPeople.Name = "lbPeople";
            this.lbPeople.Size = new System.Drawing.Size(371, 69);
            this.lbPeople.TabIndex = 0;
            this.lbPeople.SelectedIndexChanged += new System.EventHandler(this.lbPeople_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtbData);
            this.groupBox2.Location = new System.Drawing.Point(13, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(383, 212);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Данные";
            // 
            // rtbData
            // 
            this.rtbData.Location = new System.Drawing.Point(6, 19);
            this.rtbData.Name = "rtbData";
            this.rtbData.ReadOnly = true;
            this.rtbData.Size = new System.Drawing.Size(370, 187);
            this.rtbData.TabIndex = 0;
            this.rtbData.Text = "test\n123";
            // 
            // bSelect
            // 
            this.bSelect.Location = new System.Drawing.Point(239, 333);
            this.bSelect.Name = "bSelect";
            this.bSelect.Size = new System.Drawing.Size(75, 23);
            this.bSelect.TabIndex = 2;
            this.bSelect.Text = "Выбрать";
            this.bSelect.UseVisualStyleBackColor = true;
            this.bSelect.Click += new System.EventHandler(this.bSelect_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(320, 333);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // PeopleForm
            // 
            this.AcceptButton = this.bSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(408, 365);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSelect);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PeopleForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выбор абонента";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bSelect;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.ListBox lbPeople;
        private System.Windows.Forms.RichTextBox rtbData;
    }
}